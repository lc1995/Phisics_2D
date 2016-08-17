using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Phisics_2D
{
    // 2D对象类
    abstract class _2DObject : object
    {
        // 属性
        // 精确横坐标，更新实际横坐标
        public double PreX { set; get; }
        // 精确纵坐标，更新实际纵坐标
        public double PreY { set; get; }
        // 实际横坐标
        public int X { set; get; }
        // 实际纵坐标
        public int Y { set; get; }
        // 宽度
        public int Width { set; get; }
        // 高度
        public int Height { set; get; }

        // 贴图文件
        public string TextureFile { set; get; }

        // 方法
        // 构造函数
        public _2DObject(double x = 0, double y = 0, int width = 0, int height = 0, string textureFile = "")
        {
            PreX = x;
            PreY = y;
            X = (int)PreX;
            Y = (int)PreY;
            Width = width;
            Height = height;
            TextureFile = textureFile;
        }

        // 是否超出边界
        public bool IsOutOfRange(int XRange, int YRange)
        {
            if (X >= 0 && Y >= 0 && X <= (XRange - Height) && Y <= (YRange - Height))
                return false;
            else
                return true;
        }
        // 是否碰撞
        public bool IsCrashed(_2DObject obj)
        {
            System.Drawing.Rectangle rect1 = new System.Drawing.Rectangle(X, Y, Height, Height);
            System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(obj.X, obj.Y, obj.Height, obj.Height);
            if (rect1.IntersectsWith(rect2))
                return true;
            else
                return false;
        }
    }

    class PhisicalObject_2D : _2DObject
    {
        // X，Y方向速度，最大值为100
        private double xSp;
        private double ySp;
        public double XSp
        {
            get
            {
                return xSp;
            }
            set
            {
                if (value <= 100 && value >= -100)
                    xSp = value;
                else if (value > 100)
                    xSp = 100;
                else
                    xSp = -100;
            }
        }
        public double YSp
        {
            get
            {
                return ySp;
            }
            set
            {
                if (value <= 100 && value >= -100)
                    ySp = value;
                else if (value > 100)
                    ySp = 100;
                else
                    ySp = -100;
            }
        }

        // 静态数组，存放所有2D物理对象
        public static List<PhisicalObject_2D> aryPhic2D = new List<PhisicalObject_2D>();

        // 构造函数
        public PhisicalObject_2D(double x = 0, double y = 0, int width = 0, int height = 0, string textureFile = "",
            double x_Sp = 0, double y_Sp = 0) : base(x, y, width, height, textureFile)
        {
            XSp = x_Sp;
            YSp = y_Sp;

            aryPhic2D.Add(this);
        }

        // 是否与墙体碰撞，如果是，返回墙体
        public WallObject_2D IsCrashedWithWall()
        {
            Rectangle rect1 = new Rectangle(X, Y, Width, Height);
            Rectangle rect2;

            foreach (WallObject_2D w in WallObject_2D.aryWall2D)
            {
                rect2 = new Rectangle(w.X, w.Y, w.Width, w.Height);
                if (rect1.IntersectsWith(rect2))
                    return w;
            }

            return null;
        }

        // 是否有重力加速度
        public bool HasGravity()
        {
            foreach(WallObject_2D w in WallObject_2D.aryWall2D)
            {
                if (Y == (w.Y - Height))
                {
                    if ((X < w.X + w.Width) && (X + Width > w.X))
                        return false;
                }
            }
            return true;
        }
    }

    class WallObject_2D : _2DObject
    {
        // 静态数组，存放所有2D墙体对象
        public static List<WallObject_2D> aryWall2D = new List<WallObject_2D>();

        // 构造函数
        public WallObject_2D(double x = 0, double y = 0, int width = 0, int height = 0, string textureFile = "") : base(x, y, width, height, textureFile)
        {
            aryWall2D.Add(this);
        }
    }
}
