using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class CholmodInfo
    {
        public enum CholmodMatrixItemType
        {
            Real,
            Complex,
            Quaternion
        }

        public enum CholmodMatrixStorage
        {
            CRS,
            CCS,
            Triplet,
        }

        public enum CholmodMatrixStoageMethod
        {
            Normal,
            Divided,
        }

        public enum CholmodMatrixType
        {
            Dense,
            Sparse,
        }

        public double[] values = null;
        public double[] z = null;
        public int[] rowIndex = null;
        public int[] colIndex = null;

        public int nnz = 0;
        public int RowCount = 0;
        public int ColumnCount = 0;

        public CholmodMatrixType MatrixType;
        public CholmodMatrixStorage MatrixStorageType;
        public CholmodMatrixStoageMethod MatrixStorageMethod;
        public CholmodMatrixItemType MatrixItemType;

    }
}
