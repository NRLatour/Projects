using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationParsing
{
    public static class Parsing{
        public enum state{  //These are the possible states for the parsing method
            Start,          //this is the starting point for the whole parse
            Zero,           //this will be for when a zero is first entered
            Sign,           //this will be for when a "+" or "-" is entered
            Number,         //this will be for when a number is entered
            Dot,            //this will be for when a "." is entered
            Decimal,        //this will be for the decimals that are entered
            Exponent,       //this will be for when an "e" is entered
            ExpZero,        //this will be for when a 0 is enter for the exponent
            ExpNumber,      //this will be for the exponent number
            ExpSign         //this will be for when the "+" or "-" is entered for the exponent value
        }

        const int LINE1 = 0;
        const int LINE2 = 1;
        const int VAR1 = 0;
        const int VAR2 = 1;
        const char UNASSIGNED = ' ';

        /// <summary>
        ///This will take in one line, the two matrices to enter the values in, the variable characters and if it is the first line or not.
        ///it will parse it to get the one or two coefficients and the answer from the input
        /// </summary>
        /// <param name="input">This is the string that will be parsed to get</param>
        /// <param name="coefficients">this matrix holds the values of the coefficients for the variables from both equations</param>
        /// <param name="answers">this matrix holds the answers from both equations</param>
        /// <param name="var1">this is the value of the first variable (it is ' ' if it has not been assigned)</param>
        /// <param name="var2">this is the value of the second variable (it is ' ' if it has not been assigned)</param>
        /// <param name="firstLine">this will determine if it is the first line to be parsed</param>
        /// <returns>This only passes values by reference, there is no return</returns>
        public static void parseEquation(string input, Matrix coefficients, Matrix answers, ref char var1, ref char var2, bool firstLine){
            int index = 0;
            double temp;
            int line = (firstLine)? LINE1: LINE2;                                                               //this will determine which lines values are being parsed
            char nextVar;                                                                                       //this will hold the other variable that has not been assigned on 
            int nextVarIndex;
            
            temp = Parsing.parseCoefficient(input, ref index);                                              //parse the string for the first coefficient

            if (firstLine){                                                                                 //this means that var1 has not been assigned yet
                #region Save first variable and coefficient
                coefficients.SetCell(line, VAR1, temp);                                                     //assign the value for the first coefficient
                var1 = Char.ToLower(input[index]);                                                          //get the variable at the index after the coefficient
                #endregion

                index++;                                                                                    //increment to check next character

                #region Check second part of the equation for variable or answer
                switch (input[index]){                                                                      //is it the end of the equation or is there a second variable
                    #region go to answer
                    case '=':                                                                               //no second coefficient, go straight to answer
                        coefficients.SetCell(line, VAR2, 0);                                                //assign 0 to second variable
                        index++;                                                                            //start the index after the equals sign for parsing
                        answers.SetCell(line, 0, Parsing.parseDouble(input, ref index));                    //get the answer to the equation
                        break;
                    #endregion
                    #region save second coefficient and variable and answer
                    case '+':                                                                               //there is a second variable
                    case '-':
                        #region save second coefficient and variable
                        temp = Parsing.parseCoefficient(input, ref index);                                  //get second variable coefficient

                        if (input[index] == var1)                                                           //if the second variable is the same as the first throw an exception
                            throw new ParseException("Invalid characters detected, double use of the same variable!");

                        coefficients.SetCell(line, VAR2, temp);

                        var2 = Char.ToLower(input[index]);                                                  //store the second variable
                        index++;                                                                            //increment to check next character

                        if (input[index] != '=')
                            throw new ParseException("Invalid characters detected at index " + index + ". Must be \"=\"");
                        else
                            index++;                                                                        //increment to check next character
                        #endregion
                        #region save answer
                        answers.SetCell(line, 0, Parsing.parseDouble(input, ref index));                    //get answer from the end of the input
                        #endregion
                        break;
                    default:
                        throw new ParseException("Invalid characters detected at index " + index);
                        #endregion
                }
                #endregion
            }
            else{                                                                                           //this is the second line
                #region check variable to assign coefficient
                if (input[index] == var1){                                                                  //assign to the coefficient corresponsing to the variable
                    coefficients.SetCell(line, VAR1, temp);                                                 //assign the value for the first coefficient
                    nextVar = Char.ToLower(var2);
                    nextVarIndex = VAR2;
                }
                else if (input[index] == var2){
                    coefficients.SetCell(line, VAR2, temp);                                                 //assign the value for the second coefficient
                    nextVar = Char.ToLower(var1);
                    nextVarIndex = VAR1;
                }
                else if (var2 == UNASSIGNED){
                    var2 = input[index];                                                                    //assign the previously empty var2
                    coefficients.SetCell(line, VAR2, temp);
                    nextVar = Char.ToLower(var1);
                    nextVarIndex = VAR1;
                }
                else
                    throw new ParseException("Invalid variable detected at line: " + line + ", index: " + index);
                #endregion

                index++;                                                                                    //increment to check next character   

                #region Check second part of the equation
                switch (input[index]){                                                                      //is it the end of the equation or is there a second variable
                    #region go to answer
                    case '=':                                                                               //no second coefficient, go straight to answer
                        coefficients.SetCell(line, nextVarIndex, 0);                                        //assign 0 to second variable
                        index++;                                                                            //start the index after the equals sign for parsing
                        answers.SetCell(line, 0, Parsing.parseDouble(input, ref index));                    //get the answer to the equation
                        break;
                    #endregion
                    #region save second coefficient and variable and answer
                    case '+':                                                                               //there is a second variable
                    case '-':
                        #region save second coefficient and variable
                        coefficients.SetCell(line, nextVarIndex, Parsing.parseCoefficient(input, ref index)); //get second variable coefficient

                        if (input[index] != nextVar)                                                        //if the second variable is the same as the first throw an exception
                            throw new ParseException("Invalid characters detected, invalid second variable");

                        index++;                                                                            //increment to check next character

                        if (input[index] != '=')                                                            //it needs to be an equal sign or is does not work
                            throw new ParseException("Invalid characters detected at index " + index + ". Must be \"=\"");
                        else
                            index++;                                                                        //increment to check next character
                        #endregion
                        #region save answer
                        answers.SetCell(line, 0, Parsing.parseDouble(input, ref index));
                        #endregion
                        break;
                    default:
                        throw new ParseException("Invalid characters detected at index " + index);
                        #endregion
                }
                #endregion
            }
        }


        /// <summary>
        ///This will take in a string, try to parse it into a double value and return the value if successful. 
        ///endState will be true if it was successful and false if not.
        /// </summary>
        /// <param name="input">This is the string that will be parsed to get</param>
        /// <param name="index">This will be the index to start this iteration of the parse from</param>
        /// <returns>Returns the the parsed value found in the equation</returns>
        public static double parseDouble(string input, ref int index){

            char x;                         //this will hold the current character in the string to parse
            state current = state.Start;    //this will hold the current state for the switch case
            double number = 0;              //this will hold the final value to return at the end
            int sign = 1;                   //this will hold the sign of the whole number, default to positive
            int expSign = 1;                //this will hold the sign of the exponent number, default to positive
            bool endState = false;          //start as false and if it ends on a proper end state then change to true
            double decDivision = 10;        //decimals will be divided by this number to get their value
            int science = 0;                //this will hold the exponent value

            for (int i = index; i < input.Length; i++){                                 //this loop will go through the string parsing each character until it fails or ends
                x = input[i];                                                           //get the character to parse

                #region Parsing
                switch (current){                   //determine the state of the parse
                    #region Start
                    case state.Start:
                        switch (x){
                            case '+':
                            case '-':
                                if (x == '-')       //only change the value if it is negative
                                    sign = -1;
                                current = state.Sign;
                                endState = false;
                                break;
                            case '0':
                                number = 0;
                                current = state.Zero;
                                endState = true;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                number = x - '0';   //get the value of the character by subtracting the value of 0 from its character value
                                current = state.Number;
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot; //start the decimal parsing
                                endState = false;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Zero
                    case state.Zero:
                        switch (x){
                            case '.':
                                current = state.Dot;        //start the decimal parsing
                                endState = false;
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;   //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Sign
                    case state.Sign:
                        switch (x){
                            case '0':
                                number = 0;
                                current = state.Zero;
                                endState = true;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                number = x - '0';       //get the value of the character by subtracting the value of 0 from its character value
                                current = state.Number;
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot;    //start the decimal parsing
                                endState = false;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Number
                    case state.Number:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                number = number * 10 + x - '0';     //multiply the previous value by a power of 10 and add the new characters value
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot;                //start the decimal parsing
                                endState = false;
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;           //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Dot
                    case state.Dot:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                number = number + (x - '0') / decDivision;  //the value of the decimal numbers are divided by a power of 10 to give their actual value
                                current = state.Decimal;                    //continue parsing decimals
                                endState = true;
                                decDivision *= 10;                          //increment for the next decimal value (if any)
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Decimal
                    case state.Decimal:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                number = number + (x - '0') / decDivision;  //the value of the decimal numbers are divided by a power of 10 to give their actual value
                                endState = true;
                                decDivision *= 10;                          //increment for the next decimal value (if any)
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;                   //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Exponent
                    case state.Exponent:
                        switch (x){
                            case '+':
                            case '-':
                                if (x == '-')               //only change sign if it is negative, the default was positive
                                    expSign = -1;
                                current = state.ExpSign;
                                endState = false;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = x - '0';          //get the value of the character by subtracting the value of 0 from its character value
                                current = state.ExpNumber;
                                endState = true;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region ExpNumber
                    case state.ExpNumber:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = science * 10 + x - '0'; //multiply the previous value by a power of 10 and add the new characters value
                                endState = true;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region ExpSign
                    case state.ExpSign:
                        switch (x){
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = x - '0';          //get the value of the character by subtracting the value of 0 from its character value
                                current = state.ExpNumber;
                                endState = true;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    default:
                        throw new ParseException("Error invalid double\n");
                }
                #endregion
            }

            if (endState)                                                               //If the parsing ended on a valid end state, calculate the final value of the number
                number = sign * number * Math.Pow(10, expSign * science);
            else                                                                        //throw if it ended before being a valid end state
                throw new ParseException("Error invalid double\n");

            return number;                                                              //return parsed double value of the string parameter
        }


        /// <summary>
        ///This will take in a string, try to parse it into a double value, until it finds a variable and return the value if successful. 
        ///endState will be true if it was successful and false if not.
        /// </summary>
        /// <param name="input">This is the string that will be parsed to get</param>
        /// <param name="index">This will be the index to start this iteration of the parse from</param>
        /// <returns>Returns the the parsed value found in the equation</returns>
        public static double parseCoefficient(string input, ref int index){

            char x;                                                                     //this will hold the current character in the string to parse
            state current = state.Start;                                                //this will hold the current state for the switch case
            double coefficient = 0;                                                     //this will hold the final value to return at the end
            int sign = 1;                                                               //this will hold the sign of the whole number, default to positive
            int expSign = 1;                                                            //this will hold the sign of the exponent number, default to positive
            bool endState = false;                                                      //start as false and if it ends on a proper end state then change to true
            double decDivision = 10;                                                    //decimals will be divided by this number to get their value
            int science = 0;                                                            //this will hold the exponent value
            bool end = false;                                                           //end the parse when a variable is reached
            Regex variable = new Regex(@"^[a-df-z]$");                                  //can be any letter besides e

            while(index < input.Length && !end){                                        //this loop will go through the string parsing each character until it fails or ends
                x = input[index];                                                       //get the character to parse

                #region Parsing
                switch (current){                                                       //determine the state of the parse
                    #region Start
                    case state.Start:
                        switch (x){
                            case '+':
                            case '-':
                                if (x == '-')                                           //only change the value if it is negative
                                    sign = -1;
                                current = state.Sign;
                                endState = false;
                                break;
                            case '0':
                                coefficient = 0;
                                current = state.Zero;
                                endState = true;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                coefficient = x - '0';                                  //get the value of the character by subtracting the value of 0 from its character value
                                current = state.Number;
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot;                                    //start the decimal parsing
                                endState = false;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){      //if a variable is found
                                    coefficient = 1;
                                    end = true;
                                    endState = true;
                                    index--;                                            //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Zero
                    case state.Zero:
                        switch (x){
                            case '.':
                                current = state.Dot;                                    //start the decimal parsing
                                endState = false;
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;                               //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){      //if a variable is found
                                    end = true;
                                    index--;                                            //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Sign
                    case state.Sign:
                        switch (x){
                            case '0':
                                coefficient = 0;
                                current = state.Zero;
                                endState = true;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                coefficient = x - '0';                                  //get the value of the character by subtracting the value of 0 from its character value
                                current = state.Number;
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot;                                    //start the decimal parsing
                                endState = false;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){      //if a variable is found
                                    coefficient = 1;
                                    end = true;
                                    endState = true;
                                    index--;                                            //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Number
                    case state.Number:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                coefficient = coefficient * 10 + x - '0';           //multiply the previous value by a power of 10 and add the new characters value
                                endState = true;
                                break;
                            case '.':
                                current = state.Dot;                                //start the decimal parsing
                                endState = false;
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;                           //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){  //if a variable is found
                                    end = true;
                                    index--;                                        //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Dot
                    case state.Dot:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                coefficient = coefficient + (x - '0') / decDivision;    //the value of the decimal numbers are divided by a power of 10 to give their actual value
                                current = state.Decimal;                                //continue parsing decimals
                                endState = true;
                                decDivision *= 10;                                      //increment for the next decimal value (if any)
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Decimal
                    case state.Decimal:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                coefficient = coefficient + (x - '0') / decDivision; //the value of the decimal numbers are divided by a power of 10 to give their actual value
                                endState = true;
                                decDivision *= 10;                                  //increment for the next decimal value (if any)
                                break;
                            case 'E':
                            case 'e':
                                current = state.Exponent;                           //start the exponent parsing
                                endState = false;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){  //if a variable is found
                                    end = true;
                                    index--;                                        //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region Exponent
                    case state.Exponent:
                        switch (x){
                            case '+':
                            case '-':
                                if (x == '-')               //only change sign if it is negative, the default was positive
                                    expSign = -1;
                                current = state.ExpSign;
                                endState = false;
                                break;
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = x - '0';          //get the value of the character by subtracting the value of 0 from its character value
                                current = state.ExpNumber;
                                endState = true;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region ExpNumber
                    case state.ExpNumber:
                        switch (x){
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = science * 10 + x - '0';                   //multiply the previous value by a power of 10 and add the new characters value
                                endState = true;
                                break;
                            default:
                                if (variable.IsMatch(Char.ToLower(x).ToString())){  //if a variable is found
                                    end = true;
                                    index--;                                        //This will put the index to reevaluate the variable position after exiting the method
                                    break;
                                }
                                else
                                    throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    #region ExpSign
                    case state.ExpSign:
                        switch (x){
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                science = x - '0';          //get the value of the character by subtracting the value of 0 from its character value
                                current = state.ExpNumber;
                                endState = true;
                                break;
                            default:
                                throw new ParseException("Error invalid double\n");
                        }
                        break;
                    #endregion
                    default:
                        throw new ParseException("Error invalid double\n");
                }
                #endregion

                index++;
            }

            if (endState)                                                               //If the parsing ended on a valid end state, calculate the final value of the number
                coefficient = sign * coefficient * Math.Pow(10, expSign * science);
            else                                                                        //throw if it ended before being a valid end state
                throw new ParseException("Error invalid double\n");

            return coefficient;                                                         //return parsed double value of the string parameter
        }
    }
}
