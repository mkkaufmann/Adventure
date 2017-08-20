/*
 * Michael Kaufmann
 * Started 7/2/17
 * Text-based Adventure Game with speech recognition
 * Works on Windows 7+
 * 
 * TODO: add cancel (check)
 * correct processing (check)
 * add comments (check) come back to this
 * add item support (check)
 * add enemy support
 * add points (check)
 * add high score board (check)
 * organize into multiple files (check)
 * add text typing out (check)
 * add saving support (low priority)
 * permanent solution to use while empty or grab while empty (check)
 * fix pascal vs camel casing
*/
using System;
using System.Collections.Generic;//for lists
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;//for speech recognition

namespace ConsoleApp2
{
    class Program
    {
        static System.Speech.Recognition.SpeechRecognitionEngine reco = new System.Speech.Recognition.SpeechRecognitionEngine();//This is the speech recognition engine
        static void SetSpeechReco(string[] words)//used to change the words that are recognized by the engine
        {
            reco.UnloadAllGrammars();//make sure any previously loaded grammars are no longer in effect

            System.Speech.Recognition.SrgsGrammar.SrgsDocument gram = new System.Speech.Recognition.SrgsGrammar.SrgsDocument();//create a new grammar
            System.Speech.Recognition.SrgsGrammar.SrgsRule rule = new System.Speech.Recognition.SrgsGrammar.SrgsRule("CurrentCommands");//create a new rule
            System.Speech.Recognition.SrgsGrammar.SrgsOneOf list = new System.Speech.Recognition.SrgsGrammar.SrgsOneOf(words);//create a list of words to listen for

            rule.Add(list);//add the list to the rule
            gram.Rules.Add(rule);//add the rule to the grammar
            gram.Root = rule;//set the rule to the root rule

            reco.LoadGrammar(new System.Speech.Recognition.Grammar(gram));//load the grammar into the engine
        }
        //list of words that are in the game
        static word submit = new word("submit",new string[] {"enter","return","done"});
        static word cancel = new word("cancel");
        static word north = new word("north", nextWordsParam:new word[] { submit, cancel });
        static word south = new word("south", nextWordsParam: new word[] { submit, cancel });
        static word east = new word("east", nextWordsParam: new word[] { submit, cancel });
        static word west = new word("west", nextWordsParam: new word[] { submit, cancel });
        static word go = new word("go", new string[] { "head", "skedaddle", "run", "walk"},new word[] {north,south,east,west});
        static word activate = new word("activate", new string[] { "use", "utilize"});
        static word drop = new word("drop", new string[] { "leave", "dispel", "discard" });
        static word grab = new word("grab", new string[] { "pick up", "seize", "snatch","get","acquire","obtain","collect","attain"});

        //initialize leaderboard
        static Leaderboard leaderboard = new Leaderboard();

        static bool gameIsRunning = true;

        static string currentCommand = "";//used to keep track of the current command being made

        static player player = new player();//initialize the player

        static List<string> defaultCommands = new List<string>();//variable that stores the default commands

        static void setDefaultCommands()//Used to set up the commands available to be used as the first words
        {
            defaultCommands.Clear();//delete all current default commands
            foreach(string word in go.wordsReturn())//add all variants of go
            {
                defaultCommands.Add(word);
            }
            if(player.currentRoom.items.Count() > 0)//if there are items in the room, add all variants of grab
            {
                foreach(string word in grab.wordsReturn())
                {
                    defaultCommands.Add(word);
                }
            }
            if (player.inventory.Count() > 0)//if there are items in the player's inventory, add all variants of activate and drop
            {
                foreach(string word in activate.wordsReturn())
                {
                    defaultCommands.Add(word);
                }
                foreach(string word in drop.wordsReturn())
                {
                    defaultCommands.Add(word); 
                }
            }
        }
        static void processCommand()//processes the current command
        {
            string[] splitCommand = currentCommand.Split(",".ToCharArray());//split the command into separate words

            if (splitCommand.Contains("cancel"))//reset immediately if the command contains cancel
            {
                SetSpeechReco(defaultCommands.ToArray());//resets recognition to default
                currentCommand = "";//resets the current command to store the next command
                Console.WriteLine("");//move the console to the next line
                return;//exit the function
            }
            string commandPart = splitCommand[0];//store the first part of the command
            if (go.checkForEquiv(commandPart))
            {
                commandPart = splitCommand[1];
                if (north.checkForEquiv(commandPart))
                {
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        player.currentRoom.north.enter(player);
                        if (!(player.currentRoom.north.isNull))//if the room to the north isn't a wall, set the player's current room to it
                        {
                            player.currentRoom = player.currentRoom.north;
                        }
                    }
                }
                else if(south.checkForEquiv(commandPart))
                {
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        player.currentRoom.south.enter(player);
                        if (!(player.currentRoom.south.isNull))
                        {
                            player.currentRoom = player.currentRoom.south;
                        }
                    }
                }
                else if (east.checkForEquiv(commandPart))
                {
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        player.currentRoom.east.enter(player);
                        if (!(player.currentRoom.east.isNull))
                        {
                            player.currentRoom = player.currentRoom.east;
                        }
                    }
                }
                else if (west.checkForEquiv(commandPart))
                {
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        player.currentRoom.west.enter(player);
                        if (!(player.currentRoom.west.isNull))
                        {
                            player.currentRoom = player.currentRoom.west;
                        }
                    }
                }
            }else if (grab.checkForEquiv(commandPart))
            {
                commandPart = splitCommand[1];
                if (player.currentRoom.checkIfItem(commandPart))
                {
                    item item = player.currentRoom.getItem(commandPart);
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        item.PickUp(player);
                    }
                }
            }
            else if (activate.checkForEquiv(commandPart))
            {
                commandPart = splitCommand[1];
                if (player.checkIfItem(commandPart))
                {
                    item item = player.getItem(commandPart);
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        item.activate(player);
                    }
                }
            }
            else if (drop.checkForEquiv(commandPart))
            {
                commandPart = splitCommand[1];
                if (player.checkIfItem(commandPart))
                {
                    item item = player.getItem(commandPart);
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        item.Drop(player);
                    }
                }
            }
            if(player.health <= 0)
            {
                endGame(false);
                return;
            }
            //reset commands
            setDefaultCommands();
            SetSpeechReco(defaultCommands.ToArray());
            currentCommand = "";
        }
        static void endGame(bool gameWon)
        {
            if (gameWon)
            {
                type("Thank you for playing this game. For completing the game, you have been awarded a bonus 1000 points.");
                player.points += 1000;
                leaderboard.AddScore(player);
                Console.WriteLine(leaderboard.Display());
            }
            else
            {
                type("GAME OVER. It's like you didn't even try.");
                leaderboard.AddScore(player);
                Console.WriteLine(leaderboard.Display());
            }
            reco.RecognizeCompleted -= new EventHandler<System.Speech.Recognition.RecognizeCompletedEventArgs>(restartRecognition);
            reco.SpeechRecognized -= new EventHandler<System.Speech.Recognition.SpeechRecognizedEventArgs>(handleRecognition);
            Console.ReadLine();
            gameIsRunning = false;
        }
        static void type(string text)//a way to type out text while waiting for it to finish before continuing
        {
            typer typer = new typer(text);
            Task currentTask = Task.Factory.StartNew(() => { typer.start(); });
            currentTask.Wait();
        }
        static void Main(string[] args)
        {
            

            //set up event handlers for recognition
            reco.RecognizeCompleted += new EventHandler<System.Speech.Recognition.RecognizeCompletedEventArgs>(restartRecognition);
            reco.SpeechRecognized += new EventHandler<System.Speech.Recognition.SpeechRecognizedEventArgs>(handleRecognition);

            //set up the default wall
            wall wall = new wall();


            //OLD MAP

            /*room room1 = new room("Welcome to room 1. There is some help you can pick up for later. Try 'grab help submit.'");
            room room2 = new room("Welcome to room 2.");
            room room3 = new room("Welcome to room 3.");
            room room4 = new room("Welcome to room 4.");
            room room5 = new room("Welcome to room 5.");
            room room6 = new room("Welcome to room 6.");
            room room7 = new room("Welcome to room 7.");
            room room8 = new room("Welcome to room 8.");
            room room9 = new room("Welcome to room 9.");

            room1.attach("north", room5);
            room1.attach("south", room3);
            room1.attach("west", room4);
            room1.attach("east", room2);
            room9.attach("east", room5);
            room9.attach("south", room4);
            room6.attach("west", room5);
            room6.attach("south", room2);
            room7.attach("west", room3);
            room7.attach("north", room2);
            room8.attach("east", room3);
            room8.attach("north", room4);

            room9.north = wall;
            room9.west = wall;
            room4.west = wall;
            room8.west = wall;
            room8.south = wall;
            room3.south = wall;
            room7.south = wall;
            room7.east = wall;
            room2.east = wall;
            room6.east = wall;
            room6.north = wall;
            room5.north = wall;

            help help = new help();
            room1.items.Add(help);*/

            //player.currentRoom = room1;

            room room1 = new room("You wake up in a room with concrete walls and an odd stench in the air. There is a compass design on the floor that tells you that the sole door ahead is north.");
            room room2 = new room("You step through the door and hear a loud creak. When you look around, you see a potato on a table and doors to the east and west.");
            room room3 = new room("Welcome to room 3.");
            room room4 = new room("Welcome to room 4.");
            room room5 = new room("Welcome to room 5.");
            room room6 = new room("Welcome to room 6.");
            room room7 = new room("Welcome to room 7.");
            room room8 = new room("Welcome to room 8.");
            room room9 = new room("Welcome to room 9.");
            room room10 = new room("Welcome to room 9.");
            room room11 = new room("Welcome to room 9.");

            room1.attach("north", room2);
            room2.attach("east", room3);
            room2.attach("west", room4);
            room3.attach("north", room5);
            room4.attach("west", room6);
            room6.attach("north", room7);
            room7.attach("west", room8);
            room7.attach("north", room9);
            room9.attach("east", room10);
            room10.attach("east", room11);

            room1.west = wall;
            room1.south = wall;
            room1.east = wall;
            room2.north = wall;
            room4.north = wall;
            room4.south = wall;
            room6.south = wall;
            room6.west = wall;
            room7.east = wall;
            room8.north = wall;
            room8.south = wall;
            room8.west = wall;
            room9.north = wall;
            room9.west = wall;
            room10.north = wall;
            room10.south = wall;
            room11.north = wall;
            room11.south = wall;
            room11.east = wall;
            room3.south = wall;
            room3.east = wall;
            room5.east = wall;
            room5.north = wall;
            room5.west = wall;
            room2.items.Add(new potato());
            player.currentRoom = room1;

            reco.SetInputToDefaultAudioDevice();

            setDefaultCommands();
            SetSpeechReco(defaultCommands.ToArray());

            /*type("Thanks for playing my game!");
            type("To play, you need a microphone hooked up to your computer and set as the default input device.");
            type("If you plug in an input device, please restart the game.");
            type("The speech recognition is pretty accurate, but it is slow and can only process one command at a time.");
            type("Please wait until you see one part of a command before advancing with the rest of the command.");
            type("An example of a command is: Go -> North -> Submit.");
            type("The engine recognize several synonyms of each word also.");
            type("There is also a cancel command if your command is not what you want (only usable where submit can be used).");*/

            Console.WriteLine("");
            type("What is your name? (Type this out)");
            do
            {
                player.name = Console.ReadLine();
                if (player.name == null || player.name == "")
                {
                    Console.WriteLine("Please write a name.");

                } else if (player.name.Contains(",") || player.name.Contains("`"))
                {
                    Console.WriteLine("The characters '`' and ',' are reserved. Please try a different name.");
                } else if (player.name.Length > 15)
                {
                    Console.WriteLine("Please keep your name to 15 characters or less.");
                }
            } while (player.name == null || player.name.Contains(",") || player.name.Contains("`") || player.name == "" || player.name.Length > 15);


            player.currentRoom.enter(player);
            reco.RecognizeAsync();

            while (gameIsRunning)
            {}

        }
        static void restartRecognition(object sender,System.Speech.Recognition.RecognizeCompletedEventArgs e)
        {
            reco.RecognizeAsync();
        }
        static void handleRecognition(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            Console.Write(e.Result.Text + " ");

            currentCommand += e.Result.Text + ",";

            if (go.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(go.nextWordsString());
            }
            else if (north.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(north.nextWordsString());
            }
            else if (south.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(south.nextWordsString());
            }
            else if (east.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(east.nextWordsString());
            }
            else if (west.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(west.nextWordsString());
            }
            else if (activate.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(player.getItemNames());
            }
            else if (grab.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(player.currentRoom.getItemNames());
            }
            else if (drop.checkForEquiv(e.Result.Text))
            {
                SetSpeechReco(player.getItemNames());
            }
            else if (player.checkIfItem(e.Result.Text))
            {
                string[] wordsForReco = new string[submit.wordsReturn().Length + cancel.wordsReturn().Length];
                submit.wordsReturn().CopyTo(wordsForReco, 0);
                cancel.wordsReturn().CopyTo(wordsForReco, submit.wordsReturn().Length);
                SetSpeechReco(wordsForReco);
            }
            else if (player.currentRoom.checkIfItem(e.Result.Text))
            {
                string[] wordsForReco = new string[submit.wordsReturn().Length + cancel.wordsReturn().Length];
                submit.wordsReturn().CopyTo(wordsForReco, 0);
                cancel.wordsReturn().CopyTo(wordsForReco, submit.wordsReturn().Length);
                SetSpeechReco(wordsForReco);
            }
            else if (submit.checkForEquiv(e.Result.Text)||cancel.checkForEquiv(e.Result.Text))
            {
                processCommand();
            }
        }
    }
}
