using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.Common
{
    public class Bootstraper : MonoBehaviour
    {
        private List<object> m_Entites = new List<object>();   

        private EventManager m_EventManager = new EventManager();

        private void Awake()
        {
            m_Entites.Add(m_EventManager);

            FindAllControllers();
        }

        private void FindAllControllers()
        {
            var controllers = FindObjectsOfType<MonoBehaviour>()
            .OfType<IControllerEntity>()
            .Select(x =>
            {
                var orderAttrib = x.GetType().GetCustomAttributes(typeof(Order), false)
               .Cast<Order>().FirstOrDefault();
                return new
                {

                    Order = orderAttrib.Id,
                    Current = x
                };
            })
            .OrderBy(x => x.Order)
            .ToArray();

            foreach (var controller in controllers)
            { 
                m_Entites.Add(controller.Current);
            }

            foreach (var controller in controllers)
            {
                InjectDependency(controller.Current);
            }

            foreach (var controller in controllers)
            {
                Debug.Log($"{controller.Current.GetType()} register, order: {controller.Order}");
                controller.Current.PreInit();
                controller.Current.Init();
            }
        }

        private void InjectDependency(IControllerEntity controller)
        {
            var fields = controller.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var field in fields)
            {
                if (field.GetCustomAttribute(typeof(Inject)) != null)
                {
                    foreach (var entity in m_Entites)
                    {
                        if (field.FieldType == entity.GetType())
                        {
                            field.SetValue(controller, entity);
                        }
                    }
                }
            }
        }
    }
}
