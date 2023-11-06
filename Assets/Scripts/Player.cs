using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Player:MonoBehaviour
{
    [SerializeField] private Image[] _itemImages;
    [SerializeField] private ImageDataSO _dataItemSprites;
    public string Name { get; set; }
    public string Description { get; set; }

    public Dictionary<string, DialogObject> Inventory {  get; private set; } = new Dictionary<string, DialogObject>();

    private void Start()
    {
        foreach (var item in _itemImages)
            item.gameObject.SetActive(false);
    }


    internal void AddItem(DialogObject item)
    {
        Inventory.Add(item.Name, item);
        var index = Inventory.Count-1;
        _itemImages[index].sprite = _dataItemSprites.GetSpriteByName(item.Name);
        _itemImages[index].gameObject.SetActive(true);
    }

    internal void RemoveItem(DialogObject item)
    {
        int index = Inventory.Keys.ToList().IndexOf(item.Name);
        _itemImages[index].gameObject.SetActive(false);
        Inventory.Remove(item.Name);
    }
}
