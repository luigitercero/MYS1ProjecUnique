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


            crear();


            guardarModeloSalida();

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

        }

    }
}
