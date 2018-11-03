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
            combiner.Properties["MemberEnteredAddOnProcess"].Value = "EscribirBitacora";
            combiner.Properties["ParentEnteredAddOnProcess"].Value = "TotalAviones";
            combiner.Properties["BatchQuantity"].Value = "100";
            
            return combiner;
        }

        internal IIntelligentObject addConnector(INodeObject airplane, INodeObject parent, object p)
        {
            return this.getObjectList().CreateLink("Connector", airplane, parent, null);
        }

        internal IIntelligentObject addPath(INodeObject inicio, INodeObject fin, IEnumerable<FacilityLocation> points)
        {
            return this.getObjectList().CreateLink("Path", inicio, fin, points);
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

        public INodeObject getIpunt(IIntelligentObject _objcect)
        {
            INodeObject node = seekForName("Input@" + _objcect.ObjectName) as INodeObject;
            return node;
        }


        public IIntelligentObject seekForName(String name)
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

        public IPropertyDefinition createProperty()
        {
            IPropertyDefinition property = this.model.PropertyDefinitions.AddExpressionProperty("tiempoProceso", "1");
            return property;
        }

        public void addProperty(IIntelligentObject _object)
        {
            _object.Properties["ProcessingTime"].Value = "tiempoProceso";
        }

        public IExperiment createExperiment(String name)
        {
            IExperiment experiment = model.Experiments.Create(name);

            // Setup the experiment (optional)
            // Specify run times.
            IRunSetup setup = experiment.RunSetup;
            setup.StartingTime = new DateTime(2018, 10, 03);
            setup.WarmupPeriod = TimeSpan.FromHours(0);
            setup.EndingTime = experiment.RunSetup.StartingTime + TimeSpan.FromDays(1);
            experiment.ConfidenceLevel = ExperimentConfidenceLevelType.Point95;
            experiment.LowerPercentile = 25;
            experiment.UpperPercentile = 85;

            // Add event handler for events from experiment
            //experiment.ScenarioEnded += new EventHandler<ScenarioEndedEventArgs>(experiment_ScenarioEnded);
            //experiment.RunCompleted += new EventHandler<RunCompletedEventArgs>(experiment_RunCompleted);
            //experiment.RunProgressChanged += new EventHandler<RunProgressChangedEventArgs>(experiment_RunProgressChanged);
            //experiment.ReplicationEnded += new EventHandler<ReplicationEndedEventArgs>(experiment_ReplicationEnded);
            
            return experiment;
        }

        public void addScenarios(IExperiment experimento)
        {
            experimento.Scenarios.Create("Scenario2");
            experimento.Scenarios.Create("Scenario3");
        }

        public void addTiempos(IExperiment experimento)
        {
            IExperimentControl experimentControl = experimento.Controls[0];
            experimento.Scenarios[0].SetControlValue(experimentControl, "Random.Triangular(35,45,60)");
            experimento.Scenarios[0].ReplicationsRequired = 1;
            experimento.Scenarios[1].SetControlValue(experimentControl, "Random.Triangular(30,40,50)");
            experimento.Scenarios[1].ReplicationsRequired = 1;
            experimento.Scenarios[2].SetControlValue(experimentControl, "Random.Uniform(30,50)");
            experimento.Scenarios[2].ReplicationsRequired = 1;
        }

        public void addResponses(IExperiment experimento, String response1)
        {
            experimento.Responses.Create("Response1");
            experimento.Responses[0].Expression = response1;
            experimento.Responses.Create("Response2");
        }
    }
}
