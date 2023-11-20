using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ObjectType _clickObjectType;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Texture2D _spitchCursorTexture;
    [SerializeField] private Texture2D _handCursorTexture;
    [SerializeField] private Texture2D _moveCursorTexture;

    [SerializeField] private ImageDataSO _imageDataPersons;
    [SerializeField] private ImageDataSO _imageDataItems;
    [SerializeField] private ImageDataSO _imageDataWays;
    [SerializeField] private ParticleSystem _particleSystem;

    private string _name;
    private bool _isActive;
    private Vector2 _originalHotspot;

    private GameManager _gameManager;
    private DialogObject _object;

    private RectTransform _rectTransform;

    public void Init(DialogObject obj)
    {
        if (_gameManager == null)
            _gameManager = GameManager.instance;

        _object = obj;
        _isActive = obj.IsActive;

        _clickObjectType = obj.ObjectType;
        _name = obj.Name;
        GetSprite();

        SetPositionsAndScale(obj);
        gameObject.SetActive(true);

        if (_isActive)
        {
            ButtonInit();
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }
    }

    private void SetPositionsAndScale(DialogObject obj)
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        Vector2 pos = new Vector2(obj.ImageInfo.X, obj.ImageInfo.Y);
        _rectTransform.sizeDelta = new Vector2(obj.ImageInfo.Width, obj.ImageInfo.Width);
        _rectTransform.anchoredPosition = pos;
    }

    public void ButtonInit()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            _gameManager.InitObject(_object);
        });
    }

    private void GetSprite()
    {
        if (_clickObjectType == ObjectType.Item)
        {
            _image.sprite = _imageDataItems.GetSpriteByName(_name);
        }
        else if (_clickObjectType == ObjectType.Character)
        {
            _image.sprite = _imageDataPersons.GetSpriteByName(_name);
        }
        else
        {
            _image.sprite = _imageDataWays.GetSpriteByName(_name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_isActive) return;
        Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _originalHotspot = currentPosition;

        Texture2D cursorTexture = (_clickObjectType == ObjectType.Item) ? _handCursorTexture :
            (_clickObjectType == ObjectType.Character) ? _spitchCursorTexture : _moveCursorTexture;

        _gameManager.ShowSmallInfo(_object.Info);
        Cursor.SetCursor(cursorTexture, currentPosition, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _gameManager.ShowSmallInfo(string.Empty);
        Cursor.SetCursor(null, _originalHotspot, CursorMode.Auto);
    }
}
