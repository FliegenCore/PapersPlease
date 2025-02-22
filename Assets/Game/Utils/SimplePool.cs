using Common.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class SimplePool<T> where T : MonoBehaviour
    {
        private List<T> _objects = new List<T>();
        
        private Transform _parent;
        private string _assetName;

        public SimplePool(string assetName, T[] objs = null, Transform parent = null) 
        {
            if (objs != null)
            { 
                _objects.AddRange(objs);
            }

            _parent = parent;
            _assetName = assetName;
        }

        public Result<T> Spawn2D(Vector2 pos)
        {
            Result<T> result = new Result<T>();

            if (_objects.Count > 0)
            {
                result = new Result<T>(_objects.First(), true);
            }
            else
            {
                var objAsset = Resources.Load<T>(_assetName);
                T obj = Object.Instantiate(objAsset, new Vector3(pos.x, 0, pos.y), Quaternion.identity, _parent);
                result = new Result<T>(obj, true);
            }

            result.Object.gameObject.SetActive(true);
            _objects.Remove(result.Object);

            return result;
        }

        public void ReturnInPool(T obj)
        {
            if (_objects.Contains(obj))
            {
                return;
            }

            obj.gameObject.SetActive(false);

            _objects.Add(obj);
        }
    }
}
