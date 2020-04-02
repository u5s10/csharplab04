using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{

    public partial class Form1 : Form
    {
       
        public abstract class Shape
        {
            public Point locactionXY;
            public Point locactinX1Y1;
            public bool isMouseDown = false;
            public bool isMouseMoving = false;
            public Color color = Color.Black;
            public Bitmap bmapp;
            public Panel p;

            public virtual void setBmap(Bitmap b)
            {
                this.bmapp = b;
            }

            public virtual void setPanel(Panel p)
            {
                this.p = p;
            }

            public virtual void mouseDown(object sender, MouseEventArgs e) {
                isMouseDown = true;
                locactionXY = e.Location;
            }

            public virtual void mouseUp(object sender, MouseEventArgs e) { }
            
            public virtual void mouseMove(object sender, MouseEventArgs e) {
                if (isMouseDown == true)
                {
                    locactinX1Y1 = e.Location;
                    
                }
            }

            public virtual void changeColor(Color c) {
                this.color = c;
            }
            
           
        }

        public class MyFree : Shape
        {
            Pen pen;
            public override void mouseMove(object sender, MouseEventArgs e)
            {
                if(isMouseMoving && locactionXY.X!= -1 && locactionXY.Y != -1)
                {
                    using (Graphics g = Graphics.FromImage(bmapp))
                    {
                        pen = new Pen(Brushes.Black);
                        pen.Width = 5.0f;
                        pen.Color = this.color;
                        g.DrawLine(pen, locactionXY, e.Location);
                        locactionXY.X = e.X;
                        locactionXY.Y = e.Y;
                    }
                    p.Invalidate();
                }
            }
            public override void mouseDown(object sender, MouseEventArgs e)
            {
                isMouseMoving = true;
                locactionXY.X = e.X;
                locactionXY.Y = e.Y;
            }
            public override void mouseUp(object sender, MouseEventArgs e)
            {
                isMouseMoving = false;
                locactionXY.X = -1;
                locactionXY.Y = -1;
            }
        }

        public class MyLine : Shape
        {
            Pen pen;
            public override void mouseUp(object sender, MouseEventArgs e)
            {
                if (isMouseDown == true)
                {
                    locactinX1Y1 = e.Location;
                    pen = new Pen(Brushes.Black);
                    pen.Color = this.color;
                    pen.Width = 2.0f;
                    using (Graphics g = Graphics.FromImage(bmapp))
                    {
                        g.DrawLine(pen, locactionXY, locactinX1Y1);                       
                    }
                    p.Invalidate();
                }
            }
        }

        public class MyRectangle : Shape
        {
            Rectangle rec;          
            public override void mouseUp(object sender, MouseEventArgs e)
            {
                if (isMouseDown == true)
                {
                    locactinX1Y1 = e.Location;
                    isMouseDown = false;
                    rec = new Rectangle();
                    rec.X = Math.Min(locactionXY.X, locactinX1Y1.X);
                    rec.Y = Math.Min(locactionXY.Y, locactinX1Y1.Y);
                    rec.Width = Math.Abs(locactionXY.X - locactinX1Y1.X);
                    rec.Height = Math.Abs(locactionXY.Y - locactinX1Y1.Y);
                    SolidBrush mybrush = new SolidBrush(color);
                    using (Graphics g = Graphics.FromImage(bmapp))
                    {
                        g.FillRectangle(mybrush, rec); 
                    }
                    p.Invalidate();                    
                }
                
            }
        }


        Bitmap bmap;
        Shape current;
        Color currentColor = Color.Black;
        public Form1()
        {
            
            InitializeComponent();
            bmap = new Bitmap(panel2.Width, panel2.Height);
            current = new MyLine();
            current.setBmap(bmap);
            current.setPanel(panel2);
            panel2.MouseDown += new MouseEventHandler(current.mouseDown);
            panel2.MouseUp += new MouseEventHandler(current.mouseUp);
            panel2.MouseMove += new MouseEventHandler(current.mouseMove);
            
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(bmap, new Point(0, 0));
            
        }

        private void purge()
        {
            panel2.MouseDown -= new MouseEventHandler(current.mouseDown);
            panel2.MouseUp -= new MouseEventHandler(current.mouseUp);
            panel2.MouseMove -= new MouseEventHandler(current.mouseMove);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            purge();
            current = new MyLine();
            current.setBmap(bmap);
            current.setPanel(panel2);
            current.changeColor(currentColor);
            panel2.MouseDown += new MouseEventHandler(current.mouseDown);
            panel2.MouseUp += new MouseEventHandler(current.mouseUp);
            panel2.MouseMove += new MouseEventHandler(current.mouseMove);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            purge();
            current = new MyRectangle();
            current.setBmap(bmap);
            current.setPanel(panel2);
            current.changeColor(currentColor);
            panel2.MouseDown += new MouseEventHandler(current.mouseDown);
            panel2.MouseUp += new MouseEventHandler(current.mouseUp);
            panel2.MouseMove += new MouseEventHandler(current.mouseMove);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            purge();
            current = new MyFree();
            current.setBmap(bmap);
            current.setPanel(panel2);
            current.changeColor(currentColor);
            panel2.MouseDown += new MouseEventHandler(current.mouseDown);
            panel2.MouseUp += new MouseEventHandler(current.mouseUp);
            panel2.MouseMove += new MouseEventHandler(current.mouseMove);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            currentColor = Color.Red;
            current.changeColor(Color.Red);
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            currentColor = Color.Blue;
            current.changeColor(Color.Blue);
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            currentColor = Color.Green;
            current.changeColor(Color.Green);
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            currentColor = Color.Black;
            current.changeColor(Color.Black);
        }
    }
}
