using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HeaviestSubgraphConnected.Helper;
using static HeaviestSubgraphConnected.Program;

/// <summary>
/// This module contains all the functions necessary for branch and bound algorithm
/// </summary>
namespace HeaviestSubgraphConnected
{
    public static class Helper
    {
        public const int NLINKS = 10000000; //Maximum number of links, will increase if needed.
        
       
        //list to store edges
        public struct edge
        {
            public uint u; //source node
            public uint v; //target node
            public double w; //edge weight       

        }

        public struct edgelist
        {
            public uint n; //number of nodes
            public uint e; //number of edges
            public edge[] edges; //edge list
            public uint[] map; //maping: map[newID] = oldID;
        }

        //list to store contents of TermsID file
        public struct termList
        {
            public int termid;         //termid
            public string termdesc;    //termdesc
           
        }

        //list to store folder list
        public struct folderList
        {
            public string folderName;  //folderName

        }

        //list to store config details
        public struct configList
        {
            public string configKey;
            public string configValue;

            public int k;
            public string fileListPath;
            public string inputFilePath;
            public string termFilePath;
            public string statsFilePath;
            public string outputFilePath;
        }

        //list to store Vertex Weigted Degree
        public struct vertexDeg
        {
            public uint n;          //node id
            public double w;       //weight
            public bool executed;
            public bool visited;

        }

        //list to store Subgraph 
        public struct subGraph
        {
            public int n;                                       //node id
            public List<Tuple<int, int, int>> subgraphVertices; //edge list
            public int weight;
        }

        public struct vertex
        {
            public int v;
        }

        //For future use in qsort.
        internal static int compare_vertexDeg(vertexDeg a, vertexDeg b)
        {
            vertexDeg pa = a;
            vertexDeg pb = b;

            if (pa.w <= pb.w)
            {
                return 1;
            }
            return -1;
        }

        //For future use in qsort.
        internal static int compare_edges(edge a, edge b)
        {
            edge pa = a;
            edge pb = b;
            if (pa.w <= pb.w)
            {
                return 1;
            }
            return -1;
        }

        //returns the index from the edge list for given u and v
        public static int FindAnEdgeinEdgeList(edgelist el, int u, int v)
        {
            int i = 0;
            try
            {

               
                for (i = 0; i < el.e; i++)
                {
                    //returns the index for found egdes 
                    if (el.edges[i].u == u && el.edges[i].v == v)
                    {
                        break;
                    }

                }
                return i;
            }
            

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return i;

            }

        }

        //returns the index for an edge in the subgraph
        public static int FindEdgeinSubGraph(List<Tuple<int, int, int>> sg, int a, int b)
        {
            int i = -1;
            try
            {
                for (i = 0; i < sg.Count; i++)
                {
                    //find if edge already exists 
                    if (a.Equals(sg[i].Item1) && b.Equals(sg[i].Item2) || b.Equals(sg[i].Item1) && a.Equals(sg[i].Item2))
                    {
                        return i;
                    }

                }
                return -1;

            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return -1;

            }
            
        }


        //checks if the vertex has been visited
        public static bool IsVisited(int startVertex, vertexDeg vd)
        {
            int i = 0;
            bool visited = false;

            try
            {

                for (i = 0; i < vd.n; i++)
                {
                    if (vd.n == startVertex)
                    {
                        visited = vd.visited;
                    }
                }
                return visited;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return visited;

            }
        }

        //read weighted edgelist from the file named input
        public static Helper.edgelist readEdgeList(string input)
        {
            int s = NLINKS;
            edgelist el = new edgelist();
            el.edges = new edge[s];
            el.n = 0;
            el.e = 0;

            string l = "";

            try
            {
                using (StreamReader sr = new StreamReader(input))
                {

                    while ((l = sr.ReadLine()) != null)
                    {
                        var lineParts = l.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (lineParts.Length == 3)
                        {
                            el.edges[el.e] = new edge();
                            el.edges[el.e].u = uint.Parse(lineParts[0]);
                            el.edges[el.e].v = uint.Parse(lineParts[1]);
                            el.edges[el.e].w = double.Parse(lineParts[2]);

                            var wd = new List<uint>();
                            wd.Add(el.n);
                            wd.Add(el.edges[el.e].u);
                            wd.Add(el.edges[el.e].v);
                            wd.Sort();
                            wd.Reverse();
                            el.n = wd[0];

                            if (el.e++ == s)
                            {
                                s += NLINKS;
                                Array.Resize(ref el.edges, s);
                            }
                        }
                    }
                }

                el.n++;
                Array.Resize(ref el.edges, s);
                return el;
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return el;
            }
        }

        //read weighted edgelist from the file named input
        public static Helper.termList[] readTermFile(string input)
        {
            int s = NLINKS;
            termList[] term = new termList[s];

            string l = "";

            try
            {               

                using (StreamReader sr = new StreamReader(input))
                {
                    int i = 0;
                    while ((l = sr.ReadLine()) != null)
                    {
                        var lineParts = l.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (lineParts.Length > 0)
                        {

                            term[i].termdesc = lineParts[0];
                            term[i].termid = int.Parse(lineParts[1]);
                            termID.Add(int.Parse(lineParts[1]), lineParts[0]);

                        }
                        i++;
                    }
                }
                return term;
            }
            

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return term;
            }
            
            
        }

        //get folder list to be processed
        public static Helper.folderList[] readfolderList(string input)
        {
           
            folderList[] folder = new folderList[10];

            string l = "";

            try
            {
                using (StreamReader sr = new StreamReader(input))
                {
                    int i = 0;
                    while ((l = sr.ReadLine()) != null)
                    {                       
                        folder[i].folderName = l;      
                        i++;
                    }
                }
                return folder;
            }


            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return folder;
            }


        }

        //read Config file
        public static Helper.configList[] readConfigDetails(string input)
        {

            configList[] config= new configList[10];

            string l = "";

            try
            {
                using (StreamReader sr = new StreamReader(input))
                {
                    int i = 0;
                    while ((l = sr.ReadLine()) != null)
                    {
                        var lineParts = l.Split(new char[] { '='}, StringSplitOptions.RemoveEmptyEntries);

                        if (lineParts.Length > 0)
                        {

                            config[i].configKey = lineParts[0];
                            config[i].configValue =lineParts[1].Trim(Path.GetInvalidPathChars());
                            
                            configDetail.Add(lineParts[0], lineParts[1].Trim(Path.GetInvalidPathChars()));
                        }
                        i++;
                    }
                }
                return config;
            }


            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return config;
            }


        }

        //prepare weighted vertex list by sum of weights in descending order
        public static vertexDeg[] prepareWeightedVertexList(edgelist el)
        {

            uint i;
            uint j;
            vertexDeg[] nd = new vertexDeg[el.n];   //weighted vertex list storing start vertices for DFS
        
            try
            { 
                //Ordering nodes in non-increasing order of degree
                for (i = 0; i < el.n; i++)
                {
                    nd[i] = new vertexDeg();
                    nd[i].n = i;
                    nd[i].w = 0;
                }

                //calculate sum of weights for connected nodes
                for (i = 0; i < el.e; i++)
                {
                    nd[el.edges[i].u].w = nd[el.edges[i].u].w + el.edges[i].w;
                    nd[el.edges[i].v].w = nd[el.edges[i].v].w + el.edges[i].w;
                }

                //sort by descending order
                Array.Sort(nd, Helper.compare_vertexDeg);

                return nd;
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return nd;
            }

        }

        //prepare Vertex List
        public static void prepareVertexList(edgelist el, List<int> vertices)
        {

            try
            {

                int i = 0;
                for (i = 0; i < el.e; i++)
                {
                    bool Contains = vertices.Contains((int)el.edges[i].u);
                    if (!Contains)
                    {
                        vertices.Add((int)el.edges[i].u);
                    }
                    Contains = vertices.Contains((int)el.edges[i].v);
                    if (!Contains)
                    {
                        vertices.Add((int)el.edges[i].v);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

    }

}

