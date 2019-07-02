using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCC_2_TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new TicTacToe();
            game.Greeting();
        }
    }

    class TicTacToe
    {
        string playLoad = null;                     //This variable takes the user input for playing a new game or loading an existing game
        string savedGame = null;                    //Variable is used when user enters a saved game code
        int location;                               //This variable is the user input for moves
        bool isItXturn = true;                      //Boolean used to switch turns between O player and X player, X always starts the game
        int moves = 0;                              //Keep track of the amount of moves made
        string checkUp = null;                      //Accept the user input and put it through validation to make sure it is good
        bool playerVsAI = false;                    //Bool to track if the AI is playing as well
        string pvp = null;                          //variable to track user preference on playing against AI or not

        List<int> arrayX = new List<int> { };       //Locations of X in the game, used to validate loaded games
        List<int> arrayO = new List<int> { };       //Locations of Y in the game

        //Storage of our pieces
        IDictionary<int, string> boardPieces = new Dictionary<int, string>();       //Dictionary to save key value pairs
        string[] locators = new string[10];                                         //Locators used to track user input
        List<string> spaces = new List<string> { };                                 //Starts with numbers 1 - 9, goes down as spaces are taken so AI can randomize spots

        //Welcome to the game
        public void Greeting()
        {
            Console.WriteLine("Lets play a game of TicTacToe!");
            Console.WriteLine();
            Start();
        }

        //The Start method provides the first user interaction and sets up game conditions
        public void Start()
        {
            Console.Write("What would you like to do, 'Play' a new game or 'Load' a saved game?: "); 
            playLoad = Console.ReadLine();

            if (playLoad == "Play")
            {
                Console.Write("Player vs Computer = 'PvC' or Player vs Player = 'PvP': ");
                pvp = Console.ReadLine();
                if (pvp == "PvC")
                {
                    playerVsAI = true;
                    Console.WriteLine("You will now play against the computer, good luck!");
                    FreshBoard();
                    NewGame();
                }
                else if (pvp == "PvP")
                {
                    FreshBoard();
                    NewGame();
                }
                else
                {
                    Console.WriteLine("Invalid command, plase try again");
                    Console.WriteLine();
                    Start();
                }
            }
            else if (playLoad == "Load")
            {
                Console.Write("Enter your save code!: ");
                savedGame = Console.ReadLine();
                Load(savedGame);
            }
            else if (playLoad == "Clear")
            {
                Clear();
                Console.WriteLine("All cached values have been cleared");
                Start();
            }
            else if (playLoad == "End")
            {
                Console.WriteLine("Good Bye!");
                Exit();
            }
            else
            {
                Console.WriteLine("Invalid command, please enter 'Play', or 'Load' to begin");
                Start();
            }
        }

        //This method Loads a game based on user input array. it also keeps tab of how many X and O are in code and sends it to validate
        public void Load(string input)
        {
            char[] savedGameCode = input.ToCharArray();
            FreshBoard();

            for (int i = 0; i < savedGameCode.Length; i++)
            {
                if (savedGameCode[i] == 'O')
                {
                    boardPieces.Add(i + 1, "O");
                    isItXturn = true;
                    arrayO.Add(i + 1);
                    ReduceList(i + 1);
                    moves++;
                }
                else if (savedGameCode[i] == 'X')
                {
                    boardPieces.Add(i + 1, "X");
                    isItXturn = false;
                    arrayX.Add(i + 1);
                    ReduceList(i + 1);
                    moves++;
                }
                else if (savedGameCode[i] == '0')
                {
                    continue;
                }
                else if (savedGameCode[i] == 'T')
                {
                    playerVsAI = true;
                    pvp = "PvC";
                }
            }
            SavedGameValidation();
        }

        //This method validates that the user code has an equal amount of Xs and Os. If the code fails the user is asked to try again
        public void SavedGameValidation()
        {
            int hello = arrayX.Count() - arrayO.Count();

            if (savedGame.Length == 10)
            {
                if (hello == 1 || hello == -1 || hello == 0)
                {
                    UpdateGameBoard();
                    RunGame();
                }
            }
            else
            {
                Console.WriteLine("Sorry, this code is not valid");
                Clear();
                Start();
            }
        }

        //This method clears all cached values
        public void Clear()
        {
            boardPieces.Clear();
            arrayO.Clear();
            arrayX.Clear();
            moves = 0;

            for (int i = 0; i < locators.Count(); i++)
            {
                locators[i] = null;
            }
        }

        //Freshboard digits are prepared
        public void FreshBoard()
        {
            for (int i = 0; i < locators.Length; i++)
            {
                spaces.Add(Convert.ToString(i));
                locators[i] = Convert.ToString(i);
            }
        }

        //NewGame board is set up, spaces are called out
        public void NewGame()
        {
            Console.WriteLine();
            Console.WriteLine("These are the board locations");
            Draw(locators);
            RunGame();
        }

        string token = null;

        //This method switches between X or O tokens and calls either user input or computer input
        public void RunGame()
        {
            while (moves < 9)
            {
                if (isItXturn == true)
                {
                    token = "X";
                    UserInput();
                }
                else
                {
                    token = "O";
                    if (playerVsAI)
                    {
                        AIinput();
                    }
                    else
                    {
                        UserInput();
                    }
                }
            }
        }

        //User input method. Here we keep track of what the user entered and make sure only valid user input is accepted
        public void UserInput()
        {
            Console.Write("\r Enter a location for {0}: ", token);
            checkUp = Console.ReadLine();
            if (checkUp == "End")
            {
                Console.WriteLine("Good Bye!");
                Exit();
            }
            else if (checkUp == "Save")
            {
                SaveGame();
            }
            else
            {
                bool didUserEnterANumber = Int32.TryParse(checkUp, out location);
                if( didUserEnterANumber == false)
                {
                    Console.WriteLine("Please enter a number between 1 and 9. You can save by saying 'Save'");
                    RunGame();
                }
                else if (location > 9 || location < 1)
                {
                    Console.WriteLine("Please enter a number between 1 and 9. You can save by saying 'Save'");
                    RunGame();
                }
                else 
                {
                    ReduceList(location);
                    KeyValidation();
                    TokenMove(location);
                }
            }
        }

        //Remove existing token locations in the space
        public void ReduceList(int input)
        {
            if (spaces.Contains(Convert.ToString(input)))
            {
                int itemToRemove = spaces.IndexOf(Convert.ToString(input));
                spaces.RemoveAt(itemToRemove);
            }
        }

        //AI input if user decides to play against AI
        public void AIinput()
        {
            LVL1AI();

            KeyValidation();
            spaces.Remove(spaces[0]);
            Console.WriteLine("It is the Computer's turn");
            TokenMove(location);
        }

        public void LVL1AI()
        {
            for (int i = 0; i < spaces.Count; i++)
            {
                Random random = new Random();
                int randomSpot = random.Next(0, spaces.Count);
                string firstSpot = spaces[i];
                string secondSpot = spaces[randomSpot];
                spaces[i] = secondSpot;
                spaces[randomSpot] = firstSpot;
                location = Convert.ToInt32(spaces[0]);
            }
        }

        //public void LVL2AI()
        //{
        //    if (arrayX.Contains(1))
        //    {
        //        if (arrayX.Contains(2) && spaces.Contains("3"))
        //        {
        //            location = 3;
        //        }
        //        else if (arrayX.Contains(3))
        //        {
        //            //play location 2 if the location is available
        //        }
        //        else if (arrayX.Contains(5))
        //        {
        //            //play location 9 if the location is available
        //        }
        //        else if (arrayX.Contains(9))
        //        {
        //            //play location 5 if the location is available
        //        }
        //        else if (arrayX.Contains(4))
        //        {
        //            //play location 7 if the location is available
        //        }
        //        else if (arrayX.Contains(7))
        //        {
        //            //play location 4 if the location is available
        //        }
        //    }
        //}

        //Is the space occupied?
        public void KeyValidation()
        {
            if (boardPieces.ContainsKey(location) == true)
            {
                if (playerVsAI && token == "O")
                {
                    RunGame();
                }
                else
                {
                    Console.WriteLine("Invalid move, enter an empty spot");
                    RunGame();
                }
            }
        }

        //This method takes the location number and validates it then adds it to 
        public void TokenMove(int location)
        {
            boardPieces.Add(location, token);
            UpdateGameBoard();
            moves++;

            if (token == "X")
            {
                arrayX.Add(location);
                Win(arrayX, token);
                isItXturn = false;
            }
            else
            {
                arrayO.Add(location);
                Win(arrayO, token);
                isItXturn = true;
            }
        }
        
        //This method makes an array of saved keys which is given to the draw method to update the board
        public void UpdateGameBoard()
        {
            for (var i = 0; i < locators.Length; i++)
            {
                if (boardPieces.ContainsKey(i))
                {
                    locators[i] = boardPieces[i]; //if the key exists in the dictionary enter it into the array
                }
                else
                {
                    locators[i] = " "; //fill the space with blanks if the key does not exist
                }
            }
            Draw(locators); //send the array to the draw method
        }

        //This method saves the game for the user and ends the round
        public void SaveGame()
        {
            string saveCode = null;
            for (int i = 0; i < locators.Length; i++)
            {
                if (locators[i] == " ")
                {
                    locators[i] = "0";
                }
            }

            for (int i = 1; i < locators.Length; i++)
            {
                saveCode += locators[i];
            }

            if (playerVsAI == true)
            {
                saveCode += "T";
            }
            else
            {
                saveCode += "F";
            }

            Console.WriteLine("Your save code is: {0}, Hope to see you again soon!", saveCode);
            Clear();
            Exit();
        }

        //This method draws the board based on the array. It draws the columns individually and the rows as one (initially used for resizing the grid. Feautre disabled to allow playing in version 3)
        public void Draw(string[] values)
        {
            int columns = 3;
            int repeat = 0;
            int dashedLineRow = 0;
            int finalCount = 0;
            int spaces = 1;

            do
            {
                do
                {
                    Console.Write(" {0} ", values[spaces]);
                    repeat++;
                    spaces++;

                    if (repeat == columns)
                    {
                        break;
                    }
                    else if (repeat < columns)
                    {
                        Console.Write("|");
                    }
                }
                while (repeat < columns);
                repeat = 0;
                Console.WriteLine();
                
                dashedLineRow++;
                finalCount++;

                if (dashedLineRow < 3)
                {
                    Console.Write("---+---+---");
                    repeat = 0;
                    Console.WriteLine();
                }
            }
            while (finalCount < 3);
            Console.WriteLine();
        }

        

        //This method sets the basic winning conditions. Future addition: predict who is going to win by move 4, predit a draw by move 7
        public void Win(List<int> Local, string token)
        {
            
            if (Local.Contains(1) && Local.Contains(2) && Local.Contains(3))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(4) && Local.Contains(5) && Local.Contains(6))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(7) && Local.Contains(8) && Local.Contains(9))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(1) && Local.Contains(4) && Local.Contains(7))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(2) && Local.Contains(5) && Local.Contains(8))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(3) && Local.Contains(6) && Local.Contains(9))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(1) && Local.Contains(5) && Local.Contains(9))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (Local.Contains(3) && Local.Contains(5) && Local.Contains(7))
            {
                Console.WriteLine("Congrats player {0}, you win!", token);
                Exit();
            }
            else if (moves == 9)
            {
                Console.WriteLine("Looks like tt is a draw!!, play again!");
            }
        }

        //End the game
        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}