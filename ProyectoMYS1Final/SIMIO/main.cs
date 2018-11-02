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
        
        public Main() {
            ariport = new Dictionary<int, IIntelligentObject>();

        }

        internal void start(string filePath_air, string filePath_rut)
        {
            INodeObject airplaneOutput = null; INodeObject personOutput = null;
            simio = new SimioFile();
            read = new ReadCsv(filePath_air,',');
            string[] line = read.getline();
            line = read.getline();
            
            IIntelligentObject airplane = simio.createSource("airplane", 0, 0, 0);
            IIntelligentObject person = simio.createSource("person", 2, 0, 0);
            airplaneOutput = simio.getNodeOutput(airplane);
            personOutput = simio.getNodeOutput(person);
            simio.createSink("salida", 1, 1, 1);

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

            simio.saveFile();
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
            simio.createSink(line[1] + "sink", Int16.Parse(line[2]), Int16.Parse(line[3]), Int16.Parse(line[4]));

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
