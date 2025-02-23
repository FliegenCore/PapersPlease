using Core.Common;
using Core.Dialogues;
using Core.Hotel;
using Core.PlayerExpirience;
using Core.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Entities
{
    [Order(-10000)]
    public class CharactersController : MonoBehaviour, IControllerEntity
    {
        public event Action OnEnterInRoom;
        public event Action OnApproved;
        public event Action OnDenied;

        [SerializeField] private AudioSource _holy;
        [SerializeField] private Character _currentCharacter;
        [SerializeField] private DialogueWindow _dialogueWindow;
        [SerializeField] private TMP_Text _visitorsLeft;

        [Inject] private TableController _tableController;
        [Inject] private EventManager _eventManager;
        [Inject] private DocumentsController _documentsController;
        [Inject] private DayController _dayController;
        [Inject] private HotelController _hotelController;

        private Dictionary<string, CharacterType> _charactersType;
        private Dictionary<string, bool> _characterFactor;

        [SerializeField] private List<CharacterData> _characters = new List<CharacterData>();
        [SerializeField] private List<CharacterData> _dayCharacters = new List<CharacterData>();

        private DialogueData _dialogueData;

        private int _characteresLeft;

        public int CharactersLeft => _characteresLeft;
        public CharacterType CharacterType => _currentCharacter.CharacterType;

        public void PreInit()
        {
            _charactersType = new Dictionary<string, CharacterType>
            {
                { "Linda", CharacterType.Human},
                { "Amanda", CharacterType.Human},
                { "Daniel", CharacterType.Demon},
                { "Vitalik", CharacterType.Demon},
                { "Sara", CharacterType.Human},
                { "Ignat", CharacterType.Human},
                { "Gena", CharacterType.Human},
                { "Amy", CharacterType.Human},
                { "Reed", CharacterType.Human},
                { "Lion", CharacterType.Human},
                { "Rose", CharacterType.Demon},
                { "Danil", CharacterType.Human},
                { "Wolf", CharacterType.Demon},
                { "Hunter", CharacterType.Human},
                { "Mr.Shlipa", CharacterType.Human},
                { "Mika", CharacterType.Human},
                { "HouHo", CharacterType.Human},
            };

            _characterFactor = new Dictionary<string, bool>
            {
                { "Linda", false},
                { "Amanda", false},
                { "Daniel", true},
                { "Vitalik", false},
                { "Sara", false},
                { "Ignat", true},
                { "Gena", false},
                { "Amy", false},
                { "Reed", false},
                { "Lion", false},
                { "Rose", false},
                { "Danil", false},
                { "Wolf", false},
                { "Hunter", true},
                { "Mr.Shlipa", true},
                { "Mika", false},
                { "HouHo", false},
            };

            _tableController.AppriveDeniedView.ApprovedButton.OnClick += ApprovedCurrentCharacter;
            _tableController.AppriveDeniedView.DeniedButton.OnClick += DeniedCurrentCharacter;
            _tableController.AppriveDeniedView.InviteButton.OnClick += ChangeCharacter;
            _hotelController.OnRoomChoosed += GoInHotel;

            LoadAllCharacters();

            _dayController.OnDayChanged += LoadDayCharacters;

            _dialogueWindow.SetDialogueText("");
        }

        public void Init()
        {
            _currentCharacter.Initialize();
        }

        public static Sprite LoadCharacterSprite(string nme)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites\\Characters\\{nme}");

            if (sprite == null)
            {
                return null;
            }

            return sprite;
        }

        public static Sprite LoadPassportSprite(string nme)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites\\Characters\\Passports\\{nme}");

            if (sprite == null)
            {
                return null;
            }

            return sprite;
        }

        public void DeniedCurrentCharacter()
        {
            StartCoroutine(WaitDenied());
        }

        public void ApprovedCurrentCharacter()
        {
            _hotelController.EnableChooseFreeFloor();
        }

        private void GoInHotel()
        {
            StartCoroutine(WaitGoInHotel());
        }

        public void CheckFactor()
        {
            if (!_characterFactor.ContainsKey(_currentCharacter.Name))
            {
                return;
            }

            if (_characterFactor[_currentCharacter.Name] == true)
            {
                Sprite sprite = LoadCharacterSprite(_currentCharacter.Name + "_1");

                _currentCharacter.SetSprite(sprite);
            }
        }

        private IEnumerator WaitDenied()
        {
            if (_currentCharacter.CharacterType == CharacterType.Demon)
            {
                CheckFactor();
            }

            _dialogueWindow.SetDialogueText(_dialogueData.EndNegativeDialogue);

            yield return new WaitForSeconds(2f);

            _dialogueWindow.SetDialogueText("");

            _currentCharacter.Denied(() =>
            {
                _characteresLeft--;
                _visitorsLeft.text = $"Visitors left: {_characteresLeft}";

                if (_characteresLeft > 0)
                {
                    OnDenied?.Invoke();
                }
                else
                {
                    Debug.Log("Change day");
                    //change day
                }
            });
        }

        private IEnumerator WaitGoInHotel()
        {
            _dialogueWindow.SetDialogueText(_dialogueData.EndPositiveDialogue);

            yield return new WaitForSeconds(2f);
            _dialogueWindow.SetDialogueText("");

            _currentCharacter.Approved(() =>
            {

                _characteresLeft--;
                _visitorsLeft.text = $"Visitors left: {_characteresLeft}";

                if (_characteresLeft > 0)
                {
                    OnApproved?.Invoke();
                }
            });
        }

        private void ChangeCharacter()
        {
            _holy.Play();
            int randomPassport = UnityEngine.Random.Range(0, _dayCharacters.Count);
            PassportData passportData = _dayCharacters[randomPassport].Passport;
            _dialogueData = _dayCharacters[randomPassport].DialugueData;

            if (passportData == null)
            {
                return;
            }

            _documentsController.SetCurrentPassport(passportData);
            string nme = passportData.Name.Split(' ')[0].Trim('"');

            _currentCharacter.SetSprite(LoadCharacterSprite(nme));
            _currentCharacter.SetName(nme);
            _currentCharacter.SetType(_dayCharacters[randomPassport].CharacterType);

            _currentCharacter.InviteInRoom(() =>
            {
                OnEnterInRoom?.Invoke();
                _dialogueWindow.SetDialogueText(_dialogueData.StartDialogue);
                _eventManager.TriggerEvenet<OnApproveDeniedButtonsSignal>();
            });

            _dayCharacters.RemoveAt(randomPassport);
        }

        private void LoadAllCharacters()
        {
            List<PassportData> passports = _documentsController.LoadAllPassports();
            List<DialogueData> dialogues = LoadDialogues();

            int i = 0;
            foreach (var passport in passports)
            {
                string nme = passport.Name.Split(' ')[0].Trim('"');

                _characters.Add(new CharacterData
                {
                    Passport = passport,
                    CharacterType = _charactersType[nme],
                    DialugueData = dialogues[i]
                });

                i++;
            }
        }

        private void LoadDayCharacters()
        {
            _tableController.AppriveDeniedView.InviteButton.EnableInteractable();

            _dayCharacters.Clear();

            _characteresLeft = _dayController.DayData.Demons + _dayController.DayData.Humans;
            _visitorsLeft.text = $"Visitors left: {_characteresLeft}";
            for (int i = 0; i < _dayController.DayData.Demons; i++)
            {
                CharacterData demon = _characters[UnityEngine.Random.Range(0, _characters.Count)];
                if (demon.CharacterType == CharacterType.Demon && !_dayCharacters.Contains(demon))
                {
                    _dayCharacters.Add(demon);
                }
                else
                { 
                    i--;
                }
            }

            for (int i = 0; i < _dayController.DayData.Humans; i++)
            {
                CharacterData human = _characters[UnityEngine.Random.Range(0, _characters.Count)];
                if (human.CharacterType == CharacterType.Human && !_dayCharacters.Contains(human))
                {
                    _dayCharacters.Add(human);
                }
                else
                {
                    i--;
                }
            }
        }

        private List<DialogueData> LoadDialogues()
        {
            List<DialogueData> dialogues = new List<DialogueData>();

            var asset = Resources.LoadAll<TextAsset>("Dialogues");
            foreach (var dialogueJson in asset)
            { 
                dialogues.Add(JsonUtility.FromJson<DialogueData>(dialogueJson.text));
            }

            return dialogues;
        }

        private void OnDestroy()
        {
            _tableController.AppriveDeniedView.ApprovedButton.OnClick -= ApprovedCurrentCharacter;
            _tableController.AppriveDeniedView.DeniedButton.OnClick -= DeniedCurrentCharacter;
        }
    }
}

