using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsOfLinearEquations.Domain
{
    /// <summary>
    /// Constains static matrices operating methods for solving systems of linear equations.
    /// </summary>
    public static class SystemOfLinearEquations
    {
        /// <summary>
        /// Solves system of linear equations using Cramer's rule.
        /// </summary>
        /// <param name="matrixCoefficientsA">List-based representation of left hand side of SLE extended matrix. i.e. that matrix's main part concatenated rows.</param>
        /// <param name="matrixCoefficientsB">Representation of right hand side of SLE extended matrix.</param>
        public static List<Double> SolveSquareSystem(int dimension,
            List<double> coefficientsA, List<double> coefficientsB)
        {
            double matrixDeterminant = GetMatrixDeterminant(dimension, coefficientsA);
            List<double> solution = null; 
            if (matrixDeterminant != 0.0)
            {
                solution = new List<double>(dimension);
                for (int i = 0; i < dimension; i++)
                {
                    solution.Add(GetModifiedMatrixDeterminant(dimension, coefficientsA, i, coefficientsB) / matrixDeterminant);
                }
            }
            return solution;
        }

        /// <summary>
        /// Replaces specified column in matrix and calculates it's determinant.
        /// </summary>
        private static double GetModifiedMatrixDeterminant(int dimension, List<double> matrixCoefficients,
            int columnToReplaceId, List<double> replacementColumn)
        {
            List<double> modifiedMatrixCoefficients = matrixCoefficients.Select(c => c).ToList();
            for (int i = 0; i < dimension; i++)
            {
                modifiedMatrixCoefficients[dimension * i + columnToReplaceId] = replacementColumn[i];
            }
            return GetMatrixDeterminant(dimension, modifiedMatrixCoefficients);
        }

        #region Gaussian method's for calculating determinant auxiliary methods

        private static void AppendRowToMatrixRow(int dimension, List<double> matrixCoefficients,
            int matrixRowId, List<double> rowToAppendCoefficients)
        {
            for (int i = 0, matrixRowOffset = matrixRowId * dimension; i < dimension; i++)
            {
                matrixCoefficients[matrixRowOffset + i] += rowToAppendCoefficients[i];
            }
        }

        private static void ReplaceMatrixRow(int dimension, List<double> matrixCoefficients,
            int rowToReplaceId, List<double> replacementRowCoefficients)
        {
            for (int i = 0, rowOffset = rowToReplaceId * dimension; i < dimension; i++)
            {
                matrixCoefficients[rowOffset + i] = replacementRowCoefficients[i];
            }
        }

        private static List<double> GetMatrixRowMultipliedByNumber(int dimension,
                List<double> matrixCoefficients, int matrixRowId, double multiplier)
        {
            List<double> product = new List<double>(dimension);
            for (int i = matrixRowId * dimension; i < (matrixRowId + 1) * dimension; i++)
            {
                product.Add(matrixCoefficients[i] * multiplier);
            }
            return product;
        }

        private static double GetAbsMaxInRow(int dimension,
                List<double> matrixCoefficients, int matrixRowId)
        {
            double currentAbsMaximum = 0.0;
            for (int i = matrixRowId * dimension; i < (matrixRowId + 1) * dimension; i++)
            {
                if (Math.Abs(matrixCoefficients[i]) > Math.Abs(currentAbsMaximum))
                    currentAbsMaximum = matrixCoefficients[i];
            }
            return currentAbsMaximum;
        }

        /// <returns>Reducing coefficient.</returns>
        private static double ReduceMatrix(int dimension, List<double> matrixCoefficients)
        {
            double reducingCoefficient = 1.0;
            for (int i = 0; i < dimension; i++)
            {
                double rowAbsMaximum = GetAbsMaxInRow(dimension, matrixCoefficients, i);
                reducingCoefficient *= rowAbsMaximum;
                if (reducingCoefficient > 0)
                {
                    List<double> multipliedRow = GetMatrixRowMultipliedByNumber(dimension,
                            matrixCoefficients, i, Math.Pow(rowAbsMaximum, -1));
                    ReplaceMatrixRow(dimension, matrixCoefficients, i, multipliedRow);
                }
            }
            return reducingCoefficient;
        }

        /// <returns>First nonzero if it exists, zero otherwise.</returns>
        private static double GetFirstNonzeroInRow(int dimension, List<double> matrixCoefficients, int rowId)
        {
            double firstNonzero = 0;
            for (int i = rowId * dimension; i < (rowId + 1) * dimension; i++)
            {
                if (matrixCoefficients[i] != 0)
                {
                    firstNonzero = matrixCoefficients[i];
                    break;
                }
            }
            return firstNonzero;
        }

        private static void SwapRows(int dimension, List<double> matrixCoefficients, int firstRowId, int secondRowId)
        {
            int firstRowOffset = firstRowId * dimension;
            int secondRowOffset = secondRowId * dimension;
            for (int i = 0; i < dimension; i++)
            {
                double temp = matrixCoefficients[firstRowOffset + i];
                matrixCoefficients[firstRowOffset + i] = matrixCoefficients[secondRowOffset + i];
                matrixCoefficients[secondRowOffset + i] = temp;
            }
        }
        #endregion


        /// <summary>
        /// Calculates matrix determinant by Gaussian method.
        /// </summary>
        /// <param name="matrixCoefficients">List-based representation of matrix. i.e. concatenated matrix's rows.</param>
        public static double GetMatrixDeterminant(int dimension, List<double> matrixCoefficients)
        {
            matrixCoefficients = matrixCoefficients.Select(c => c).ToList();
            double determinant = 1.0;

            //determinant *= ReduceMatrix(dimension, matrixCoefficients);
            if (determinant == 0.0)
            {
                return determinant;
            }

            #region diagonal reduction

            for (int i = 0; i < dimension; i++)
            {
                if (matrixCoefficients[i * dimension + i] == 0.0)
                {
                    int j = i;

                    for (int k = i + 1; k < dimension; k++)
                    {
                        if (matrixCoefficients[k * dimension + i] != 0.0)
                        {
                            j = k;
                            break;
                        }
                    }

                    if (j == i)
                    {
                        continue;
                    }
                    else
                    {
                        SwapRows(dimension, matrixCoefficients, i, j);
                        determinant *= -1.0;
                    }
                }

                for (int j = i + 1; j < dimension; j++) // reducing nonzeros underneath
                {
                    if (matrixCoefficients[j * dimension + i] != 0)
                    {
                        List<double> multipliedRow = GetMatrixRowMultipliedByNumber(dimension, matrixCoefficients, i,
                                    -1 * matrixCoefficients[j * dimension + i]);
                        // Cheat: passing to the method just one row instead of whole matrix.
                        multipliedRow = GetMatrixRowMultipliedByNumber(dimension, multipliedRow, 0,
                            Math.Pow(matrixCoefficients[i * dimension + i], -1));
                        AppendRowToMatrixRow(dimension, matrixCoefficients, j, multipliedRow);
                    }
                }
            }

            #endregion

            #region Completing computing Determinant

            for (int i = 0; i < dimension; i++)
            {
                determinant *= matrixCoefficients[i * dimension + i];
            }

            #endregion


            return determinant;
        }

        /// <summary>
        /// Replaces specified matrix's column by new one.
        /// </summary>
        /// <param name="matrixCoefficients">List-based representation of matrix. i.e. concatenated matrix's rows.</param>
        public static List<double> ReplaceSquareMatrixColumn(int dimension, int columnId,
            List<double> matrixCoefficients, List<double> newColumn)
        {
            for (int i = 0; i < dimension; i++)
            {
                matrixCoefficients[i * dimension + columnId] = newColumn[i];
            }
            return matrixCoefficients;
        }
    }
}
