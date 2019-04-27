using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

//This static class is just to provide methods to output colored text to the console.

namespace EquationParsing
{
    public static class ColorText
    {
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

        public static void BlueWriteLine(string s){
            ForegroundColor = ConsoleColor.Blue;
            WriteLine(s);
            ResetColor();
        }

        public static void BlueWrite(string s){
            ForegroundColor = ConsoleColor.Blue;
            Write(s);
            ResetColor();
        }

        public static void YellowWriteLine(string s){
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(s);
            ResetColor();
        }

        public static void YellowWrite(string s){
            ForegroundColor = ConsoleColor.Yellow;
            Write(s);
            ResetColor();
        }

        public static void CyanWriteLine(string s){
            ForegroundColor = ConsoleColor.Cyan;
            WriteLine(s);
            ResetColor();
        }

        public static void CyanWrite(string s){
            ForegroundColor = ConsoleColor.Cyan;
            Write(s);
            ResetColor();
        }
    }
}
