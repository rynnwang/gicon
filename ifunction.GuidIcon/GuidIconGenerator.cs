using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace ifunction.GuidIcon
{
    /// <summary>
    /// Class GuidIconGenerator.
    /// </summary>
    public class GuidIconGenerator
    {
        #region Fields

        /// <summary>
        /// Gets or sets the size of the icon.
        /// </summary>
        /// <value>The size of the icon.</value>
        public int IconSize
        {
            get;
            protected set;
        }

        /// <summary>
        /// The unit square size
        /// </summary>
        int unitSquareSize;

        #endregion

        #region Costructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidIconGenerator" /> class.
        /// </summary>
        /// <param name="iconSize">Size of the icon.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        public GuidIconGenerator(int iconSize, int unitSquareSize = 5)
        {
            Initialize(iconSize, unitSquareSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidIconGenerator" /> class.
        /// It would use guid to hash a size. (Size range: 5 - 8)
        /// <example>
        /// GUID: {E947B991-8115-43A1-9AE6-85E817D26D8E}
        /// Segment = 8115
        /// Icon size would be: ((0x81 ^ 0x15) % 4) + 3 = 5
        /// </example>
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public GuidIconGenerator(Guid guid, int unitSquareSize = 5)
        {
            string segment2 = guid.ToString().Split('-')[1];
            var _iconSize = 3 + ((Convert.ToInt32(segment2.Substring(0, 2), 16) ^ Convert.ToInt32(segment2.Substring(2, 2), 16)) % 4);

            Initialize(_iconSize, unitSquareSize);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Generates the icon.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        /// <returns>Bitmap.</returns>
        public Bitmap GenerateIcon(Guid guid, int unitSquareSize)
        {
            if (unitSquareSize < 5)
            {
                unitSquareSize = this.unitSquareSize;
            }

            IconSymmetry symmetry;
            Color color;
            bool[] points;
            var iconSize = GetFactor(guid, out color, out symmetry, out points);

            bool[,] imagePoints = GetPoints(points, symmetry);

            return DrawIcon(color, imagePoints, unitSquareSize);
        }

        #endregion

        #region Guid to factor

        /// <summary>
        /// Gets the factor.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="color">The color.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <param name="pointNumber">The point number.</param>
        /// <returns>System.Int32 for icon size (5- 8).</returns>
        private int GetFactor(Guid guid, out Color color, out IconSymmetry symmetry, out bool[] points)
        {
            var segments = guid.ToString().Split('-');

            string segment1 = segments[0];
            string segment2 = segments[1];
            string segment3 = segments[2];

            color = HexToRGB(segment1);
            var _iconSize = 3 + ((Convert.ToInt32(segment2.Substring(0, 2), 16) ^ Convert.ToInt32(segment2.Substring(2, 2), 16)) % 4);
            symmetry = (IconSymmetry)((Convert.ToInt32(segment3.Substring(0, 2), 16) ^ Convert.ToInt32(segment3.Substring(2, 2), 16)) % 4);
            points = GetBasePoints(segments[3] + segments[4], symmetry);

            return _iconSize;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <returns>System.Boolean[][].</returns>
        private bool[,] GetPoints(bool[] basePoints, IconSymmetry symmetry)
        {
            bool[,] result = new bool[IconSize, IconSize];
            bool isEven = IconSize % 2 == 0;
            int halfLength = ((int)(IconSize / 2)) + (IconSize % 2);
            int sum = 0;

            switch (symmetry)
            {
                case IconSymmetry.Vertical:
                    for (int i = 0; i < IconSize; i++)
                    {
                        for (int j = 0; j < halfLength; j++)
                        {
                            result[i, IconSize - 1 - j] = result[i, j] = basePoints[i * halfLength + j];
                        }
                    }
                    break;
                case IconSymmetry.Horizontal:
                    for (int j = 0; j < halfLength; j++)
                    {
                        for (int i = 0; i < IconSize; i++)
                        {
                            result[IconSize - 1 - j, i] = result[j, i] = basePoints[i * halfLength + j];
                        }
                    }
                    break;
                case IconSymmetry.LeftDiagonal:
                    for (int i = 0; i < IconSize; i++)
                    {
                        for (int j = i; j < IconSize; j++)
                        {
                            result[j, i] = result[i, j] = basePoints[sum];
                            sum++;
                        }
                    }
                    break;
                case IconSymmetry.RightDiagonal:
                    for (int i = 0; i < IconSize; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            result[j, i] = result[i, j] = basePoints[sum];
                            sum++;
                        }
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Initializes the specified _icon size.
        /// </summary>
        /// <param name="_iconSize">Size of the _icon.</param>
        /// <param name="_unitSquareSize">Size of the _unit square.</param>
        private void Initialize(int _iconSize, int _unitSquareSize)
        {
            this.IconSize = _iconSize;
            this.unitSquareSize = _unitSquareSize;

            if (this.unitSquareSize < 5)
            {
                this.unitSquareSize = 5;
            }
        }

        /// <summary>
        /// Gets the color by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Color.</returns>
        private Color GetColorByGuid(Guid guid)
        {
            //return ConvertAHSBToColor();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the base points.
        /// This method just calculate the image bit hit by Icon Symmetry, whose count is not equals to the image size.
        /// <example>
        /// GUID: {E947B991-8115-43A1-9AE6-85E817D26D8E}
        /// Hex String = 9AE685E817D26D8E
        /// According to icon size is 5x5 to 8x8, so array size would be from 15 to 36 (
        /// Icon size would be: ((0x81 ^ 0x15) % 4) + 3 = 5
        /// </example>
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <returns>System.Boolean[].</returns>
        private bool[] GetBasePoints(string hexString, IconSymmetry symmetry)
        {
            int arraySize;

            if (symmetry == IconSymmetry.Horizontal || symmetry == IconSymmetry.Vertical)
            {
                arraySize = (((int)(IconSize / 2)) + (IconSize % 2)) * IconSize;
            }
            else
            {
                arraySize = ((IconSize) * (IconSize - 1) / 2) + IconSize;
            }

            bool[] result = new bool[arraySize];

            var value = Convert.ToInt64(hexString, 16) & ((long)Math.Pow(2, arraySize) - 1);
            var bitString = Convert.ToString(value, 2);

            if (bitString.Length < arraySize)
            {
                bitString = (new string('0', arraySize - bitString.Length)) + bitString;
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = bitString[i] == '1';
            }

            return result;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="symmetry">The symmetry.</param>
        /// <returns>System.Boolean[][].</returns>
        private bool[,] ConvertBasePointsToBmpPoints(IconSymmetry symmetry, bool[] basePoints)
        {
            bool[,] result = new bool[IconSize, IconSize];

            //var points = GetBasePoints(guid, symmetry);

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
        /// Draws the icon.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="points">The points.</param>
        /// <param name="unitSquareSize">Size of the unit square.</param>
        /// <returns>Bitmap.</returns>
        private Bitmap DrawIcon(Color color, bool[,] points, int unitSquareSize = 5)
        {
            if (unitSquareSize < 5)
            {
                unitSquareSize = 5;
            }

            var bmp = new Bitmap(unitSquareSize * IconSize, unitSquareSize * IconSize);

            using (var graphicImage = Graphics.FromImage(bmp))
            {
                graphicImage.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                var brush = new SolidBrush(color);

                for (var i = 0; i < IconSize; i++)
                {
                    for (var j = 0; j < IconSize; j++)
                    {
                        if (points[i, j])
                        {
                            graphicImage.FillRectangle(brush, new Rectangle(i * unitSquareSize, j * unitSquareSize, unitSquareSize, unitSquareSize));
                        }
                    }
                }

                ////render as Jpeg
                //bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                ////      img = File(mem.GetBuffer(), "image/Jpeg");
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
        /// <exception cref="System.ArgumentOutOfRangeException">alpha;Value must be within a range of 0 - 255.
        /// or
        /// hue;Value must be within a range of 0 - 360.
        /// or
        /// saturation;Value must be within a range of 0 - 1.
        /// or
        /// brightness;Value must be within a range of 0 - 1.</exception>
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
            return HexToRGB(Convert.ToInt64(hexString, 16));
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
