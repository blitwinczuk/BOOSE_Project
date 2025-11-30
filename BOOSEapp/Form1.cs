using BOOSE;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        AppCanvas canvas;
        CommandFactory Factory;
        StoredProgram Program;
        IParser Parser;
        private TextBox txtProgram;

        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine("BOOSE Application Started");
            Debug.WriteLine(AboutBOOSE.about());
            canvas = new AppCanvas(1200, 750);

            Factory = new AppCommandFactory();
            Program = new StoredProgram(canvas);
            Parser = new Parser(Factory, Program);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas.Set(pictureBox1.Width, pictureBox1.Height);
        }

        public string UserInput { get => textBox1.Text; set => textBox1.Text = value; }


        public void button1_Click(object sender, EventArgs e)
        {
            string userInput = UserInput;
            Parser.ParseProgram(UserInput);
            Program.Run();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var bmp = (Bitmap)canvas.getBitmap();
            e.Graphics.DrawImage((Image)canvas.getBitmap(), 0, 0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
