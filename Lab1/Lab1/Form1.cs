using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        enum ShapeType { line, rectangle, ellipse };
        //determine shape type, default to line
        private ShapeType currentType = ShapeType.line;
        private struct PreviousShapes
        {
            //save previous shape data for panel resizing
            public ShapeType type;
            public int X1, X2, Y1, Y2;
            public Color color;
        }
        //store previous shape data in a list
        private readonly List<PreviousShapes> drawings = new List<PreviousShapes>();

        public Form1()
        {
            InitializeComponent();
            panel1.Paint += panel1_Paint;
        }
     

        private Color GetColor()
        {
            //determine color based on RGB track bars
            return Color.FromArgb(redTrackBar.Value, greenTrackBar.Value, blueTrackBar.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int oldX, oldY;

        private void lineButton_Click(object sender, EventArgs e)
        {
            currentType = ShapeType.line;
        }

        private void rectangleButton_Click(object sender, EventArgs e)
        {
            currentType = ShapeType.rectangle;
        }
        private void ellipseButton_Click(object sender, EventArgs e)
        {
            currentType = ShapeType.ellipse;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            oldX = e.X;
            oldY = e.Y;
        }

        private Rectangle MakeRect(int x1, int y1, int x2, int y2)
        {
            //left coordinate
            int x = Math.Min(x1, x2);
            //top coordinate
            int y = Math.Min(y1, y2);
            //width
            int w = Math.Abs(x2 - x1);
            //height
            int h = Math.Abs(y2 - y1);
            return new Rectangle(x, y, w, h);
        }

        private void panel1_MouseUp_1(object sender, MouseEventArgs e)
        {
            //save new drawing
            drawings.Add(new PreviousShapes
            {
                type = currentType,
                X1 = oldX,
                Y1 = oldY,
                X2 = e.X,
                Y2 = e.Y,
                color = GetColor()
            });

            //repaint panel
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var d in drawings)
            {
                using (Pen pen = new Pen(d.color))
                {

                    if (d.type == ShapeType.line)
                    {
                        e.Graphics.DrawLine(pen, d.X1, d.Y1, d.X2, d.Y2);
                    }
                    else if (d.type == ShapeType.rectangle)
                    {
                        e.Graphics.DrawRectangle(pen, MakeRect(d.X1, d.Y1, d.X2, d.Y2));
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(pen, MakeRect(d.X1, d.Y1, d.X2, d.Y2));
                    }
                }
            }

        }
    }
}
