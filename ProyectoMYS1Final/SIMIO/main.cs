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

        internal void start(string filePath)
        {
            INodeObject airplaneOutput = null; INodeObject personOutput = null;
            simio = new SimioFile();
            read = new ReadCsv(filePath,',');
            string[] line = read.getline();
            line = read.getline();
            createBase(airplaneOutput, personOutput);

            while (line != null) {
                createAirPort(line, airplaneOutput, personOutput);
                line = read.getline();
            }

            simio.saveFile();
        }
        private void createAirPort(String[] line, INodeObject airplane, INodeObject person)
        {

            IIntelligentObject _object = simio.createCombiner(line[1], Int16.Parse(line[2]), Int16.Parse(line[3]), Int16.Parse(line[4]));
            this.ariport.Add(Int16.Parse(line[0]),_object);
            INodeObject parent = this.simio.getParentIpunt(_object);
            INodeObject member = this.simio.getMemberIpunt(_object);
            IIntelligentObject connect1 = simio.addConnector(airplane, parent, null);
            IIntelligentObject connect2 = simio.addConnector(airplane, parent, null);

            simio.addFailure(_object, line[5]);
            

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
