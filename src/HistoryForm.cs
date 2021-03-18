using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exercise7
{
    public partial class HistoryForm : Form
    {
        SQLiteConnection conn;
        string connString = "Data Source=LPaint.db;Version = 3;";

        public HistoryForm()
        {
            InitializeComponent();
            LoadHistoryToListView();

            listView1.GridLines = true;
        }
        private void LoadHistoryToListView()
        {
            conn = new SQLiteConnection(connString);
            conn.Open();
            String selectQuery = "SELECT shape, time FROM History";
            SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string[] row = {reader.GetString(0), reader.GetString(1)};
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                }
            }
            conn.Close();
        }
        public void Add(string shapeName, string timestamp)
        {
            conn = new SQLiteConnection(connString);
            conn.Open();

            String insertQuery = "INSERT INTO history(shape, time) VALUES(@name, @time)";
            SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@name", shapeName);
            cmd.Parameters.AddWithValue("@time", timestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            LoadHistoryToListView();
        }
    }
}
