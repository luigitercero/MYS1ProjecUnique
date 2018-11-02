using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimioAPI;
using System.Windows.Forms;
using System.IO;

namespace ProyectoMYS1Final.SIMIO
{
    
    class SimioFile
    {
        static ISimioProject SimioProject;
        String inputFiele = Application.StartupPath + "/SimioModel.spfx";
        String outputFile = Application.StartupPath + "/final/SimioShow.spfx";
        public IModel model;

        string[] warnings;

        public SimioFile() {
            if (File.Exists(this.outputFile))
            {
                File.Delete(this.outputFile);
            }
            SimioProject = SimioProjectFactory.LoadProject(inputFiele, out warnings);
            this.model = SimioProject.Models[1];
        }

        public void saveFile()
        {
            SimioProjectFactory.SaveProject(SimioProject, this.outputFile, out warnings);
        }

        public IIntelligentObject createObject(String type,String name,int possX,int possY, int possZ) {
            IIntelligentObjects _objects = this.model.Facility.IntelligentObjects;
            IIntelligentObject _object = _objects.CreateObject(type, new FacilityLocation(possX, possZ, possY));
            _object.ObjectName = name;
            return _object;
        }

        public IIntelligentObject createServer(String name, int x, int y, int z) {
           return createObject("Server", name, x, y, z);
        }

        public IIntelligentObject createCombiner(String name, int x, int y, int z)
        {
            IIntelligentObject combiner = createObject("Combiner", name, x, y, z);
            combiner.Properties["ExitedAddOnProcess"].Value = "EscribirBitacora";
            return combiner;
        }

        internal IIntelligentObject addConnector(INodeObject airplane, INodeObject parent, object p)
        {
            return this.getObjectList().CreateLink("Connector", airplane, parent, null);
        }

        public IIntelligentObject createSource(String name, int x, int y, int z)
        {
            IIntelligentObject source = createObject("Source", name, x, y, z);
            if(name.Equals("person"))
                source.Properties["ExitedAddOnProcess"].Value = "CrearPersonas";
            return source;
        }

        public IIntelligentObject createSink(String name, int x, int y, int z)
        {
            return createObject("Sink", name, x, y, z);
        }
        public IIntelligentObject createTransferNode(String name, int x, int y, int z)
        {
            return createObject("Trasnfer Node", name, x, y, z);
        }

        public INodeObject getNodeOutput(IIntelligentObject _objcect) {
            INodeObject node = seekForName("Output@" + _objcect.ObjectName) as INodeObject;
            return node;
        }

        public INodeObject getParentIpunt(IIntelligentObject _objcect)
        {
            INodeObject node = seekForName("ParentInput@" + _objcect.ObjectName) as INodeObject;
            return node;
        }

        public INodeObject getMemberIpunt(IIntelligentObject _objcect)
        {
            INodeObject node = seekForName("MemberInput@" + _objcect.ObjectName) as INodeObject;
            return node;
        }


        private IIntelligentObject seekForName(String name)
        {
            //IIntelligentObject entity = null;
            IIntelligentObjects list = model.Facility.IntelligentObjects;
            return list[name];

        }
        public IIntelligentObjects getObjectList() {
            IIntelligentObjects list = model.Facility.IntelligentObjects;
            return list;
        }

        public  void addFailure(IIntelligentObject _object, String nameFail) {
            _object.Properties["FailureType"].Value = nameFail;
        }

    }
}
