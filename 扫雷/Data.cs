using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 扫雷
{
   public class Data
    {
        /// <summary>
        /// 高度和宽度
        /// </summary>
        int height;
        int width;

        public delegate void gameover(object send,int [,] info);

        public gameover over;

        public gameover refresh;

        int notDig = -1;
        int marked = 9;
        int has = 1;
        int notHas = 0;
        /// <summary>
        /// 设定雷的数目
        /// </summary>
        int sum;

        /// <summary>
        /// 没有被挖的区域的总数
        /// </summary>
        int left;
        /// <summary>
        /// 不同位置是否包含雷
        /// </summary>
        int[,] boom;

        /// <summary>
        /// 包含各个坑的挖掘状态
        /// -1代表没有挖掘，需要进行优化，第一次肯定不会点到雷
        /// 0到8代表周围有几个雷，
        /// 9代表标记这里是雷
        /// 10 代表被挖开，并且这里是雷
        /// </summary>
        int[,] dig;

        /// <summary>
        /// 帮助玩家分析概率分布
        /// </summary>
        int[,] direct;
        

        public Data()
        {

        }

        /// <summary>
        /// 根据雷的数量，初始化随机生成雷
        /// </summary>
        public void init()
        {
            boom = new int[width,height];
            dig = new int[width,height];
            for (int i=0;i<width;i++)
            {
                for (int n=0;n<height;n++)
                {
                    dig[i, n] = notDig;
                }
            }
            direct = new int[width,height];
            Random ran = new Random(DateTime.Now.Millisecond);
            while(sum!=0)
            {
                int w = ran.Next(0, width);
                int h = ran.Next(0, height);
                if (boom[w,h]==0)
                {
                    boom[w, h] = 1;
                    sum--;
                }
            }
            left = width * height;
        }

        void mark(int w,int h)
        {
            if (w < width && h < height)
            {
                //如果没有被挖的话
                if (dig[w, h] == notDig)
                {
                    dig[w, h] = marked;
                } else if (dig[w,h]==marked)
                {
                    dig[w, h] = notDig;
                }
            }
            else
            {
                throw new Exception("参数错误");
            }
        }


        public void SetWidthHeight(int w,int h)
        {
            this.width = w;
            this.height = h;
        }

        public void SetBoomSum(int sum)
        {
            this.sum = sum;
        }

        /// <summary>
        /// 挖掘某个位置，自动更新dig数组
        /// </summary>
        /// <param name="height"></param>
        /// <param name="weight"></param>
        public void digging(int w,int h)
        {
            if (w >= 0 && w < width && h >= 0 && h < height)
            {
                if ((dig[w, h] == notDig || dig[w, h] == marked) && boom[w, h] == notHas)
                {
                        solveDig(w,h);
                }
                else
                {
                    if (over != null)
                        over(this,boom);
                }
            }
            else
            {
                throw new Exception("参数错误");
            }
        }


        private void solveDig(int w,int h)
        {
            if (boom[w,h]==notHas)
            {
                int sum = 0;
                for (int i=w-1;i<=w+1;i++)
                {
                    for (int n=h-1;n<=h+1;n++)
                    {
                        try {
                            if (boom[i, n] == has)
                                sum++;
                        }
                        catch (IndexOutOfRangeException e)
                        {

                        }
                    }
                }
                if(sum!=0)
                dig[w, h] = sum;
                //当点击的区域没有雷的时候
                else
                {
                    expand(w,h);
                }
            }
            else
                throw new Exception("参数错误");
            
        }

        /// <summary>
        /// 纯白区域扩展
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        private void expand(int w,int h)
        {
            if(w>=0 && w<width && h>=0 && h<height){   
                if ((dig[w,h]==notDig || dig[w, h] == marked) && boom[w,h]==notHas){
                    int sum = 0;
                    for (int i = w - 1; i <= w + 1; i++)
                    {
                        for (int n = h - 1; n <= h + 1; n++)
                        {
                            try
                            {
                                if (boom[i, n] == has)
                                    sum++;
                            }
                            catch (IndexOutOfRangeException){}
                        }
                    }
                    //
                    dig[w, h] = sum;
                    if (sum==0)
                    {
                        for (int i = w - 1; i <= w + 1; i++)
                        {
                            for (int n = h - 1; n <= h + 1; n++)
                            {
                                expand(i,n);
                            }
                        }
                    }
                
                }
            }
        }




    }
}
