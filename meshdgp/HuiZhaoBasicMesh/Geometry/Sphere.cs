using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    [Serializable]
    public struct Sphere
    {
        // Summary:
        //     Gets or sets the sphere's center.
        public Vector3D Center { get; set; }
        //
        // Summary:
        //     Gets or sets the sphere's radius.
        public float Radius { get; set; }

        //public Sphere(Vector3D center, float radius)
        //{
        //    this.Center = center;
        //    this.Radius = radius;
        //}
    }
}
