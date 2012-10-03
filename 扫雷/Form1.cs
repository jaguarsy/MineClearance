using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace 扫雷
{
    
    public partial class Form1 : Form
    {
        public long lNum = 0;
        public Button[] bt;
        public int[] a=new int[100];
        public int[] b=new int[100];
        public int[] tagbt = new int[100];
        public int[] bjbt = new int[100];
        public int i, k, temp, win1=0, surnum=0,tagnum=0;
       // private FileStream fs;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "已标记0个地雷";
            label2.Text = "共10个地雷";
            label3.Text = "已用时0秒";
            //fs = new System.IO.FileStream(Environment.ExpandEnvironmentVariables("%windir%\\system32\\taskmgr.exe"), System.IO.FileMode.Open, System.IO.FileAccess.Write);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            win1 = 0;
            surnum = 0;
            tagnum = 0;
            label1.Text = "已标记0个地雷";
            label3.Text = "已用时0秒";
            lNum = 0;
            timer1.Start();
            bt = new Button[100];
            Random random = new Random();
            for (i = 0; i <100; i++)                    
            {
                a[i] = i ;
                b[i] = 0;
                tagbt[i] = -1;
                bjbt[i] = 0;
            }
            for (int x = 0; x < 10; x++)                //控件数组初始化
            {
                for ( i = 0; i < 10; i++)
                {
                    bt[i + x * 10] = new Button();
                    bt[i + x * 10].Top = x * 29;                //换行，即调整与顶端的距离
                    bt[i + x * 10].Left = i * 29 + 190;         //右移，即调整与左端的距离
                    bt[i + x * 10].Size = new Size(30, 30);     //调整按钮大小
                    bt[i + x * 10].TabIndex = i + x * 10;       //标记按钮的下标值
                    Controls.Add(bt[i + x * 10]);               //创建按钮
                    bt[i + x * 10].MouseDown += new MouseEventHandler(buttonAll_MouseDown);  //自定义单击事件
                }
            }
            for (i = 0; i < 10; i++)     //产生十五个不重复的随机数，放在a[]数组最后十五个
            {
                k = random.Next(0, 99 - i);
                temp = a[k];
                a[k] = a[99 - i];
                a[99 - i] = temp;
            }
            for (i = 0; i < 10; i++)     //将这十五个随机数放到b数组中，同时改变对应按钮的text值
            {
                b[a[99 - i]] = 1;
            }
        }

        int surround(int loc)               //此按钮周围的地雷个数统计
        {
            int n=0;
            if ( loc % 10 != 0 && loc > 10) {if(b[loc - 11] == 1 )n++;}
            if ( loc >= 10) { if (b[loc - 10] == 1)n++; }
            if ( loc % 10 != 9 && loc >= 10) { if (b[loc - 9] == 1)n++; }
            if ( loc % 10 != 0 && loc>0) { if (b[loc - 1] == 1)n++; }
            if ( loc % 10 != 9 && loc<99 ) { if (b[loc + 1] == 1)n++; }
            if ( loc % 10 != 0 && loc <= 89) { if (b[loc + 9] == 1)n++; }
            if ( loc <= 89) { if (b[loc + 10] == 1)n++; }
            if ( loc % 10 != 9 && loc < 89) { if (b[loc + 11] == 1)n++; }
            return n;
        }

        void buttonshow()                                       //失败时显示所有按钮情况
        {
            for (i = 0; i < 100; i++)
            {
                if (b[i] == 1) bt[i].BackgroundImage = imageList1.Images[0];
                else if (surround(i) == 0) bt[i].BackgroundImage = imageList1.Images[10];
                else bt[i].BackgroundImage = imageList1.Images[surround(i)];
            }
        }

        void buttonrefresh()                                    //所有按钮删除
        {
            for (i = 0; i < 100; i++) this.Controls.Remove(bt[i]);
        }

        void showthenull(int loc)                                      //显示连续的白色区域
        {
            bt[loc].BackgroundImage = imageList1.Images[10];
            tagbt[loc] = 0;                                 //标记为已经扫描过
            bjbt[loc] = 3;                              //标记为特殊函数点开，防止右击出错
            if (loc >= 10) 
            { 
                if (surround(loc - 10) == 0 && tagbt[loc - 10]!=0)showthenull(loc - 10);
                else if (surround(loc - 10) != 0 && tagbt[loc - 10] != 0)
                {
                    tagbt[loc - 10] = 0;
                    bt[loc - 10].BackgroundImage = imageList1.Images[surround(loc - 10)];
                    bjbt[loc - 10] = 2;                 //标记为特殊函数点开，防止右击出错
                }
            }
            if (loc % 10 != 0 && loc > 0) 
            {
                if (surround(loc - 1) == 0 && tagbt[loc - 1] != 0) showthenull(loc - 1);
                else if (surround(loc - 1) != 0 && tagbt[loc - 1] != 0)
                {
                    tagbt[loc - 1] = 0;
                    bt[loc - 1].BackgroundImage = imageList1.Images[surround(loc - 1)];
                    bjbt[loc - 1] = 2;
                }

            }
            if (loc % 10 != 9 && loc < 99) 
            {
                if (surround(loc + 1) == 0 && tagbt[loc + 1] != 0) showthenull(loc + 1);
                else if (surround(loc + 1) != 0 && tagbt[loc + 1] != 0)
                {
                    tagbt[loc + 1] = 0;
                    bt[loc + 1].BackgroundImage = imageList1.Images[surround(loc + 1)];
                    bjbt[loc + 1] = 2;
                }
            }
            if (loc <= 89) 
            {
                if (surround(loc + 10) == 0 && tagbt[loc + 10] != 0) showthenull(loc + 10);
                else if (surround(loc + 10) != 0 && tagbt[loc + 10] != 0)
                {
                    tagbt[loc + 10] = 0;
                    bt[loc + 10].BackgroundImage = imageList1.Images[surround(loc + 10)];
                    bjbt[loc + 10] = 2;
                }
            }
        }


        private   void   buttonAll_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
        {
            if (e.Button == MouseButtons.Right && tagnum < 10 && bjbt[((Button)sender).TabIndex] == 0)             //是否右击，并且是否已标记
            {
                    tagnum++;
                    bjbt[((Button)sender).TabIndex] = 1;
                    label1.Text = "已标记" + tagnum.ToString() + "个地雷";
                    ((Button)sender).BackgroundImage = imageList1.Images[9];
                    if (b[((Button)sender).TabIndex] == 1)              //如果标记的地方正好是地雷
                    {
                        win1++;
                    }
                    if (win1 == 10)                         //第一种胜利条件
                    {
                        Form frm2 = new Form2();
                        timer1.Stop();
                        frm2.ShowDialog();
                        buttonrefresh();
                    }
             }
                else if (e.Button == MouseButtons.Right && bjbt[((Button)sender).TabIndex] == 1)   //若已标记则撤销标记
                {
                    tagnum--;
                    bjbt[((Button)sender).TabIndex] = 0;
                    label1.Text = "已标记" + tagnum.ToString() + "个地雷";
                    ((Button)sender).BackgroundImage = null;
                    if (b[((Button)sender).TabIndex] == 1)              //如果标记的地方正好是地雷,第一种胜利条件值减1
                    {
                        win1--;
                    }
                }
                else if (e.Button == MouseButtons.Left && bjbt[((Button)sender).TabIndex] != 1)            //是否左击
                {
                    bjbt[((Button)sender).TabIndex] = 2;
                    if (b[((Button)sender).TabIndex] == 0)           //如果这里没有地雷
                    {
                        surnum = surround(((Button)sender).TabIndex);           //统计地雷数
                        if (surnum == 0)                                           //如果周围没有地雷
                        {
                            showthenull(((Button)sender).TabIndex);
                        }
                        else
                        {
                            ((Button)sender).BackgroundImage = imageList1.Images[surnum];   //显示地雷数
                        }
                    }
                    else
                    {
                        ((Button)sender).BackgroundImage = imageList1.Images[0];        //显示地雷
                        Form frm3 = new Form3();                                //失败
                        timer1.Stop();
                        frm3.ShowDialog();
                        buttonrefresh();
                    }
                }
        }

        private void timer1_Tick(object sender, EventArgs e)            //计时器
        {
            lNum++;
            this.label3.Text = "已用时" + lNum.ToString() + "秒";
        }


    }
}
