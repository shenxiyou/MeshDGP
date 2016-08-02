using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphicResearchHuiZhao
{
    [DebuggerDisplay(@"\{{Lower},{Upper}\}")]
    

    public static class GreateColor
    {
        private class DefinedColor
        {
            public ColorRange HueRange { get; set; }
            public Point[] LowerBounds { get; set; }
            public ColorRange SaturationRange { get; set; }
            public ColorRange BrightnessRange { get; set; }
        }

        private static readonly Dictionary<ColorScheme, DefinedColor> ColorDictionary = new Dictionary<ColorScheme, DefinedColor>();

        private readonly static object LockObj = new object();
        private static Random _rng = new Random();

        static GreateColor()
        {
            // Populate the color dictionary
            LoadColorBounds();
        }

        public static Color GetColor(ColorScheme scheme, Luminosity luminosity)
        {
            int H, S, B;

            // First we pick a hue (H)
            H = PickHue(scheme);

            // Then use H to determine saturation (S)
            S = PickSaturation(H, luminosity, scheme);

            // Then use S and H to determine brightness (B).
            B = PickBrightness(H, S, luminosity);

            // Then we return the HSB color in the desired format
            return HsvToColor(H, S, B);
        }

        public static Color[] GetColors(ColorScheme scheme, Luminosity luminosity, int count)
        {
            var ret = new Color[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = GetColor(scheme, luminosity);
            }
            return ret;
        }

        public static Color[] GetColors(params ColorOptions[] options)
        {
            if (options == null) throw new ArgumentNullException("options");

            return options.Select(o => GetColor(o.ColorScheme, o.Luminosity)).ToArray();
        }

        /// <summary>
        /// Reseeds the random number generated.
        /// </summary>
        /// <param name="seed">The number used to reseed the random number generator.</param>
        public static void Seed(int seed)
        {
            lock (LockObj)
            {
                _rng = new Random(seed);
            }
        }
        /// <summary>
        /// Reseeds the random number generated.
        /// </summary>
        public static void Seed()
        {
            lock (LockObj)
            {
                _rng = new Random();
            }
        }

        private static int PickHue(ColorScheme scheme)
        {
            var hueRange = GetHueRange(scheme);
            var hue = RandomWithin(hueRange);

            // Instead of storing red as two separate ranges,
            // we group them, using negative numbers
            if (hue < 0) hue = 360 + hue;

            return hue;
        }

        private static int PickSaturation(int hue, Luminosity luminosity, ColorScheme scheme)
        {
            if (luminosity == Luminosity.Random)
            {
                return RandomWithin(0, 100);
            }

            if (scheme == ColorScheme.Monochrome)
            {
                return 0;
            }

            var saturationRange = GetColorInfo(hue).SaturationRange;

            var sMin = saturationRange.Lower;
            var sMax = saturationRange.Upper;

            switch (luminosity)
            {
                case Luminosity.Bright:
                    sMin = 55;
                    break;

                case Luminosity.Dark:
                    sMin = sMax - 10;
                    break;

                case Luminosity.Light:
                    sMax = 55;
                    break;
            }

            return RandomWithin(sMin, sMax);
        }

        private static int PickBrightness(int H, int S, Luminosity luminosity)
        {
            var bMin = GetMinimumBrightness(H, S);
            var bMax = 100;

            switch (luminosity)
            {
                case Luminosity.Dark:
                    bMax = bMin + 20;
                    break;

                case Luminosity.Light:
                    bMin = (bMax + bMin) / 2;
                    break;

                case Luminosity.Random:
                    bMin = 0;
                    bMax = 100;
                    break;
            }

            return RandomWithin(bMin, bMax);
        }

        private static int GetMinimumBrightness(int H, int S)
        {
            var lowerBounds = GetColorInfo(H).LowerBounds;

            for (var i = 0; i < lowerBounds.Length - 1; i++)
            {
                var s1 = lowerBounds[i].X;
                var v1 = lowerBounds[i].Y;

                var s2 = lowerBounds[i + 1].X;
                var v2 = lowerBounds[i + 1].Y;

                if (S >= s1 && S <= s2)
                {
                    var m = (v2 - v1) / (s2 - s1);
                    var b = v1 - m * s1;

                    return (int)(m * S + b);
                }
            }

            return 0;
        }

        private static ColorRange GetHueRange(ColorScheme colorInput)
        {
            DefinedColor color;
            if (ColorDictionary.TryGetValue(colorInput, out color))
            {
                if (color.HueRange != null)
                {
                    return color.HueRange;
                }
            }

            return new ColorRange(0, 360);
        }

        private static DefinedColor GetColorInfo(int hue)
        {
            // Maps red colors to make picking hue easier
            if (hue >= 334 && hue <= 360)
            {
                hue -= 360;
            }

            var ret = ColorDictionary.FirstOrDefault(c => c.Value.HueRange != null &&
                                                          hue >= c.Value.HueRange[0] &&
                                                          hue <= c.Value.HueRange[1]);

            Debug.Assert(ret.Value != null);

            return ret.Value;
        }

        private static int RandomWithin(ColorRange range)
        {
            return RandomWithin(range.Lower, range.Upper);
        }
        private static int RandomWithin(int lower, int upper)
        {
            lock (LockObj)
            {
                return _rng.Next(lower, upper + 1);
            }            
        }

        private static void DefineColor(ColorScheme scheme, int[] hueRange, int[,] lowerBounds)
        {
            int[][] jagged = new int[lowerBounds.GetLength(0)][];
            for (int i = 0; i < lowerBounds.GetLength(0); i++)
            {
                jagged[i] = new int[lowerBounds.GetLength(1)];
                for (int j = 0; j < lowerBounds.GetLength(1); j++)
                {
                    jagged[i][j] = lowerBounds[i, j];
                }
            }

            var sMin = jagged[0][0];
            var sMax = jagged[jagged.Length - 1][0];
            var bMin = jagged[jagged.Length - 1][1];
            var bMax = jagged[0][1];

            ColorDictionary[scheme] = new DefinedColor()
                {
                    HueRange = ColorRange.ToRange(hueRange),
                    LowerBounds = jagged.Select(j => new Point(j[0], j[1])).ToArray(),
                    SaturationRange = new ColorRange(sMin, sMax),
                    BrightnessRange = new ColorRange(bMin, bMax)
                };
        }

        private static void LoadColorBounds()
        {
            DefineColor(
                ColorScheme.Monochrome,
                null,
                new[,] { { 0, 0 }, { 100, 0 } }
                );

            DefineColor(
                ColorScheme.Red,
                new[] { -26, 18 },
                new[,] { { 20, 100 }, { 30, 92 }, { 40, 89 }, { 50, 85 }, { 60, 78 }, { 70, 70 }, { 80, 60 }, { 90, 55 }, { 100, 50 } }
                );

            DefineColor(
                ColorScheme.Orange,
                new[] { 19, 46 },
                new[,] { { 20, 100 }, { 30, 93 }, { 40, 88 }, { 50, 86 }, { 60, 85 }, { 70, 70 }, { 100, 70 } }
                );

            DefineColor(
                ColorScheme.Yellow,
                new[] { 47, 62 },
                new[,] { { 25, 100 }, { 40, 94 }, { 50, 89 }, { 60, 86 }, { 70, 84 }, { 80, 82 }, { 90, 80 }, { 100, 75 } }
                );

            DefineColor(
                ColorScheme.Green,
                new[] { 63, 178 },
                new[,] { { 30, 100 }, { 40, 90 }, { 50, 85 }, { 60, 81 }, { 70, 74 }, { 80, 64 }, { 90, 50 }, { 100, 40 } }
                );

            DefineColor(
                ColorScheme.Blue,
                new[] { 179, 257 },
                new[,] { { 20, 100 }, { 30, 86 }, { 40, 80 }, { 50, 74 }, { 60, 60 }, { 70, 52 }, { 80, 44 }, { 90, 39 }, { 100, 35 } }
                );

            DefineColor(
                ColorScheme.Purple,
                new[] { 258, 282 },
                new[,] { { 20, 100 }, { 30, 87 }, { 40, 79 }, { 50, 70 }, { 60, 65 }, { 70, 59 }, { 80, 52 }, { 90, 45 }, { 100, 42 } }
                );

            DefineColor(
                ColorScheme.Pink,
                new[] { 283, 334 },
                new[,] { { 20, 100 }, { 30, 90 }, { 40, 86 }, { 60, 84 }, { 80, 80 }, { 90, 75 }, { 100, 73 } }
                );
        }

        public static Color HsvToColor(int hue, int saturation, double value)
        {
            // this doesn't work for the values of 0 and 360
            // here's the hacky fix
            var h = Convert.ToDouble(hue);
            if (h == 0)
            {
                h = 1;
            }
            if (h == 360)
            {
                h = 359;
            }

            // Rebase the h,s,v values
            h = h / 360.0;
            var s = saturation / 100.0;
            var v = value / 100.0;

            var hInt = (int)Math.Floor(h * 6.0);
            var f = h * 6 - hInt;
            var p = v * (1 - s);
            var q = v * (1 - f * s);
            var t = v * (1 - (1 - f) * s);
            var r = 256.0;
            var g = 256.0;
            var b = 256.0;

            switch (hInt)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
            }
            var c = Color.FromRgb((byte)Math.Floor(r * 255.0),
                                  (byte)Math.Floor(g * 255.0),
                                  (byte)Math.Floor(b * 255.0));

            return c;
        }
    }

  

  
}