using ProyectoMYS1Final.SIMIO;
using SimioAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoMYS1Final
{
    public partial class Form1 : Form
    {


        static ISimioProject _simioproyecto;
        String _rutaBase = Application.StartupPath + "/SimioModel.spfx";
        String _rutaSalida = Application.StartupPath + "/final/SimioShow.spfx";
        IModel _modelo;
        string[] warnings;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(_rutaSalida)) {
                File.Delete(_rutaSalida);
            }
            _simioproyecto = SimioProjectFactory.LoadProject(_rutaBase, out warnings);
            _modelo = _simioproyecto.Models[1];


            //crear();

   
            //guardarModeloSalida();

        }
        private void guardarModeloSalida() {
            SimioProjectFactory.SaveProject(_simioproyecto, _rutaSalida,out warnings);
        }

        private void crear() {
            IIntelligentObjects _objetos = _modelo.Facility.IntelligentObjects;
            int x = 0;
            int z = 0;
            int y = 1;
            IIntelligentObject combiner = _objetos.CreateObject("Combiner", new FacilityLocation(x, z, y));
            combiner.ObjectName = "fabi";
            INodeObject input = seekForName("MemberInput@" + combiner.ObjectName) as INodeObject;
            INodeObject output = seekForName("ParentInput@" + combiner.ObjectName) as INodeObject;
            List<FacilityLocation> listpoints = new List<FacilityLocation>();
            FacilityLocation fl = new FacilityLocation(35, 36, 86);
            listpoints.Add(fl);
            IIntelligentObject connect1 = _objetos.CreateLink("Connector", input, output, listpoints);
        }

        private IIntelligentObject seekForName(String name) {
            //IIntelligentObject entity = null;
            IIntelligentObjects list = _modelo.Facility.IntelligentObjects;
            foreach (var entity in list)
            {
                if (entity.ObjectName == name) {
                    return entity;
                }
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv fieles(*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            //ReadCsv read = new ReadCsv();
            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);

            Main main = new Main();

            main.start(filePath);
        }
        
    }
}
