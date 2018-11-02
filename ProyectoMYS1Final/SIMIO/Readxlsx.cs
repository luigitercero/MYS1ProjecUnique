using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Threading.Tasks;

namespace ProyectoMYS1Final.SIMIO
{
    class Readxlsx
    {
        List<string> lineas = new List<string>();
        private int numLinea;
        private int totalFilas;
        public Readxlsx(string path)
        {
            open(path);
            numLinea = 0;
        }
        
        public void open(String path)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
            {
                var sheet = xlPackage.Workbook.Worksheets.First();
                totalFilas = sheet.Dimension.End.Row;
                var totalColumnas = sheet.Dimension.End.Column;
                
                for (int numFila = 1; numFila <= totalFilas; numFila++)
                {
                    var row = sheet.Cells[numFila, 1, numFila, totalColumnas].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    lineas.Add(string.Join(",", row));
                }
            }
        }

        public string[] getline()
        {
            if (numLinea < totalFilas)
            {
                String sLine = lineas[numLinea];
                Console.WriteLine(numLinea);
                numLinea++;
                if (sLine != null)
                    return sLine.Split(',');
            }
            return null;
        }
    }
}
