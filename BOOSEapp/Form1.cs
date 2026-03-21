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

                if (interpreter != null)
                {
                    LogInDebug.Instance.Log("Interpreter selected: " + interpreter.GetType().Name);
                    interpreter.Run(UserInput);
                }
                else
                {
                    LogInDebug.Instance.Log("Using BOOSE parser");
                    Parser.ParseProgram(UserInput);
                    Program.Run();
                }

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

            if (lower.Contains("method ") || lower.Contains("call "))
                return new Method(canvas);

            if (lower.Contains("if ") || lower.Contains("else") || lower.Contains("end if") ||
                lower.Contains("while ") || lower.Contains("end while") ||
                lower.Contains("for ") || lower.Contains("end for") ||
                lower.Contains("array ") || lower.Contains("boolean") ||
                lower.Contains("random ") || lower.Contains("input ") ||
                lower.Contains("colour ") || lower.Contains("fill") ||
                lower.Contains("tri "))
                return new Array(canvas);

            if (lower.Contains("real "))
                return new Real(canvas);

            if (lower.Contains("int "))
                return new Int(canvas);

            return null;
        }
    }
}