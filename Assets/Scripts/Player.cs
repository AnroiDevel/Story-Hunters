using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] private Image[] _itemImages;
    [SerializeField] private ImageDataSO _dataItemSprites;
    public string Name { get; set; }
    public string Description { get; set; }

    public Dictionary<string, DialogObject> Inventory { get; private set; } = new Dictionary<string, DialogObject>();

    private void Start()
    {
        foreach (var item in _itemImages)
            item.gameObject.SetActive(false);
    }


    internal void AddItem(DialogObject item)
    {
        Inventory.Add(item.Name, item);
        var index = Inventory.Count - 1;
        _itemImages[index].sprite = _dataItemSprites.GetSpriteByName(item.Name);
        if (_itemImages[index].sprite)
            _itemImages[index].gameObject.SetActive(true);

        if (Inventory.ContainsKey("Надпись у стены") && Inventory.ContainsKey("Надпись на полу") && Inventory.ContainsKey("Надпись за каналом"))
        {
            Inventory.Add("ALL", new DialogObject());
            RemoveItem("Надпись у стены");
            RemoveItem("Надпись на полу");
            RemoveItem("Надпись за каналом");
        }
    }

    internal void RemoveItem(string itemName)
    {
        int index = Inventory.Keys.ToList().IndexOf(itemName);
        _itemImages[index].gameObject.SetActive(false);
        Inventory.Remove(itemName);
    }
}
