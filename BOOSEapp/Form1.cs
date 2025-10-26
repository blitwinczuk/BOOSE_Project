using BOOSE;
using System.Diagnostics;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        AppCanvas canvas;
        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());
            canvas = new AppCanvas(800, 450);
     
            

        }

    }
}
