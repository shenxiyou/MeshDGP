using System;
 
using System.IO;
using System.ComponentModel;
 
 

 

namespace GraphicResearchHuiZhao
{
    #region ToolsSettingRecord
    public class ToolsSettingRecord : BaseRecord
    {
        public enum EnumSelectingMethod { Rectangle, Point };

        private string name = "";
        private double rotationAngle = 5;
        private double scalingRatio = 1.1;
        private bool selectionLaser = true;
        private double depthTolerance = -0.0001;
        private EnumSelectingMethod selectionMethod = EnumSelectingMethod.Rectangle;
        private bool updateNLC = false;
        private int numberOfNLC = 3;
        private bool checking = false;
        private double searchingRadius = 0.1;

        [Category("Viewing Tool")]
        public double RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }
        [Category("Viewing Tool")]
        public double ScalingRatio
        {
            get { return scalingRatio; }
            set { if (value != 0) scalingRatio = value; }
        }


        [Category("Selection Tool")]
        public bool Laser
        {
            get { return selectionLaser; }
            set { selectionLaser = value; }
        }
        [Category("Selection Tool")]
        public double DepthTolerance
        {
            get { return depthTolerance; }
            set { depthTolerance = value; }
        }

        [Category("Selection Tool")]
        public EnumSelectingMethod SelectionMethod
        {
            get { return selectionMethod; }
            set { selectionMethod = value; }
        }

        [Category("Moving Tool")]
        public bool UpdateNLC
        {
            get { return updateNLC; }
            set { updateNLC = value; }
        }
        [Category("Moving Tool")]
        public int NumberOfNLC
        {
            get { return numberOfNLC; }
            set { numberOfNLC = value; }
        }
        [Category("Moving Tool")]
        public bool Checking
        {
            get { return checking; }
            set { checking = value; }
        }

        [Category("OB Adding Tool")]
        public double SearchingRadius
        {
            get { return searchingRadius; }
            set { searchingRadius = value; }
        }

        public ToolsSettingRecord(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
    #endregion
}
