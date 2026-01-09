class Player
{
    public Room CurrentRoom { get; set; }
    private int health;
    private Inventory backpack;

    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(25);
    }

    public void Damage(int amount)
    {
        health = health - amount;
    }
    public int Heal(int amount)
    {
        return Math.Min(health + amount, 100);
    }
    public bool IsAlive()
    {
        return health > 0;
    }
    public int GetHealthStatus()
    {
        return health;
    }
    public bool TakeFromChest(string itemName)
    {
        Item GetItem = CurrentRoom.DeletItem(itemName);
        if (GetItem == null)
        {
            return false;
        }

        backpack.Put(itemName, GetItem);
        return true;
    }
    public bool DropToChest(string itemName)
    {
        Item DropItem = backpack.Get(itemName);
        if (DropItem == null)
        {
            return false;
        }
        CurrentRoom.Chest.Put(itemName, DropItem);

        return true;
    }
    public string ShowInventory()
    {
        return backpack.Show();
    }
    public string Use(string itemName)
    {
        Item UseItem = backpack.Get(itemName);
        if (UseItem == null)
        {
            return "You dont have this item in your backpack";

        }
        if (itemName == "potion")
        {
            health = health + 5;
            return "Glug! You drank the potion. Your health is now " + health;

        }
        backpack.Put(itemName, UseItem);
        return "you use this" + itemName;
    }
}