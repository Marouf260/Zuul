using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
// using NAudio.Wave;

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
    //  Damge en ook niet minder dan 0
    public void Damage(int amount)
    {
        // Je moet 'health =' ervoor zetten, anders gebeurt er niets!
        health = Math.Max(health - amount, 0);
    }
    public void Heal(int amount)
    {
        // Zelfde hier: sla het resultaat op in de health variabele
        health = Math.Min(health + amount, 100);
    }
    // Is de player nog aan het leven?
    public bool IsAlive()
    {
        return health > 0;
    }
    // get health 
    public int GetHealthStatus()
    {
        return health;
    }
    // Take item
    public bool TakeFromChest(string itemName)
    {
        Item GetItem = CurrentRoom.Chest.Get(itemName);
        if (GetItem == null)
        {
            Console.WriteLine($"Thier is no {itemName} to take.");
            return false;
        }

        backpack.Put(itemName, GetItem);
        Console.WriteLine("you take " + itemName);

        return true;
    }
    // items dropen
    public bool DropToChest(string itemName)
    {
        Item DropItem = backpack.Get(itemName);
        if (DropItem == null)
        {
            Console.WriteLine($"You don't have this item ''{itemName}'' to drop.");
            return false;
        }
        Console.WriteLine($"You drop {itemName}");
        CurrentRoom.Chest.Put(itemName, DropItem);

        return true;
    }
    // tas kijken
    public string ShowInventory()
    {
        return backpack.Show();
    }

    // USE item
    public string Use(string itemName)
    {

        Item UseItem = backpack.Get(itemName);
        if (UseItem == null)
        {
            return "You dont have this item in your backpack";

        }
        // Switch met message dat is gedaan om alle item te USE 
        string message = "";
        switch (itemName.ToLower())
        {
            case "potion":
                Heal(5);
                message = "Glug! You drank the potion. Your health is now " + health;
                backpack.Get(itemName);
                break;
            case "key":
                message = "Click! You unlocked the door .";
                backpack.Put(itemName, UseItem);
                break;

            case "small_medkit":
                Heal(10);
                message = " Glug! Your health is now : " + health;
                backpack.Get(itemName);
                break;
            case "medkit":
                Heal(5);
                message = " Glug! Your health is now : " + health;
                backpack.Get(itemName);
                break;
            case "acid":
                int damage = rand.Next(5, 11);
                Damage(damage);
                message = $"You take {damage} damage from drinking acid. Health now:  " + health;
                backpack.Get(itemName);
                break;
            case "broken_medkit":
                int brokdamage = rand.Next(5, 16);
                Damage(brokdamage);
                message = $"You take {brokdamage} damage from drinking broken medkit. Health now:  " + health;
                backpack.Get(itemName);
                break;

            default:
                message = $"You use {itemName}, but nothing happens.";
                break;

        }
        return message;
    }
    // Guard attack met Axe en Sword
    public string UseAxe(string itemName)
    {
        Item UseItem = backpack.Get(itemName);
        if (UseItem == null)
        {
            return "You dont have this item in your backpack";

        }
        backpack.Put(itemName, UseItem);
        string message = "";
        int damage = 0;
        if (itemName == "axe")
        {
            damage = rand.Next(5, 10);
        }
        else if (itemName == "sword")

        {
            Console.WriteLine("🗡️");
            damage = rand.Next(10, 25);
        }
        else
        {
            return $"You cannot attack with {itemName}.";
        }
        if (CurrentRoom.HasAliveGuard())
        {
            AttackGuard(damage);
            message = $"You hit the guard 🛡️ ,with {itemName} for {damage} damage🩸 . Guard health: {CurrentRoom.RoomGuard.GetHealth()}❤️\n";

        }
        else
        {
            return "There is no guard in this room.";
        }
        if (!CurrentRoom.RoomGuard.IsAlive())
        {
            message += "\nThe guard is dead 💀 .\nYou can go now.";

            Item loot = CurrentRoom.RoomGuard.Loot();
            if (loot != null)
            {
                CurrentRoom.Chest.Put(loot.Description, loot);
                message += $"\n\nThe guard dropped a {loot.Description}!";
            }
        }
        return message;
    }
    // Die is om de guard te damage
    public void AttackGuard(int damage)
    {
        if (CurrentRoom.HasAliveGuard())
        {
            CurrentRoom.RoomGuard.TakeDamage(damage);
            if (CurrentRoom.RoomGuard.IsAlive())
            {
                  int playerDamage = rand.Next(1, 10);
                Damage(playerDamage);
                Console.WriteLine($"The guard hits back! You take {playerDamage} damage 🩸."); 
            }
        }
    }
}