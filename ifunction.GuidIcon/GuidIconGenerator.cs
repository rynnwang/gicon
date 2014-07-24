using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace ifunction.GuidIcon
{
    public static class GuidIconGenerator
    {
        /// <summary>
        /// Value indicating the icon would consisted with 8x8 squares.
        /// </summary>
        const int iconSize = 8;

        private static Bitmap GenerateIcon(Guid guid, int unitSquareSize = 5)
        {
            IconSymmetry symmetry = IconSymmetry.Vertical;

            return GenerateIcon(guid, symmetry, unitSquareSize);
        }

        /// <summary>
        /// Generates the icon.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        /// <returns>Bitmap.</returns>
        private static Bitmap GenerateIcon(Guid guid, IconSymmetry symmetry, int unitSquareSize)
        {

            bool[,] imagePoints = GetPoints(guid, symmetry);
            Color color = GetColorByGuid(guid);

            return DrawIcon(color, imagePoints, unitSquareSize);
        }

        /// <summary>
        /// Gets the color by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Color.</returns>
        private static Color GetColorByGuid(Guid guid)
        {
            // Sample: {E947B991-8115-43A1-9AE6-85E817D26D8E}

            //return ConvertAHSBToColor();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <returns>System.Boolean[][].</returns>
        private static bool[,] GetPoints(Guid guid, IconSymmetry symmetry)
        {
            bool[,] result = new bool[iconSize, iconSize];

            var points = GetBasePoints(guid, symmetry);

            switch (symmetry)
            {
                case IconSymmetry.Vertical:
                    break;
                case IconSymmetry.Horizontal:
                    break;
                case IconSymmetry.LeftDiagonal:
                    break;
                case IconSymmetry.RightDiagonal:
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the base points.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <returns>System.Boolean[][].</returns>
        private static bool[] GetBasePoints(Guid guid, IconSymmetry symmetry)
        {
            int arraySize;

            if (symmetry == IconSymmetry.Horizontal || symmetry == IconSymmetry.Vertical)
            {
                arraySize = (((int)(iconSize / 2)) + (iconSize % 2)) * iconSize;
            }
            else
            {
                arraySize = ((iconSize) * (iconSize - 1) / 2) + iconSize;
            }

            bool[] result = new bool[arraySize];

            //Todo

            return result;
        }

        /// <summary>
        /// Draws the icon.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="points">The points.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        /// <returns>Bitmap.</returns>
        private static Bitmap DrawIcon(Color color, bool[,] points, int unitSquareSize = 5)
        {
            if (unitSquareSize < 5)
            {
                unitSquareSize = 5;
            }

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(unitSquareSize * 5, unitSquareSize * 5))
            using (var graphicImage = Graphics.FromImage(bmp))
            {
                graphicImage.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                graphicImage.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                //      img = File(mem.GetBuffer(), "image/Jpeg");

                return bmp;
            }
        }

        /// <summary>
        /// Converts the color from AHSB to color.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="brightness">The brightness.</param>
        /// <returns>Color.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// alpha;Value must be within a range of 0 - 255.
        /// or
        /// hue;Value must be within a range of 0 - 360.
        /// or
        /// saturation;Value must be within a range of 0 - 1.
        /// or
        /// brightness;Value must be within a range of 0 - 1.
        /// </exception>
        private static Color ConvertAHSBToColor(int alpha, float hue, float saturation, float brightness)
        {
            if (0 > alpha
                || 255 < alpha)
            {
                throw new ArgumentOutOfRangeException(
                    "alpha",
                    alpha,
                    "Value must be within a range of 0 - 255.");
            }

            if (0f > hue
                || 360f < hue)
            {
                throw new ArgumentOutOfRangeException(
                    "hue",
                    hue,
                    "Value must be within a range of 0 - 360.");
            }

            if (0f > saturation
                || 1f < saturation)
            {
                throw new ArgumentOutOfRangeException(
                    "saturation",
                    saturation,
                    "Value must be within a range of 0 - 1.");
            }

            if (0f > brightness
                || 1f < brightness)
            {
                throw new ArgumentOutOfRangeException(
                    "brightness",
                    brightness,
                    "Value must be within a range of 0 - 1.");
            }

            if (0 == saturation)
            {
                return Color.FromArgb(
                                    alpha,
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }

        /// <summary>
        /// Hexadecimals to RGB.
        /// e.g.: 22FF0033
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <returns>Color.</returns>
        private static Color HexToRGB(string hexString)
        {
            return HexToRGB(Convert.ToInt64(hexString));
        }

        /// <summary>
        /// Hexadecimals to RGB.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns>Color.</returns>
        private static Color HexToRGB(long hex)
        {
            int a = (int)((hex & 0xFF000000) >> 48);
            int r = (int)((hex & 0xFF0000) >> 32);
            int g = (int)((hex & 0xFF00) >> 16);
            int b = (int)(hex & 0xFF);
            Color result = Color.FromArgb(a, r, g, b);

            return result;
        }
    }
}
