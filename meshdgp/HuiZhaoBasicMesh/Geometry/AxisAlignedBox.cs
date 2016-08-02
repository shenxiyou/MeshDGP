using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    [Serializable]
    public struct AxisAlignedBox
    {
        // Summary:
        //     Gets or sets the maximum point which is the box's maximum X and Y coordinates.
        public Vector3D Max { get; set; }
        //
        // Summary:
        //     Gets or sets the minimum point which is the box's minimum X and Y coordinates.
        public Vector3D Min { get; set; }

        //
        // Summary:
        //     Initializes a new instance of the Sharp3D.Math.Geometry3D.AxisAlignedBox
        //     class using given minimum and maximum points.
        //
        // Parameters:
        //   min:
        //     A Sharp3D.Math.Core.Vector3F instance representing the minimum point.
        //
        //   max:
        //     A Sharp3D.Math.Core.Vector3F instance representing the maximum point.
        //public AxisAlignedBox(Vector3D min, Vector3D max)
        //{
        //    Max = max;
        //    Min = min;
        //}

    }
}
