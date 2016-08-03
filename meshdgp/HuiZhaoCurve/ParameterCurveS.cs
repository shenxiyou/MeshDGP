using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class ParameterCurve
    {
        public TriMesh CreateCurve(EnumCurveSimple curveType)
        {
            TriMesh mesh = null;
            switch (curveType)
            {

                case EnumCurveSimple.Line:

                    mesh = ParameterCurve.Instance.CreateMeshLine();
                    break;

                case EnumCurveSimple.Circle:

                    mesh = ParameterCurve.Instance.CreateMeshCricle(0.5);
                    break;

                case EnumCurveSimple.DoubleArcEpicycloid:

                    mesh = ParameterCurve.Instance.CreateDoubleArcEpicycloidLine(0.5, 0.5);
                    break;

                case EnumCurveSimple.Hreat:

                    mesh = ParameterCurve.Instance.CreateHreatLine(0.5);
                    break;

                case EnumCurveSimple.Fermet:

                    mesh = ParameterCurve.Instance.CreateFermatCurve(0.5);
                    break;
                case EnumCurveSimple.Star:

                    mesh = ParameterCurve.Instance.CreateStartLine(0.5);
                    break;
                case EnumCurveSimple.Tablot:

                    mesh = ParameterCurve.Instance.CreateTalbotCurve(0.5, 0.5, 0.5);
                    break;

                case EnumCurveSimple.CircularHelix:

                    mesh = ParameterCurve.Instance.CreateCircularHelixLine();
                    break;
                case EnumCurveSimple.Sine:
                    mesh = ParameterCurve.Instance.CreateSineCurve();
                    break;

                case EnumCurveSimple.Peach:

                    mesh = ParameterCurve.Instance.CreateMeshPeach();
                    break;

                case EnumCurveSimple.Spring:

                    mesh = ParameterCurve.Instance.CreateMeshSpring(0.3);
                    break;

                case EnumCurveSimple.Hxecqx:

                    mesh = ParameterCurve.Instance.CreateMeshHxecqx();
                    break;
                case EnumCurveSimple.Butterfly:

                    mesh = ParameterCurve.Instance.CreateMeshButterfly();
                    break;
                case EnumCurveSimple.Span:

                    mesh = ParameterCurve.Instance.CreateMeshSpan();
                    break;
                case EnumCurveSimple.Sin:

                    mesh = ParameterCurve.Instance.CreateMeshSin();
                    break;
                case EnumCurveSimple.Image:
                    mesh = ParameterCurve.Instance.CreateMeshImage();
                    break;
                case EnumCurveSimple.Photo:
                    mesh = ParameterCurve.Instance.CreateMeshPhoto();
                    break;

                case EnumCurveSimple.Epitrochoid:

                    mesh = ParameterCurve.Instance.CreateEpitrochoid();
                    break;

                case EnumCurveSimple.TricuspidValveLine:

                    mesh = ParameterCurve.Instance.CreateTricuspidValveLine();
                    break;

                case EnumCurveSimple.ProbabilityCurve:

                    mesh = ParameterCurve.Instance.CreateProbabilityCurve();
                    break;
                case EnumCurveSimple.Versiera:

                    mesh = ParameterCurve.Instance.CreateVersiera();
                    break;
                case EnumCurveSimple.AchimedeanSpiral:

                    mesh = ParameterCurve.Instance.CreateAchimedeanSpiral();
                    break;
                case EnumCurveSimple.Log:

                    mesh = ParameterCurve.Instance.CreateLog();
                    break;
                case EnumCurveSimple.Cissoid:

                    mesh = ParameterCurve.Instance.CreateCissoid();
                    break;
                case EnumCurveSimple.Tan:

                    mesh = ParameterCurve.Instance.CreateTan();
                    break;


                case EnumCurveSimple.螺旋线:

                    mesh = ParameterCurve.Instance.CreateSpineLine();
                    break;

                case EnumCurveSimple.葉形线:

                    mesh = ParameterCurve.Instance.CreateFoliumCircle(0.5);
                    break;

                case EnumCurveSimple.锥形螺旋线:

                    mesh = ParameterCurve.Instance.CreateConeshapeLine(0.5, 0.5);
                    break;

                case EnumCurveSimple.渐开线:

                    mesh = ParameterCurve.Instance.CreateInvoluteCurve(0.5);
                    break;

                case EnumCurveSimple.对数曲线:

                    mesh = ParameterCurve.Instance.CreateLogarithmCurve(0.5);
                    break;
                case EnumCurveSimple.蝴蝶曲线:

                    mesh = ParameterCurve.Instance.CreateButterflyLine(0.5);
                    break;
                case EnumCurveSimple.球面螺旋线:

                    mesh = ParameterCurve.Instance.CreateGlobalCurve(0.5, 0.5, 0.5);
                    break;
                case EnumCurveSimple.蝶形弹簧:
                    mesh = ParameterCurve.Instance.CreateSpringLine();
                    break;


                case EnumCurveSimple.Long_short:

                    mesh = ParameterCurve.Instance.CreateLong();
                    break;

                case EnumCurveSimple.Lissajous:

                    mesh = ParameterCurve.Instance.CreateLissajous();
                    break;

                case EnumCurveSimple.epicycloid:

                    mesh = ParameterCurve.Instance.CreateEpicycloid();
                    break;
                case EnumCurveSimple.threee_leaves:

                    mesh = ParameterCurve.Instance.CreateThree();
                    break;
                case EnumCurveSimple.parabolic:

                    mesh = ParameterCurve.Instance.CreateParabolic();
                    break;

                case EnumCurveSimple.Rhodonea:

                    mesh = ParameterCurve.Instance.CreateRhodonea();
                    break;

                case EnumCurveSimple.helix:

                    mesh = ParameterCurve.Instance.CreateHelix();
                    break;

                case EnumCurveSimple.Four_leaves:

                    mesh = ParameterCurve.Instance.Four_leaves();
                    break;


                case EnumCurveSimple.太阳线:

                    mesh = ParameterCurve.Instance.CreateMeshSun();
                    break;

                case EnumCurveSimple.塔形螺旋线:

                    mesh = ParameterCurve.Instance.CreateTALine();
                    break;

                case EnumCurveSimple.花瓣线:

                    mesh = ParameterCurve.Instance.CreateFlower();
                    break;
                case EnumCurveSimple.双元宝线:

                    mesh = ParameterCurve.Instance.CreateDoubleY();
                    break;
                case EnumCurveSimple.蝴蝶:

                    mesh = ParameterCurve.Instance.CreateButterFly();
                    break;

                case EnumCurveSimple.双鱼:

                    mesh = ParameterCurve.Instance.CreateDoubleFish();
                    break;

                case EnumCurveSimple.阿基米德螺线的变形:

                    mesh = ParameterCurve.Instance.CreateAjLine();
                    break;

                case EnumCurveSimple.渐开线2:

                    mesh = ParameterCurve.Instance.CreateJk();
                    break;
            }
            return mesh;
        }
    }
}
