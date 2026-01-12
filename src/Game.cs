using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private bool kitchenLocked = true;

	

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room celler = new Room("You are in the dark celler");
		Room kitchen = new Room("you are in the dark kitchen");
		Room library = new Room("In the library");
		Room hallway = new Room("in a hallway");
		Room garden = new Room("in the garden");




		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);
		theatre.AddExit("north", hallway);


		hallway.AddExit("south", theatre);
		hallway.AddExit("east", library);

		library.AddExit("west", hallway);



		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);
		office.AddExit("down", celler);
		office.AddExit("east", kitchen);

		celler.AddExit("up", office);

		kitchen.AddExit("west", office);
		// Create your Items here
		// ...
		Item potion = new Item(3, "potion");
		Item key = new Item(1, "key");
		Item small_medkits = new Item(6, "small_medkits");
		Item medkits = new Item(7, "medkits");
		Item acid = new Item(2, "acid");
		Item broken_medkit = new Item(3, "broken_medkit");
		
		// And add them to the Rooms
		// ...
		theatre.Chest.Put("potion", potion);
		office.Chest.Put("key", key);
		theatre.Chest.Put("small_medkits", small_medkits);
		office.Chest.Put("medkits", medkits);
		outside.Chest.Put("acid", acid);
		celler.Chest.Put("broken_medkit", broken_medkit);


		// Start game outside
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
				Console.WriteLine("Thank you for playing you are dei.Try agin!!");
				return;
			}
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
	private void UseItem(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you want to use?");
			return;
		}string itemName = command.SecondWord;
		if(itemName == "key" && kitchenLocked)
		{
			kitchenLocked = false;
			Console.WriteLine(player.Use(itemName));
			// Console.WriteLine("The door is unlocked.");
			
			return;


		}
		Console.WriteLine(player.Use(itemName));


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
		}if (nextRoom.GetLongDescription().Contains("kitchen") && kitchenLocked)
		{
			Console.WriteLine("The door are locked you need a KEY.");
			return;

		}
		
		player.CurrentRoom = nextRoom;
		player.Damage(5);
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine("You damage 5 of your health.");

	}
}
