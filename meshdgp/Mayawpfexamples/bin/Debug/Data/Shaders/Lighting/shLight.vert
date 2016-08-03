//
// Vertex shader for spherical harmonic lighting
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3  DiffuseColor;
uniform float ScaleFactor;

const float C1 = 0.429043;
const float C2 = 0.511664;
const float C3 = 0.743125;
const float C4 = 0.886227;
const float C5 = 0.247708;

#ifdef OTS
// Constants for Old Town Square lighting
const vec3 L00  = vec3 ( 0.871297,  0.875222,  0.864470);
const vec3 L1m1 = vec3 ( 0.175058,  0.245335,  0.312891);
const vec3 L10  = vec3 ( 0.034675,  0.036107,  0.037362);
const vec3 L11  = vec3 (-0.004629, -0.029448, -0.048028);
const vec3 L2m2 = vec3 (-0.120535, -0.121160, -0.117507);
const vec3 L2m1 = vec3 ( 0.003242,  0.003624,  0.007511);
const vec3 L20  = vec3 (-0.028667, -0.024926, -0.020998);
const vec3 L21  = vec3 (-0.077539, -0.086325, -0.091591);
const vec3 L22  = vec3 (-0.161784, -0.191783, -0.219152);
#endif

#if 0
// Constants for Grace Cathedral lighting
const vec3 L00  = vec3 ( 0.78908,  0.43710,  0.54161);
const vec3 L1m1 = vec3 ( 0.39499,  0.34989,  0.60488);
const vec3 L10  = vec3 (-0.33974, -0.18236, -0.26940);
const vec3 L11  = vec3 (-0.29213, -0.05562,  0.00944);
const vec3 L2m2 = vec3 (-0.11141, -0.05090, -0.12231);
const vec3 L2m1 = vec3 (-0.26240, -0.22401, -0.47479);
const vec3 L20  = vec3 (-0.15570, -0.09471, -0.14733);
const vec3 L21  = vec3 ( 0.56014,  0.21444,  0.13915);
const vec3 L22  = vec3 ( 0.21205, -0.05432, -0.30374);
#endif

#if 0
// Constants for Eucalyptus Grove lighting
const vec3 L00  = vec3 ( 0.3783264,  0.4260425,  0.4504587);
const vec3 L1m1 = vec3 ( 0.2887813,  0.3586803,  0.4147053);
const vec3 L10  = vec3 ( 0.0379030,  0.0295216,  0.0098567);
const vec3 L11  = vec3 (-0.1033028, -0.1031690, -0.0884924);
const vec3 L2m2 = vec3 (-0.0621750, -0.0554432, -0.0396779);
const vec3 L2m1 = vec3 ( 0.0077820, -0.0148312, -0.0471301);
const vec3 L20  = vec3 (-0.0935561, -0.1254260, -0.1525629);
const vec3 L21  = vec3 (-0.0572703, -0.0502192, -0.0363410);
const vec3 L22  = vec3 ( 0.0203348, -0.0044201, -0.0452180);
#endif

#if 0
// Constants for St. Peter's Basilica lighting
const vec3 L00  = vec3 ( 0.3623915,  0.2624130,  0.2326261);
const vec3 L1m1 = vec3 ( 0.1759130,  0.1436267,  0.1260569);
const vec3 L10  = vec3 (-0.0247311, -0.0101253, -0.0010745);
const vec3 L11  = vec3 ( 0.0346500,  0.0223184,  0.0101350);
const vec3 L2m2 = vec3 ( 0.0198140,  0.0144073,  0.0043987);
const vec3 L2m1 = vec3 (-0.0469596, -0.0254485, -0.0117786);
const vec3 L20  = vec3 (-0.0898667, -0.0760911, -0.0740964);
const vec3 L21  = vec3 ( 0.0050194,  0.0038841,  0.0001374);
const vec3 L22  = vec3 (-0.0818750, -0.0321501,  0.0033399);
#endif

#if 0
// Constants for Uffizi Gallery lighting
const vec3 L00  = vec3 (  0.3168843,  0.3073441,  0.3495361);
const vec3 L1m1 = vec3 (  0.3711289,  0.3682168,  0.4292092);
const vec3 L10  = vec3 ( -0.0034406, -0.0031891, -0.0039797);
const vec3 L11  = vec3 ( -0.0084237, -0.0087049, -0.0116718);
const vec3 L2m2 = vec3 ( -0.0190313, -0.0192164, -0.0250836);
const vec3 L2m1 = vec3 ( -0.0110002, -0.0102972, -0.0119522);
const vec3 L20  = vec3 ( -0.2787319, -0.2752035, -0.3184335);
const vec3 L21  = vec3 (  0.0011448,  0.0009613,  0.0008975);
const vec3 L22  = vec3 ( -0.2419374, -0.2410955, -0.2842899);
#endif

#if 0
// Constants for Galileo's tomb lighting
const vec3 L00  = vec3 ( 1.0351604,  0.7603549,  0.7074635);
const vec3 L1m1 = vec3 ( 0.4442150,  0.3430402,  0.3403777);
const vec3 L10  = vec3 (-0.2247797, -0.1828517, -0.1705181);
const vec3 L11  = vec3 ( 0.7110400,  0.5423169,  0.5587956);
const vec3 L2m2 = vec3 ( 0.6430452,  0.4971454,  0.5156357);
const vec3 L2m1 = vec3 (-0.1150112, -0.0936603, -0.0839287);
const vec3 L20  = vec3 (-0.3742487, -0.2755962, -0.2875017);
const vec3 L21  = vec3 (-0.1694954, -0.1343096, -0.1335315);
const vec3 L22  = vec3 ( 0.5515260,  0.4222179,  0.4162488);
#endif

#if 0
// Constants for Vine Street kitchen lighting
const vec3 L00  = vec3 ( 0.6396604,  0.6740969,  0.7286833);
const vec3 L1m1 = vec3 ( 0.2828940,  0.3159227,  0.3313502);
const vec3 L10  = vec3 ( 0.4200835,  0.5994586,  0.7748295);
const vec3 L11  = vec3 (-0.0474917, -0.0372616, -0.0199377);
const vec3 L2m2 = vec3 (-0.0984616, -0.0765437, -0.0509038);
const vec3 L2m1 = vec3 ( 0.2496256,  0.3935312,  0.5333141);
const vec3 L20  = vec3 ( 0.3813504,  0.5424832,  0.7141644);
const vec3 L21  = vec3 ( 0.0583734,  0.0066377, -0.0234326);
const vec3 L22  = vec3 (-0.0325933, -0.0239167, -0.0330796);
#endif

#if 0
// Constants for Breezeway lighting
const vec3 L00  = vec3 ( 0.3175995,  0.3571678,  0.3784286);
const vec3 L1m1 = vec3 ( 0.3655063,  0.4121290,  0.4490332);
const vec3 L10  = vec3 (-0.0071628, -0.0123780, -0.0146215);
const vec3 L11  = vec3 (-0.1047419, -0.1183074, -0.1260049);
const vec3 L2m2 = vec3 (-0.1304345, -0.1507366, -0.1702497);
const vec3 L2m1 = vec3 (-0.0098978, -0.0155750, -0.0178279);
const vec3 L20  = vec3 (-0.0704158, -0.0762753, -0.0865235);
const vec3 L21  = vec3 ( 0.0242531,  0.0279176,  0.0335200);
const vec3 L22  = vec3 (-0.2858534, -0.3235718, -0.3586478);
#endif

#if 0
// Constants for Campus Sunset lighting
const vec3 L00  = vec3 ( 0.7870665,  0.9379944,  0.9799986);
const vec3 L1m1 = vec3 ( 0.4376419,  0.5579443,  0.7024107);
const vec3 L10  = vec3 (-0.1020717, -0.1824865, -0.2749662);
const vec3 L11  = vec3 ( 0.4543814,  0.3750162,  0.1968642);
const vec3 L2m2 = vec3 ( 0.1841687,  0.1396696,  0.0491580);
const vec3 L2m1 = vec3 (-0.1417495, -0.2186370, -0.3132702);
const vec3 L20  = vec3 (-0.3890121, -0.4033574, -0.3639718);
const vec3 L21  = vec3 ( 0.0872238,  0.0744587,  0.0353051);
const vec3 L22  = vec3 ( 0.6662600,  0.6706794,  0.5246173);
#endif

#if 0
// Constants for Funston Beach Sunset lighting
const vec3 L00  = vec3 ( 0.6841148,  0.6929004,  0.7069543);
const vec3 L1m1 = vec3 ( 0.3173355,  0.3694407,  0.4406839);
const vec3 L10  = vec3 (-0.1747193, -0.1737154, -0.1657420);
const vec3 L11  = vec3 (-0.4496467, -0.4155184, -0.3416573);
const vec3 L2m2 = vec3 (-0.1690202, -0.1703022, -0.1525870);
const vec3 L2m1 = vec3 (-0.0837808, -0.0940454, -0.1027518);
const vec3 L20  = vec3 (-0.0319670, -0.0214051, -0.0147691);
const vec3 L21  = vec3 ( 0.1641816,  0.1377558,  0.1010403);
const vec3 L22  = vec3 ( 0.3697189,  0.3097930,  0.2029923);
#endif

#ifdef Montreal
// Constants for Montreal lights festival lighting
const vec3 L00  = vec3 ( 0.664519,  0.286751,  0.253735)*1.5;
const vec3 L1m1 = vec3 ( 0.143787, -0.033508,  0.019203)*1.5;
const vec3 L10  = vec3 (-0.027470, -0.038257, -0.012850)*1.5;
const vec3 L11  = vec3 ( 0.020727,  0.014133,  0.013889)*1.5;
const vec3 L2m2 = vec3 ( 0.009926, -0.001891, -0.016442)*1.5;
const vec3 L2m1 = vec3 ( 0.116069,  0.071375,  0.096411)*1.5;
const vec3 L20  = vec3 (-0.124285, -0.044735, -0.022606)*1.5;
const vec3 L21  = vec3 ( 0.112684,  0.021489, -0.024643)*1.5;
const vec3 L22  = vec3 (-0.047316,  0.003080, -0.020458)*1.5;
#endif

#ifdef Underwater
// Constants for underwater lighting
const vec3 L00  = vec3 ( 0.117671,  0.542921,  0.714927)*2.5;
const vec3 L1m1 = vec3 (-0.140035, -0.363003, -0.490826)*2.5;
const vec3 L10  = vec3 (-0.080812, -0.360948, -0.401222)*2.5;
const vec3 L11  = vec3 (-0.065017, -0.214640, -0.249566)*2.5;
const vec3 L2m2 = vec3 ( 0.080082,  0.233630,  0.249594)*2.5;
const vec3 L2m1 = vec3 ( 0.129008,  0.305243,  0.368893)*2.5;
const vec3 L20  = vec3 (-0.041884, -0.109182, -0.118479)*2.5;
const vec3 L21  = vec3 ( 0.047522,  0.121900,  0.148977)*2.5;
const vec3 L22  = vec3 (-0.062091, -0.000349,  0.047598)*2.5;
#endif

#ifdef GoldenGate
// Constants for Golden Gate lighting
const vec3 L00  = vec3 ( 4.88161,  5.48490,  6.07302)/3.0;
const vec3 L1m1 = vec3 (-2.13128, -2.95222, -4.42096)/3.0;
const vec3 L10  = vec3 (-3.80898, -4.22344, -4.53079)/3.0;
const vec3 L11  = vec3 (-1.68098, -1.41778, -1.06535)/3.0;
const vec3 L2m2 = vec3 ( 1.46769,  1.20539,  0.79498)/3.0;
const vec3 L2m1 = vec3 ( 1.70156,  2.26629,  3.00788)/3.0;
const vec3 L20  = vec3 ( 0.46313,  0.36516, -0.01727)/3.0;
const vec3 L21  = vec3 ( 2.15828,  1.86879, 1.47719)/3.0;
const vec3 L22  = vec3 ( 2.38652,  2.33841, 1.59030)/3.0;
#endif

#ifdef H2Hummer
// Constants for Hummer Limo interior
const vec3 L00  = vec3 ( 0.744630,  0.342710,  0.334308)*2.0;
const vec3 L1m1 = vec3 ( 0.118382,  0.055162,  0.113828)*2.0;
const vec3 L10  = vec3 (-0.056065, -0.004089,  0.043308)*2.0;
const vec3 L11  = vec3 (-0.045723, -0.018347, -0.015030)*2.0;
const vec3 L2m2 = vec3 (-0.023537, -0.014534, -0.003501)*2.0;
const vec3 L2m1 = vec3 (-0.012294, -0.014507, -0.003506)*2.0;
const vec3 L20  = vec3 ( 0.108824,  0.040208,  0.076417)*2.0;
const vec3 L21  = vec3 ( 0.013933,  0.005914, -0.024783)*2.0;
const vec3 L22  = vec3 ( 0.032227,  0.052928, -0.101217)*2.0;
#endif

#ifdef Minnehaha
// Constants for Minnehaha Waterfall
const vec3 L00  = vec3 ( 0.422588,  0.346191,  0.445074)*2.0;
const vec3 L1m1 = vec3 (-0.063156, -0.127639, -0.362034)*2.0;
const vec3 L10  = vec3 ( 0.088431,  0.078842,  0.040722)*2.0;
const vec3 L11  = vec3 ( 0.021056,  0.019716, -0.007385)*2.0;
const vec3 L2m2 = vec3 ( 0.019087,  0.001245,  0.005354)*2.0;
const vec3 L2m1 = vec3 ( 0.020367,  0.013270,  0.047913)*2.0;
const vec3 L20  = vec3 ( 0.116549,  0.066853, -0.019363)*2.0;
const vec3 L21  = vec3 ( 0.076509,  0.033355, -0.021672)*2.0;
const vec3 L22  = vec3 (-0.033395, -0.077979, -0.240282)*2.0;
#endif

void main(void)
{
    vec3 tnorm      = normalize(gl_NormalMatrix * gl_Normal);
    tnorm = vec3(gl_TextureMatrix[0] * vec4(tnorm, 1.0));
    tnorm.y = -tnorm.y; // (our coefficients seem to be vertically flipped)
    DiffuseColor    = C1 * L22 * (tnorm.x * tnorm.x - tnorm.y * tnorm.y) +
                      C3 * L20 * tnorm.z * tnorm.z +
                      C4 * L00 -
                      C5 * L20 +
                      2.0 * C1 * L2m2 * tnorm.x * tnorm.y +
                      2.0 * C1 * L21  * tnorm.x * tnorm.z +
                      2.0 * C1 * L2m1 * tnorm.y * tnorm.z +
                      2.0 * C2 * L11  * tnorm.x +
                      2.0 * C2 * L1m1 * tnorm.y +
                      2.0 * C2 * L10  * tnorm.z;

    DiffuseColor   *= ScaleFactor;
    DiffuseColor   *= accessibility;
    DiffuseColor   *= vec3(gl_Color);

    gl_Position     = ftransform();
    shadowSetup();
}