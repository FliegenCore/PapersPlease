using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.PlayerExpirience
{
    [Order(100)]
    public class DocumentsController : MonoBehaviour, IControllerEntity
    {
        [Inject] private CharactersController _charactersController;
        [Inject] private TableController _tableController;

        [SerializeField] private PassportView _passportView;

        private PassportData _currentPassportData;

        private List<PassportData> _allPassports;

        [SerializeField] private List<PassportData> _dayPassports;

        public void PreInit()
        {
            _dayPassports = new List<PassportData>();

            _charactersController.OnDenied += ResetDocuments;
            _charactersController.OnEnterInRoom += ProvidePassport;

            _tableController.AppriveDeniedView.ApprovedButton.OnClick += RemovePassport;
            _tableController.AppriveDeniedView.DeniedButton.OnClick += RemovePassport;
        }

        public void Init()
        {
            LoadDayPassports();
        }

        public void ResetDocuments()
        {
            _currentPassportData = null;
        }

        public PassportData LoadPassport()
        {
            if (_dayPassports.Count <= 0)
            {
                //day end
                return null;
            }

            _currentPassportData = _dayPassports[0];
            _dayPassports.RemoveAt(0);

            return _currentPassportData;
        }

        private void ProvidePassport()
        {
            if (_currentPassportData == null)
            {
                return;
            }

            string nme = _currentPassportData.Name.Split('\n')[0].Trim('"');
            Sprite sprite = CharactersController.LoadCharacterSprite(nme);

            _passportView.SetPassportInfo(sprite, _currentPassportData.Name,
                _currentPassportData.PassportNumber, _currentPassportData.Day,
                _currentPassportData.Mounth, _currentPassportData.Year);

            _passportView.Enable();
        }

        private void RemovePassport()
        {
            _passportView.Disable();
        }

        private void LoadDayPassports()
        {
            _dayPassports.Clear();

            if (_allPassports == null)
            {
                _allPassports = new List<PassportData>();

                var allPassportsJson = Resources.LoadAll<TextAsset>("Passports");

                foreach (var json in allPassportsJson)
                {
                    try
                    {
                        _allPassports.Add(JsonUtility.FromJson<PassportData>(json.text));
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error parsing JSON: " + e.Message + " for file: " + json.name);
                    }
                }
            }

            List<PassportData> availablePassports = new List<PassportData>(_allPassports);

            for (int i = 0; i < _charactersController.CharactersLeft; i++)
            {
                if (availablePassports.Count == 0) break;

                int randomIndex = Random.Range(0, availablePassports.Count);
                _dayPassports.Add(availablePassports[randomIndex]);
                availablePassports.RemoveAt(randomIndex); 
            }
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
