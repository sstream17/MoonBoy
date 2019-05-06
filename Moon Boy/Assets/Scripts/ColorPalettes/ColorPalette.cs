using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ActionCode.ColorPalettes
{
    /// <summary>
    /// Color Palettes are a set of finite colors. They are used to change the sprite's texture colors at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "New Color Palette", menuName = "Action Code/Color Palette", order = 200)]
    public sealed class ColorPalette : ScriptableObject
    {
        public Color[] colors;
        [SerializeField] private string _colorsPositionFilePath;

        public int Id { get; private set; }
        private static int IdCounter = -1;

        public ColorPalette()
        {
            Id = ++IdCounter;
        }

        public string GetColorPositionsContent(Texture2D texture)
        {
            StringBuilder[] lines = new StringBuilder[colors.Length];
            for (int i = 0; i < lines.Length; i++) lines[i] = new StringBuilder();

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color textureColor = texture.GetPixel(x, y);
                    if (textureColor.a > 0f)
                    {
                        for (int i = 0; i < colors.Length; i++)
                        {
                            if (colors[i] == textureColor)
                            {
                                lines[i].Append(x + "," + y + ";");
                                break;
                            }
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                sb.AppendLine(lines[i].ToString());
            }

            return sb.ToString();
        }


        public Point2[][] GetColorPositionsMatrix()
        {
            if (_colorsPositionFilePath.Length == 0)
            {
                Debug.LogError("Color Position File Path not set.");
                return null;
            }
            TextAsset colorsPositionFile = Resources.Load(Path.GetFileNameWithoutExtension(_colorsPositionFilePath)) as TextAsset;
            string fileText = colorsPositionFile.text;
            string[] lines = fileText.Split(new char [1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length != colors.Length)
            {
                Debug.LogError("Error on Colors Position File. Line numbers do not match with palette color number.");
                return null;
            }

            Point2[][] matrix = new Point2[colors.Length][];

            char[] separator = new char[1] { ';' };
            for (int i = 0; i < matrix.Length; i++)
            {
                string[] tuplas = lines[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                matrix[i] = new Point2[tuplas.Length - 1];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    string[] point = tuplas[j].Split(',');
                    matrix[i][j] = new Point2(int.Parse(point[0]), int.Parse(point[1]));
                }
            }

            return matrix;
        }
    }

    public struct Point2
    {
        public int x;
        public int y;

        public Point2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
