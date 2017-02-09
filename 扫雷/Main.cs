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
        int sum = 0;
        int width = 0;
        int height = 0;
        int picHeight = 500;
        int picWidth = 500;
        Bitmap picture;
        Graphics pen;
        Bitmap[] boomPics;
        Bitmap notDig;
        Bitmap boom;
        Bitmap marded;


        public Main()
        {
            InitializeComponent();
        }


        void initResource()
        {
            picture = new Bitmap(picWidth,picHeight);
            SolidBrush brush = new SolidBrush(Color.Blue);
            Font ziti = new Font("黑体", 100);
            int w=150;
            int h = 150;
            boomPics = new Bitmap[9];
            for (int i = 0; i < 9; i++)
            {
                boomPics[i] = new Bitmap(w, h);
                Graphics pen = Graphics.FromImage(boomPics[i]);
                if (i != 0)
                    pen.DrawString(i.ToString(), ziti, brush, new PointF(15, 15));
            }
            boom = new Bitmap(w,h);
            marded = new Bitmap(w,h);
            notDig = new Bitmap(w,h);
            setBackColor(boom,Color.Red);
            setBackColor(marded,Color.Blue);
            setBackColor(notDig,Color.DarkSeaGreen);
            setBackColor(picture,Color.White);
        }

        void setBackColor(Bitmap tu,Color color)
        {
            for (int i=0;i<tu.Width;i++)
            {
                for (int n=0;n<tu.Height;n++)
                {
                    tu.SetPixel(i, n, color);
                }
            }
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
            
            data.updateView();
        }


        void refresh(object send,int [,] data)
        {
            int widSpace = picture.Width / width;
            int heiSpace = picture.Height / height;

            for (int i=0;i<width; i++)
            {
                for (int n=0;n<height;n++)
                {
                    Rectangle zone = new Rectangle(new Point(widSpace*i+2,n*heiSpace+2), new Size(widSpace-4,heiSpace-4));

                    if (data[i, n] >= 0 && data[i, n] <= 8)
                        pen.DrawImage(boomPics[data[i, n]], zone);
                    if (data[i,n]==-1)
                    {
                        pen.DrawImage(notDig, zone);
                    }
                    if (data[i, n] == 9)
                    {
                        pen.DrawImage(marded,zone);
                    }
                    if (data[i,n]==10)
                    {
                        pen.DrawImage(boom,zone);
                    }
                }
            }
            pictureBox1.Image = picture;
        }



        void timeChange(object send,EventArgs e)
        {
            time++;
            long temp = time;
            show = (temp / 3600).ToString() + "："+(temp / 60).ToString() + "：" + (temp % 60).ToString();
            textBox1.Text = show;
        }

        Data data = new Data();


        private void Main_Load(object sender, EventArgs e)
        {
            initResource();
            pen = Graphics.FromImage(picture);
            pictureBox1.Image = picture;
            data.SetWidthHeight(15, 15);
            data.SetBoomSum(15);
            data.init();
            width = 15;
            height = 15;
            data.refresh = refresh;
        }

    }
}
