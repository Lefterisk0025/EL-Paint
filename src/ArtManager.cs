using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exercise7
{
    enum ArtMode { HandDraw, Erase, Line, Rectangle, Ellipse, Circle}
    //Τάξη υπέυθυνη να ζωγραφίζει τα σχήματα που δηλώνονται στη λίστα με τα actions. Σε κάθε RePaint του canva (οταν καλείτε η Invalidate() 
    //δηλαδή), η συνάρτηση αυτή κάνει iterate τη λίστα με τα action και ζωγραφίζει το αντίστοιχο σχήμα σύμφωνα με το είδος και 
    //τα σημεία που εχουν δηλωθεί στο action
    class ArtManager
    {
        Pen pen;
        public void Draw(Graphics G, List<ArtAction> actions)
        {          
            foreach (ArtAction da in actions)
            {
                pen = new Pen(da.color, da.width);
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                if (da.points.Count > 1)
                {
                    if (da.artMode == ArtMode.HandDraw)
                        G.DrawCurve(pen, da.points.ToArray(), 0.0f);
                    else if (da.artMode == ArtMode.Erase)
                        G.DrawCurve(new Pen(da.color, da.width), da.points.ToArray(), 0.0f); 
                    else if (da.artMode == ArtMode.Line)
                        G.DrawLine(pen, da.points[0], da.points[da.points.Count - 1]);
                    else if (da.artMode == ArtMode.Rectangle)
                        G.DrawRectangle(pen, Math.Min(da.points[0].X, da.points[da.points.Count - 1].X), Math.Min(da.points[0].Y, da.points[da.points.Count - 1].Y), Math.Abs(da.points[0].X - da.points[da.points.Count - 1].X), Math.Abs(da.points[0].Y - da.points[da.points.Count - 1].Y));
                    else if (da.artMode == ArtMode.Ellipse)
                    {
                        Rectangle rect = new Rectangle((int)Math.Min(da.points[0].X, da.points[da.points.Count - 1].X), (int)Math.Min(da.points[0].Y, da.points[da.points.Count - 1].Y), (int)Math.Abs(da.points[0].X - da.points[da.points.Count - 1].X), (int)Math.Abs(da.points[0].Y - da.points[da.points.Count - 1].Y));
                        G.DrawEllipse(pen, rect);
                    }
                    else if (da.artMode == ArtMode.Circle)
                    {
                        Rectangle rect = new Rectangle((int)Math.Min(da.points[0].X, da.points[da.points.Count - 1].X), (int)Math.Min(da.points[0].Y, da.points[da.points.Count - 1].Y), (int)Math.Abs(da.points[0].Y - da.points[da.points.Count - 1].Y), (int)Math.Abs(da.points[0].Y - da.points[da.points.Count - 1].Y));
                        G.DrawEllipse(pen, rect);
                    }
                }
            }           
        }
    }
    //Περιέχει πληροφορίες για κάθε ενέργεια που συμβαίνει στον canva 
    class ArtAction
    {
        public Color color;
        public float width;
        public ArtMode artMode;
        public List<PointF> points;

        public ArtAction(Color color, float width ,ArtMode artMode)
        {
            this.color = color;
            this.width = width;
            this.artMode = artMode;
            points = new List<PointF>();
        }
    }   
}
