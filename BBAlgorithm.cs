using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HeaviestSubgraphConnected.Program;
using static HeaviestSubgraphConnected.Helper;

namespace HeaviestSubgraphConnected
{
    public class BBAlgorithm
    {
        public static void depthFirstSearch(int startVertex, double ub, Graph<int> graph, int k, Helper.edgelist el, vertexDeg[] vdList, List<Tuple<int, int, int>> edges, bool visited, int i, subGraph[] subGraphList)
        {
            try
            {

                int iKey;
                int iVal;
                var iWeight = new List<Tuple<int, int, int>> { };
                int index = 0;
                int sWeight = 0;         //subgraph weight induced so far
                bool exit = true;


                while (exit == true)
                {
                    //get the value for visited                
                    visited = vdList[i].visited;

                    //check for adjacent vertices and select the vertex with max weight
                    if (graph.AdjacencyList.ContainsKey(startVertex) && visited != true)
                    {
                        int j = 0;
                        //set the vertex as visited

                        iWeight.Clear();

                        //check every neighbor and add their weights to a list to choose max weight later
                        foreach (var neighbor in graph.AdjacencyList[startVertex])
                        {
                            iKey = startVertex;
                            iVal = neighbor;
                            foreach (var edge in edges)
                            {
                                //add the weights to a list 
                                if (edge.Item1.Equals(iKey) && edge.Item2.Equals(iVal) || edge.Item2.Equals(iKey) && edge.Item1.Equals(iVal))
                                {
                                    iWeight.Add(new Tuple<int, int, int>(edge.Item1, edge.Item2, edge.Item3));
                                }

                            }

                        }

                        //sort weights of connected vertices in descending order
                        iWeight.Sort((pair1, pair2) => pair2.Item3.CompareTo(pair1.Item3));

                        int a = 0;
                        int b = 0;
                        int result = 0;

                        bool exists = false;

                        //add an edge which doesn't exists in the subgraph yet 
                        foreach (var w in iWeight)
                        {
                            result = Helper.FindEdgeinSubGraph(subGraphList[i].subgraphVertices, w.Item1, w.Item2);
                            if (result >= 0) //edge already exists in the list
                            {
                                exists = true;
                            }
                            else
                            {
                                exists = false;
                                a = w.Item1;
                                b = w.Item2;
                                break;
                            }

                        }

                        //find an add the edge from the edge list
                        index = Helper.FindAnEdgeinEdgeList(el, a, b);

                        var ListOfAllVertices = new List<int>();

                        foreach (var itemX in subGraphList)
                        {
                            if (!ListOfAllVertices.Contains(itemX.n))
                            {
                                ListOfAllVertices.Add(itemX.n);
                            }
                        }


                        //execute the below only if upperbound is greater than best value
                        if (ub > subGraphList[i].weight && ListOfAllVertices.Distinct().ToList().Count <=k)
                        {

                            //induce this vertex into subgraph
                            if ((int)el.edges[index].u != startVertex)
                            {
                                subGraphList[i].subgraphVertices.Add(Tuple.Create((int)el.edges[index].v, (int)el.edges[index].u, (int)el.edges[index].w));
                                startVertex = (int)el.edges[index].u;
                            }
                            else
                            {
                                subGraphList[i].subgraphVertices.Add(Tuple.Create((int)el.edges[index].u, (int)el.edges[index].v, (int)el.edges[index].w));
                                startVertex = (int)el.edges[index].v;
                            }

                            sWeight = 0;
                            //update the weight of the subgraph induced so far
                            foreach (var w in subGraphList[i].subgraphVertices)
                            {
                                sWeight += (Convert.ToInt32(w.Item3));
                            }

                            subGraphList[i].weight = sWeight;

                            //compute new upper bound
                            int s = subGraphList[i].subgraphVertices.Count * (subGraphList[i].subgraphVertices.Count - 1) / 2;
                            ub = sWeight + (k * (k - 1)) / 2 - s;

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        exit = false;
                        break;
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
