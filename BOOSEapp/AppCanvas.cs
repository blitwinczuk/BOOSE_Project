using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEapp
{
    
    internal class AppCanvas : ICanvas
    {
        Bitmap CanvasBitmap;
        Graphics g;
        private int xPos, yPos;
        Pen Pen;
        public AppCanvas(int xsize, int ysize) {

            CanvasBitmap = new Bitmap(xsize, ysize);
            g = Graphics.FromImage(CanvasBitmap);
            xPos = 0;
            yPos = 0;
            Pen = new Pen(Color.Black);
        }

        public int Xpos { get => xPos; set => xPos = value; }
        public int Ypos { get => yPos; set => yPos = value; }
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

        public void Circle(int radius, bool filled)
        {
            g.DrawEllipse(Pen, Xpos, Ypos, radius * 2, radius * 2);
        }

        public void Clear()
        {
            g.Clear(Color.White);
        }

        public void DrawTo(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public object getBitmap()
        {
            return CanvasBitmap;
        }

        public void MoveTo(int x, int y)
        {
            Xpos = x;
            Ypos = y;
        }

        public void Rect(int width, int height, bool filled)
        {
            if (filled)
            {
                using (var brush = new SolidBrush(Pen.Color))
                {
                    g.FillRectangle(brush, Xpos, Ypos, width, height);
                }
            }
        }

        public void Reset()
        {
            xPos = 100;
            yPos = 100;
        }

        public void Set(int width, int height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException("width/height must be > 0");

            // Dispose existing graphics and bitmap to avoid leaks
            if (g != null)
            {
                g.Dispose();
                g = null;
            }
            if (CanvasBitmap != null)
            {
                CanvasBitmap.Dispose();
                CanvasBitmap = null;
            }

            // Create new backing bitmap and graphics
            CanvasBitmap = new Bitmap(width, height);
            g = Graphics.FromImage(CanvasBitmap);

            // Optional: initialize drawing surface
            g.Clear(Color.White);

            // Optionally reset cursor/position to a safe location
            xPos = 0;
            yPos = 0;
        }

        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
        }

        public void Tri(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void WriteText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
