using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Design;

namespace GraphicResearchHuiZhao
{
    public static class FormAttributeTransform
    {
        static FormAttributeTransform()
        {
            TypeDescriptor.AddAttributes(typeof(Color), new[] { new EditorAttribute(typeof(ColorEditorEx), typeof(UITypeEditor)) });
 
            SettingTransform();
        }

         

 

        static void SettingTransform()
        {
            List<PropertyAdditionalAttributes> list = new List<PropertyAdditionalAttributes>();
            

          
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "RotationAxis",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-360, 360, 0.01)}
            });

            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "ModelTranslate",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-1, 1, 0.001)}
            });


            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "ModelRotation",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-360, 360, 0.1)}
            });


            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "ModelScale",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-2, 2, 0.001)}
            });

            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "ModelShearing",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-2, 2, 0.01)}
            });
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "ModelMirror",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(0, 3, 0.1)}
            });
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "EyePos",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.01)}
            });

            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "EyeDirection",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-360, 360, 0.1)}
            });


            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "Quaternion",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-360, 360, 0.1)}
            });
 
 
 
            

            PropertyAttributesTypeDescriptionProvider provider = new PropertyAttributesTypeDescriptionProvider(
                typeof(SettingTransform), list.ToArray());
            TypeDescriptor.AddProvider(provider, typeof(SettingTransform));
        }

        public static void SetUp() { }
    }
}
