using BOOSE;
using System;
using System.Drawing;

namespace BOOSEapp
{
    /// <summary>
    /// Custom canvas implementation used by the BOOSE interpreter.
    /// Responsible for drawing shapes and managing the drawing cursor.
    /// </summary>
    public class AppCanvas : ICanvas
    {
        private Bitmap CanvasBitmap;
        private Graphics g;
        private int xPos, yPos;
        private Pen Pen;

        /// <summary>
        /// Initializes a new drawing canvas with the specified size.
        /// </summary>
        /// <param name="xsize">Width of the canvas.</param>
        /// <param name="ysize">Height of the canvas.</param>
        public AppCanvas(int xsize, int ysize)
        {
            CanvasBitmap = new Bitmap(xsize, ysize);
            g = Graphics.FromImage(CanvasBitmap);
            xPos = 0;
            yPos = 0;
            Pen = new Pen(Color.Black);
        }

        /// <summary>
        /// Gets or sets the current X position of the drawing cursor.
        /// </summary>
        public int Xpos
        {
            get => xPos;
            set => xPos = value;
        }

        /// <summary>
        /// Gets or sets the current Y position of the drawing cursor.
        /// </summary>
        public int Ypos
        {
            get => yPos;
            set => yPos = value;
        }

        /// <summary>
        /// Gets or sets the current pen colour.
        /// </summary>
        public object PenColour
        {
            get => Pen.Color;
            set
            {
                if (value is Color c)
                {
                    Pen.Color = c;
                }
                else if (value is int argb)
                {
                    Pen.Color = Color.FromArgb(argb);
                }
            }
        }

        /// <summary>
        /// Draws a circle from the current cursor position.
        /// </summary>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="filled">Determines whether the circle is filled.</param>
        public void Circle(int radius, bool filled)
        {
            if (filled)
            {
                using (var brush = new SolidBrush(Pen.Color))
                {
                    g.FillEllipse(brush, Xpos - radius, Ypos - radius, radius * 2, radius * 2);
                }
            }
            else
            {
                int diameter = radius * 2;
                int startX = Xpos - radius;
                int startY = Ypos - radius;

                g.DrawEllipse(Pen, startX, startY, diameter, diameter);
            }
        }

        /// <summary>
        /// Clears the canvas to white.
        /// </summary>
        public void Clear()
        {
            g.Clear(Color.White);
        }

        /// <summary>
        /// Draws a line from the current cursor position to the given coordinates.
        /// </summary>
        /// <param name="x">Target X coordinate.</param>
        /// <param name="y">Target Y coordinate.</param>
        public void DrawTo(int x, int y)
        {
            g.DrawLine(Pen, Xpos, Ypos, x, y);
            Xpos = x;
            Ypos = y;
        }

        /// <summary>
        /// Returns the bitmap representing the canvas.
        /// </summary>
        /// <returns>Bitmap object.</returns>
        public object getBitmap()
        {
            return CanvasBitmap;
        }

        /// <summary>
        /// Moves the cursor to a new position without drawing.
        /// </summary>
        /// <param name="x">New X coordinate.</param>
        /// <param name="y">New Y coordinate.</param>
        public void MoveTo(int x, int y)
        {
            Xpos = x;
            Ypos = y;
        }

        /// <summary>
        /// Draws a rectangle from the current cursor position.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="filled">Determines whether the rectangle is filled.</param>
        public void Rect(int width, int height, bool filled)
        {
            if (filled)
            {
                using (var brush = new SolidBrush(Pen.Color))
                {
                    g.FillRectangle(brush, Xpos, Ypos, width, height);
                }
            }
            else
            {
                g.DrawRectangle(Pen, Xpos, Ypos, width, height);
            }
        }

        /// <summary>
        /// Resets the cursor to default position.
        /// </summary>
        public void Reset()
        {
            xPos = 100;
            yPos = 100;
        }

        /// <summary>
        /// Resizes the canvas.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        public void Set(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("width/height must be > 0");

            g?.Dispose();
            CanvasBitmap?.Dispose();

            CanvasBitmap = new Bitmap(width, height);
            g = Graphics.FromImage(CanvasBitmap);
            g.Clear(Color.White);

            xPos = 0;
            yPos = 0;
        }

        /// <summary>
        /// Sets the drawing colour using RGB values.
        /// </summary>
        /// <param name="red">Red component.</param>
        /// <param name="green">Green component.</param>
        /// <param name="blue">Blue component.</param>
        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Draws a triangle using the current cursor position.
        /// The triangle can be drawn as an outline or filled shape.
        /// </summary>
        /// <param name="width">Width of the triangle.</param>
        /// <param name="height">Height of the triangle.</param>
        public void Tri(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new CanvasException("Width and height must be greater than 0.");

            Point[] points = new Point[]
            {
        new Point(Xpos, Ypos + height),
        new Point(Xpos + width / 2, Ypos),
        new Point(Xpos + width, Ypos + height)
            };

            g.DrawPolygon(Pen, points);
        }

        /// <summary>
        /// Writes text to the canvas at the current cursor position.
        /// Supports plain text including symbols like £, $, %.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            // this will delete the surrounding quotes if the text is a string literal, allowing symbols to be displayed correctly
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2);
            }

            using (Font font = new Font("Arial", 12))
            using (Brush brush = new SolidBrush(Pen.Color))
            {
                g.DrawString(text, font, brush, Xpos, Ypos);
                //auto move tekst to next line
                Ypos += 20;
            }
        }
    }
    
}
