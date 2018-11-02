using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoMYS1Final
{
    public partial class CSVview : Form
    {
        string ruta;
        int tipo;
        public CSVview()
        {
            InitializeComponent();
        }

        public CSVview(string path,int tipo)
        {
            this.ruta = path;
            this.tipo = tipo;
            InitializeComponent();
            DataTable datos = ConvertCSVtoDataTable(path);
            dataGridView1.DataSource = datos;

        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.tipo == 1)
            {
                Form1.ruta_airports = this.ruta;
            }
            else
            {
                Form1.ruta_vuelos = this.ruta;
            }
            this.Close();
        }
    }
}
