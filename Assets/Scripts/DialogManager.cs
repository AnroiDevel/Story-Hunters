using System;
using System.Collections;
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

    private float _delay = 0.1f;


    public async Task ShowLongInfoAsync(string text, bool isDialog)
    {
        var strArr = text.Split('*');
        if (strArr.Length > 0)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            StartCoroutine(ShowMessagesAsync(strArr, isDialog, () =>
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
        var txt = isDialog ? _dialogText : _mainText;
        var tempDelay = _delay;
        foreach (string text in strArr)
        {
            _pechat.Play();
            _delay = tempDelay;
            txt.text = string.Empty;
            foreach (char c in text)
            {
                txt.text += c;
                yield return new WaitForSeconds(_delay);
            }
            _pechat.Stop();
            yield return new WaitForSeconds(1);
        }

        _delay = tempDelay;
        onComplete?.Invoke();
    }

    public void EndDialog()
    {
        _blockedClickPanel.SetActive(false);
        _speakerMan.gameObject.SetActive(false);
        _dialogText.gameObject.SetActive(false);
    }
}

