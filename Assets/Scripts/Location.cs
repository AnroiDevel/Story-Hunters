using System;
using System.Collections.Generic;
[Serializable]

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string[] Info { get; set; }

    public List<DialogObject> Objects { get; set; }
}
