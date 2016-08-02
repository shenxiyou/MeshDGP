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
    public static class FormAttributeOpenGL
    {
        static FormAttributeOpenGL()
        {
            TypeDescriptor.AddAttributes(typeof(Color), new[] { new EditorAttribute(typeof(ColorEditorEx), typeof(UITypeEditor)) });

            DisplaySetting();
            SettingLight<SettingLightDirect>();
            SettingLight<SettingLightPoint>();
            SettingLight<SettingLightSpot>();
            SettingClipPlane();
            SettingTexture();
        }

        static void DisplaySetting()
        {
            List<PropertyAdditionalAttributes> list = new List<PropertyAdditionalAttributes>();
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "Alpha",
                PropertyType = typeof(int),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(NumericScrollBarEditor), typeof(UITypeEditor)),
                    new RangeAttribute(0, 255, 1)}
            });

            PropertyAttributesTypeDescriptionProvider provider = new PropertyAttributesTypeDescriptionProvider(
                typeof(DisplaySetting), list.ToArray());
            TypeDescriptor.AddProvider(provider, typeof(DisplaySetting));
        }

        static void SettingLight<T>() where T : SettingLightDirect
        {
            List<PropertyAdditionalAttributes> list = new List<PropertyAdditionalAttributes>();
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "LightPosition",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.1)}
            });
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "SpotDirection",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.1)}
            });

            PropertyAttributesTypeDescriptionProvider provider = new PropertyAttributesTypeDescriptionProvider(
                typeof(T), list.ToArray());
            TypeDescriptor.AddProvider(provider, typeof(T));
        }

        static void SettingClipPlane()
        {
            List<PropertyAdditionalAttributes> list = new List<PropertyAdditionalAttributes>();
            for (int i = 0; i < 6; i++)
            {
                list.Add(new PropertyAdditionalAttributes
                {
                    PropertyName = "Plane" + i,
                    PropertyType = typeof(Plane),
                    Attributes = new Attribute[]{
                        new EditorAttribute(typeof(PlaneEditor), typeof(UITypeEditor)),
                        new RangeAttribute(-2.5, 2.5, 0.005)}
                });
            }

            PropertyAttributesTypeDescriptionProvider provider = new PropertyAttributesTypeDescriptionProvider(
                typeof(SettingClipPlane), list.ToArray());
            TypeDescriptor.AddProvider(provider, typeof(SettingClipPlane));
        }

        static void SettingTexture()
        {
            List<PropertyAdditionalAttributes> list = new List<PropertyAdditionalAttributes>();
            foreach (var item in "STR")
            {
                list.Add(new PropertyAdditionalAttributes
                {
                    PropertyName = "Plane" + item,
                    PropertyType = typeof(Plane),
                    Attributes = new Attribute[]{
                        new EditorAttribute(typeof(PlaneEditor), typeof(UITypeEditor)),
                        new RangeAttribute(-10, 10, 0.1)}
                });
            }

            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "Transform",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.1)}
            });
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "Rotation",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.1)}
            });
            list.Add(new PropertyAdditionalAttributes
            {
                PropertyName = "Scale",
                PropertyType = typeof(Vector3D),
                Attributes = new Attribute[]{
                    new EditorAttribute(typeof(VectorEditor), typeof(UITypeEditor)),
                    new RangeAttribute(-10, 10, 0.1)}
            });

            PropertyAttributesTypeDescriptionProvider provider = new PropertyAttributesTypeDescriptionProvider(
                typeof(SettingTexture), list.ToArray());
            TypeDescriptor.AddProvider(provider, typeof(SettingTexture));
        }

        public static void SetUp() { }
    }
}
