using System;
using System.Security.Cryptography;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university 🏛️.");
		Room theatre = new Room("in a lecture theatre 👨‍🏫.");
		Room pub = new Room("in the campus pub.");
		Room lab = new Room("in a computing lab 🖥️.");
		Room office = new Room("in the computing admin office 🖥️ 🏢.");
		Room cellar = new Room("in the dark cellar 🕯️ 🏚️ 🧭.");
		Room kitchen = new Room("in a dark kitchen with a guard 🍳 💀 🛡️ .");
		Room library = new Room("in a quiet library 🏛️ 📚 🪑.");
		Room hallway = new Room(" standing in a long hallway 🏫🧍🧍🚪.");
		Room classroom = new Room("in a classroom 👩‍🏫 🪑 📚");




		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);
		theatre.AddExit("north", hallway);


		hallway.AddExit("south", theatre);
		hallway.AddExit("east", library);

		library.AddExit("west", hallway);
		library.AddExit("south", classroom);
		
		


		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);
		office.AddExit("down", cellar);
		office.AddExit("east", kitchen);

		cellar.AddExit("up", office);
		// celler.("up", );


		kitchen.AddExit("west", office);
		
		// Create your Items here
		// ...
		Item potion = new Item(3, "potion");
		Item key = new Item(1, "key");
		Item small_medkits = new Item(6, "small_medkit");
		Item medkits = new Item(7, "medkit");
		Item acid = new Item(2, "acid");
		Item broken_medkit = new Item(3, "broken_medkit");
		Item axe = new Item(3, "axe");
		Item sword = new Item(3, "sword");

		// And add them to the Rooms
		// ...
		theatre.Chest.Put("potion", potion);
		theatre.Chest.Put("small_medkits", small_medkits);
		office.Chest.Put("medkits", medkits);
		outside.Chest.Put("acid", acid);
		cellar.Chest.Put("broken_medkit", broken_medkit);
		outside.Chest.Put("axe", axe);
		outside.Chest.Put("sword", sword);

		// Start game outside
        Guards Boss = new Guards(100, key); 
        Guards boss = new Guards(100, medkits); 
        hallway.AddLock(); 
		outside.RoomGuard = Boss;
        kitchen.RoomGuard =  boss;
        theatre.RoomGuard =  boss;


		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			if (!player.IsAlive())
			{
				Console.WriteLine("Thank you for playing you're died! Try agin!!");
				return;
			}
			 if(player.CurrentRoom.GetLongDescription().Contains("classroom")) 
{
				Console.WriteLine("Congratulations!! You are in the classroom");
				finished = true;
				return;
}

			 // In de Play() loop:

			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}
	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;
		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}
		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "look":
				Look();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				UseItem(command);
				break;
			case "attack":
				attack(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
		}

		return wantToQuit;
	}
	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.

private void attack(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you want to attack?");
			return;
		}else if (!command.HasThirdWord())
		{
			Console.WriteLine("At which door do you want to fight the guard? (e.g., 'attack guard axe')");
			return;
		}else
			{
				Console.WriteLine("You can't attack with that!");
			}
		
			string target = command.SecondWord;
			string itemName = command.ThirdWord;
			if (target == "guard")
		{
			Console.WriteLine(player.UseAxe(itemName));

		}
		else
		{
			Console.WriteLine("What do you want to attack ??");
		}
	}
	private void UseItem(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you want to use?");
			return;
		}
		string itemName = command.SecondWord;
		string direction = command.ThirdWord;
		if (itemName == "key")
		{
			if (string.IsNullOrEmpty(direction))
			{
				Console.WriteLine("Which door do you want to unlock? (e.g., 'use key east')");
				return;
			}

			Room targetRoom = player.CurrentRoom.GetExit(direction);
			if (targetRoom == null)
			{
				Console.WriteLine("There is no door in that direction!");
			}
			else
			{
				targetRoom.RoomUnlocked(itemName);
				Console.WriteLine($"Click! You unlocked the door to the {direction}.");
			}
			}
		else
			{
				Console.WriteLine(player.Use(itemName));
			}
	}
	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you take?");
			return;
		}
		string itemName = command.SecondWord;
		player.TakeFromChest(itemName);
	}
	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you to drop?");
			return;
		}
		string itemName = command.SecondWord;
		player.DropToChest(itemName);
	}
	private void PrintStatus()
	{
		Console.WriteLine(player.GetHealthStatus());
		Console.WriteLine("You have in your backpack: " + player.ShowInventory());

	}
	private void Look()

	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine(player.CurrentRoom.GetRoomItems());
	if (player.CurrentRoom.HasAliveGuard())
{
    Console.WriteLine("There are guards in this room!");
}
else
{
    Console.WriteLine("The room seems quiet. No guards in sight.");
}

	}
	private void PrintHelp()
	{ 
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}
		string direction = command.SecondWord;
		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		} if (nextRoom.IsRoomLocked())
        {
            Console.WriteLine("The door is locked.");
            return;
        }
	

		player.CurrentRoom = nextRoom;
		player.Damage(10);
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine("You damage 10 of your health.");
	}
}
