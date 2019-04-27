using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

/// <summary>
/// This class was created by Nicolas Latour as a base for menu selections
/// It allows you to send an array of choices, a question to ask and returns the
/// index of the selected.
/// This menu is designed to be operated with the up and down arrow key
/// </summary>

namespace Matrix_Manipulation
{
    public static class Menu
    {
        //This will give the user a menu of options with one colored to show the selected option
        //They navigate the menu with the up and down arrow keys and select with enter
        public static int GetSelection(string[] choices, string menu_question){
            int index = 0; //This will get the index of the option selected by the user
            bool selected = false; //This will end the menu

            //This will be used for looping around the text from above or below
            int upper_limit = choices.Length - 1;
            int lower_limit = 0;

            while (!selected){
                Clear();
                WriteLine(menu_question + ":\n");

                //This will output the options and color the currently selected one
                for (int i = 0; i < choices.Length; i++){
                    if (i == index)
                        GreenWriteLine(choices[i]);
                    else
                        WriteLine(choices[i]);
                }

                WriteLine();

                CursorVisible = false;
                ConsoleKeyInfo key_pressed = ReadKey();

                if (key_pressed.Key == ConsoleKey.Enter){ //select an option and signal the end of the loop
                    selected = true;
                }
                else if (key_pressed.Key == ConsoleKey.UpArrow){ //move up the selected index or circle to the bottom
                    if (index <= lower_limit)
                        index = upper_limit;
                    else
                        index--;
                }
                else if (key_pressed.Key == ConsoleKey.DownArrow){ //move down the selected index or circle to the top
                    if (index >= upper_limit)
                        index = lower_limit;
                    else
                        index++;
                }
            }

            return index;
        }

        public static void GreenWriteLine(string s){
            ForegroundColor = ConsoleColor.Green;
            WriteLine(s);
            ResetColor();
        }

        public static void GreenWrite(string s){
            ForegroundColor = ConsoleColor.Green;
            Write(s);
            ResetColor();
        }

        public static void RedWriteLine(string s){
            ForegroundColor = ConsoleColor.Red;
            WriteLine(s);
            ResetColor();
        }

        public static void RedWrite(string s){
            ForegroundColor = ConsoleColor.Red;
            Write(s);
            ResetColor();
        }
    }
}
