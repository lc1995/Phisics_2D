using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Phisics_2D
{
    public partial class Form1 : Form
    {
        // 用于测试的单位
        private PhisicalObject_2D pig = new PhisicalObject_2D(450, 480, 20, 20, "", 0, 0);

        private WallObject_2D wall_0 = new WallObject_2D(100, 400, 50, 20, "");
        private WallObject_2D wall_1 = new WallObject_2D(200, 400, 50, 20, "");
        private WallObject_2D wall_2 = new WallObject_2D(300, 400, 50, 20, "");
        private WallObject_2D wall_3 = new WallObject_2D(400, 400, 50, 20, "");
        private WallObject_2D wall_4 = new WallObject_2D(150, 300, 50, 20, "");
        private WallObject_2D wall_5 = new WallObject_2D(250, 300, 50, 20, "");
        private WallObject_2D wall_6 = new WallObject_2D(350, 300, 50, 20, "");
        private WallObject_2D wall_7 = new WallObject_2D(200, 200, 50, 20, "");
        private WallObject_2D wall_8 = new WallObject_2D(300, 200, 50, 20, "");
        private WallObject_2D wall_9 = new WallObject_2D(250, 100, 50, 20, "");

        // 屏幕外墙，不需要绘制
        private WallObject_2D wallUpside = new WallObject_2D(-100, -100, 700, 100, "");
        private WallObject_2D wallDownside = new WallObject_2D(-100, 500, 700, 100, "");
        private WallObject_2D wallLeftside = new WallObject_2D(-100, 0, 100, 500, "");
        private WallObject_2D wallRightside = new WallObject_2D(500, 0, 100, 500, "");

        // 数组，用于保存所有非重复键值
        private ArrayList aryKeys = new ArrayList();

        private GameScene gameScene;

        public Form1()
        {
            InitializeComponent();

            // 初始化游戏画面
            gameScene = new GameScene();
            gameScene.Location = new Point(10, 10);
            gameScene.Size = new Size(500, 500);
            gameScene.BackColor = Color.Black;
            gameScene.Paint += gameScene_Paint;
            Controls.Add(gameScene);

        }

        // 物理引擎计时
        private void phsicalTimer_Tick(object sender, EventArgs e)
        {
            // 重力加速度，更新Y方向速度
            if(pig.HasGravity())
                pig.YSp += 0.4;

            // 更新位置
            pig.PreY += pig.YSp;
            pig.Y = (int)pig.PreY;
            pig.PreX += pig.XSp;
            pig.X = (int)pig.PreX;

            gameScene.Invalidate();

            // 碰撞检测并回滚位置
            WallObject_2D w = pig.IsCrashedWithWall();
            bool isCrash = false;
            while (pig.IsCrashedWithWall() != null)
            {
                // 放慢100倍，往回补偿，精度为1像素
                pig.PreY -= pig.YSp / 100;
                pig.PreX -= pig.XSp / 100;
                pig.Y = (int)pig.PreY;
                pig.X = (int)pig.PreX;

                // 发生碰撞
                isCrash = true;
            }

            // 碰撞方向检测
            if(isCrash && w != null)
            {
                // 右侧碰撞
                if((pig.X + pig.Width) <= w.X)
                {
                    pig.XSp = 0;
                }
                // 左侧碰撞
                else if(pig.X >= (w.X + w.Width))
                {
                    pig.XSp = 0;
                }
                // 底部碰撞
                else if((pig.Y + pig.Height) <= w.Y)
                {
                    pig.YSp = 0;
                }
                // 顶部碰撞
                else if(pig.Y >= (w.Y + w.Height))
                {
                    pig.YSp = 0;
                }
            }
        }

        // 自定义游戏画面控件，双缓冲
        class GameScene : PictureBox
        {
            public GameScene() : base()
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                                           ControlStyles.AllPaintingInWmPaint,
                                           true);

                this.UpdateStyles();
            }
        }

        // 游戏画面重绘
        private void gameScene_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle rect = new Rectangle(pig.X, pig.Y, pig.Width, pig.Height);
            SolidBrush brush = new SolidBrush(Color.Red);

            g.FillRectangle(brush, rect);

            SolidBrush brush2 = new SolidBrush(Color.Yellow);
            foreach(WallObject_2D w in WallObject_2D.aryWall2D)
            {
                rect = new Rectangle(w.X, w.Y, w.Width, w.Height);
                g.FillRectangle(brush2, rect);
            }
        }

        // 按键响应
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (aryKeys.Contains(e.KeyCode))
                return;

            aryKeys.Add(e.KeyCode);
            switch(e.KeyCode)
            {
                case Keys.W:
                    pig.YSp = -10;
                    break;
                case Keys.A:
                    pig.XSp = -5;
                    break;
                case Keys.D:
                    pig.XSp = 5;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            aryKeys.Remove(e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.W:
                    break;
                case Keys.A:
                    pig.XSp = 0;
                    break;
                case Keys.D:
                    pig.XSp = 0;
                    break;
            }
        }
    }
}
