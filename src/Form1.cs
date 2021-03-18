using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exercise7
{   
    public partial class Form1 : Form
    {
        //Σημεία για τα σχήματα που ζωγραφίζει ο AutoPainter
        PointF[] houseCords = { new PointF(270f, 300f), new PointF(270f, 490f), new PointF(500f, 490f), new PointF(500f, 300f),
                                new PointF(270f, 300f), new PointF(385f, 150f), new PointF(500f, 300f), new PointF(0f, 0f),
                                new PointF(300f, 335f), new PointF(300f, 385f), new PointF(350f, 385f), new PointF(350f, 335f),
                                new PointF(300f, 335f), new PointF(0f, 0f), new PointF(420f, 335f), new PointF(420f, 385f),
                                new PointF(470f, 385f), new PointF(470f, 335f), new PointF(420f, 335f), new PointF(0f, 0f),
                                new PointF(360f, 490f), new PointF(360f, 420f), new PointF(410f, 420f), new PointF(410f, 490f)};

        PointF[] robotCords = 
        { 
            //Body
            new PointF(315f, 230f),  new PointF(315f, 370f),  new PointF(430f, 370f),  new PointF(430f, 230f), new PointF(315f, 230f),
            new PointF(0f, 0f), new PointF(343.75f, 370f), new PointF(343.75f, 400f), new PointF(401.25f, 400f), new PointF(401.25f, 370f),
            //L-Arm
            new PointF(0f, 0f), new PointF(315f, 245f), new PointF(290f, 245f), new PointF(290f, 290f), new PointF(315f, 290f), new PointF(0f, 0f),
            new PointF(290f, 267.5f), new PointF(210f, 200f), new PointF(0f, 0f), new PointF(190f, 220f), new PointF(240f, 180f),
            new PointF(200f, 160f), new PointF(0f, 0f), new PointF(190f, 220f), new PointF(160f, 200f),
            //R-Arm
            new PointF(0f, 0f), new PointF(430f, 245f), new PointF(455f, 245f), new PointF(455f, 290f), new PointF(430f,290f), new PointF(0f, 0f),
            new PointF(455f, 267.5f), new PointF(530f, 195f), new PointF(0f, 0f), new PointF(515f, 170f), new PointF(555f, 220f),
            new PointF(580f, 190f), new PointF(0f, 0f), new PointF(515f,170f), new PointF(550f, 162f),
            //Head
            new PointF(0f, 0f), new PointF(315f, 210f), new PointF(430f, 210f), new PointF(430f, 150f), new PointF(315f, 150f), new PointF(315f, 210f),
            new PointF(0f, 0f), new PointF(330f,165f), new PointF(330f, 185f), new PointF(415f, 185f), new PointF(415f, 165f), new PointF(330f,165f)
        };

        PointF[] starCords = { new PointF(320f, 190f), new PointF(370f, 90f), new PointF(420f, 190f), new PointF(520f, 240f),
                               new PointF(420f, 290f), new PointF(370f, 390f), new PointF(320f, 290f), new PointF(220f, 240f), new PointF(320f, 190f)  };

        PointF[] swordCords = { new PointF(390f, 400f), new PointF(390f, 130f), new PointF(367.5f, 40f), new PointF(345f, 130f), new PointF(345f, 400f),
                                new PointF(0f, 0f), new PointF(290f, 400f), new PointF(450f, 400f), new PointF(450f, 415f), new PointF(290f, 415f), new PointF(290f, 400f),
                                new PointF(0f, 0f), new PointF(360f, 415f), new PointF(360f, 500f), new PointF(0f, 0f), new PointF(380f, 415f), new PointF(380f, 500f),
                                new PointF(360f, 415f), new PointF(0f, 0f), new PointF(345f, 500f), new PointF(345f, 535f), new PointF(395f, 535f),
                                new PointF(395f, 500f), new PointF(345f, 500f)};

        string fileName = "Untitled";
        Bitmap bmp;

        Panel currentMenu;
        
        int i = 0;
        Pen pen;
        ArtMode artMode;
        List<ArtAction> actions = new List<ArtAction>();
        ArtManager artManager = new ArtManager();

        HistoryForm history;

        int j;
        PointF[] targetCords;

        CanvasSettings canvasSettings;
        public Form1()
        {
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new ColorTable());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "ELPaint - " + fileName;

            BrushSettingsMenu.Show();
            ShapesMenu.Hide();
            EraserMenu.Hide();
            AutoPainterMenu.Hide();
            artMode = ArtMode.HandDraw;

            currentMenu = BrushSettingsMenu;

            pen = new Pen(Color.Black, 10f);           
            for (int i = 1; i <= 10; i++)            
                BrushSizeCB.Items.Add(i);            
            BrushSizeCB.SelectedItem = 5;

            history = new HistoryForm();
        }
        
        // Στο region Toolbar βρίσκεται ο κώδικας που αφορά τα μενού που υπάρχουν στο Tool Section της φόρμας (οχι του menustrip).
        //Αφορά μόνο την εναλλαγή των μενού και του ArtMode, δηλαδή την λειτουργία που θα εκτελεί η εφαρμογή
        #region ---------------------------Toolbar---------------------------
        //Μέθοδος υπεύθηνη για την εναλλαγή των μενού. Το μενού με τις ρυθμίσεις της πένας (brush)θέλουμε να φαίνεται συνέχεια και πάνω
        //απο κάθε άλλο μένου 
        private void ToggleToolMenu(Panel currentMenu, Panel targetMenu)
        {
            if(currentMenu != BrushSettingsMenu)
                currentMenu.Hide();
            if(targetMenu != null && !targetMenu.Visible)
                targetMenu.Show();  
        }
        private void BrushBtn_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.HandDraw;           
            ToggleToolMenu(currentMenu, null);
            currentMenu = BrushSettingsMenu;
        }
        private void EraserBtn_Click(object sender, EventArgs e)
        {
            //Άλλαξε την λειτουργεία σε γόμμα 
            artMode = ArtMode.Erase;
            //Αν δεν φαίνεται ηδη το μενού που ζήτησε να δει ο χρήστης τότε εμφάνισε το
            ToggleToolMenu(currentMenu, EraserMenu);
            currentMenu = EraserMenu;
        }
        private void ShapesBtn_Click(object sender, EventArgs e)
        {
            ToggleToolMenu(currentMenu, ShapesMenu);
            currentMenu = ShapesMenu;
        }
        private void AutoPainterBtn_Click(object sender, EventArgs e)
        {
            ToggleToolMenu(currentMenu, AutoPainterMenu);
            currentMenu = AutoPainterMenu;
        }

        private void LineBtn_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.Line;
        }
        private void RectangleBtn_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.Rectangle;
        }
        private void EllipseBtn_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.Ellipse;
        }
        private void CircleBtn_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.Circle;
        }
        private void HandDraw_ColorPickerBtn_Click(object sender, EventArgs e)
        {
            HandDraw_ColorIndic.BackColor = SetColor();
        }
        #endregion

        // Στο region Menustrip βρίσκεται ο κώδικας που αφορά τα μενού που υπάρχουν στο Menu strip της φόρμας.
        //Αφορά μόνο την εναλλαγή των μενού και του ArtMode, δηλαδή την λειτουργία που θα εκτελεί η εφαρμογή
        #region ---------------------------Menustrip---------------------------
        //Κουμπί εκκαθάρισης του canva.
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            i = 0;
            actions.Clear();
            if (canvas.Image != null)
                canvas.Image = null;
            canvas.Invalidate();
        }
        private void brushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.HandDraw;
            ToggleToolMenu(currentMenu, null);
            currentMenu = BrushSettingsMenu;
        }

        private void eraserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artMode = ArtMode.Erase;
            ToggleToolMenu(currentMenu, EraserMenu);
            currentMenu = EraserMenu;
        }

        private void shapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolMenu(currentMenu, ShapesMenu);
            currentMenu = ShapesMenu;
        }

        private void autoPainterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolMenu(currentMenu, AutoPainterMenu);
            currentMenu = AutoPainterMenu;
        }
        private void historyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            history = new HistoryForm();
            history.Show();
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Εισαγωγή εικόνας απο τον υπολογιστή 
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    canvas.Image = Image.FromFile(openFileDialog.FileName); 
                    //Αλλαγή του ονόματος της φόρμας για να ταιριάζει με την είκονα που εισάχθηκε
                    fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    this.Text = "ELPaint - " + fileName; 
                }
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Δημιουργία bitmap απο το περιεχόμενο του canva
            bmp = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            canvas.DrawToBitmap(bmp, canvas.ClientRectangle);

            //Εξαγωγή του περιεχομένου του Canva(pictureBox) απο την εφαρμογή στον υπολογιστή 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Images|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }   
        }
        private void canvasSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Άνοιγμα του μενού που αλλάζει το μέγεθος του canva
            canvasSettings = new CanvasSettings(this, canvas.Width.ToString(), canvas.Height.ToString());
            canvasSettings.Show();
        }
        #endregion

        #region ---------------------------Draw---------------------------
        //Όταν πατηθεί το ποντίκι, ανάλογα με την λειτουργία που έχει επιλεχθεί δημιουργείτε ενα νέο action
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            //Setting pen
            pen.Width = float.Parse(BrushSizeCB.SelectedItem.ToString());
            pen.Color = HandDraw_ColorIndic.BackColor;

            if (artMode == ArtMode.HandDraw)            
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.HandDraw));                                     
            else if (artMode == ArtMode.Erase)            
                actions.Add(new ArtAction(Color.White, pen.Width, ArtMode.Erase));            
            else if (artMode == ArtMode.Line)            
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.Line));            
            else if (artMode == ArtMode.Rectangle)           
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.Rectangle));            
            else if (artMode == ArtMode.Ellipse)            
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.Ellipse));            
            else if (artMode == ArtMode.Circle)           
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.Circle));                                           
        }
        //Το κάθε action περιέχει μια λίστα με points η οποία 'γεμίζει' καθώς κινείται το ποντίκι
        //Μόλις καταγράφεται ενα point γίνεται redrawn ο canvaw ώστε οι αλλαγές να φαίνονται live
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Cordinates_X.Text = "X: " + e.X;
            Cordinates_Y.Text = "Y: " + e.Y;

            if (e.Button == MouseButtons.Left)
            {
                actions[i].points.Add(e.Location);
                canvas.Invalidate();
            }              
        }
        //Μόλις τελειώσει το εκάστοτε action, ο counter της λίστας των actions αυξάνεται και καταγράφεται το αντίστοιχο σχήμα στο ιστορικό
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            i++;
            if (artMode == ArtMode.HandDraw)            
                history.Add("Free Draw", DateTime.Now.ToString());           
            else if (artMode == ArtMode.Line)            
                history.Add("Line", DateTime.Now.ToString());            
            else if (artMode == ArtMode.Rectangle)
                history.Add("Rectangle", DateTime.Now.ToString());
            else if (artMode == ArtMode.Ellipse)
                history.Add("Ellipse", DateTime.Now.ToString());
            else if (artMode == ArtMode.Circle)
                history.Add("Circle", DateTime.Now.ToString());
        }
        //Κάθε φορά που καλείτε η Invalidate(), καλείτε και αυτό το event, το οποίο με τη σειρά του καλέι την συνάρτηση Draw() του 
        //ArtManager, αυτού δηλάδη που είναι υπεύθυνος να ζωγραφίζει το σχήμα που ζητήθηκε
        private void canvas_Paint(object sender, PaintEventArgs e)
        {            
            artManager.Draw(e.Graphics, actions);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;            
        }
        #endregion

        // Στο region AutoPainter Shapes Buttons βρίσκεται ο κώδικας που αφορά την λειτουργία της αυτόματης ζωγραφικής.       
        #region ---------------------------AutoPainter Shapes Buttons---------------------------
        //Το κάθε κουμπί αφού ενημερώσει το ιστορικό οτι θα φτιαχτεί ενα νεο σχήμα, εκχωρεί σε ενα πίνακα τις συντεταγμένες που θέλει 
        //να ζωγραφιστούν, ετοιμάζει τον timer και την πένα, ξεκινάει τον timer
        private void AP_RobotBtn_Click(object sender, EventArgs e)
        {
            history.Add("Robot", DateTime.Now.ToString());
            targetCords = robotCords;
            SetupAutoPaint();
        }
        private void AP_StarBtn_Click(object sender, EventArgs e)
        {
            history.Add("Star", DateTime.Now.ToString());
            targetCords = starCords;
            SetupAutoPaint();
        }
        private void AP_HouseBtn_Click(object sender, EventArgs e)
        {
            history.Add("House", DateTime.Now.ToString());
            targetCords = houseCords;
            SetupAutoPaint();
        }
        private void AP_SwordBtn_Click(object sender, EventArgs e)
        {
            history.Add("Sword", DateTime.Now.ToString());
            targetCords = swordCords;
            SetupAutoPaint();
        }
        private void SetupAutoPaint()
        {
            j = 0;
            pen.Width = float.Parse(BrushSizeCB.SelectedItem.ToString());
            pen.Color = HandDraw_ColorIndic.BackColor;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Έλεγχος για το αν έφτασε στο τέλος της λίστας  
            if (j == targetCords.Length - 1)
            {
                timer1.Stop();
                return;
            }
            //Για να μπορεί να πηγαίνει σημεία που δεν είναι γειτονικά(συνεχή) χρησιμοποιείθηκε ενα dummy σημείο το (0,0), 
            //το οποίο οταν το βρίσκει η εφαρμογή δεν το ζωγραφίζει και περνάει στο επόμενο.
            if ((targetCords[j + 1].X != 0 && targetCords[j + 1].Y != 0) && (targetCords[j].X != 0 && targetCords[j].Y != 0))
            {
                actions.Add(new ArtAction(pen.Color, pen.Width, ArtMode.Line));
                actions[i].points.Add(targetCords[j]);
                actions[i].points.Add(targetCords[j + 1]);
                i++;
                canvas.Invalidate();
            }
            j++;
        }
        #endregion

        #region ---------------------------Setters---------------------------
        private Color SetColor()
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                return colorDialog1.Color;
            }
            else
                return Color.Black;
        }
        public void SetCanvasSize(int width, int height)
        {
            canvas.Width = width;
            canvas.Height = height;
        }
        #endregion
    }
    public class ColorTable : ProfessionalColorTable
    {
        Color color = Color.FromArgb(64, 64, 64);
        public override Color MenuItemSelected
        {
            get { return Color.Gray; }
        }
        public override Color MenuItemBorder
        {
            get { return color; }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return color; }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return color; }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get { return color; }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return color; }
        }
    }
}
