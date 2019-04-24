using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

/* Due Date:            2017, December 8th
 * Software Designer:   Nicolas Latour #1168942
 * Course:              420-306-AB Algorithm Design (Fall 2017)
 * Deliverable:         Project --- Equation Parsing
 * 
 * Description:         This program will take in an equation in the form of ax+by=c from the user.
 *                      Then it will take the equation and parse it one character at a time to 
 *                      indentify the coefficient of the first variable, the character of the first
 *                      variable, the second coefficient, the character of the second variable and 
 *                      the answer for the line. The coefficients and answers are set in matrices
 *                      the variables are saved in variables and then it does the same for the second
 *                      equation. In the second equation, it will match the coefficients to the variables 
 *                      when placing them in the matrix. Finally, it will calculate the values of the 
 *                      variables by getting the inverse of the coefficients matrix and multiplying it 
 *                      with the answers matrix. It will output the coefficient matrix, the inverse 
 *                      coefficient matrix, the answers matrix, the variable values matrix and the final 
 *                      values of the variables. It will loop the program until the user enters exit.
 */

namespace EquationParsing{
    public class Program{
        const string EXIT = "exit";                                                                                     //The string that the user needs to write to exit the main program loop
        const int LINE1 = 0;
        const int LINE2 = 1;
        const int VAR1 = 0;
        const int VAR2 = 1;
        const char UNASSIGNED = ' ';

        static void Main(string[] args){
            string input1;                                                                                              //first line user input

            //Test();                                                                                                     //This will show all of the test scenarios

            //Get user input section
            Write("Enter the first equation (enter EXIT to end)\t-->  ");                                                                                                //Get the user to enter a string to parse
            input1 = ReadLine();
            
            while (input1 != EXIT){                                                                                     //loop until the user writes "exit"
                ParseEquations(input1);
                
                Write("Enter the first equation (enter EXIT to end)\t-->  ");                                                                                            //Get the user to enter a string to parse
                input1 = ReadLine();
            }

            WriteLine("Bye!");                                                                                          //farewell message
            ReadLine();
        }




        static void ParseEquations(string input1 = "", string line2 = ""){
            bool valid = true; 
            char var1 = UNASSIGNED, var2 = UNASSIGNED;                                                                  //blank space will be the default when it is unassigned
            Matrix coefficients;                                                                                        //this will hold the coefficients for the variables
            Matrix answers;                                                                                             //this will hold the answers for both equations
            Matrix inverse;                                                                                             //this will hold the inverse of the coefficients matrix
            Matrix varValues;                                                                                           //this will hold the values of the variables after solving for them
            string input2 = line2;                                                                                      //second line user input
                                            
            if(input1 != ""){
                WriteLine("Equation 1: " + input1 + "\n");
            }

            coefficients = new Matrix(2, 2);                                                                            //initialize three matrices
            coefficients = new Matrix(2, 2);                                                                            //initialize three matrices
            answers = new Matrix(2, 1);                                                                                 //initialize three matrices

            try{                                                                                                        //parse first equation
                Parsing.parseEquation(input1, coefficients, answers, ref var1, ref var2, true);
            }
            catch (ParseException e){
                WriteLine(e + "\n");
                valid = false;
            }

            if (valid){
                //Get second line and parse the variables
                if (input2 == ""){
                    Write("\nEnter the second equation \t-->  ");
                    input2 = ReadLine();
                }
                else
                    WriteLine("Equation 2: " + input2 + "\n");

                try{                                                                                                    //parse second equation
                    Parsing.parseEquation(input2, coefficients, answers, ref var1, ref var2, false);
                }
                catch (ParseException e){
                    WriteLine(e + "\n");
                    valid = false;
                }

                if (valid) { 
                    WriteLine("\nThis is the coefficients matrix\n" + coefficients.ToString());                         //output the values of the coefficients

                    try{
                        inverse = coefficients.Inverse();                                                               //calculate the inverse of the coefficient matrix
                        WriteLine("This is the inverse of the coefficients matrix\n" + inverse.ToString());

                        WriteLine("This is the answers matrix\n" + answers.ToString());

                        varValues = inverse.Mult(answers);                                                              //find matrix X by doing coefficients.inverse * answers matrices
                        WriteLine("This is the solution of the multiplication of the inverse coefficients matrix and the answers matrix\n" + varValues.ToString());

                        WriteLine(var1 + " = " + varValues.GetCell(VAR1, 0));
                        WriteLine(var2 + " = " + varValues.GetCell(VAR2, 0));
                    }
                    catch (Exception e){
                        WriteLine(e + "\nThe equations could not be calculated\n");
                    }
                }
            }

            WriteLine("\nPress Enter to continue...");
            ReadLine();

            Clear();
        }

        static void Test(){
            string input1;                                                                                              //first line user input
            string input2;                                                                                              //second line user input
            
            //Test 1    Success
            WriteLine("Test 1, expected result: success\n");
            input1 = "3x+2y=5";
            input2 = "-3x+2y=5.5";
            ParseEquations(input1, input2);

            //Test 2    Success
            WriteLine("Test 2, expected result: success\n");
            input1 = "+3x+2y=5";
            input2 = "-3x+2y=5.5";
            ParseEquations(input1, input2);

            //Test 3   Success
            WriteLine("Test 3, expected result: success\n");
            input1 = "-3y=2";
            input2 = "x-y=15";
            ParseEquations(input1, input2);

            //Test 4  Failed
            WriteLine("Test 4, expected result: failure from too many variables, invalid equation\n");                  
            input1 = "3x+2y+2x=6";
            input2 = "3.0E-1x+2.0e-2y=5.33E+1";
            ParseEquations(input1);

            //Test 5    Success
            WriteLine("Test 5, expected result: success\n");
            input1 = "3x+2y=5";
            input2 = "3y+2x=5";
            ParseEquations(input1, input2);

            //Test 6    Failed                                                                                            //only 1 input entered because it is expected to fail
            WriteLine("Test 6, expected result: failure from invalid syntax\n");                            
            input1 = "x2+3y=15";
            ParseEquations(input1);

            //Test 7    Failed                                                                                            //only 1 input entered because it is expected to fail
            WriteLine("Test 7, expected result: failure from invalid syntax\n");
            input1 = "xy+2y=12";
            ParseEquations(input1);

            //Test 8    Failed                                                                                            //only 1 input entered because it is expected to fail
            WriteLine("Test 8, expected result: failure from too many variables\n");
            input1 = "3a+2b=5";
            input2 = "3c+2a=5";
            ParseEquations(input1, input2);

            //Test 9    Success
            WriteLine("Test 9, expected result: success\n");
            input1 = "0.123e2y-13e-1x=1.11e1";
            input2 = "3.0E-1x+2.0e-2y=5.33E+1";
            ParseEquations(input1, input2);

            //Test 10   Failed
            WriteLine("Test 10, expected result: failure invalid variable in line 2\n");
            input1 = "0.123e2y-13e-1x=1.11e1";
            input2 = "3.0E-1x+2.0e-2a=5.33E+1";
            ParseEquations(input1, input2);
        }
    }
}