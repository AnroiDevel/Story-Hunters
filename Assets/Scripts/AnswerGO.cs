using UnityEngine;
using UnityEngine.UI;


public class AnswerGO : MonoBehaviour
{
    [SerializeField] private InputField _text;
    [SerializeField] private InputField _nextLocationId;

    public InputField Text { get => _text; private set => _text = value; }

    public int GetNextLocationId()
    {
        var val = 0;
        if (_nextLocationId.text != string.Empty)
            val = int.Parse(_nextLocationId.text);
        return val;
    }

    internal void Clear()
    {
        _text.text = string.Empty;
        _nextLocationId.text = string.Empty;
    }

    internal void Init(Answer answ)
    {
        _text.text = answ.Text;
        _nextLocationId.text = answ.NextLocationId.ToString();
    }
}
