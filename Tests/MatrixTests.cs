using System;
using CoreLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void TwoMatrixMultiplication()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            Matrix m2 = new Matrix(new double[,] { { 7, 8 }, { 9, 10 }, { 11, 12 } });

            Matrix resultMatrix = m1*m2;

            Matrix targetMatrix = new Matrix(new double[,] { { 58, 64 }, { 139, 154 } });
            
            Assert.AreEqual(resultMatrix, targetMatrix);
        }

        [TestMethod]
        public void TwoMatrixAddition()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 3 }, { 1, 0}, { 1, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 0, 0 }, { 7, 5}, { 2, 1 } });

            Matrix resultMatrix = m1 + m2;

            Matrix targetMatrix = new Matrix(new double[,] { { 1, 3 }, { 8, 5 }, { 3, 3 } });

            Assert.AreEqual(resultMatrix, targetMatrix);
        }

        [TestMethod]
        public void TwoMatrixSubstraction()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 3 }, { 1, 0 }, { 1, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 0, 0 }, { 7, 5 }, { 2, 1 } });

            Matrix resultMatrix = m1 - m2;

            Matrix targetMatrix = new Matrix(new double[,] { { 1, 3 }, { -6, -5 }, { -1, 1 } });

            Assert.AreEqual(resultMatrix, targetMatrix);
        }

        [TestMethod]
        public void ScalarMatrixMultiplication()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            Matrix resultMatrix = 2.0d * m;

            Matrix targetMatrix = new Matrix(new double[,] { { 2, 4, 6 }, { 8, 10, 12 } });

            Assert.AreEqual(resultMatrix, targetMatrix);
        }
    }
}
