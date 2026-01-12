class Player
{
    public Room CurrentRoom { get; set; }
    private int health;
    private Inventory backpack;
    private Random rand; 

    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(25);
        rand = new Random();
    }

    public void Damage(int amount)
    {
        health = health - amount;
    }
    public void Heal(int amount)
    {
        health = Math.Min(health + amount, 100);

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
            Console.WriteLine($"Thier is no {itemName} to take.");
            return false;
        }

        backpack.Put(itemName, GetItem);
        Console.WriteLine("you take " + itemName);

        return true;
    }
    public bool DropToChest(string itemName)
    {
        Item DropItem = backpack.Get(itemName);
        if (DropItem == null)
        {
            Console.WriteLine($"You don't have this item ''{itemName}'' to drop.");
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
        if (string.IsNullOrEmpty(itemName))
        {
            return "You must specify an item!";
        }
        Item UseItem = backpack.Get(itemName);
        if (UseItem == null)
        {
            return "You dont have this item in your backpack";

        }
        string message = "";
        switch (itemName.ToLower())
        {
            case "potion":
                Heal(5);
                message = "Glug! You drank the potion. Your health is now " + health;
                backpack.Delet(itemName);
                break;
            case "key":
                Heal(5);
                message = "The door is unlocked.";
                backpack.Put(itemName,UseItem);
                break;
            case "small_medkits":
                Heal(10);
                message = " Glug! Your health is now : " + health;
                backpack.Delet(itemName);
                break;
             case "medkits":
                Heal(5);
                message = " Glug! Your health is now : "+ health;
                backpack.Delet(itemName);
                break;
            case "acid":
                int damage = rand.Next(5, 11);
                Damage(damage);
                message =$"You take {damage} damage from drinking acid. Health now:  " + health;
                backpack.Delet(itemName);
                break;
            case "broken_medkit":
                int brokdamage = rand.Next(5, 16);
                Damage(brokdamage);
                message =$"You take {brokdamage} damage from drinking broken medkit. Health now:  " + health;

                backpack.Delet(itemName);
                break;

                default:
            message = $"You use {itemName}, but nothing happens.";
            break;
           
        }
        return message;
    }
}