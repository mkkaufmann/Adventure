/*
 * Michael Kaufmann
 * Started 7/2/17
 * Text-based Adventure Game with speech recognition
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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;

namespace ConsoleApp2
{
    class Program
    {
        static System.Speech.Recognition.SpeechRecognitionEngine reco = new System.Speech.Recognition.SpeechRecognitionEngine();
        static void setSpeechReco(string[] words)
        {
            reco.UnloadAllGrammars();

            System.Speech.Recognition.SrgsGrammar.SrgsDocument gram = new System.Speech.Recognition.SrgsGrammar.SrgsDocument();
            System.Speech.Recognition.SrgsGrammar.SrgsRule rule = new System.Speech.Recognition.SrgsGrammar.SrgsRule("CurrentCommands");
            System.Speech.Recognition.SrgsGrammar.SrgsOneOf list = new System.Speech.Recognition.SrgsGrammar.SrgsOneOf(words);

            rule.Add(list);
            gram.Rules.Add(rule);
            gram.Root = rule;

            reco.LoadGrammar(new System.Speech.Recognition.Grammar(gram));
        }

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

        
        
        static string currentCommand = "";

        static player player = new player();

        static List<string> defaultCommands = new List<string>();

        static void setDefaultCommands()
        {
            defaultCommands.Clear();
            foreach(string word in go.wordsReturn())
            {
                defaultCommands.Add(word);
            }
            if(player.currentRoom.items.Count() > 0)
            {
                foreach(string word in grab.wordsReturn())
                {
                    defaultCommands.Add(word);
                }
            }
            if (player.inventory.Count() > 0)
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
        static void processCommand()
        {
            string[] splitCommand = currentCommand.Split(",".ToCharArray());

            if (splitCommand.Contains("cancel"))
            {
                setSpeechReco(defaultCommands.ToArray());
                currentCommand = "";
                Console.WriteLine("");
                return;
            }
            string commandPart = splitCommand[0];
            if (go.checkForEquiv(commandPart))
            {
                commandPart = splitCommand[1];
                if (north.checkForEquiv(commandPart))
                {
                    commandPart = splitCommand[2];
                    if (submit.checkForEquiv(commandPart))
                    {
                        player.currentRoom.north.enter();
                        if (!(player.currentRoom.north.isNull))
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
                        player.currentRoom.south.enter();
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
                        player.currentRoom.east.enter();
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
                        player.currentRoom.west.enter();
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
            setDefaultCommands();
            setSpeechReco(defaultCommands.ToArray());
            currentCommand = "";
        }
        static void type(string text)
        {
            typer typer = new typer(text);
            Task currentTask = Task.Factory.StartNew(() => { typer.start(); });
            currentTask.Wait();
        }
        static void Main(string[] args)
        {
            Leaderboard leaderboard = new Leaderboard();

            reco.RecognizeCompleted += new EventHandler<System.Speech.Recognition.RecognizeCompletedEventArgs>(restartRecognition);
            reco.SpeechRecognized += new EventHandler<System.Speech.Recognition.SpeechRecognizedEventArgs>(handleRecognition);

            wall wall = new wall();
            room room1 = new room("Welcome to room 1. There is some help you can pick up for later. Try 'grab help submit.'");
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
            room1.items.Add(help);

            player.currentRoom = room1;

            reco.SetInputToDefaultAudioDevice();

            setDefaultCommands();
            setSpeechReco(defaultCommands.ToArray());

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


            player.currentRoom.enter();

            reco.RecognizeAsync();

            while (Console.ReadKey().Key != ConsoleKey.Enter)
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
                setSpeechReco(go.nextWordsString());
            }
            else if (north.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(north.nextWordsString());
            }
            else if (south.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(south.nextWordsString());
            }
            else if (east.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(east.nextWordsString());
            }
            else if (west.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(west.nextWordsString());
            }
            else if (activate.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(player.getItemNames());
            }
            else if (grab.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(player.currentRoom.getItemNames());
            }
            else if (drop.checkForEquiv(e.Result.Text))
            {
                setSpeechReco(player.getItemNames());
            }
            else if (player.checkIfItem(e.Result.Text))
            {
                string[] wordsForReco = new string[submit.wordsReturn().Length + cancel.wordsReturn().Length];
                submit.wordsReturn().CopyTo(wordsForReco, 0);
                cancel.wordsReturn().CopyTo(wordsForReco, submit.wordsReturn().Length);
                setSpeechReco(wordsForReco);
            }
            else if (player.currentRoom.checkIfItem(e.Result.Text))
            {
                string[] wordsForReco = new string[submit.wordsReturn().Length + cancel.wordsReturn().Length];
                submit.wordsReturn().CopyTo(wordsForReco, 0);
                cancel.wordsReturn().CopyTo(wordsForReco, submit.wordsReturn().Length);
                setSpeechReco(wordsForReco);
            }
            else if (submit.checkForEquiv(e.Result.Text)||cancel.checkForEquiv(e.Result.Text))
            {
                processCommand();
            }
        }
    }
}
