using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMYS1Final.SIMIO
{
    class ReadCsv
    {

        StreamReader file;
        private char separator;
        public ReadCsv(string path) {
            separator = ';';
            open(path);
        }
        /**
         * el separador se usa para delimitar acciones
         */
        public ReadCsv(string path, char separator) {
            this.separator = separator;
            open(path);
        }
        public Boolean open(String path) {
          
            StreamReader objReader = new StreamReader(path);
            file = objReader;
            return false;
        }

        public string[] getline() {
            String sLine = file.ReadLine();
            if (sLine != null)
                return sLine.Split(separator);
            return null;
        }




    }
}
