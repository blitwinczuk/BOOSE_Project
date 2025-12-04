using BOOSE;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        private AppCanvas canvas;
        private CommandFactory Factory;
        private StoredProgram Program;
        private IParser Parser;

        public Form1()
        {
            InitializeComponent();

            Debug.WriteLine("BOOSE Application Started");
            Debug.WriteLine(AboutBOOSE.about());

            // Set up canvas, factory, stored program and parser
            canvas = new AppCanvas(1200, 750);
            Factory = new AppCommandFactory(canvas);
            Program = new StoredProgram(canvas);
            Parser = new Parser(Factory, Program);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Make the canvas match the picture box size
            canvas.Set(pictureBox1.Width, pictureBox1.Height);
        }

        /// <summary>
        /// Gets or sets the BOOSE program text entered by the user.
        /// </summary>
        public string UserInput
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        /// <summary>
        /// Run button click handler: parses and runs the BOOSE program,
        /// then redraws the canvas.
        /// </summary>
        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Parser.ParseProgram(UserInput);
                Program.Run();
                pictureBox1.Invalidate(); // triggers Paint ? draws canvas
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show(ex.Message, "BOOSE Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var bmp = (Bitmap)canvas.getBitmap();
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmp, 0, 0);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // optional: add interactions if you want later
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // optional: e.g. live validation
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // optional – only if you add textBox2 in the designer
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // optional – only if you add panel1 in the designer
        }
    }
}
