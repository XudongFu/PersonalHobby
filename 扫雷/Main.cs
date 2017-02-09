using System;
using System.Drawing;
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
        Color digged = Color.ForestGreen;

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
                setBackColor(boomPics[i],digged);
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

        void over(object sender,ref int[,] data)
        {

            MessageBox.Show("游戏结束");
            gameHeight.Enabled = true;
            gameWidth.Enabled = true;
            boomSum.Enabled = true;
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
                sum = int.Parse(boomSum.Text);
            }
            catch (Exception)
            {
                sum = 15;
            }
            try
            {
                width = int.Parse(gameWidth.Text);
            }
            catch (Exception)
            {
                width = 15;
            }
            try
            {
                height = int.Parse(gameHeight.Text);
            }
            catch (Exception)
            {
                height = 15;
            }
            gameHeight.Text = height.ToString();
            gameWidth.Text = width.ToString();
            boomSum.Text = sum.ToString();
            gameHeight.Enabled = false;
            gameWidth.Enabled = false;
            boomSum.Enabled = false;
            data.SetBoomSum(sum);
            data.init();
            data.SetWidthHeight(width,height);
            data.updateView();
        }


        void refresh(object send,ref int [,] data)
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
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            data.SetWidthHeight(15, 15);
            data.SetBoomSum(15);
            width = 15;
            height = 15;
            data.refresh = refresh;
            data.over = over;
            //雷的数量
            boomSum.Text = 20.ToString();
            gameWidth.Text = 15.ToString();
            gameHeight.Text = 15.ToString();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int widSpace = picture.Width / width;
            int heiSpace = picture.Height / height;
            int w = e.X / widSpace;
            int h = e.Y / heiSpace;
            if (e.Button==MouseButtons.Left)
            {
                data.digging(w, h);
            }
            if (e.Button==MouseButtons.Right)
            {
                data.mark(w,h);
            }
            data.updateView();
        }
    }
}
