using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Encoder = System.Drawing.Imaging.Encoder;

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


        Bitmap currentBitmap;
        Panel currentPanel;
        TabPage currentTabPage;
        int currentIndex;
        List<Bitmap> bitmaps;
        Shape currentShape;
        Color currentColor = Color.Black;
        static Random rnd = new Random();
        public Form1()
        {
            
            InitializeComponent();
            tabControl1.TabPages[0].Text = "0";
            currentIndex = 0;
            bitmaps = new List<Bitmap>();
            currentBitmap = new Bitmap(panel2.Width, panel2.Height, PixelFormat.Format32bppArgb);
            currentShape = new MyLine();
            currentPanel = panel2;
            currentShape.setBmap(currentBitmap);
            currentShape.setPanel(currentPanel);
            currentPanel.BackgroundImage = currentBitmap;
            currentPanel.MouseDown += new MouseEventHandler(currentShape.mouseDown);
            currentPanel.MouseUp += new MouseEventHandler(currentShape.mouseUp);
            currentPanel.MouseMove += new MouseEventHandler(currentShape.mouseMove);
            bitmaps.Add(currentBitmap);
            
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(currentBitmap, new Point(0, 0));
            
        }

        private void purge()
        {
            currentPanel.MouseDown -= new MouseEventHandler(currentShape.mouseDown);
            currentPanel.MouseUp -= new MouseEventHandler(currentShape.mouseUp);
            currentPanel.MouseMove -= new MouseEventHandler(currentShape.mouseMove);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            purge();
            currentShape = new MyLine();
            currentShape.setBmap(currentBitmap);
            currentShape.setPanel(currentPanel);
            currentShape.changeColor(currentColor);
            currentPanel.MouseDown += new MouseEventHandler(currentShape.mouseDown);
            currentPanel.MouseUp += new MouseEventHandler(currentShape.mouseUp);
            currentPanel.MouseMove += new MouseEventHandler(currentShape.mouseMove);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            purge();
            currentShape = new MyRectangle();
            currentShape.setBmap(currentBitmap);
            currentShape.setPanel(currentPanel);
            currentShape.changeColor(currentColor);
            currentPanel.MouseDown += new MouseEventHandler(currentShape.mouseDown);
            currentPanel.MouseUp += new MouseEventHandler(currentShape.mouseUp);
            currentPanel.MouseMove += new MouseEventHandler(currentShape.mouseMove);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            purge();
            currentShape = new MyFree();
            currentShape.setBmap(currentBitmap);
            currentShape.setPanel(currentPanel);
            currentShape.changeColor(currentColor);
            currentPanel.MouseDown += new MouseEventHandler(currentShape.mouseDown);
            currentPanel.MouseUp += new MouseEventHandler(currentShape.mouseUp);
            currentPanel.MouseMove += new MouseEventHandler(currentShape.mouseMove);          
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            currentColor = Color.Red;
            currentShape.changeColor(Color.Red);
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            currentColor = Color.Blue;
            currentShape.changeColor(Color.Blue);
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            currentColor = Color.Green;
            currentShape.changeColor(Color.Green);
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            currentColor = Color.Black;
            currentShape.changeColor(Color.Black);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // TODO: create new tab, new bitmap, move panel to new tab, update panel background to new bitmap
            string title = (tabControl1.TabCount).ToString();
            TabPage myTabPage = new TabPage(title);
            tabControl1.TabPages.Add(myTabPage);
            myTabPage.Controls.Add(currentPanel);
            Bitmap myTabPageBitmap = new Bitmap(panel2.Width, panel2.Height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(myTabPageBitmap).Clear(Color.White);
            bitmaps.Add(myTabPageBitmap);
            currentBitmap = myTabPageBitmap;
            currentShape.setBmap(currentBitmap);
            currentShape.setPanel(currentPanel);
            currentPanel.BackgroundImage = currentBitmap;
            currentTabPage = myTabPage;

            tabControl1.SelectedTab = currentTabPage;           
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if(tabControl1.TabCount == 1 )
            {
                Environment.Exit(0);
            }
            int previousIndex = tabControl1.TabPages.IndexOf(currentTabPage) - 1;
            currentIndex = Int32.Parse(currentTabPage.Text);
            if(previousIndex == -1)
            {
                previousIndex = 0;
            }
            bitmaps.RemoveAt(currentIndex);
            for(int i = currentIndex + 1; i<tabControl1.TabPages.Count; i++)
            {
                tabControl1.TabPages[i].Text =  (Int32.Parse( tabControl1.TabPages[i].Text) - 1).ToString();
            }
            tabControl1.TabPages.Remove(currentTabPage);
            currentTabPage = tabControl1.TabPages[previousIndex];
            tabControl1.SelectedTab = currentTabPage;
        }

        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            // TODO: move panel to selected tab, update panel background to new bitmap

                label1.Text = (tabControl1.TabPages.IndexOf(e.TabPage)).ToString();
                //label1.Text = e.TabPage.Text;
                currentIndex = Int32.Parse(e.TabPage.Text);
                currentTabPage = tabControl1.TabPages[currentIndex];
                currentTabPage.Controls.Add(currentPanel);
                currentPanel.BackgroundImage = bitmaps[currentIndex];
                currentBitmap = bitmaps[currentIndex];

                currentShape.setBmap(currentBitmap);
                currentShape.setPanel(currentPanel);
                      
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try { 
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                currentBitmap.Save(sfd.FileName, format);
            }
            // just save bitmap
            }
            catch (Exception)
            {
                MessageBox.Show("There was a problem saving the file." +
                    "Check the file permissions.");
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            // Wrap the creation of the OpenFileDialog instance in a using statement,
            // rather than manually calling the Dispose method to ensure proper disposal
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Images|*.png;*.bmp;*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    currentBitmap = new Bitmap(dlg.FileName);
                    bitmaps[currentIndex] = currentBitmap;
                    currentPanel.BackgroundImage = currentBitmap;

                    currentShape.setBmap(currentBitmap);
                    currentShape.setPanel(currentPanel);
                }
            }
        }
    }
}
