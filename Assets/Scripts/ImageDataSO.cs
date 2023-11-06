using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImageData", menuName = "Custom/SpritesSO")]
public class ImageDataSO : ScriptableObject
{
    public SpriteObj[] SpriteObjs;

    public Sprite GetSpriteByName(string name)
    {
        foreach (var sprite in SpriteObjs)
        {
            if (string.Equals(sprite.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return sprite.Sprite;
            }
        }
        return null;
    }
}


[Serializable]
public class SpriteObj
{
    public string Name;
    public Sprite Sprite;
}

