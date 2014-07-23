using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace ifunction.GuidIcon
{
    public static class GuidIconGenerator
    {
        /// <summary>
        /// Generates the icon.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap GenerateIcon(Guid guid, int unitSquareSize = 5)
        {
            bool[,] imagePoints = new bool[8, 4];
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
            //return ConvertAHSBToColor();
            throw new NotImplementedException();
        }

        private static Bitmap DrawIcon(Color color, bool[,] bits, int unitSquareSize = 5)
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
        public static Color ConvertAHSBToColor(int alpha, float hue, float saturation, float brightness)
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
    }
}
