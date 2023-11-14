using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    [SerializeField] private AudioSource _pechat;
    [SerializeField] private GameObject _blockedClickPanel;
    [SerializeField] private Text _mainText;
    [SerializeField] private Image _speakerMan;
    [SerializeField] private Text _dialogText;
    [SerializeField] private GameObject _variantsPanel;
    [SerializeField] private Button[] _variantsButtons;
    [SerializeField] private GameObject _endSignal;
    [SerializeField] private ImageDataSO _spekersImges;

    private float _delay;
    private float _defaultDelay = 0.1f;



    public async Task ShowLongInfoAsync(string text)
    {
        DeactivateSpeaker();
        var strArr = text.Split('*');
        if (strArr.Length > 0)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            StartCoroutine(ShowMessagesAsync(strArr, () =>
            {
                taskCompletionSource.SetResult(true);
            }));

            await taskCompletionSource.Task;
        }
        EndDialog();
    }

    public void Skip()
    {
        _delay = 0;
    }


    private void DisableVariantsButtons()
    {
        foreach (var answ in _variantsButtons)
        {
            answ.gameObject.SetActive(false);
        }
    }


    private void ActivateSpeaker(string name)
    {
        _mainText.text = string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            DeactivateSpeaker();
        }
        else
        {
            ActivateSpeakerWithImage(name);
        }
    }

    private void DeactivateSpeaker()
    {
        _speakerMan.gameObject.SetActive(false);
        _dialogText.gameObject.SetActive(false);
        _mainText.gameObject.SetActive(true);
    }

    private void ActivateSpeakerWithImage(string name)
    {
        name = name.Trim();
        _speakerMan.sprite = _spekersImges.GetSpriteByName(name);
        _speakerMan.gameObject.SetActive(true);
        _dialogText.gameObject.SetActive(true);
        _mainText.gameObject.SetActive(false);
    }


    private IEnumerator ShowMessagesAsync(string[] strArr, Action onComplete)
    {
        _blockedClickPanel.SetActive(true);
        Text txtObj;

        var tempDelay = _defaultDelay;
        foreach (string text in strArr)
        {
            var str = text.Split("D:");
            var speaker = string.Empty;
            var info = string.Empty;
            if (str.Length > 1)
            {
                speaker = str[0];
                txtObj = _dialogText;
                info = str[1];
            }
            else
            {
                info = str[0];
                txtObj = _mainText;
            }
            ActivateSpeaker(speaker);

            _endSignal.SetActive(false);
            _pechat.Play();
            _delay = tempDelay;
            txtObj.text = string.Empty;
            foreach (char c in info)
            {
                txtObj.text += c;
                yield return new WaitForSeconds(_delay);
            }
            _pechat.Stop();
            //if (text != strArr.Last())
                _delay = tempDelay;
            _endSignal?.SetActive(true);
            while (_delay > 0)
            {
                yield return null;
            }
        }

        _delay = _defaultDelay;
        onComplete?.Invoke();
    }


    public void EndDialog()
    {
        _blockedClickPanel.SetActive(false);
        _speakerMan.gameObject.SetActive(false);
        _dialogText.gameObject.SetActive(false);
        _mainText.gameObject.SetActive(true);
    }
}

