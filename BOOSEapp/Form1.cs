using System.ComponentModel;
using BOOSE;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Main user interface for the BOOSE interpreter.
    /// Allows the user to input BOOSE programs, execute them,
    /// and display graphical output on the canvas.
    /// </summary>
    public partial class Form1 : Form
    {
        private AppCanvas canvas;
        private CommandFactory Factory;
        private StoredProgram Program;
        private IParser Parser;

        /// <summary>
        /// Initializes the main form and sets up BOOSE components.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            Debug.WriteLine("BOOSE Application Started");
            Debug.WriteLine(AboutBOOSE.about());

            canvas = new AppCanvas(2500, 1500);
            Factory = new AppCommandFactory(canvas);
            Program = new StoredProgram(canvas);
            Parser = new Parser(Factory, Program);
            LogInDebug.Instance.Log("Application started");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas.Set(pictureBox1.Width, pictureBox1.Height);


        }

        /// <summary>
        /// Gets or sets the BOOSE program entered by the user.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UserInput
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                canvas.Clear();

                LogInDebug.Instance.Log("Executing program");

                IInterpreter interpreter = GetInterpreter(UserInput);

                LogInDebug.Instance.Log("Interpreter selected: " + interpreter.GetType().Name);

                interpreter.Run(UserInput);

                pictureBox1.Invalidate();

                LogInDebug.Instance.Log("Program finished");
            }
            catch (Exception ex)
            {
                LogInDebug.Instance.Log("Error: " + ex.Message);

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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            canvas.Clear();            
            pictureBox1.Invalidate(); 
        }


        /// <summary>
        /// Selects the appropriate interpreter strategy based on the program content.
        /// </summary>
        /// <param name="input">Program entered by the user.</param>
        /// <returns>An interpreter implementing the selected strategy.</returns>
        private IInterpreter GetInterpreter(string input)
        {
            string lower = input.ToLower();

            return lower.Contains("method") || lower.Contains("call") ? new Method(canvas)
                 : lower.Contains("if") || lower.Contains("while") || lower.Contains("for") ? new Array(canvas)
                 : lower.Contains("real") ? new Real(canvas)
                 : new Int(canvas);
        }
    }
}