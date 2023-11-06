using UnityEngine;
using UnityEngine.UI;


public class FontSizeController : MonoBehaviour
{

    // Constants for PlayerPref keys
    private const string FontSizeMainKey = "FontSizeMain";
    private const string FontSizeAnswerKey = "FontSizeAnswer";

    // References to UI Text elements
    [SerializeField] private Text[] mainTexts;
    [SerializeField] private Text[] answerTexts;

    [SerializeField] private Text valueMainText;
    [SerializeField] private Text valueAnswerText;

    [SerializeField] private Slider _fontSizeMainSlider;
    [SerializeField] private Slider _fontSizeAnswerSlider;

    private int _defaultSizeFont = 18;

    private void Start()
    {
        // Load saved values from PlayerPrefs
        if (PlayerPrefs.HasKey(FontSizeMainKey))
        {
            int mainSize = PlayerPrefs.GetInt(FontSizeMainKey);
            int answerSize = PlayerPrefs.GetInt(FontSizeAnswerKey);

            _fontSizeMainSlider.value = mainSize;
            _fontSizeAnswerSlider.value = answerSize;
        }
        else
        {
            _fontSizeMainSlider.value = _defaultSizeFont;
            _fontSizeAnswerSlider.value = _defaultSizeFont;
        }

    }

    public void SetMainFontSize(float size)
    {

        // Update all main texts
        foreach (Text text in mainTexts)
        {
            text.fontSize = (int)size;
        }

        // Update value display
        valueMainText.text = size.ToString();

        // Save to PlayerPrefs
        PlayerPrefs.SetInt(FontSizeMainKey, (int)size);
    }

    public void SetAnswerFontSize(float size)
    {

        // Update all answer texts
        foreach (Text text in answerTexts)
        {
            text.fontSize = (int)size;
        }

        // Update value display
        valueAnswerText.text = size.ToString();

        // Save to PlayerPrefs
        PlayerPrefs.SetInt(FontSizeAnswerKey, (int)size);
    }

}