using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        int hor;
        int vert;
        int[,] vak;
        int breedte;
        int hoogte;
        int beurt;
        int aantal;
        int richtingx;
        int richtingy;
        public Form1()
        {
            beurt = 1;
            this.hor = 6;
            this.vert = 6;
            vak = new int[hor, vert];
            int handig = (vert / 2) - 1;
            this.aantal = 0;
            this.richtingx = 0;
            this.richtingy = 0;
            // beginwaardes vastgezet
            for (int i = 0; i < vert; i++)
            {

                for (int j = 0; j < vert; j++)
                {
                    if ((i == 2 && j == 2) || (i == 3 && j == 3))
                    {
                        vak[i, j] = 1;
                    }
                    else if ((i == 2 && j == 3) || (i == 3 && j == 2))
                    {
                        vak[i, j] = 2;
                    }
                    else
                    {
                        vak[i, j] = 0;
                    }
                }
            }

            InitializeComponent();
            breedte = Panel.Size.Width / hor;
            hoogte = Panel.Size.Height / vert;
            Panel.Paint += this.tekenveld;
            Panel.MouseClick += this.zetwaardes;
            Panel.Paint += this.tekencirkel;
        }

        private void Reset_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void zetwaardes(object sender, MouseEventArgs mea)
        {

            for (int a = 0; a < hor; a++)
            {
                if ((a * Panel.Size.Width / hor) < mea.X && mea.X < ((a + 1) * Panel.Size.Width / hor))
                {
                    for (int b = 0; b < vert; b++)
                    {

                        if ((b * Panel.Size.Height / vert) < mea.Y && mea.Y < ((b + 1) * Panel.Size.Height / vert))
                            if (mogelijk(a, b))
                            {
                                this.vak[a, b] = beurt;
                                slastenen(a, b);
                                //aanpasinrichting(a, b);

                                if (beurt == 1)
                                {
                                    beurt = 2;
                                    label1.Text = "blauw is aan zet";
                                }

                                else
                                {
                                    beurt = 1;
                                    label1.Text = "rood is aan zet";
                                }
                            }
                    }
                }
            }
            Panel.Invalidate();
        }

        bool mogelijk(int x, int y)
        {
            if (vak[x, y] == 0)
            {
                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {

                        if ((x + dx < hor && y + dy < vert) && (x + dx >= 0 && y + dy >= 0))
                        {
                            if (vak[x + dx, y + dy] != beurt && vak[x + dx, y + dy] != 0)
                            {
                                if (ingesloten(x, y, dx, dy))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

            }
            return false;


        }

        bool ingesloten(int x, int y, int dx, int dy)
        {


            for (int a = 1; a < 6; a++)
            {
                if ((x + a * dx < hor && y + a * dy < vert) && (x + a * dx >= 0 && y + a * dy >= 0))
                {
                    if (vak[x + a * dx, y + a * dy] == beurt)
                    {
                        this.richtingx = dx;
                        this.richtingy = dy;
                        this.aantal = a;
                        return true;
                    }
                    if (vak[x + a * dx, y + a * dy] == 0)
                        return false;
                }
            }
            return false;
        }

        int afstand(int x, int y, int dx, int dy)
        {
            for (int a = 1; a < 6; a++)
            {
                if ((x + a * dx < hor && y + a * dy < vert) && (x + a * dx >= 0 && y + a * dy >= 0))
                {
                    if (vak[x + a * dx, y + a * dy] == beurt)
                    {
                        return a;
                    }
                    if (vak[x + a * dx, y + a * dy] == 0)
                        return 0;
                }
            }
            return 0;
        }

        private void slastenen (int x, int y)
        {
                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {

                        if ((x + dx < hor && y + dy < vert) && (x + dx >= 0 && y + dy >= 0))
                        {
                            if (vak[x + dx, y + dy] != beurt && vak[x + dx, y + dy] != 0)
                            {
                                int a = this.afstand(x, y, dx, dy);
                                {
                                    for (int b = 1; b < a; b++)
                                    {
                                        vak[x + b * dx, y + b * dy] = beurt;
                                    }
                                }
                            }
                        }
                    }
                }

        }
        

        private void tekenveld(object sender, PaintEventArgs pea)
        {
            for (int x = 0; x < hor; x++)
            {
                for (int y = 0; y < vert; y++)
                {
                    pea.Graphics.DrawRectangle(Pens.Black, 0 + x * (Panel.Size.Width / hor), 0 + y * (Panel.Size.Height / vert), Panel.Size.Width / hor, Panel.Size.Height / vert);
                    if (mogelijk(x, y))
                    {
                        pea.Graphics.DrawEllipse(Pens.Gray, 0 + x * (Panel.Size.Width / hor), 0 + y * (Panel.Size.Height / vert), Panel.Size.Width / hor, Panel.Size.Height / vert);
                    }
                }
            }
        }

        private void tekencirkel(object sender, PaintEventArgs pea)
        {
            for (int x = 0; x < hor; x++)
            {
                for (int y = 0; y < vert; y++)
                {
                    if (this.vak[x, y] == 1)
                        pea.Graphics.FillEllipse(Brushes.Red, 0 + x * (Panel.Size.Width / hor), 0 + y * (Panel.Size.Height / vert), Panel.Size.Width / hor, Panel.Size.Height / vert);
                    if (this.vak[x, y] == 2)
                        pea.Graphics.FillEllipse(Brushes.Blue, 0 + x * (Panel.Size.Width / hor), 0 + y * (Panel.Size.Height / vert), Panel.Size.Width / hor, Panel.Size.Height / vert);

                }
            }
        }

    }
}