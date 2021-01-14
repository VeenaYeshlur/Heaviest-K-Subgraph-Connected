using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using static HeaviestSubgraphConnected.Helper;
using System.Diagnostics;

namespace HeaviestSubgraphConnected
{

    public static class Program
    {

        public static Stopwatch Watch { get; private set; }
        public static string outputFolderName;
        public static string outputFile;
        public static string statsFile;

        // Creating a dictionary 
        // using Dictionary<TKey,TValue> class 
        public static Dictionary<int, string> termID = new Dictionary<int, string>();

        // Creating a dictionary 
        // using Dictionary<TKey,TValue> class 
        public static Dictionary<string, string> configDetail = new Dictionary<string, string>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                //processing time 
                Watch = new Stopwatch();
                Watch.Start();

                int k = 7;
                int i;
                int j;
                int l = (k * (k - 1)) / 2;   //default upper bound

                //initialise
                edgelist el;
                vertexDeg nd;
                termList[] termList = new termList[3000];
                folderList[] folderList = new folderList[11];
                configList[] configList = new configList[10];

                //store list of vertices
                List<int> vertices = new List<int>();               

                //get the path for config file stored in current directory
                var path =Directory.GetCurrentDirectory() + "\\HkCSconfig.ini";

                //read config details
                configList = readConfigDetails(path);

                string fileListPath = "";
                string inputFilePath = "";
                string termFilePath = "";
                string statsFilePath = "";

                string kVal = "";
                configDetail.TryGetValue("k", out kVal); k = Int32.Parse(kVal) + 2;
                configDetail.TryGetValue("fileListPath", out fileListPath);
                configDetail.TryGetValue("inputFilePath", out inputFilePath);
                configDetail.TryGetValue("termFilePath", out termFilePath);
                configDetail.TryGetValue("statsFilePath", out statsFilePath);
                

                //read foldername to be processed
                folderList = readfolderList(fileListPath);

                //string fileListPath = "C:\\Reading\\MSc Project\\FileList.txt";
                //string inputFilePath = "C:\\Reading\\MSc Project\\Test Data\\";
                //string termFilePath = "C:\\Reading\\MSc Project\\Test Data\\";
                //string statsFilePath = "C:\\Reading\\MSc Project\\KF";

                //branch bound algorithm recursive iteration
                for (int g = 0; g < 10; g++)
                {
                    Console.WriteLine(g);
                    //input filepath
                    string inputFile = inputFilePath + folderList[g].folderName + "\\TermGraph.txt";
                    string termFile = termFilePath + folderList[g].folderName + "\\TermsID.txt";
                    string statsFile = statsFilePath + k.ToString() + "Stats.txt";

                    outputFile = "C:\\Reading\\MSc Project\\K" + kVal.ToString() + "Foutput_" + folderList[g].folderName + ".txt";
                    outputFolderName = folderList[g].folderName;

                    Console.Write("Reading weighted edgelist from file {0}\n");
                    //read edgelist from the input file
                    el = readEdgeList(inputFile);

                    //sorting edges in non-increasing order of weight
                    Array.Sort(el.edges, Helper.compare_edges);

                    //read TermsID file into a list for lookup
                    termList = readTermFile(termFile);


                    //calculate weighted node degree
                    vertexDeg[] vdList = new vertexDeg[el.n];
                    vdList = prepareWeightedVertexList(el);

                    //vertex list of the main graph
                    prepareVertexList(el, vertices);

                    //store edges as tuple
                    var edges = new List<Tuple<int, int, int>> { };
                    for (i = 0; i < el.e; i++)
                    {
                        edges.Add(Tuple.Create((int)el.edges[i].u, (int)el.edges[i].v, (int)el.edges[i].w));

                    }

                    //deduce the main graph with vertices and edges
                    var graph = new Graph<int>(vertices, edges);

                    var BBalgorithm = new BBAlgorithm();
                    int vCount = vertices.Count;
                    int startVertex = 0;
                    double ub = 0;
                    double defaultub = 0;       //default upper bound

                    j = 0;
                    bool visited;

                    //compute upper bound
                    for (i = 0; i < l; i++)
                    {
                        ub += el.edges[i].w;
                    }

                    defaultub = ub; //k* ub / el.e;
                    //List to store subGraph Structure 
                    subGraph[] subGraphList = new subGraph[NLINKS];

                    //loop through the vertices and call DFS function to create subgraph
                    for (i = 0; i < vCount && vdList[i].w>100; i++)
                    {
                        //assign startVertex from vertexdeg list with highest weight
                        startVertex = (int)vdList[i].n;

                        //initialise
                        visited = false;
                        ub = defaultub;

                        vdList[i].visited = false;
                        subGraphList[i].n = startVertex;
                        subGraphList[i].subgraphVertices = new List<Tuple<int, int, int>> { };

                        //call DFS function to find the next connected vertex
                        BBAlgorithm.depthFirstSearch(startVertex, ub, graph, k, el, vdList, edges, visited, i, subGraphList);

                    }

                    //find max weight in the subgraph
                    int bestSolution = subGraphList.Max(w => w.weight);
                    var MaxWeightIndex = new List<int>();

                    i = 0;
                    //store the index for the node with max weight
                    while (subGraphList[i].subgraphVertices != null)
                    {
                        if (subGraphList[i].weight == bestSolution)
                        {
                            MaxWeightIndex.Add(i);
                        }
                        i++;
                    }


                    //print results on screen
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form1 myForm = new Form1();
                    myForm.UpdateText(MaxWeightIndex, subGraphList, Int32.Parse(kVal));
                    Application.Run(myForm);

                    Watch.Stop();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
   

}



