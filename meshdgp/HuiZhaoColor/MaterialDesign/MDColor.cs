using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class MDColor
    {
        public List<string> Red = new List<string>();
        public List<string> Pink = new List<string>();
        public List<string> Purple = new List<string>();
        public List<string> DeepPurple = new List<string>();
        public List<string> Indigo = new List<string>();
        public List<string> Blue = new List<string>();
        public List<string> LightBlue = new List<string>();
        public List<string> Cyan = new List<string>();
        public List<string> Teal = new List<string>();
        public List<string> Green = new List<string>();
        public List<string> LightGreen = new List<string>();
        public List<string> Lime = new List<string>();
        public List<string> Yellow = new List<string>();
        public List<string> Amber = new List<string>();
        public List<string> Orange = new List<string>();
        public List<string> DeepOrange = new List<string>();
        public List<string> Brown = new List<string>();
        public List<string> Grey = new List<string>();
        public List<string> BlueGrey = new List<string>();








        public List<string> Popular = new List<string>(); 


        public MDColor()
        {
            InitRed();
            InitPink();
            InitPurple();
            InitDeepPurple();
            InitIndigo();
            InitBlue();
            InitLightBlue();
            InitCyan();
            InitTeal();
            InitGreen();
            InitLightGreen();
            InitLime();
            InitYellow();
            InitAmber();
            InitOrange();
            InitDeepOrange();
            InitBrown();
            InitGrey();
            InitBlueGrey();


            InitPopular();


             
        }

        public void InitRed()
        {
            Red.Add("FFEBEE");
            Red.Add("FFCDD2");
            Red.Add("EF9A9A");
            Red.Add("E57373");
            Red.Add("EF5350");
            Red.Add("F44336");
            Red.Add("E53935");
            Red.Add("D32F2F");
            Red.Add("C62828");
            Red.Add("B71C1C");
            Red.Add("FF8A80");
            Red.Add("FF5252");
            Red.Add("FF1744");
            Red.Add("FF1744"); 
        }

        public void InitPink()
        {
            Pink.Add("FCE4EC");
            Pink.Add("F8BBD0");
            Pink.Add("F48FB1");
            Pink.Add("F06292");
            Pink.Add("EC407A");
            Pink.Add("E91E63");
            Pink.Add("D81B60");
            Pink.Add("C2185B");
            Pink.Add("AD1457");
            Pink.Add("880E4F");
            Pink.Add("FF80AB");
            Pink.Add("FF4081");
            Pink.Add("F50057");
            Pink.Add("C51162");
        }

        public void InitPurple()
        {
            Purple.Add("F3E5F5");
            Purple.Add("E1BEE7");
            Purple.Add("CE93D8");
            Purple.Add("BA68C8");
            Purple.Add("AB47BC");
            Purple.Add("9C27B0");
            Purple.Add("8E24AA");
            Purple.Add("7B1FA2");
            Purple.Add("6A1B9A");
            Purple.Add("4A148C");
            Purple.Add("EA80FC");
            Purple.Add("E040FB");
            Purple.Add("D500F9");
            Purple.Add("AA00FF");
        }



        public void InitDeepPurple()
        {
            DeepPurple.Add("673AB7");
            DeepPurple.Add("EDE7F6");
            DeepPurple.Add("D1C4E9");
            DeepPurple.Add("B39DDB");
            DeepPurple.Add("9575CD");
            DeepPurple.Add("7E57C2");
            DeepPurple.Add("673AB7");
            DeepPurple.Add("5E35B1");
            DeepPurple.Add("512DA8");
            DeepPurple.Add("4527A0");
            DeepPurple.Add("311B92");
            DeepPurple.Add("B388FF");
            DeepPurple.Add("7C4DFF");
            DeepPurple.Add("651FFF");
        }

        public void InitIndigo()
        {
            Indigo.Add("E8EAF6");
            Indigo.Add("C5CAE9");
            Indigo.Add("9FA8DA");
            Indigo.Add("7986CB");
            Indigo.Add("5C6BC0");
            Indigo.Add("3F51B5");
            Indigo.Add("3949AB");
            Indigo.Add("303F9F");
            Indigo.Add("283593");
            Indigo.Add("1A237E");
            Indigo.Add("8C9EFF");
            Indigo.Add("536DFE");
            Indigo.Add("3D5AFE");
            Indigo.Add("304FFE");
        }


        public void InitBlue()
        {
            Blue.Add("E3F2FD");
            Blue.Add("BBDEFB");
            Blue.Add("90CAF9");
            Blue.Add("64B5F6");
            Blue.Add("42A5F5");
            Blue.Add("2196F3");
            Blue.Add("1E88E5");
            Blue.Add("1976D2");
            Blue.Add("1565C0");
            Blue.Add("0D47A1");
            Blue.Add("82B1FF");
            Blue.Add("448AFF");
            Blue.Add("2979FF");
            Blue.Add("2962FF");
        }


        public void InitLightBlue()
        {
            LightBlue.Add("E1F5FE");
            LightBlue.Add("B3E5FC");
            LightBlue.Add("81D4FA");
            LightBlue.Add("4FC3F7");
            LightBlue.Add("29B6F6");
            LightBlue.Add("03A9F4");
            LightBlue.Add("039BE5");
            LightBlue.Add("0288D1");
            LightBlue.Add("0277BD");
            LightBlue.Add("01579B");
            LightBlue.Add("80D8FF");
            LightBlue.Add("40C4FF");
            LightBlue.Add("00B0FF");
            LightBlue.Add("0091EA");
        }



        public void InitCyan()
        {
            Cyan.Add("E0F7FA");
            Cyan.Add("B2EBF2");
            Cyan.Add("80DEEA");
            Cyan.Add("4DD0E1");
            Cyan.Add("26C6DA");
            Cyan.Add("00BCD4");
            Cyan.Add("00ACC1");
            Cyan.Add("0097A7");
            Cyan.Add("00838F");
            Cyan.Add("006064");
            Cyan.Add("84FFFF");
            Cyan.Add("18FFFF");
            Cyan.Add("00E5FF");
            Cyan.Add("00B8D4");
        }



        public void InitTeal()
        {
            Teal.Add("E0F2F1");
            Teal.Add("B2DFDB");
            Teal.Add("80CBC4");
            Teal.Add("4DB6AC");
            Teal.Add("26A69A");
            Teal.Add("009688");
            Teal.Add("00897B");
            Teal.Add("00796B");
            Teal.Add("00695C");
            Teal.Add("004D40");
            Teal.Add("A7FFEB");
            Teal.Add("64FFDA");
            Teal.Add("1DE9B6");
            Teal.Add("00BFA5");
        }


        public void InitGreen()
        {
            Green.Add("E8F5E9");
            Green.Add("C8E6C9");
            Green.Add("A5D6A7");
            Green.Add("81C784");
            Green.Add("66BB6A");
            Green.Add("4CAF50");
            Green.Add("43A047");
            Green.Add("388E3C");
            Green.Add("2E7D32");
            Green.Add("1B5E20");
            Green.Add("B9F6CA");
            Green.Add("69F0AE");
            Green.Add("00E676");
            Green.Add("00C853");
        }






        public void InitLightGreen()
        {
            LightGreen.Add("F1F8E9");
            LightGreen.Add("DCEDC8");
            LightGreen.Add("C5E1A5");
            LightGreen.Add("AED581");
            LightGreen.Add("9CCC65");
            LightGreen.Add("8BC34A");
            LightGreen.Add("7CB342");
            LightGreen.Add("689F38");
            LightGreen.Add("558B2F");
            LightGreen.Add("33691E");
            LightGreen.Add("CCFF90");
            LightGreen.Add("B2FF59");
            LightGreen.Add("76FF03");
            LightGreen.Add("64DD17");
        }



        public void InitLime()
        {
            Lime.Add("F9FBE7");
            Lime.Add("F0F4C3");
            Lime.Add("E6EE9C");
            Lime.Add("DCE775");
            Lime.Add("D4E157");
            Lime.Add("CDDC39");
            Lime.Add("C0CA33");
            Lime.Add("AFB42B");
            Lime.Add("9E9D24");
            Lime.Add("827717");
            Lime.Add("F4FF81");
            Lime.Add("EEFF41");
            Lime.Add("C6FF00");
            Lime.Add("AEEA00");
        }



        public void InitYellow()
        {
            Yellow.Add("FFFDE7");
            Yellow.Add("FFF9C4");
            Yellow.Add("FFF59D");
            Yellow.Add("FFF176");
            Yellow.Add("FFEE58");
            Yellow.Add("FFEB3B");
            Yellow.Add("FDD835");
            Yellow.Add("FBC02D");
            Yellow.Add("F9A825");
            Yellow.Add("F57F17");
            Yellow.Add("FFFF8D");
            Yellow.Add("FFFF00");
            Yellow.Add("FFEA00");
            Yellow.Add("FFD600");
        }



        public void InitAmber()
        {
            Amber.Add("FFF8E1");
            Amber.Add("FFECB3");
            Amber.Add("FFE082");
            Amber.Add("FFD54F");
            Amber.Add("FFCA28");
            Amber.Add("FFC107");
            Amber.Add("FFB300");
            Amber.Add("FFA000");
            Amber.Add("FF8F00");
            Amber.Add("FF6F00");
            Amber.Add("FFE57F");
            Amber.Add("FFD740");
            Amber.Add("FFC400");
            Amber.Add("FFAB00");
        }


        public void InitOrange()
        {
            Orange.Add("FFF3E0");
            Orange.Add("FFE0B2");
            Orange.Add("FFCC80");
            Orange.Add("FFB74D");
            Orange.Add("FFA726");
            Orange.Add("FF9800");
            Orange.Add("FB8C00");
            Orange.Add("F57C00");
            Orange.Add("EF6C00");
            Orange.Add("E65100");
            Orange.Add("FFD180");
            Orange.Add("FFAB40");
            Orange.Add("FF9100");
            Orange.Add("FF6D00");
        }


        public void InitDeepOrange()
        {
            DeepOrange.Add("FBE9E7");
            DeepOrange.Add("FFCCBC");
            DeepOrange.Add("FFAB91");
            DeepOrange.Add("FF8A65");
            DeepOrange.Add("FF7043");
            DeepOrange.Add("FF5722");
            DeepOrange.Add("F4511E");
            DeepOrange.Add("E64A19");
            DeepOrange.Add("D84315");
            DeepOrange.Add("BF360C");
            DeepOrange.Add("FF9E80");
            DeepOrange.Add("FF6E40");
            DeepOrange.Add("FF3D00");
            DeepOrange.Add("DD2C00");
        }



        public void InitBrown()
        {
            Brown.Add("EFEBE9");
            Brown.Add("D7CCC8");
            Brown.Add("BCAAA4");
            Brown.Add("A1887F");
            Brown.Add("8D6E63");
            Brown.Add("795548");
            Brown.Add("6D4C41");
            Brown.Add("5D4037");
            Brown.Add("4E342E");
            Brown.Add("3E2723"); 
        }


        public void InitGrey()
        {
            Grey.Add("FAFAFA");
            Grey.Add("F5F5F5");
            Grey.Add("EEEEEE");
            Grey.Add("E0E0E0");
            Grey.Add("BDBDBD");
            Grey.Add("9E9E9E");
            Grey.Add("757575");
            Grey.Add("616161");
            Grey.Add("424242");
            Grey.Add("212121"); 
        }


        public void InitBlueGrey()
        {
            BlueGrey.Add("ECEFF1");
            BlueGrey.Add("CFD8DC");
            BlueGrey.Add("B0BEC5");
            BlueGrey.Add("90A4AE");
            BlueGrey.Add("78909C");
            BlueGrey.Add("607D8B");
            BlueGrey.Add("546E7A");
            BlueGrey.Add("455A64");
            BlueGrey.Add("37474F");
            BlueGrey.Add("263238"); 
        }



        public void InitPopular()
        {
            Popular.Add("1abc9c");
            Popular.Add("2ecc71");
            Popular.Add("3498db");
            Popular.Add("9b59b6");
            Popular.Add("34495e");
            Popular.Add("16a085");
            Popular.Add("27ae60");
            Popular.Add("2980b9");
            Popular.Add("8e44ad");
            Popular.Add("2c3e50");
            Popular.Add("f1c40f");
            Popular.Add("e67e22");
            Popular.Add("e74c3c");
            Popular.Add("95a5a6");
            Popular.Add("f39c12");
            Popular.Add("d35400");
            Popular.Add("c0392b");
            Popular.Add("ecf0f1");
            Popular.Add("bdc3c7");
            Popular.Add("7f8c8d");
        }



    }
}
