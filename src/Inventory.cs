using Microsoft.VisualBasic.FileIO;

class Inventory
{
    private int maxWeight;
    private Dictionary<string, Item> items;
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }
     public string Show()
    {
        string inside = "";
        foreach (string itemName in items.Keys)
        {
            inside = inside + itemName + ",";
        }
        return inside;
    }
    public bool Put(string itemName,Item item)
    {
        if(FreeWeight() >= item.Weight)
        {
            items.Add(itemName, item);
            return true;
        }

        return false;

    }
    public Item Get(string itemName)
    {
        if(items.ContainsKey(itemName))
        {
            Item FoundItem = items[itemName];
            items.Remove(itemName);
            return FoundItem;
        }
        return null;

    }
    public int TotalWeight()
    {
        int total = 0;
        foreach (Item item in items.Values)
        {
            total = total + item.Weight;
        }

        return total;

    }
    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }public void Delet(string itemName)
    {
        
            items.Remove(itemName);
        
        
    }
   

}

