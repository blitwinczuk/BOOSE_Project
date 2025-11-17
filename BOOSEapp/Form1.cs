using BOOSE;
using System.Diagnostics;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        AppCanvas canvas;
        CommandFactory Factory;
        StoredProgram Program;
        IParser Parser;
        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());
            canvas = new AppCanvas(800, 450);
            canvas.Circle(50, false);

            Factory = new AppCommandFactory();
            Program = new StoredProgram(canvas);
            Parser = new Parser(Factory, Program);
            

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap b = (Bitmap)canvas.getBitmap();
            Parser.ParseProgram("MOVETO 100,100\nCIRCLE 50\nDRAWTO 50,50");
            Program.Run();
            g.DrawImage(b, 0, 0);
        }
    }
}
