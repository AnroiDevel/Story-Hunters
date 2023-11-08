using System;

[Serializable]
public class DialogObject
{
    public string Name { get; set; }
    public string Info { get; set; }
    public ObjectType ObjectType { get; set; }
    public ImageInfo ImageInfo { get; set; }
    public bool IsActive { get; set; }
    public string[] Dialogs { get; set; }
}

