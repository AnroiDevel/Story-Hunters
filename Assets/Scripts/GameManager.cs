﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TextAsset _questData;

    [SerializeField] private Button[] _variantsButtons;
    [SerializeField] private ClickableObject[] _objects;

    [SerializeField] private GameObject _blockedClickPanel;
    [SerializeField] private Text _mainText;
    [SerializeField] private Image _speakerMan;
    [SerializeField] private Text _dialogText;
    [SerializeField] private GameObject _variantsPanel;

    [SerializeField] private Image _locationImage;

    [SerializeField] private ImageDataSO _locationsImageData;
    [SerializeField] private ImageDataSO _itemsImageData;
    [SerializeField] private ImageDataSO _personsImageData;

    [SerializeField] private Player _player;

    [SerializeField] private DialogManager _dialogManager;

    private Dictionary<int, Location> _locationsDict;

    private Location _currentLocation;

    private void Start()
    {
        instance = this;
        Load();
        StartGame();
    }

    private void Load()
    {
        string json = _questData.text;

        var locations = JsonConvert.DeserializeObject<List<Location>>(json);
        _locationsDict = new Dictionary<int, Location>();
        foreach (Location location in locations)
        {
            _locationsDict.Add(location.Id, location);
        }
    }

    public void StartGame()
    {
        ApplyLocate(_locationsDict[1]);
    }

    private async void ApplyLocate(Location currentLocation)
    {
        _locationImage.sprite = _locationsImageData.GetSpriteByName(currentLocation.Name);
        _currentLocation = currentLocation;

        // Clear previous data
        ClearAllLocationFields();
        ShowObjects(currentLocation);

        await _dialogManager.ShowLongInfoAsync(currentLocation.Info, false);
    }

    private void ShowObjects(Location currentLocation)
    {
        if (currentLocation.Objects != null)
        {
            for (int i = 0; i < currentLocation.Objects.Count; i++)
            {
                var obj = currentLocation.Objects[i];
                _objects[i].Init(obj);
            }
        }
    }

    public void ShowSmallInfo(string text)
    {
        _mainText.text = text;
    }

    private void ClearAllLocationFields()
    {
        _variantsPanel.SetActive(false);
        foreach (var answ in _variantsButtons)
        {
            answ.gameObject.SetActive(false);
        }
        foreach (var perons in _objects)
        {
            perons.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    internal void InitButtons(DialogObject dialogObject)
    {
        if (dialogObject.Variants == null) return;

        _blockedClickPanel.SetActive(true);
        DisableAnswerButtons();
        _speakerMan.gameObject.SetActive(false);
        var variantCount = dialogObject.Variants.Length;
        if (variantCount > 0)
        {
            _variantsPanel.SetActive(true);
        }

        if (dialogObject.ObjectType.Equals(ObjectType.Pers))
        {
            _speakerMan.sprite = _personsImageData.GetSpriteByName(dialogObject.Name);
        }
        InitVariantsButtons(dialogObject, variantCount);
    }

    private void InitVariantsButtons(DialogObject dialogObject, int variantCount)
    {
        for (int i = 0; i < variantCount; i++)
        {
            var btn = _variantsButtons[i];
            btn.gameObject.SetActive(true);
            var text = btn.GetComponentInChildren<Text>();
            var allInfo = dialogObject.Variants[i].Split("*");
            text.text = allInfo[0];

            btn.onClick.RemoveAllListeners();

            if (allInfo.Length > 1)
            {
                foreach (var info in allInfo.Skip(1))
                {
                    switch (info)
                    {
                        case string actionInfo when actionInfo.Contains("Close"):
                            btn.onClick.AddListener(() => CloseDialog());
                            break;

                        case string actionInfo when actionInfo.Contains("Spitch"):
                            btn.onClick.AddListener(async () =>
                            {
                                var taskCompletionSource = new TaskCompletionSource<bool>();
                                await ShowSpitchDialogAsync(dialogObject.Dialogs);
                            });
                            break;

                        case string actionInfo when actionInfo.Contains("Take"):
                            btn.onClick.AddListener(() => TakeItem(dialogObject));
                            break;

                        // Add more cases for other actions if needed

                        default:
                            // Handle unknown action
                            break;
                    }
                }
            }
        }
    }

    private void CloseDialog()
    {
        _dialogText.gameObject.SetActive(false);
        _speakerMan.gameObject.SetActive(false);
        _variantsPanel.SetActive(false);
        _blockedClickPanel.SetActive(false);
    }

    private async Task ShowSpitchDialogAsync(string[] dialog)
    {
        _variantsPanel.SetActive(false);
        _speakerMan.gameObject.SetActive(true);
        _dialogText.gameObject.SetActive(true);

        foreach (var dialogPart in dialog)
        {
            await _dialogManager.ShowLongInfoAsync(dialogPart, true);
        }
        _dialogManager.EndDialog();
    }





    private void TakeItem(DialogObject dialogObject)
    {
        _player.AddItem(dialogObject);
        _currentLocation.Objects.Remove(dialogObject);
        ShowObjects(_currentLocation);
        _variantsPanel.SetActive(false);
        _blockedClickPanel.SetActive(false);
    }

    private void DisableAnswerButtons()
    {
        foreach (var answ in _variantsButtons)
        {
            answ.gameObject.SetActive(false);
        }
    }
}
