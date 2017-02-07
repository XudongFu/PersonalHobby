using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 扫雷
{
    public partial class Main : Form
    {
        Timer usedTime;
        long time = 0;
        string show = "";
        int sum=0;
        int width=0;
        int height=0;

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            usedTime = new Timer();
            time = 0;
            usedTime.Interval = 1000;
            usedTime.Enabled = true;
            usedTime.Tick += timeChange;
            try {
                sum = int.Parse(textBox4.Text);
            }
            catch (Exception)
            {
                sum = 0;
            }
            try
            {
                width = int.Parse(textBox3.Text);
            }
            catch (Exception)
            {
                width = 0;
            }
            try
            {
                height = int.Parse(textBox2.Text);
            }
            catch (Exception)
            {
                height = 0;
            }
            textBox2.Text = height.ToString();
            textBox3.Text = width.ToString();
            textBox4.Text = sum.ToString();
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }


        void refresh(object send,int [,] data)
        {

        }



        void timeChange(object send,EventArgs e)
        {
            time++;
            long temp = time;
            show = (temp / 3600).ToString() + "："+(temp / 60).ToString() + "：" + (temp % 60).ToString();
            textBox1.Text = show;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
