using System.Collections.Generic;

public class Inventory
{
    public List<Item> Items { get; set; }

    public Inventory()
    {
        Items = new List<Item>();
    }
}
