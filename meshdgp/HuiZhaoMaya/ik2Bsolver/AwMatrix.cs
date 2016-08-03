// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


using Autodesk.Maya.OpenMaya;

namespace MayaNetPlugin
{
    class AwMatrix
    {
        public double[,] matrix = new double[4,4];

        public AwMatrix()
        {
            matrix[0,1] = matrix[0,2] = matrix[0,3] =
            matrix[1,0] = matrix[1,2] = matrix[1,3] =
            matrix[2,0] = matrix[2,1] = matrix[2,3] =
            matrix[3,0] = matrix[3,1] = matrix[3,2] = 0.0;
            matrix[0,0] = matrix[1,1] = matrix[2,2] = matrix[3,3] = 1.0;
        }

        public void setMatrix( MMatrix m)
        {
            matrix[0, 0] = m[0,0];
            matrix[0, 1] = m[0,1];
            matrix[0, 2] = m[0,2];
            matrix[0, 3] = m[0,3];
            matrix[1, 0] = m[1,0];
            matrix[1, 1] = m[1,1];
            matrix[1, 2] = m[1,2];
            matrix[1, 3] = m[1,3];
            matrix[2, 0] = m[2,0];
            matrix[2, 1] = m[2,1];
            matrix[2, 2] = m[2,2];
            matrix[2, 3] = m[2,3];
            matrix[3, 0] = m[3,0];
            matrix[3, 1] = m[3,1];
            matrix[3, 2] = m[3,2];
            matrix[3, 3] = m[3,3];
        }

        public void setToIdentity()
        {
            matrix[0,0] = 1.0;
            matrix[0,1] = 0.0;
            matrix[0,2] = 0.0;
            matrix[0,3] = 0.0;
            matrix[1,0] = 0.0;
            matrix[1,1] = 1.0;
            matrix[1,2] = 0.0;
            matrix[1,3] = 0.0;
            matrix[2,0] = 0.0;
            matrix[2,1] = 0.0;
            matrix[2,2] = 1.0;
            matrix[2,3] = 0.0;
            matrix[3,0] = 0.0;
            matrix[3,1] = 0.0;
            matrix[3,2] = 0.0;
            matrix[3,3] = 1.0;
        }
        public double getIndex(uint i, uint j)
        {
            return matrix[i, j];
        }
    }
}
