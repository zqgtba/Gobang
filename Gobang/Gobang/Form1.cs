using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using ClassLibrary;
using System.IO;

namespace Gobang
{
    public partial class Form1 : Form
    {
        GobangClass gobang = new GobangClass();
        //开关1，是否点击
        bool capture1 = false;
        //开关2，人人对战
        bool capture2 = false;
        //开关3，暂停
        bool capture3 = true;
        //开关4，控制AI
        bool capture4 = false;
        //开关5，控制AI2.0
        bool capture5 = false;
        //开关6，复盘专用
        bool capture6 = false;
        //开关7，读谱专用
        bool capture7 = false;
        //开关8,落子前
        bool capture8 = false;
        //初始化棋盘位置
        private int leftX = 50;
        private int leftY = 50;
        //记录棋子的线性表
        List<Point> pieces = new List<Point>(225);
        List<Color> piecesColor = new List<Color>(225);
        int n=0;
        //复盘专用
        int m, n0, n1;
        //模式（0代表无，1代表人人模式，2代表人机-简单，3代表人机-复杂）
        int model = 0;
        //预落子红标
        Point p0 = new Point();

        public Form1()
        {
            InitializeComponent();
        }
      
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black, 1);
            //画棋盘
            for (int i = 0; i < 15; i++)
            {
                g.DrawLine(p, leftX + i * 30, leftY, leftX + i * 30, leftY + gobang.Length);
                g.DrawLine(p, leftX, leftY + i * 30, leftX + gobang.Length, leftY + i * 30);
            }
            SolidBrush a = new SolidBrush(Color.Black);
            g.FillEllipse(a, leftX + 3 * 30 - 3, leftY + 3 * 30 - 3, 6, 6);
            g.FillEllipse(a, leftX + 11 * 30 - 3, leftY + 3 * 30 - 3, 6, 6);
            g.FillEllipse(a, leftX + 3 * 30 - 3, leftY + 11 * 30 - 3, 6 ,6);
            g.FillEllipse(a, leftX + 11 * 30 - 3, leftY + 11 * 30 - 3, 6, 6);
            g.FillEllipse(a, leftX + 7 * 30 - 3, leftY + 7 * 30 - 3, 6, 6);
            if (capture1 & (capture2|capture4 |capture5|capture6  ) )
            { 
                //画棋子
                for (int i = 0; i < n; i++)
                {
                    SolidBrush pieceb = new SolidBrush(piecesColor[i]);
                    g.FillEllipse(pieceb, new Rectangle(pieces[i].X - gobang.Radix, pieces[i].Y - gobang.Radix, 2 * gobang.Radix, 2 * gobang.Radix));
                }
            }
            if((gobang .Winer !=null)|(gobang .ban >=1)|capture7 )
            {
                //画棋子
                for (int i = 0; i < n; i++)
                {
                    SolidBrush pieceb = new SolidBrush(piecesColor[i]);
                    g.FillEllipse(pieceb, new Rectangle(pieces[i].X - gobang.Radix, pieces[i].Y - gobang.Radix, 2 * gobang.Radix, 2 * gobang.Radix));
                }
            }
            if (capture8)
            {
                //画预落子红标
                Pen pen = new Pen(Color.Red,2);
                g.DrawLine(pen, p0.X - gobang.Radix, p0.Y - gobang.Radix, p0.X - gobang.Radix / 2, p0.Y - gobang.Radix);
                g.DrawLine(pen, p0.X - gobang.Radix, p0.Y - gobang.Radix, p0.X - gobang.Radix, p0.Y - gobang.Radix / 2);
                g.DrawLine(pen, p0.X - gobang.Radix, p0.Y + gobang.Radix, p0.X - gobang.Radix / 2, p0.Y + gobang.Radix);
                g.DrawLine(pen, p0.X - gobang.Radix, p0.Y + gobang.Radix, p0.X - gobang.Radix, p0.Y + gobang.Radix / 2);
                g.DrawLine(pen, p0.X + gobang.Radix, p0.Y - gobang.Radix, p0.X + gobang.Radix / 2, p0.Y - gobang.Radix);
                g.DrawLine(pen, p0.X + gobang.Radix, p0.Y - gobang.Radix, p0.X + gobang.Radix, p0.Y - gobang.Radix / 2);
                g.DrawLine(pen, p0.X + gobang.Radix, p0.Y + gobang.Radix, p0.X + gobang.Radix / 2, p0.Y + gobang.Radix);
                g.DrawLine(pen, p0.X + gobang.Radix, p0.Y + gobang.Radix, p0.X + gobang.Radix, p0.Y + gobang.Radix / 2);
            }
            capture1 = false;
            capture8 = false;
        }      

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            //人人模式
            if (e.X < 470 + 12 & e.Y < 470 + 12 & e.X > 50 - 12 & e.Y > 50 - 12 & capture2 & capture3 )  
            {
                //记录点击位置
                gobang.JudgePosition(e.X, e.Y);
                Point p = new Point(gobang.PieceX, gobang.PieceY);
                //开关，判断是否重复
                bool cap = true;
                if (pieces.Count > 0)
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {
                        if (p == pieces[i])
                        {
                            cap = false;
                        }
                    }
                }
                //写入棋子位置
                if (cap)
                {
                    //打开开关1
                    capture1 = true;
                    pieces.Add(p);
                    n = pieces.Count;
                    //写入棋子颜色
                    if (piecesColor[n - 1] == Color.Black)
                    {
                        piecesColor.Add(Color.White);
                        label3.Text = "白";
                    }
                    else
                    {
                        piecesColor.Add(Color.Black);
                        label3.Text = "黑";
                    }
                    for(int i = 0; i < n; i++)
                    {
                        if (piecesColor[i] == Color.Black)
                            gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 1;
                        else
                            gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 2;
                    }
                    Invalidate();
                    //判断胜负
                    if (pieces.Count > 0)
                    {
                        gobang.JudgeGame((pieces[n - 1].X - 50) / 30 + 4, (pieces[n - 1].Y - 50) / 30 + 4);
                    }
                    if (gobang.Winer != null)
                    {
                        capture2 = false;
                        timer1.Enabled = false;
                    }
                    if (gobang.Winer != null)
                    {
                        MessageBox.Show(gobang.Winer);
                    }
                    Invalidate();
                    if (capture2 & (gobang.cap1 | gobang.cap2 | gobang.cap3)) 
                    {
                        //判断禁手
                        if (pieces.Count > 0)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                gobang.JudgeBan((pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4);
                            }
                        }
                        if (gobang.ban >= 1)
                        {
                            capture2 = false;
                            timer1.Enabled = false;
                        }
                        if (gobang.ban >= 1)
                        {
                            MessageBox.Show("黑子禁手，判输！");
                        }
                        Invalidate();
                    }
                }               
            }
            //人机模式
            if (e.X < 470 + 12 & e.Y < 470 + 12 & e.X > 50 - 12 & e.Y > 50 - 12 & (capture4|capture5 ) & capture3 )
            {
                //记录点击位置
                gobang.JudgePosition(e.X, e.Y);
                Point p = new Point(gobang.PieceX, gobang.PieceY);
                //开关，判断是否重复
                bool cap = true;
                if (pieces.Count > 0)
                {
                    for (int i = 0; i < pieces.Count; i++)
                    {
                        if (p == pieces[i])
                        {
                            cap = false;
                        }
                    }
                }
                //写入棋子位置
                if (cap)
                {
                    //打开开关1
                    capture1 = true;
                    pieces.Add(p);
                    n = pieces.Count;
                    //写入棋子颜色
                    if (piecesColor[n - 1] == Color.Black)
                    {
                        piecesColor.Add(Color.White);
                        label3.Text = "白(机)";
                    }
                    else
                    {
                        piecesColor.Add(Color.Black);
                        label3.Text = "黑(人)";
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (piecesColor[i] == Color.Black)
                            gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 1;
                        else
                            gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 2;
                    }
                    Invalidate();                    
                    //判断胜负
                    if (pieces.Count > 0)
                    {
                        gobang.JudgeGame((pieces[n - 1].X - 50) / 30 + 4, (pieces[n - 1].Y - 50) / 30 + 4);
                    }
                    if (gobang.Winer != null)
                    {
                        if (capture4)
                            capture4 = false;
                        else
                            capture5 = false;
                        timer1.Enabled = false;
                    }
                    if (gobang.Winer != null)
                    {
                        MessageBox.Show(gobang.Winer);
                    }
                    Invalidate();
                    if ((capture4|capture5 ) & (gobang.cap1 | gobang.cap2 | gobang.cap3))
                    {
                        //判断禁手
                        if (pieces.Count > 0)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                gobang.JudgeBan((pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4);
                            }
                        }
                        if (gobang.ban >= 1)
                        {
                            if (capture4)
                                capture4 = false;
                            else
                                capture5 = false;
                            timer1.Enabled = false;
                        }
                        if (gobang.ban >= 1)
                        {
                            MessageBox.Show("黑子禁手，判输！");
                        }
                    }
                    //延迟0.2秒
                    //Thread.Sleep(200);
                    //AI
                    if (capture4|capture5  )
                    {
                        Point p1 = new Point();
                        if (capture4)
                        {
                            //简单模式
                            AI ai1 = new AI();
                            ai1.GetScores(gobang.panel, 2);
                            ai1.BP();
                            p1 = new Point(ai1.BestPosition.pieceX, ai1.BestPosition.pieceY);
                        }
                        else if (capture5)
                        {
                            //复杂模式
                            AI2 ai2 = new AI2();
                            ai2.GetScores(gobang.panel, 2);
                            ai2.BP();
                            p1 = new Point(ai2.BestPosition.pieceX, ai2.BestPosition.pieceY);
                        }
                        //打开开关1
                        capture1 = true;
                        pieces.Add(p1);
                        n = pieces.Count;
                        //写入棋子颜色
                        if (piecesColor[n - 1] == Color.Black)
                        {
                            piecesColor.Add(Color.White);
                            label3.Text = "白(机)";
                        }
                        else
                        {
                            piecesColor.Add(Color.Black);
                            label3.Text = "黑(人)";
                        }
                        for (int i = 0; i < n; i++)
                        {
                            if (piecesColor[i] == Color.Black)
                                gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 1;
                            else
                                gobang.panel[(pieces[i].X - 50) / 30 + 4, (pieces[i].Y - 50) / 30 + 4] = 2;
                        }
                        Invalidate();
                        //判断胜负
                        if (pieces.Count > 0)
                        {
                            gobang.JudgeGame((pieces[n - 1].X - 50) / 30 + 4, (pieces[n - 1].Y - 50) / 30 + 4);
                        }
                        if (gobang.Winer != null)
                        {
                            if (capture4)
                                capture4 = false;
                            else
                                capture5 = false;
                            timer1.Enabled = false;
                        }
                        if (gobang.Winer != null)
                        {
                            MessageBox.Show(gobang.Winer);
                        }
                        Invalidate();
                    }
                }
            }
        }        

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //位置
            label1.Text = e.Location.ToString();
            capture1 = true;
            //预落子红标位置
            if (e.X < 470 + 12 & e.Y < 470 + 12 & e.X > 50 - 12 & e.Y > 50 - 12 & (capture2|capture4 |capture5 )& capture3)
            {
                //记录位置
                gobang.JudgePosition(e.X, e.Y);
                p0 = new Point(gobang.PieceX, gobang.PieceY);
                capture8 = true;
            }
            Invalidate();
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //人人模式
                if (capture2  & capture3)
                {
                    pieces.RemoveAt(pieces.Count - 1);
                    piecesColor.RemoveAt(piecesColor.Count - 1);
                    n -= 1;
                    if (label3.Text == "黑")
                        label3.Text = "白";
                    else
                        label3.Text = "黑";
                    capture1 = true;
                    Invalidate();
                }
                //人机模式
                if ((capture4|capture5 ) & capture3)
                {
                    pieces.RemoveAt(pieces.Count - 1);
                    pieces.RemoveAt(pieces.Count - 1);
                    piecesColor.RemoveAt(piecesColor.Count - 1);
                    piecesColor.RemoveAt(piecesColor.Count - 1);
                    n -= 2;                    
                    capture1 = true;
                    Invalidate();
                }
                //已分胜负
                if(gobang .Winer !=null)
                {
                    MessageBox.Show("棋局已定，不得悔棋！");
                }
            }
            catch
            {
                MessageBox.Show("你无棋可悔！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gobang.Winer == null & pieces.Count > 0 & n1 == 0 & !capture7)
            {
                m = pieces.Count;
                if (button2.Text == "复盘" | button2.Text == "重新复盘")
                    n = 0;
                else
                    n = n0;
                capture3 = false;
                暂停ToolStripMenuItem1.Text = "继续";
                timer1.Enabled = true;
                暂停ToolStripMenuItem1.ForeColor = Color.Red;
            }
            if (gobang.Winer != null | n1 == 1 | capture7)
            {
                n1 = 1;
                gobang.NewGame();
                m = pieces.Count;
                if (button2.Text == "复盘" | button2.Text == "重新复盘")
                    n = 0;
                else
                    n = n0;
                capture3 = false;
                capture6 = true;
                timer1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                button2.Text = "继续复盘";
                n0 = n;
            }
        }        

        private void 人人对战ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //初始化
            timer1.Enabled = false;
            m = 0;
            n0 = 0;
            n1 = 0;
            n = 0;
            model = 1;
            gobang.NewGame();
            capture1 = true;
            capture4 = false;
            capture5 = false;
            capture6 = false;
            capture7 = false;
            capture8 = false;
            pieces.Clear();
            piecesColor.Clear();
            label3.Text = "黑";
            button2.Text = "复盘";
            暂停ToolStripMenuItem1.Text = "暂停";
            暂停ToolStripMenuItem1.ForeColor = Color.Black;
            piecesColor.Add(Color.Black);
            capture3 = true;
            //打开开关2
            if (!capture2)
            {
                capture2 = true;
            }
            Invalidate();
        }

        private void 简单ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //初始化
            timer1.Enabled = false;
            m = 0;
            n0 = 0;
            n1 = 0;
            n = 0;
            model = 2;
            gobang.NewGame();
            capture1 = true;
            capture5 = false;
            capture6 = false;
            capture7 = false;
            capture8 = false;
            pieces.Clear();
            piecesColor.Clear();
            label3.Text = "黑(人)";
            button2.Text = "复盘";
            暂停ToolStripMenuItem.Text = "暂停";
            暂停ToolStripMenuItem.ForeColor = Color.Black;
            piecesColor.Add(Color.Black);
            capture3 = true;
            //打开开关4
            if (!capture4)
            {
                capture4 = true;
            }
            Invalidate();
        }

        private void 困难ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //初始化
            timer1.Enabled = false;
            m = 0;
            n0 = 0;
            n1 = 0;
            n = 0;
            model = 3;
            gobang.NewGame();
            capture1 = true;
            capture4 = false;
            capture6 = false;
            capture7 = false;
            capture8 = false;
            pieces.Clear();
            piecesColor.Clear();
            label3.Text = "黑(人)";
            button2.Text = "复盘";
            暂停ToolStripMenuItem1.Text = "暂停";
            暂停ToolStripMenuItem1.ForeColor = Color.Black;
            piecesColor.Add(Color.Black);
            capture3 = true;
            //打开开关5
            if (!capture5)
            {
                capture5 = true;
            }
            Invalidate();
        }

        private void 读取存档ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string s;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    gobang.NewGame();
                    timer1.Enabled = false;
                    pieces.Clear();
                    piecesColor.Clear();
                    StreamReader srFile = new StreamReader(openFileDialog.FileName);
                    s = srFile.ReadToEnd();
                    string[] ss = s.Split(' ');
                    n = Convert.ToInt32(ss[2]);
                    int j = 3;
                    for (int i = 0; i < n; i++)
                    {
                        Point pp = new Point(Convert.ToInt32(ss[j]), Convert.ToInt32(ss[j + 1]));
                        pieces.Add(pp);
                        j += 2;
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (ss[j] == "1")
                            piecesColor.Add(Color.Black);
                        else if (ss[j] == "2")
                            piecesColor.Add(Color.White);
                        j += 1;
                    }
                    if (ss[0] == "0")
                    {
                        if (ss[1] == "1")
                        {
                            //初始化
                            timer1.Enabled = false; ;
                            model = 1;
                            gobang.NewGame();
                            capture1 = true;
                            capture4 = false;
                            capture5 = false;
                            capture6 = false;
                            capture7 = false;
                            capture8 = false;
                            button2.Text = "复盘";
                            暂停ToolStripMenuItem1.Text = "暂停";
                            暂停ToolStripMenuItem1.ForeColor = Color.Black;
                            capture3 = true;
                            if (piecesColor[n - 1] == Color.White)
                            {
                                piecesColor.Add(Color.Black);
                                label3.Text = "黑";
                            }
                            else
                            {
                                piecesColor.Add(Color.White);
                                label3.Text = "白";
                            }
                            //打开开关2
                            if (!capture2)
                            {
                                capture2 = true;
                            }
                            Invalidate();
                            MessageBox.Show("该棋局为残局（人人模式），未分胜负！");
                        }
                        else if (ss[1] == "2")
                        {
                            //初始化
                            timer1.Enabled = false;
                            model = 2;
                            gobang.NewGame();
                            capture1 = true;
                            capture5 = false;
                            capture6 = false;
                            capture7 = false;
                            capture8 = false;
                            label3.Text = "黑(人)";
                            button2.Text = "复盘";
                            暂停ToolStripMenuItem1.Text = "暂停";
                            暂停ToolStripMenuItem1.ForeColor = Color.Black;
                            piecesColor.Add(Color.Black);
                            capture3 = true;
                            //打开开关4
                            if (!capture4)
                            {
                                capture4 = true;
                            }
                            Invalidate();
                            MessageBox.Show("该棋局为残局（人机模式-简单），未分胜负！");
                        }
                        else if (ss[1] == "3")
                        {
                            //初始化
                            timer1.Enabled = false;
                            model = 3;
                            gobang.NewGame();
                            capture1 = true;
                            capture4 = false;
                            capture6 = false;
                            capture7 = false;
                            capture8 = false;
                            label3.Text = "黑(人)";
                            button2.Text = "复盘";
                            暂停ToolStripMenuItem1.Text = "暂停";
                            暂停ToolStripMenuItem1.ForeColor = Color.Black;
                            piecesColor.Add(Color.Black);
                            capture3 = true;
                            //打开开关5
                            if (!capture5)
                            {
                                capture5 = true;
                            }
                            Invalidate();
                            MessageBox.Show("该棋局为残局（人机模式-困难），未分胜负！");
                        }
                    }
                    else
                    {
                        gobang.NewGame();
                        timer1.Enabled = false;
                        model = 0;
                        button2.Text = "复盘";
                        暂停ToolStripMenuItem1.Text = "暂停";
                        label3.Text = "无";
                        暂停ToolStripMenuItem1.ForeColor = Color.Black;
                        capture1 = true;
                        capture2 = true;
                        capture4 = true;
                        capture5 = true;
                        capture6 = false;
                        capture7 = true;
                        capture8 = false;
                        Invalidate();
                        if (ss[0] == "1")
                        {
                            MessageBox.Show("该棋局黑子胜！");
                        }
                        else if (ss[0] == "2")
                            MessageBox.Show("该棋局白子胜！");
                        capture2 = false;
                        capture4 = false;
                        capture5 = false;
                    }
                    srFile.Close();
                }
                catch
                {
                    MessageBox.Show("Read Error");
                }
            }
        }

        private void 保存棋局ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt(*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter swFile = new StreamWriter(saveFileDialog.FileName);
                    if (gobang.Winer == null)
                    {
                        if (gobang.ban == 0)
                            swFile.Write("0 ");
                        else
                            swFile.Write("2 ");
                    }
                    else if (gobang.Winer == "黑子胜")
                        swFile.Write("1 ");
                    else
                        swFile.Write("2 ");
                    swFile.Write(model.ToString() + " ");
                    swFile.Write(n.ToString() + " ");
                    for (int i = 0; i < n; i++)
                    {
                        swFile.Write(pieces[i].X.ToString() + " ");
                        swFile.Write(pieces[i].Y.ToString() + " ");
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (piecesColor[i] == Color.Black)
                            swFile.Write("1 ");
                        else
                            swFile.Write("2 ");
                    }
                    swFile.Close();
                }
                catch
                {
                    MessageBox.Show("Write Error");
                }
            }
        }

        private void 图像1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BackgroundImage = Properties.Resources._104731_No80_580100;
        }

        private void 图像2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BackgroundImage = Properties.Resources.b13edebdb27170c8a68f93c732165c9f;
        }

        private void 图像3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BackgroundImage = Properties.Resources.TIM图片20191204234210;
        }

        private void 暂停ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture2 | capture4 | capture5)
            {
                if (capture3)
                {
                    capture3 = false;
                    暂停ToolStripMenuItem1.Text = "继续";
                }
                else
                {
                    capture3 = true;
                    暂停ToolStripMenuItem1.Text = "暂停";
                    button2.Text = "复盘";
                    暂停ToolStripMenuItem1.ForeColor = Color.Black;
                    timer1.Enabled = false;
                }
            }
        }

        private void 终止ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gobang.NewGame();
            m = 0; timer1.Enabled = false;
            n0 = 0;
            n1 = 0;
            n = 0;
            model = 0;
            pieces.Clear();
            piecesColor.Clear();
            button2.Text = "复盘";
            暂停ToolStripMenuItem1.Text = "暂停";
            label3.Text = "无";
            暂停ToolStripMenuItem1.ForeColor = Color.Black;
            capture1 = true;
            capture2 = true;
            capture4 = true;
            capture5 = true;
            capture6 = false;
            capture8 = false;
            Invalidate();
            capture2 = false;
            capture4 = false;
            capture5 = false;
        }

        private void 三三禁手关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!gobang.cap1)
            {
                gobang.cap1 = true;
                三三禁手关闭ToolStripMenuItem.Text = "三三禁手：开启";
                三三禁手关闭ToolStripMenuItem.ForeColor = Color.Green;
            }
            else
            {
                gobang.cap1 = false;
                三三禁手关闭ToolStripMenuItem.Text = "三三禁手：关闭";
                三三禁手关闭ToolStripMenuItem.ForeColor = Color.Red;
            }
        }

        private void 四四禁手关闭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!gobang.cap2)
            {
                gobang.cap2 = true;
                四四禁手关闭ToolStripMenuItem1.Text = "四四禁手：开启";
                四四禁手关闭ToolStripMenuItem1.ForeColor = Color.Green;
            }
            else
            {
                gobang.cap2 = false;
                四四禁手关闭ToolStripMenuItem1.Text = "四四禁手：关闭";
                四四禁手关闭ToolStripMenuItem1.ForeColor = Color.Red;
            }
        }

        private void 长连禁手关闭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!gobang.cap3)
            {
                gobang.cap3 = true;
                长连禁手关闭ToolStripMenuItem1.Text = "长连禁手：开启";
                长连禁手关闭ToolStripMenuItem1.ForeColor = Color.Green;
            }
            else
            {
                gobang.cap3 = false;
                长连禁手关闭ToolStripMenuItem1.Text = "长连禁手：关闭";
                长连禁手关闭ToolStripMenuItem1.ForeColor = Color.Red;
            }
        }

        private void 游戏规则ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("黑子先行，五连为胜\n若无禁手，则六连，七连等也判胜。\n白子后行，五连，六连等皆判胜。");
        }

        private void 禁手规则ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("三三禁手：黑子有两个相连活三则判负。\n四四禁手：黑子有两个相连四连在判负。\n长连禁手：黑子有六连等在判负。\n先五为胜：黑子先得到五连则不考虑禁手。");
        }

        private void 运行问题提示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在棋局中，若鼠标在棋盘上没有红标，则点一下暂停或继续！！！");
        }

        private void 开发人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("数试81  谢国庆  2184111520");
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("C#大作业");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (n < m)
                n++;
            if (n == m)
                button2.Text = "重新复盘";
            capture1 = true;
            Invalidate();
        }



        //以下全部为无效代码，但不可删除！！！
        private void Form1_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void 人人对战ToolStripMenuItem_Click_1(object sender, EventArgs e) { }
        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 终止ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 简单ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 困难ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 三三禁手ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 四四禁手关闭ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 长连禁手关闭ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 保存棋局ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 读取存档ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 图像2ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 图像1ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 图像3ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 预落子红标开启ToolStripMenuItem_Click(object sender, EventArgs e) { }
    }
}
