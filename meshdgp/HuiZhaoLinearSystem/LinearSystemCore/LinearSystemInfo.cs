using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class LinearSystemInfo
    {
        private EnumSolver sloverType = EnumSolver.SuperLULU;

        public EnumSolver SolverType
        {
            get
            {
                return sloverType;
            }
            set
            {
                sloverType = value;
            }
        }


        private bool matlab = false;

        public bool Matlab
        {
            get
            {
                return matlab;
            }
            set
            {
                matlab=value ;
            } 
        }

        
    }
}
