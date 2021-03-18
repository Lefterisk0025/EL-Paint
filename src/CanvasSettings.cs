using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exercise7
{
    public partial class CanvasSettings : Form
    {
        Form1 form1;
        public CanvasSettings()
        {
            InitializeComponent();
        }
        public CanvasSettings(Form1 form1, string sizeX, string sizeY)
        {
            InitializeComponent();

            this.form1 = form1;
            textBox1.Text = sizeX;
            textBox2.Text = sizeY;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width = int.Parse(textBox1.Text);
            int height = int.Parse(textBox2.Text);

            if (width <= 1650 && height <= 950)
                form1.SetCanvasSize(width, height);
            else
                MessageBox.Show("Invalid size. Maximum size: 1650, 950");
        }
    }
}
