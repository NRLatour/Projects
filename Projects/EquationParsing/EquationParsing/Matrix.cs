using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Due Date:            2017, October 27th
 * Software Designer:   Nicolas Latour #1168942
 * Course:              420-306-AB Algorithm Design (Fall 2017)
 * Deliverable:         Assignment #4 --- Matrix
 * 
 * Description:         This class is used to create and manipulate matrices in the 
                        form of 2d arrays of doubles. There are methods to perform
                        matrix multiplication, scalar multiplications, inverse, additions,
                        and transpose, as well as a ToString override.
 */

namespace EquationParsing{
    public class Matrix{
        //Matrix properties
        private int rows;
        private int cols;
        private double [,] matrix;

        static Random rand = new Random(); //used for randomly generating values for the matrices

        //Read-Only property
        public int Row{
            get{
                return this.rows;
            }
        }

        //Read-Only property
        public int Col{
            get{
                return this.cols;
            }
        }

        /// <summary>
        /// Name: Constructor
        /// Purpose: It is the regular constructor for the Matrix class
        /// Variables: matrix: 2d array of doubles holding the values
        /// Paramenters: row size and column size for the 2d array
        /// Returns: a Matrix with the 3 parameters entered
        /// Exceptions: if the row or column size is 0 or lower, an exception is thrown
        /// </summary>
        /// <param name="rowsize"></param>
        /// <param name="colsize"></param>
        //This constructor creates the array for the matrix and call the initialization
        public Matrix(int rowsize, int colsize){
            if (rowsize <= 0 || colsize <= 0)
                throw new MatrixException("You can't have 0 or lower for a row size for a matrix");            
            
            this.matrix = new double[rowsize, colsize];
            this.rows = rowsize;
            this.cols = colsize;
            InitializeMatrix();            
        }

        //This constructor copies the values of another matrix into this
        public Matrix(Matrix m)
        {
            this.matrix = new double[m.Row, m.Col];
            this.rows = m.Row;
            this.cols = m.Col;
            CopyMatrix(m);
        }

        //Copies the values from another matrix into this one
        public void CopyMatrix(Matrix m)
        {
            for (int i = 0; i < this.rows; i++){
                for (int j = 0; j < this.cols; j++){
                    this.matrix[i, j] = m.GetCell(i, j);
                }
            }
        }


        //Initialize matrix with all 0
        public void InitializeMatrix(){
            for (int i = 0; i < this.rows; i++){
                for (int j = 0; j < this.cols; j++){
                    this.matrix[i, j] = 0;
                }
            }
        }

        //Randomly fill matrix
        public void RandomFill(){
            for (int i = 0; i < this.rows; i++){
                for (int j = 0; j < this.cols; j++){
                    this.matrix[i, j] = rand.NextDouble() * 10;
                }
            }
        }

        //Returns the value of the cell at the sent coordinates
        public double GetCell(int row, int col){
            if (row < 0 || col < 0 || row > this.Row - 1 || col > this.Col - 1)
                throw new MatrixException("Row or Column choice invalid for getting a matrix cells data");

            return this.matrix[row, col];
        }

        //Set value for a cell at the sent coordinates
        public void SetCell(int row, int col, double value){
            if (row < 0 || col < 0 || row > this.Row - 1 || col > this.Col - 1)
                throw new MatrixException("Row or Column choice invalid for assigning a matrix cells value");

            this.matrix[row, col] = value;
        }

        /// <summary>
        /// Name: Scalar Multiplier
        /// Purpose: will return a matrix that is a scalar multiplication of the matrix calling the method with the scalar parameter 
        /// Variables: scalarMult: matrix holding the scalar multiplication of the matrix calling the method with the scalar parameter 
        /// Paramenters: rhs is a double to multiply the matrix by
        /// Returns: a Matrix
        /// Exceptions: none
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public Matrix Mult(double rhs){
            Matrix scalarMult = new Matrix(rows, cols);

            for (int i = 0; i < this.rows; i++){
                for (int j = 0; j < this.cols; j++){
                    scalarMult.SetCell(i, j, this.matrix[i, j] * rhs);
                }
            }

            return scalarMult;
        }

        /// <summary>
        /// Name: Matrix Multiplier
        /// Purpose: will return a matrix that is a Matrix multiplication of the matrix calling the method with the rhs matrix parameter
        /// Variables: matrixMult: it is a Matrix multiplication of the matrix calling the method with the rhs matrix parameter
        /// Paramenters: rhs is a double to multiply the matrix by
        /// Returns: a Matrix
        /// Exceptions: If the column size of the calling matrix is not equal to the row of the parameter matrix it will throw an exception
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public Matrix Mult(Matrix rhs){
            if (this.cols != rhs.Row) //You can only multiply matrices if the first matrix column size is equal to the second's row size
                throw new MatrixException("Invalid matrix multiplication size");
            

            Matrix matrixMult = new Matrix(rows, rhs.Col);
            int size = this.cols; //A matrix multiplication will have a number of additions as the column size of the left hand side matrix
            double value; //This will work as an accumulator to get the multiplication total


            for (int i = 0; i < matrixMult.Row; i++){
                for (int j = 0; j < matrixMult.Col; j++){
                    value = 0;
                    for (int k = 0; k < size; k++){ //this will get the different values for the matrix multiplication
                        value += (this.matrix[i,k] * rhs.GetCell(k,j));
                    }
                    matrixMult.SetCell(i, j, value);
                }
            }

            return matrixMult;
        }

        /// <summary>
        /// Name: Matrix Addition
        /// Purpose: will return a matrix that is a Matrix addition of the matrix calling the method with the rhs matrix parameter
        /// Variables: matrixAddition: it is a Matrix addition of the matrix calling the method with the rhs matrix parameter
        /// Paramenters: rhs is a matrix to add with the matrix to get the matrixAddition
        /// Returns: a Matrix
        /// Exceptions: If the dimensions of the two matrices are not equivalent, an exception is thrown
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        //Return a matrix created from the addition of this matrix with the matrix parameter 
        public Matrix Add(Matrix rhs){
            if (this.rows != rhs.Row || this.cols != rhs.Col)
                throw new MatrixException("To add matrices they must both have the same dimensions.");            

            Matrix matrixAddition = new Matrix(this.rows, this.cols);

            //Add each cell from the same position in both matrices and put the sum in the same coordinates or the addition matrix
            for (int i = 0; i < matrixAddition.Row; i++){
                for (int j = 0; j < matrixAddition.Row; j++){
                    matrixAddition.SetCell(i, j, this.matrix[i, j] + rhs.GetCell(i,j));
                }
            }

            return matrixAddition;
        }

        /// <summary>
        /// Name: Matrix Transpose
        /// Purpose: will return a matrix that is a Matrix that will hold the transpose of the matrix calling the method
        /// Variables: transpose: it is a Matrix that will hold the transpose of the matrix calling the method
        /// Paramenters: none
        /// Returns: Return a transpose of the matrix calling the method
        /// Exceptions: none
        /// /// </summary>
        /// <returns></returns>

        public Matrix Transpose(){
            Matrix transpose = new Matrix(this.cols, this.rows);

            //The transpose will switch the rows into columns from one matrix to the other
            for (int i = 0; i < transpose.Row; i++){
                for (int j = 0; j < transpose.Col; j++){
                    transpose.SetCell(i, j, this.matrix[j, i]);
                }
            }

            return transpose;
        }

        /// <summary>
        /// Name: Matrix Inverse
        /// Purpose: Return an inverse of the matrix calling the method
        /// Variables:  inverse: it is a Matrix that will hold the inverse of the matrix calling the method
        ///             These will be used for calculating the cell values of the Inverse matrix:
        ///             a: will hold the value of cell [0,0] 
        ///             b: will hold the value of cell [0,1]
        ///             c: will hold the value of cell [1,0]
        ///             d: will hold the value of cell [1,1]
        ///             determinant: this will hold the value of the determinant of the original matrix
        ///             inv_det: it is the determinant to the power of -1 (1/determinant)
        /// Paramenters: none
        /// Returns: Return a transpose of the matrix calling the method
        /// Exceptions: if the rows or columns don't equal 2, thow an exception
        ///             if the determinant is 0, throw an exception before counting the inverse of the determinant
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse(){
            if (this.rows != 2 || this.cols != 2)
                throw new Exception("This inversion can only be done with a 2x2 matrix.");
            
            Matrix inverse = new Matrix(this.rows, this.cols);
            double a = this.matrix[0, 0];
            double b = this.matrix[0, 1];
            double c = this.matrix[1, 0];
            double d = this.matrix[1, 1];            
            double determinant = (a * d - b * c);

            if (determinant == 0)
                throw new MatrixException("There is no inverse to this matrix because the determinant is 0");

            double inv_det = 1 / determinant; //get the inverse of the determinant (1 / determinant) 

            inverse.SetCell(0, 0, d * inv_det);
            inverse.SetCell(0, 1, b * inv_det * -1);
            inverse.SetCell(1, 0, c * inv_det * -1);
            inverse.SetCell(1, 1, a * inv_det);

            return inverse;
        }

        /// <summary>
        /// Name: ToString
        /// Purpose: Return a string value to ouput the matrix to console
        /// Variables:  max: holds the maximum length of any string value in the matrix array
        ///             output: the string value of the entire matrix
        /// Paramenters: none
        /// Returns: Return a string value to output the matrix
        /// Exceptions: none
        /// </summary>
        /// <returns></returns>
        public override string ToString(){
            int max = maxLength(); //gets the maximum character length in the matrix to space it out in the output

            string output = ""; //initialize the string to be able to add to it

            for (int i = 0; i < this.rows; i++){
                output += "[ ";

                for (int j = 0; j < this.cols; j++){
                    for (int k = 0; k < max - this.matrix[i, j].ToString("N2").Length; k++){
                        output += " ";
                    }

                    output += this.matrix[i, j].ToString("N2") + " ";
                }
                output += "]";
                output += "\n"; //Start new row
            }

            return output;
        }

        //Returns the max character length in the matrix
        public int maxLength(){
            int max = 1;

            for (int i = 0; i < this.rows; i++){
                for (int j = 0; j < this.cols; j++){
                    max = (this.matrix[i, j].ToString("N2").Length > max) ? this.matrix[i, j].ToString("N2").Length : max;
                }
            }

            return max;
        }
    }
}
