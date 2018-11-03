using SimioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMYS1Final.SIMIO
{
    class Main
    {
        private SimioFile simio;
        ReadCsv read;
        Dictionary <int,IIntelligentObject> ariport;
        String cadenaResponse1;
        
        public Main() {
            ariport = new Dictionary<int, IIntelligentObject>();
            cadenaResponse1 = "";
        }

        internal void start(string filePath_air, string filePath_rut)
        {
            INodeObject airplaneOutput = null; INodeObject personOutput = null;
            simio = new SimioFile();
            read = new ReadCsv(filePath_air,',');
            string[] line = read.getline();
            line = read.getline();
            
            IIntelligentObject airplane = simio.createSource("airplane", 0, 0, 0);
           // airplane.Properties["EntityType"].Value = "airplane";
            airplane.Properties["InterarrivalTime"].Value = "Random.Poisson(0.2)";
            IIntelligentObject person = simio.createSource("person", 2, 0, 0);
            airplaneOutput = simio.getNodeOutput(airplane);
            personOutput = simio.getNodeOutput(person);
            simio.createSink("salida", 1, 1, 1);

            //creacion de Referenced Property para los escenarios del experimento
            IPropertyDefinition propiedadEscenarios = simio.createProperty();

            while (line != null) {
                createAirPort(line, airplaneOutput, personOutput);
                line = read.getline();
            }

            //rutas
            read = new ReadCsv(filePath_rut, ',');
            string[] line_r = read.getline();
            line_r = read.getline();

            while (line_r != null)
            {
                createRuta(line_r);
                line_r = read.getline();
            }

           
            //creando el experimento
            IExperiment experiment = simio.createExperiment("Experimento");
            simio.addScenarios(experiment);
            simio.addTiempos(experiment);
            //simio.addResponses(experiment, cadenaResponse1);

            experiment.Responses.Create("Response1");
            experiment.Responses[0].Expression = cadenaResponse1;
            experiment.Responses.Create("Response2");

            IExperiment experiment2 = simio.model.Experiments[0];
                    //experiment.res
                    experiment.RunCompleted += new EventHandler<RunCompletedEventArgs>(experiment_RunCompleted);                    
                    experiment2.RunAsync();

            //experiment.RunAsync();
            simio.saveFile();
        }

        private void experiment_RunCompleted(object sender, RunCompletedEventArgs e)
        {

            IExperiment exper = (IExperiment)sender;
            System.Windows.Forms.MessageBox.Show((exper.Scenarios[0].ResponseValues["Response1"].ToString()));
            System.Windows.Forms.MessageBox.Show((exper.Scenarios[0].ResponseValues["Response2"].ToString()));
            Console.WriteLine(exper.Scenarios[0].ResponseValues["Response1"].ToString());
        }

        private void createRuta(String[] line)
        {
            //encontrar los nodos
            INodeObject inicio = simio.seekForName("Output@" + ariport[Int16.Parse(line[1])].ObjectName) as INodeObject;
            INodeObject fin = simio.seekForName("Input@" + ariport[Int16.Parse(line[0])].ObjectName+"sink") as INodeObject;
            //puntos
            List<FacilityLocation> listpoints = new List<FacilityLocation>();
            //FacilityLocation fl = new FacilityLocation(35, 36, 86);
            //listpoints.Add(fl);
            IIntelligentObject path=simio.addPath(inicio, fin, listpoints);
            Console.WriteLine(path.Properties);

        }
        private void createAirPort(String[] line, INodeObject airplane, INodeObject person)
        {

            IIntelligentObject _object = simio.createCombiner(line[1], Int16.Parse(line[2]), Int16.Parse(line[3]), Int16.Parse(line[4]));
            
            this.ariport.Add(Int16.Parse(line[0]), _object);
            INodeObject parent = this.simio.getParentIpunt(_object);
            INodeObject member = this.simio.getMemberIpunt(_object);
            simio.addConnector(airplane, parent, null);
            simio.addConnector(person, member, null);
            simio.addFailure(_object, line[5]);
            simio.addProperty(_object);
            simio.createSink(line[1] + "sink", Int16.Parse(line[2]), Int16.Parse(line[3]), Int16.Parse(line[4]));
            if (cadenaResponse1 != "")
            {
                cadenaResponse1 += " + " + line[1] + "sink.InputBuffer.NumberEntered";
            }
            else
            {
                cadenaResponse1 = line[1] + "sink.InputBuffer.NumberEntered";
            }
        }
        private void createBase(INodeObject airplaneOutput, INodeObject personOutput) {
            IIntelligentObject airplane = simio.createSource("airplane", 0, 0, 0);
            IIntelligentObject person = simio.createSource("person", 2, 0, 0);
            airplaneOutput = simio.getNodeOutput(airplane);
            personOutput = simio.getNodeOutput(person);
            simio.createSink("salida", 1, 1, 1);
        }
        

       
    }

   
}
