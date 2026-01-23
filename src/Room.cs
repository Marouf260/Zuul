using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

class Room
{
	// Private fields
	private string description;
	private Inventory chest;
	private Dictionary<string, Room> exits; // stores exits of this room.
	
	private bool isLocked;
    public Guards RoomGuard { get; set; }

	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		chest = new Inventory(999999);
		isLocked = false;
		RoomGuard = null;
	}
	// room lock
	public bool IsRoomLocked()
	{
		return isLocked;

	}
	public void AddLock()
	{
		isLocked = true;
	}
	public void RoomUnlocked(string itemName)
	{
		if (itemName == "key")
		{
			isLocked = false;
			return;

		}
	}
	public bool HasAliveGuard()
	{
		return RoomGuard != null && RoomGuard.IsAlive();
	}
	// private void ShowGuardAnimation()
	// {
	// 	 string[] frames =
	// 	{
    //     @" (o_o)
    //       /|\
    //       / \",

    //     @" (O_O)
    //       /|\
    //       / \",
	// 	   @" (0_0)
    //       /|\
    //       / \"
    // };

    // foreach (var frame in frames)
    // {
    //     Console.Clear();
    //     Console.WriteLine(frame);
    //     Thread.Sleep(300);
    // }
	// }
	public Inventory Chest
	{
		get { return chest; }
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
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}
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
