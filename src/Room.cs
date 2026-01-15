using System.Collections.Generic;

class Room
{
	// Private fields
	private string description;
	private Inventory chest;
	private Dictionary<string, Room> exits; // stores exits of this room.
	private bool hasguard;
	private int healthGuard;
	private bool RoomLocked;
	private Item guardLoot;



	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".
	public bool HasGuards()
	{
		return hasguard;

	}

	public int HealthGuard()
	{
		return healthGuard;
	}
	public void GuardDamage(int amount)
	{
		healthGuard -= amount;
		if (healthGuard <= 0)
		{
			healthGuard = 0;
			hasguard = false;
			
		}
	}
	public void AddGuard(int hp, Item loot)
	{
		hasguard = true;
		healthGuard = hp;
		guardLoot = loot;

	}
	public Item GetGuardLoot()
	{
		Item loot = guardLoot;
		guardLoot = null;
		return loot;
	}


	// room lock
	public bool IsRoomLocked()
	{
		return RoomLocked;

	}
	public void AddLock()
	{
		RoomLocked = true;
	}
	public void RoomUnlocked(string itemName)
	{
		if(itemName == "key")
		{
			RoomLocked = false;
			return;

		}
	}
	
	public Inventory Chest
	{
		get { return chest; }
	}

	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		chest = new Inventory(999999);
		hasguard = false;
		RoomLocked = false;
		healthGuard = 0;

	}
	public string GetRoomItems()
	{
		string items = chest.Show();
		if (string.IsNullOrEmpty(items))

		{
			return "There are no items here.";
		}
		else
		{
			return "Items here: " + items;
		}
}
	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}
	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}
	// Return a long description of this room, in the form:
	//     You are in the kitchen.
	//     Exits: north, west
	public string GetLongDescription()
	{
		string str = "You are ";
		str += description;
		str += ".\n";
		str += GetExitString();
		return str;
	}
	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}
	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	private string GetExitString()
	{
		string str = "Exits: ";
		str += String.Join(", ", exits.Keys);
		return str;
	}
}
