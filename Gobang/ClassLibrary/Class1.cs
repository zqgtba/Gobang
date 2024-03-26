using System;

namespace ClassLibrary
{
    //颜色的枚举
    public enum PieceColor { Empty, Black, White }
    //棋子的结构体
    public struct Piece
    {
        public int pieceX;
        public int pieceY;
        public PieceColor pieceColor;
    }
    public class GobangClass
    { 
        //棋盘的阶
        private int rank = 15;
        public int Rank { get { return rank; } }
        //棋盘的长，宽
        private int length = 420;
        public int Length { get { return length; } }
        //棋子的半径
        private int radix = 12;
        public int Radix { get { return radix; } }
        //胜者
        private string winer;
        public string Winer { get { return winer; } }
        //单个棋子
        private Piece piece = new Piece ();       
        public int PieceX { get { return piece.pieceX; } }
        public int PieceY { get { return piece.pieceY; } }
        //棋局矩阵（0代表空，1代表黑子，2代表白子）
        public int[,] panel = new int[23, 23];
        //开关1，控制三三禁手
        public bool cap1 = false;
        //开关2，控制四四禁手
        public bool cap2 = false;
        //开关3，控制长连禁手
        public bool cap3 = false;
        public int ban;
        //初始化
        public void NewGame()
        {
            ban = 0;
            winer = null;
            piece.pieceX = 0;
            piece.pieceY = 0;
            piece.pieceColor = PieceColor.Empty;
            for(int i=0;i<22;i++)
            {
                for(int j=0;j<22;j++)
                {
                    panel[i, j] = 0;
                }
            }
        }
        public void NewWiner()
        {
            winer = null;
        }
        //判断点击位置
        public void JudgePosition(int clickX,int clickY)
        {
            piece.pieceX = (((clickX - 50) + 15) / 30) * 30 + 50;
            piece.pieceY = (((clickY - 50) + 15) / 30) * 30 + 50;
        }
        //判断禁手
        public void JudgeBan(int x,int y)
        {
            int ban1 = 0;
            int ban2 = 0;
            int ban3 = 0;
            //只判断黑子
            if(panel [x,y]==1)
            {
                AI aiBan = new AI();
                aiBan.Calculate(panel, 1, x, y);
                if (cap1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (aiBan.num[i] == 4 & aiBan.a[i] == 0)
                            ban1 += 1;                        
                    }
                }
                if(cap2)
                {
                    for(int i=0;i<4;i++)
                    {
                        if (aiBan.num[i] == 5)
                            ban2 += 1;
                    }
                }
                if(cap3)
                {
                    for(int i=0;i<4;i++)
                    {
                        if (aiBan.num[i] >= 7)
                            ban3 += 2;
                    }
                }
                if (ban1 >= 2 | ban2 >= 2 | ban3 >= 2)
                    ban += 1;
            }
        }
        //判断棋局胜负
        public void JudgeGame(int x,int y)
        {
            int color = panel[x, y];
            int num1 = 0, num2 = 0, num3 = 0, num4 = 0;
            //水平方向
            for(int i=0;i<23-x ;i++)
            {
                if (panel[x + i, y] == color)
                    num1 += 1;
                else
                    break;
            }
            for(int j=0;j<x ;j++)
            {
                if (panel[x - j, y] == color)
                    num1 += 1;
                else
                    break;
            }
            //竖直方向
            for (int i = 0; i < 23 - y; i++)
            {
                if (panel[x , y+i] == color)
                    num2 += 1;
                else
                    break;
            }
            for (int j = 0; j < y; j++)
            {
                if (panel[x , y-j] == color)
                    num2 += 1;
                else
                    break;
            }
            //斜向上方向
            for (int i = 0; i < 23 - x&i<y; i++)
            {
                if (panel[x + i, y-i] == color)
                    num3 += 1;
                else
                    break;
            }
            for (int j = 0; j < x&j<23-y; j++)
            {
                if (panel[x - j, y+j] == color)
                    num3 += 1;
                else
                    break;
            }
            //斜向下方向
            for (int i = 0; i < 23 - x&i<23-y; i++)
            {
                if (panel[x + i, y+i] == color)
                    num4 += 1;
                else
                    break;
            }
            for (int j = 0; j < x&j<y; j++)
            {
                if (panel[x - j, y-j] == color)
                    num4 += 1;
                else
                    break;
            }
            if (cap3)
            {
                if ((num1 == 6 | num2 == 6 | num3 == 6 | num4 == 6) & color == 1)
                    winer = "黑子胜";
                if ((num1 >= 6 | num2 >= 6 | num3 >= 6 | num4 >= 6) & color == 2)
                    winer = "白子胜";
            }
            else if(!cap3)
            {
                if ((num1 >= 6 | num2 >= 6 | num3 >= 6 | num4 >= 6) & color == 1)
                    winer = "黑子胜";
                if ((num1 >= 6 | num2 >= 6 | num3 >= 6 | num4 >= 6) & color == 2)
                    winer = "白子胜";
            }
        }
    }
    //简单模式
    public class AI
    {
        //四个方向的分数以及总分
        public int num1, a1;
        public int num2, a2;
        public int num3, a3;
        public int num4, a4;
        public int score;
        public int[] num = new int[4];
        public int[] scores = new int[4];
        public int[] a = new int[4];
        public int[,] Scores = new int[23, 23];
        //初始化
        public AI()
        {
            num1 = 0;num2 = 0;num3 = 0;num4 = 0;
            a1 = 0;a2 = 0;a3 = 0;a4 = 0;
            score = 0;
            for(int i=0;i<4;i++)
            {
                num[i] = 0;
            }
            for(int i=0;i<4;i++)
            {
                scores[i] = 0;
            }
            for(int i=0;i<4;i++)
            {
                a[i] = 0;
            }
            for(int i=0;i<22;i++)
            {
                for(int j=0;j<22;j++)
                {
                    Scores[i, j] = 0;
                }
            }
        }
        public void PartNew()
        {
            num1 = 0; num2 = 0; num3 = 0; num4 = 0;
            a1 = 0; a2 = 0; a3 = 0; a4 = 0;
            score = 0;
            for (int i = 0; i < 4; i++)
            {
                num[i] = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                scores[i] = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                a[i] = 0;
            }
        }
        public Piece BestPosition;
        //计算某点处相连的点，以及两边是否被堵
        public void Calculate(int[,]panel,int color,int x,int y)
        {
            //水平方向
            for (int i = 0; i < 23 - x; i++)
            {
                if (panel[x + i, y] == color)
                    num1 += 1;
                else if (panel[x + i, y] == 0)
                    break;
                else
                {
                    a1 += 1;
                    break;
                }
            }
            for (int j = 0; j < x; j++)
            {
                if (panel[x - j, y] == color)
                    num1 += 1;
                else if (panel[x - j, j] == 0)
                    break;
                else
                {
                    a1 += 1;
                    break;
                }
            }
            //竖直方向
            for (int i = 0; i < 23 - y; i++)
            {
                if (panel[x, y + i] == color)
                    num2 += 1;
                else if (panel[x, y + i] == 0)
                    break;
                else
                {
                    a2 += 1;
                    break;
                }
            }
            for (int j = 0; j < y; j++)
            {
                if (panel[x, y - j] == color)
                    num2 += 1;
                else if (panel[x, y - j] == 0) 
                    break;
                else
                {
                    a2 += 1;
                    break;
                }
            }
            //斜向上方向
            for (int i = 0; i < 23 - x & i < y; i++)
            {
                if (panel[x + i, y - i] == color)
                    num3 += 1;
                else if (panel[x + i, y - i] == 0) 
                    break;
                else
                {
                    a3 += 1;
                    break;
                }
            }
            for (int j = 0; j < x & j < 23 - y; j++)
            {
                if (panel[x - j, y + j] == color)
                    num3 += 1;
                else if (panel[x - j, y + j] == 0) 
                    break;
                else
                {
                    a3 += 1;
                    break;
                }
            }
            //斜向下方向
            for (int i = 0; i < 23 - x & i < 23 - y; i++)
            {
                if (panel[x + i, y + i] == color)
                    num4 += 1;
                else if (panel[x + i, y + i] == 0) 
                    break;
                else
                {
                    a4 += 1;
                    break;
                }
            }
            for (int j = 0; j < x & j < y; j++)
            {
                if (panel[x - j, y - j] == color)
                    num4 += 1;
                else if (panel[x - j, y - j] == 0) 
                    break;
                else
                {
                    a4 += 1;
                    break;
                }
            }
            num[0] = num1;num[1] = num2;num[2] = num3;num[3] = num4;
            a[0] = a1;a[1] = a2;a[2] = a3;a[3] = a4;
        }
        //计分函数1，2
        public void GetScore()
        {
            for(int i=0;i<4;i++)
            {
                if (num[i] == 2 & a[i] == 2)
                    scores[i] = 30;
                if (num[i] == 2 & a[i] == 1)
                    scores[i] = 50;
                if (num[i] == 2 & a[i] == 0)
                    scores[i] = 100;
                if (num[i] == 3 & a[i] == 2)
                    scores[i] = 100;
                if (num[i] == 3 & a[i] == 1)
                    scores[i] = 200;
                if (num[i] == 3 & a[i] == 0)
                    scores[i] = 500;
                if (num[i] == 4 & a[i] == 2)
                    scores[i] = 500;
                if (num[i] == 4 & a[i] == 1)
                    scores[i] = 1000;
                if (num[i] == 4 & a[i] == 0)
                    scores[i] = 3000;
                if (num[i] == 5 & a[i] == 2)
                    scores[i] = 1000;
                if (num[i] == 5 & a[i] == 1)
                    scores[i] = 3000;
                if (num[i] == 5 & a[i] == 0)
                    scores[i] = 50000;
                if (num[i] >= 6)
                    scores[i] = 100000;
                score += scores[i];
            }

        }
        public void GetScore2()
        {
            for (int i = 0; i < 4; i++)
            {
                if (num[i] == 2 & a[i] == 2)
                    scores[i] = 30;
                if (num[i] == 2 & a[i] == 1)
                    scores[i] = 50;
                if (num[i] == 2 & a[i] == 0)
                    scores[i] = 100;
                if (num[i] == 3 & a[i] == 2)
                    scores[i] = 100;
                if (num[i] == 3 & a[i] == 1)
                    scores[i] = 200;
                if (num[i] == 3 & a[i] == 0)
                    scores[i] = 500;
                if (num[i] == 4 & a[i] == 2)
                    scores[i] = 500;
                if (num[i] == 4 & a[i] == 1)
                    scores[i] = 1000;
                if (num[i] == 4 & a[i] == 0)
                    scores[i] = 3000;
                if (num[i] == 5 & a[i] == 2)
                    scores[i] = 1000;
                if (num[i] == 5 & a[i] == 1)
                    scores[i] = 3000;
                if (num[i] == 5 & a[i] == 0)
                    scores[i] = 60000;
                if (num[i] >= 6)
                    scores[i] = 150000;
                score += scores[i];
            }
        }
        //写入函数矩阵
        public virtual void GetScores(int[,]panel,int color)
        {
            for(int i=1;i<23;i++)
            {
                for(int j=1;j<23;j++)
                {
                    if(panel [i,j]!=0)
                    {
                        if (panel[i + 1, j] == 0 & i + 1 < 20) 
                        {
                            panel[i + 1, j] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i + 1, j);
                            ai.GetScore();
                            Scores[i + 1, j] = ai.score;
                            panel[i + 1, j] = 0;
                        }
                        if (panel[i, j + 1] == 0 & j + 1 < 20) 
                        {
                            panel[i, j + 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i, j + 1);
                            ai.GetScore();
                            Scores[i, j + 1] = ai.score;
                            panel[i, j + 1] = 0;
                        }
                        if (panel[i - 1, j] == 0 & i - 1 > 3) 
                        {
                            panel[i - 1, j] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i - 1, j);
                            ai.GetScore();
                            Scores[i - 1, j] = ai.score;
                            panel[i - 1, j] = 0;
                        }
                        if (panel[i, j - 1] == 0 & j - 1 > 3) 
                        {
                            panel[i, j - 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i, j - 1);
                            ai.GetScore();
                            Scores[i, j - 1] = ai.score;
                            panel[i, j - 1] = 0;
                        }
                        if (panel[i + 1, j + 1] == 0 & i + 1 < 20 & j + 1 < 20)  
                        {
                            panel[i + 1, j + 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i + 1, j + 1);
                            ai.GetScore();
                            Scores[i + 1, j + 1] = ai.score;
                            panel[i + 1, j + 1] = 0;
                        }
                        if (panel[i + 1, j - 1] == 0 & i + 1 < 20 & j - 1 > 3) 
                        {
                            panel[i + 1, j - 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i + 1, j - 1);
                            ai.GetScore();
                            Scores[i + 1, j - 1] = ai.score;
                            panel[i + 1, j - 1] = 0;
                        }
                        if (panel[i - 1, j + 1] == 0 & i - 1 > 3 & j + 1 < 20) 
                        {
                            panel[i - 1, j + 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i - 1, j + 1);
                            ai.GetScore();
                            Scores[i - 1, j + 1] = ai.score;
                            panel[i - 1, j + 1] = 0;
                        }
                        if (panel[i - 1, j - 1] == 0 & i - 1 > 3 & j - 1 > 3) 
                        {
                            panel[i - 1, j - 1] = color;
                            AI ai = new AI();
                            ai.Calculate(panel, color, i - 1, j - 1);
                            ai.GetScore();
                            Scores[i - 1, j - 1] = ai.score;
                            panel[i - 1, j - 1] = 0;
                        }
                    }
                }
            }
        }
        //得出最佳点
        public void BP ()
        {
            int X = 0, Y = 0, b = 0;
            for(int i=0;i<23;i++)
            {
                for(int j=0;j<23;j++)
                {
                    if(Scores [i,j]>b)
                    {
                        X = i;Y = j;b = Scores[i, j];
                    }
                }
            }
            BestPosition.pieceX = (X - 4) * 30 + 50;
            BestPosition.pieceY = (Y - 4) * 30 + 50;
        }
    }
    //复杂模式
    public class AI2 : AI
    {
        //重写计分函数
        public override void GetScores(int[,] panel, int color)
        {
            for (int i = 1; i < 23; i++)
            {
                for (int j = 1; j < 23; j++)
                {
                    if (panel[i, j] != 0)
                    {
                        if (panel[i + 1, j] == 0 & i + 1 < 20)
                        {
                            panel[i + 1, j] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i + 1, j);
                            ai.GetScore2();
                            Scores[i + 1, j] += ai.score;
                            panel[i + 1, j] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i + 1, j);
                            ai.GetScore();
                            Scores[i + 1, j] += ai.score;
                            panel[i + 1, j] = 0;
                        }
                        if (panel[i, j + 1] == 0 & j + 1 < 20)
                        {
                            panel[i , j + 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i , j + 1);
                            ai.GetScore2();
                            Scores[i , j + 1] += ai.score;
                            panel[i , j + 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i , j + 1);
                            ai.GetScore();
                            Scores[i , j + 1] += ai.score;
                            panel[i , j + 1] = 0;
                        }
                        if (panel[i - 1, j] == 0 & i - 1 > 3)
                        {
                            panel[i - 1, j] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i - 1, j);
                            ai.GetScore2();
                            Scores[i - 1, j] += ai.score;
                            panel[i - 1, j] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i - 1, j);
                            ai.GetScore();
                            Scores[i - 1, j] += ai.score;
                            panel[i - 1, j] = 0;
                        }
                        if (panel[i, j - 1] == 0 & j - 1 > 3)
                        {
                            panel[i, j - 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i, j - 1);
                            ai.GetScore2();
                            Scores[i, j - 1] += ai.score;
                            panel[i, j - 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i, j - 1);
                            ai.GetScore();
                            Scores[i, j - 1] += ai.score;
                            panel[i, j - 1] = 0;
                        }
                        if (panel[i + 1, j + 1] == 0 & i + 1 < 20 & j + 1 < 20)
                        {
                            panel[i + 1, j + 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i + 1, j + 1);
                            ai.GetScore2();
                            Scores[i + 1, j + 1] += ai.score;
                            panel[i + 1, j + 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i + 1, j + 1);
                            ai.GetScore();
                            Scores[i + 1, j + 1] += ai.score;
                            panel[i + 1, j + 1] = 0;
                        }
                        if (panel[i + 1, j - 1] == 0 & i + 1 < 20 & j - 1 > 3)
                        {
                            panel[i + 1, j - 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i + 1, j - 1);
                            ai.GetScore2();
                            Scores[i + 1, j - 1] += ai.score;
                            panel[i + 1, j - 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i + 1, j - 1);
                            ai.GetScore();
                            Scores[i + 1, j - 1] += ai.score;
                            panel[i + 1, j - 1] = 0;
                        }
                        if (panel[i - 1, j + 1] == 0 & i - 1 > 3 & j + 1 < 20)
                        {
                            panel[i - 1, j + 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i - 1, j + 1);
                            ai.GetScore2();
                            Scores[i - 1, j + 1] += ai.score;
                            panel[i - 1, j + 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i - 1, j + 1);
                            ai.GetScore();
                            Scores[i - 1, j + 1] += ai.score;
                            panel[i - 1, j + 1] = 0;
                        }
                        if (panel[i - 1, j - 1] == 0 & i - 1 > 3 & j - 1 > 3)
                        {
                            panel[i - 1, j - 1] = 1;
                            AI ai = new AI();
                            ai.Calculate(panel, 1, i - 1, j - 1);
                            ai.GetScore2();
                            Scores[i - 1, j - 1] += ai.score;
                            panel[i - 1, j - 1] = 2;
                            ai.PartNew();
                            ai.Calculate(panel, 2, i - 1, j - 1);
                            ai.GetScore();
                            Scores[i - 1, j - 1] += ai.score;
                            panel[i - 1, j - 1] = 0;
                        }
                    }
                }
            }
        }
    }
}
