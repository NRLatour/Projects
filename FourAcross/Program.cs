using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourAcross
{
    class Program
    {

        const int ROWS = 7;
        const int COLS = 8;
        const int MAXTURNS = ROWS * COLS;

        static void Main(string[] args)
        {
            //This is the grid
            string[,] grid = new string[ROWS, COLS];

            //Player 1 determines if it is player 1's turn or not
            bool player1 = true;

            // valid will determine the end of the validation loops
            bool valid;

            //This will determine if there was a winner
            bool win;

            //This will determine the end of the playing session
            bool endGame;

            //colchoice will be the column that the player puts their X or O in
            int colChoice = 0;

            //k will determine if there was a tie by running out of places to put 
            int k = 0;

            //This will get the user input to avoid errors
            string input;

            // These are the inputs for the two players
            //char x = 'X', o = 'O';

            //These will keep the scores
            int p1Score, p2Score, numTie;

            //These initialize the scores
            p1Score = p2Score = numTie = 0;

            //Display a greeting message
            Console.WriteLine("Welcome to 4 Across.\n\nThis is a 2 player game, please decide who will be Player 1 as the Xs and who will be Player 2 as the Os.\n");

            do
            {

                //Display the grid for the first time
                Console.WriteLine("    1   2   3   4   5   6   7   8 ");
                for (int i = ROWS - 1; i >= 0; i--)
                {
                    Console.Write((i + 1) + ":| ");
                    for (int j = 0; j < COLS; j++)
                    {
                        grid[i, j] = " ";
                        Console.Write(grid[i, j] + " | ");
                    }
                    Console.WriteLine();
                }

                //This will be the bool to determine if someone won
                win = false;

                //Start the game
                for (int i = 0; i < MAXTURNS; i++)
                {
                    //Initialize as false to start the while loops
                    valid = false;

                    //Start player1 turn
                    if (player1)
                    {
                        //This will print in Red for player 1
                        Console.ForegroundColor = ConsoleColor.Red;

                        while (!valid)
                        {
                            //Get user input
                            Console.Write("Player 1: Please choose a column to put your X in: ");
                            input = Console.ReadLine();

                            //Validate user input
                            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input != "6" && input != "7" && input != "8")
                            {
                                Console.Write("Player 1: Please choose a column number between 1 and 8 to put your X in: ");
                                input = Console.ReadLine();
                            }

                            colChoice = Int32.Parse(input) - 1;

                            //Check if the column has space
                            for (k = 0; k < ROWS; k++)
                            {
                                if (grid[k, colChoice] == " ")
                                {
                                    grid[k, colChoice] = "X";
                                    valid = true;
                                    break;
                                }
                            }

                            //Display error if there was no space in the column
                            if (k == ROWS)
                            {
                                Console.WriteLine("That column is full...");
                                valid = false;
                            }
                        }

                        //This will reset the color display
                        Console.ResetColor();

                    }//End of Player 1 turn
                    //Start player 2 turn
                    else
                    {

                        // This will make Player 2 display in Green
                        Console.ForegroundColor = ConsoleColor.Green;
                        while (!valid)
                        {
                            //Get user input
                            Console.Write("Player 2: Please choose a column to put your O in: ");
                            input = Console.ReadLine();

                            //Validate user input
                            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input != "6" && input != "7" && input != "8")
                            {
                                Console.Write("Player 2: Please choose a column number between 1 and 8 to put your O in: ");
                                input = Console.ReadLine();
                            }

                            colChoice = Int32.Parse(input) - 1;

                            //Check if the column has space
                            for (k = 0; k < ROWS; k++)
                            {
                                if (grid[k, colChoice] == " ")
                                {
                                    grid[k, colChoice] = "O";
                                    valid = true;
                                    break;
                                }
                            }

                            //Display error if there was no space in the column
                            if (k == ROWS)
                            {
                                Console.WriteLine("That column is full...");
                                valid = false;
                            }
                        }

                        //This will reset the color display
                        Console.ResetColor();

                    }//End of Player2 turn

                    Console.Clear();

                    //Display the grid
                    Console.WriteLine("    1   2   3   4   5   6   7   8 ");
                    for (int l = ROWS - 1; l >= 0; l--)
                    {
                        Console.Write((l + 1) + ":| ");
                        for (int j = 0; j < COLS; j++)
                        {
                            if(grid[l, j] == "X")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(grid[l, j]);
                                Console.ResetColor();                                
                            }
                            else if (grid[l, j] == "O")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(grid[l, j]);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write(grid[l, j]);   
                            }

                            Console.Write(" | ");
                        }
                        Console.WriteLine();
                    }

                    // Check if someone won
                    if (i >= 6 && victory(grid, colChoice, k))
                    {
                        win = true;
                        break;
                    }

                    //Changes player
                    player1 = !player1;
                }

                //Display if there was a winner, and which one if there was
                if (!win)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("The Game Is A TIE!");
                    Console.ResetColor();
                    numTie++;
                }
                else if (player1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Player 1 WINS!");
                    Console.ResetColor();
                    p1Score++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Player 2 WINS!");
                    Console.ResetColor();
                    p2Score++;
                }


                //This asks the player if they want to play again
                Console.Write("\nWould you like to play again? yes or no? ");
                input = Console.ReadLine().ToUpper();

                //This validates the user input
                while (input != "YES" && input != "NO")
                {
                    Console.Write("Please enter either \"yes\" or \"no\": ");
                    input = Console.ReadLine().ToUpper();
                }

                //Determines whether or not to restart the game
                if (input == "YES")
                {
                    endGame = true;
                    Console.Clear();
                }
                else
                {
                    endGame = false;
                }

            } while (endGame);

            //Display the score at the end
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nPlayer 1 has won " + p1Score + " times.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Player 2 has won " + p2Score + " times.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("There have been " + numTie + " ties.\n");
            Console.ResetColor();

            //Display farewell message
            Console.WriteLine("\nThank you for playing the 4 Across game, I hope you had fun!\n");
        }

        public static bool victory(string[,] arr, int col, int row)
        {
            bool win = false;

            int col1 = col + 1, col2 = col + 2, col3 = col + 3;
            int row1 = row + 1, row2 = row + 2, row3 = row + 3;
            int colm1 = col - 1, colm2 = col - 2, colm3 = col - 3;
            int rowm1 = row - 1, rowm2 = row - 2, rowm3 = row - 3;

            /*
            This is a list of all of the short versions of variables
            string main = arr[row, col];
            string l3 = arr[row, colm3],      l2 = arr[row, colm2],   l1 = arr[row, colm1];
            string d1 = arr[rowm1, col],      d2 = arr[rowm2, col],   d3 = arr[rowm3, col];
            string r1 = arr[row, col1],       r2 = arr[row, col2],    r3 = arr[row, col3];
            string ul3 = arr[row3, colm3],    ul2 = arr[row2, colm2], ul1 = arr[row1, colm1];
            string ur1 = arr[row1, col1],     ur2 = arr[row2, col2],  ur3 = arr[row3, col3];
            string dl3 = arr[rowm3, colm3],   dl2 = arr[rowm2, colm2], dl1 = arr[rowm1, colm1];
            string dr1 = arr[rowm1, col1],    dr2 = arr[rowm2, col2], dr3 = arr[rowm3, col3];
            */

            switch (col)
            {
                case 4:
                case 3:
                case 2:
                case 1:
                case 0:
                    //These are shorter versions of the variiables for the functions 
                    string main = arr[row, col];
                    string r1 = arr[row, col1], r2 = arr[row, col2], r3 = arr[row, col3];

                    //Check if the row wins
                    if (rowRight(main, r1, r2, r3))
                    {
                        win = true;
                        break;
                    }

                    //Check if the diagonal up right wins
                    if (row < 4)
                    {
                        string ur1 = arr[row1, col1], ur2 = arr[row2, col2], ur3 = arr[row3, col3];

                        if (upRight(main, ur1, ur2, ur3))
                        {
                            win = true;
                            break;
                        }
                    }
                    //Check if the column wins
                    if (row > 2)
                    {
                        string d1 = arr[rowm1, col], d2 = arr[rowm2, col], d3 = arr[rowm3, col];

                        if (colCheck(main, d1, d2, d3))
                        {
                            win = true;
                            break;
                        }
                    }

                    //Check if diagonal down right wins
                    if (row > 2)
                    {
                        string dr1 = arr[rowm1, col1], dr2 = arr[rowm2, col2], dr3 = arr[rowm3, col3];

                        if (downRight(main, dr1, dr2, dr3))
                        {
                            win = true;
                            break;
                        }
                    }
                    break;

                default:
                    break;
            }

            if (!win)
                switch (col)
                {
                    case 5:
                    case 4:
                    case 3:
                    case 2:
                    case 1:
                        //These are shorter versions of the variiables for the functions 
                        string main = arr[row, col];
                        string l1 = arr[row, colm1];
                        string r1 = arr[row, col1], r2 = arr[row, col2];

                        //Check if mid diagonal down right wins
                        if (row > 1 && row < 6)
                        {
                            string ul1 = arr[row1, colm1], dr1 = arr[rowm1, col1], dr2 = arr[rowm2, col2];

                            if (midDownRight(main, dr1, dr2, ul1))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if diagonal down right wins
                        if (row > 1 && row < 4)
                        {
                            string ur1 = arr[row1, col1], ur2 = arr[row2, col2], dl1 = arr[rowm1, colm1];

                            if (midUpRight(main, ur1, ur2, dl1))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if middle left wins
                        if (midRight(main, r1, r2, l1))
                        {
                            win = true;
                            break;
                        }
                        break;

                    default:
                        break;
                }

            if (!win)
                switch (col)
                {
                    case 6:
                    case 5:
                    case 4:
                    case 3:
                    case 2:
                        //These are shorter versions of the variiables for the functions 
                        string main = arr[row, col];
                        string l2 = arr[row, colm2], l1 = arr[row, colm1]; ;
                        string r1 = arr[row, col1];

                        //Check if middle diagonal up left wins
                        if (row > 0 && row < 4)
                        {
                            string dr1 = arr[rowm1, col1], ul2 = arr[row2, colm2], ul1 = arr[row1, colm1]; ;
                            if (midUpLeft(main, ul1, ul2, dr1))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if middle diagonal down left wins
                        if (row > 1 && row < 6)
                        {
                            string ur1 = arr[row1, col1], dl2 = arr[rowm2, colm2], dl1 = arr[rowm1, colm1];

                            if (midDownLeft(main, dl1, dl2, ur1))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if middle left wins
                        if (midLeft(main, r1, l1, l2))
                        {
                            win = true;
                            break;
                        }
                        break;

                    default:
                        break;

                }

            if (!win)
                switch (col)
                {
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3:
                        //These are shorter versions of the variiables for the functions 
                        string main = arr[row, col];
                        string l3 = arr[row, colm3], l2 = arr[row, colm2], l1 = arr[row, colm1];

                        //Check if diagonal down left wins
                        if (row > 2)
                        {

                            string dl3 = arr[rowm3, colm3], dl2 = arr[rowm2, colm2], dl1 = arr[rowm1, colm1];

                            if (downLeft(main, dl1, dl2, dl3))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if diagonal up left wins
                        if (row < 4)
                        {
                            string ul3 = arr[row3, colm3], ul2 = arr[row2, colm2], ul1 = arr[row1, colm1];

                            if (upLeft(main, ul1, ul2, ul3))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if the column wins
                        if (row > 2)
                        {
                            string d1 = arr[rowm1, col], d2 = arr[rowm2, col], d3 = arr[rowm3, col];

                            if (colCheck(main, d1, d2, d3))
                            {
                                win = true;
                                break;
                            }
                        }

                        //Check if the row wins
                        if (rowLeft(main, l1, l2, l3))
                        {
                            win = true;
                            break;
                        }
                        break;

                    default:
                        break;
                }

            return win;
        }

        //Start at the bottom left going up to the right
        public static bool upRight(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the up left going down to the right
        public static bool downRight(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the bottom right going up to the left
        public static bool upLeft(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the middle right going up to the left
        public static bool midUpLeft(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the middle right going down to the left
        public static bool midDownLeft(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the top right going down to the left
        public static bool downLeft(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from middle left bottom going up right
        public static bool midUpRight(string arr, string input, string arr2, string arr3)
        {
            if (input == arr && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from middle left top going down right
        public static bool midDownRight(string arr, string input, string arr2, string arr3)
        {
            if (input == arr && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the top down 3
        public static bool colCheck(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the left going to the right
        public static bool rowRight(string input, string arr1, string arr2, string arr3)
        {
            if (input == arr1 && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from middle left going right
        public static bool midRight(string arr, string input, string arr2, string arr3)
        {
            if (input == arr && input == arr2 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the middle right going left
        public static bool midLeft(string arr, string arr1, string input, string arr3)
        {
            if (input == arr && input == arr1 && input == arr3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Start from the right going left
        public static bool rowLeft(string arr, string arr1, string arr2, string input)
        {
            if (input == arr && input == arr1 && input == arr2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}