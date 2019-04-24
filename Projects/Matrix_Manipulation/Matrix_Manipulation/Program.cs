using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

/*Due Date:             2017, October 27th
 * Software Designer:   Nicolas Latour #1168942
 * Course:              420-306-AB Algorithm Design (Fall 2017)
 * Deliverable:         Assignment #4 --- Matrix
 * 
 * Description:         This section is used to let the user choose which feature of the Matrix
                        class they want to test and then will show them the different ways that 
                        they work and the different ways that they will not work and throw 
                        exceptions.
 */

namespace Matrix_Manipulation{
    class Program{
        enum choices
        {
            Quit,
            Contructor,
            Set,
            Get,
            Scalar_Multiplication,
            Matrix_Multiplication,
            Transpose,
            Matrix_Addition,
            Inverse
        }

        static void Main(string[] args){
            int selection;

            do {
                selection = Menu.GetSelection(Enum.GetNames(typeof(choices)), "Select one of the following situations to test (If the program seems to stop, press enter to continue to the next step)");

                Clear();

                switch ((choices)selection)
                {
                    case choices.Quit:
                        break;
                    case choices.Contructor:
                        BasicMatrix();
                        break;
                    case choices.Set:
                        SetTest();
                        break;
                    case choices.Get:
                        GetTest();
                        break;
                    case choices.Scalar_Multiplication:
                        ScalarMultTest();
                        break;
                    case choices.Matrix_Multiplication:
                        MatrixMultTest();
                        break;
                    case choices.Transpose:
                        TransposeTest();
                        break;
                    case choices.Matrix_Addition:
                        MatrixAddTest();
                        break;
                    case choices.Inverse:
                        MatrixInverseTest();
                        break;
                    default:
                        break;
                }
            } while (selection != (int)choices.Quit);
            
            WriteLine("End of Matrix Demonstrations");
            ReadLine();

            
        }
                
        static void BasicMatrix(){
            WriteLine("This is the basic Matrix constructor test!\n");

            //Generate and output matrix
            Matrix original = new Matrix(2, 2);
            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());
            ReadLine();

            //test RandomFill and output
            original.RandomFill();
            WriteLine("Randomly Fill Original Matrix:\n" + original.ToString());

            Menu.RedWriteLine("\nIf you try to create a Matrix with row or column sizes equal to or lower than 0, it will throw an exception");
            ReadLine();
        }

        static void SetTest(){
            WriteLine("This is the Set test!\n");

            //Generate and output matrix
            Matrix original = new Matrix(2, 2);
            original.RandomFill();
            int row, col; //these are the coordinates of the cell that will be selected 
            double value; //This will be the value the user wants to set the selected cell to

            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString() + "\nPress enter to continue to the selection section");
            ReadLine();
            getCellPosition(original, out row, out col, "Please select a cell (press enter) to change its value");

            Write("\nYou chose the cell ({0},{1}), now, what would you like to change its value to: ", row, col);
            while (!double.TryParse(ReadLine(), out value)){
                Write("Incorrect input! Please enter a numberical value: ");
            }

            original.SetCell(row, col, value);

            WriteLine("\nThis is the Matrix with the changed value:\n" + original.ToString());

            Menu.RedWriteLine("\nIf you try to call the Set method with a negative column or row size, it will throw an exception");

            ReadLine();
        }

        static void getCellPosition(Matrix m, out int row, out int col, string question)
        {
            int max = m.maxLength(); //gets the maximum character length in the matrix to space it out in the output
            
            //these are the coordinates of the cell that is currently selected 
            row = 0;
            col = 0;    
            
            bool selected = false; //This will end the menu

            CursorVisible = false; //This will make the cursor invisible until the end of the selection process

            while (!selected)
            { //continue until the user has pressed enter to select an index
                Clear();

                WriteLine(question);

                for (int i = 0; i < m.Row; i++)
                {
                    Write("[ ");

                    for (int j = 0; j < m.Col; j++)
                    {
                        for (int k = 0; k < max - m.GetCell(i, j).ToString().Length; k++)
                        { //output spaces to have a uniform size
                            Write(" ");
                        }

                        if (i == row && j == col) //output the selected index as green to show that it is selected
                            Menu.GreenWrite(m.GetCell(i, j).ToString("N2") + " ");
                        else
                            Write(m.GetCell(i, j).ToString("N2") + " ");
                    }
                    Write("]");
                    if (i + 1 < m.Row) //Start new row if it is not the end of the matrix
                        WriteLine();
                }

                WriteLine("\nYou are currently selecting the cell ({0},{1})", row, col);

                changeIndex(m, ref row, ref col, ref selected);
            }
            CursorVisible = true; //make the cursor visible again for the rest of the program
        }

        static void changeIndex(Matrix m, ref int row, ref int col, ref bool selected){
            //This will be used for looping around the text from above or below
            int upper_limit = m.Row - 1;
            int lower_limit = 0;
            int right_limit = m.Col - 1;
            int left_limit = 0;

            
            ConsoleKeyInfo key_pressed = ReadKey();

            if (key_pressed.Key == ConsoleKey.Enter)
            { //select an option and signal the end of the loop
                selected = true;
            }
            else if (key_pressed.Key == ConsoleKey.UpArrow)
            { //move up the selection or circle to the bottom
                if (row <= lower_limit)
                {
                    row = upper_limit;
                }
                else
                {
                    row--;
                }
            }
            else if (key_pressed.Key == ConsoleKey.DownArrow)
            { //move down the selected index or circle to the top
                if (row >= upper_limit)
                {
                    row = lower_limit;
                }
                else
                {
                    row++;
                }
            }
            else if (key_pressed.Key == ConsoleKey.RightArrow)
            { //move right the selected index or circle to the left
                if (col >= right_limit)
                {
                    col = left_limit;
                }
                else
                {
                    col++;
                }
            }
            else if (key_pressed.Key == ConsoleKey.LeftArrow)
            { //move left the selected index or circle to the right
                if (col <= left_limit)
                {
                    col = right_limit;
                }
                else
                {
                    col--;
                }
            }
        }

        static void GetTest(){
            WriteLine("This is the Get test!\n");

            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();

            int row, col; //these are the coordinates of the cell that will be selected 

            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString() + "\nPress enter to continue to the selection section");
            ReadLine();
            getCellPosition(original, out row, out col, "Please select a cell (press enter) to get its value");

            WriteLine("\nThe value in cell ({0},{1}) is {2}.", row, col, original.GetCell(row, col).ToString("N2"));

            Menu.RedWriteLine("\nIf you try to call the Get method with a negative column or row size, it will throw an exception");
            ReadLine();
        }
        
        static void ScalarMultTest(){

            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();

            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());

            //Scalar Multiplication and output
            Matrix scaMult = original.Mult(4);
            WriteLine("Scalar multiple of Original with 4:\n" + scaMult.ToString());
            ReadLine();
        }

        static void MatrixMultTest(){
            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();
            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());

            Matrix other = new Matrix(2, 2);
            other.RandomFill();
            WriteLine("\nThis is another 2 by 2 Matrix:\n" + original.ToString());

            //Matrix Multiplication and output
            Matrix matMult = original.Mult(other);
            WriteLine("\nThis is a Matrix Multiplication of the two matrices:\n" + matMult.ToString());

            WriteLine("=================================================================");

            //This section is creating a matrix that is not compatible for matrix multiplication
            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());
            Matrix InvalidMult = new Matrix(3, 2);
            InvalidMult.RandomFill();
            WriteLine("\nThis is a 3 by 2 Matrix:\n" + InvalidMult.ToString());
            
            Menu.RedWriteLine("If you try to multiply these two matrices, because the col size of the first is not the same as the row size of the second one, it will throw an exception");
            
            ReadLine();
        }
        
        static void TransposeTest(){
            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();
            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());

            //Test Transpose
            Matrix transpose = original.Transpose();
            WriteLine("Transpose of Original:\n" + transpose.ToString());
            ReadLine();
        }
        
        static void MatrixAddTest(){
            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();
            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());

            //Generate another matrix and fill with random values
            Matrix other = new Matrix(2, 2);
            other.RandomFill();
            WriteLine("This is a 2 by 2 Matrix:\n" + other.ToString());
            
            Matrix addition = original.Add(other);
            
            WriteLine("\nThis the addition of the two matrices:\n" + addition.ToString());

            WriteLine("=================================================================");

            //Create an invalid matrix addition
            WriteLine("\nThis is a 2 by 2 Matrix:\n" + original.ToString());
            Matrix InvalidMult = new Matrix(3, 2);
            InvalidMult.RandomFill();
            WriteLine("\nThis is a 3 by 2 Matrix:\n" + InvalidMult.ToString());

            Menu.RedWriteLine("If you try to add these two matrices, because the sizes are not the same, it will throw an exception");
            
            ReadLine();
        }

        static void MatrixInverseTest(){
            //Generate matrix and fill with random values
            Matrix original = new Matrix(2, 2);
            original.RandomFill();

            WriteLine("This is a 2 by 2 Matrix:\n" + original.ToString());

            //Create inverse from original
            Matrix inverse = original.Inverse();
            WriteLine("Inverse Matrix of Original:\n" + inverse.ToString());

            WriteLine("=================================================================");

            Matrix InvalidSizeInv = new Matrix(3, 2);
            InvalidSizeInv.RandomFill();
            WriteLine("This is a 3 by 2 Matrix:\n" + InvalidSizeInv.ToString());
            Menu.RedWriteLine("If you try to get the inverse of this matrix, because it is not a 2x2 matrix, it will throw an exception\n");

            WriteLine("=================================================================");

            int value;
            Matrix InvalidDetInv = new Matrix(2, 2);
            for (int i = 0; i < InvalidDetInv.Row; i++){
                value = i + 1;
                for (int j = 0; j < InvalidDetInv.Col; j++){
                    InvalidDetInv.SetCell(i, j, value);
                    value += value;
                }
            }
            WriteLine("This is a 2 by 2 Matrix:\n" + InvalidDetInv.ToString());
            Menu.RedWriteLine("If you try to get the inverse of this matrix, because its determinant is 0, it will throw an exception\n");
            
            ReadLine();
        }
    }
}
