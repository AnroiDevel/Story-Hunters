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

    private float _delay;
    private float _defaultDelay = 0.1f;



    public async Task ShowLongInfoAsync(string text, bool isCharacter)
    {
        var strArr = text.Split('*');
        if (strArr.Length > 0)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            StartCoroutine(ShowMessagesAsync(strArr, isCharacter, () =>
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


    private IEnumerator ShowMessagesAsync(string[] strArr, bool isDialog, Action onComplete)
    {
        _blockedClickPanel.SetActive(true);
        Text txt;
        if (isDialog)
        {
            txt = _dialogText;
            _speakerMan.gameObject.SetActive(true);
            _dialogText.gameObject.SetActive(true);
            _mainText.gameObject.SetActive(false);
        }
        else
        {
            _speakerMan.gameObject.SetActive(false);
            _dialogText.gameObject.SetActive(false);
            _mainText.gameObject.SetActive(true);
            txt = _mainText;
        }

        var tempDelay = _defaultDelay;
        foreach (string text in strArr)
        {
            _endSignal.SetActive(false);
            _pechat.Play();
            _delay = tempDelay;
            txt.text = string.Empty;
            foreach (char c in text)
            {
                txt.text += c;
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
    }
}

