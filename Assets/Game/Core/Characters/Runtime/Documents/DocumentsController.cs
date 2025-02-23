using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.PlayerExpirience
{
    [Order(-100001)]
    public class DocumentsController : MonoBehaviour, IControllerEntity
    {
        [Inject] private CharactersController _charactersController;
        [Inject] private TableController _tableController;

        [SerializeField] private PassportView _passportView;

        private PassportData _currentPassportData;

        private List<PassportData> _allPassports;


        public void PreInit()
        {
            _allPassports = new List<PassportData>();   
            _charactersController.OnDenied += ResetDocuments;
            _charactersController.OnEnterInRoom += ProvidePassport;

            _tableController.AppriveDeniedView.ApprovedButton.OnClick += RemovePassport;
            _tableController.AppriveDeniedView.DeniedButton.OnClick += RemovePassport;
        }

        public void Init()
        {

        }

        public void ResetDocuments()
        {
            _currentPassportData = null;
        }

        public void SetCurrentPassport(PassportData passport)
        {
            _currentPassportData = passport;
        }

        private void ProvidePassport()
        {
            if (_currentPassportData == null)
            {
                return;
            }

            string nme = _currentPassportData.Name.Split(' ')[0].Trim('"');

            Sprite sprite = CharactersController.LoadPassportSprite(nme);

            if (sprite == null)
            {
                sprite = CharactersController.LoadCharacterSprite(nme);
            }

            _passportView.SetPassportInfo(sprite, _currentPassportData.Name,
                _currentPassportData.PassportNumber, _currentPassportData.Day,
                _currentPassportData.Mounth, _currentPassportData.Year);

            _passportView.Enable();
        }

        private void RemovePassport()
        {
            _passportView.Disable();
        }

        public List<PassportData> LoadAllPassports()
        {
            var passportsJson = Resources.LoadAll<TextAsset>($"Passports");

            if (passportsJson == null)
            {
                return null;
            }

            foreach (var passportJson in passportsJson)
            {
                _allPassports.Add(JsonUtility.FromJson<PassportData>(passportJson.text));
            }

            return _allPassports;
        }

        private void OnDestroy()
        {
            _charactersController.OnDenied -= ResetDocuments;
            _charactersController.OnEnterInRoom -= ProvidePassport;

            _tableController.AppriveDeniedView.ApprovedButton.OnClick -= RemovePassport;
            _tableController.AppriveDeniedView.DeniedButton.OnClick -= RemovePassport;
        }
    }
}
