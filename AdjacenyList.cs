using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaviestSubgraphConnected
{
    class AdjacencyList
    {

        Dictionary<int, List<Tuple<int, int>>> adjacencyList;

        // Constructor - creates an empty Adjacency List
        public AdjacencyList(int vertices)
        {
            adjacencyList = new Dictionary<int, List<Tuple<int, int>>>();
        }

        // Appends a new Edge to the linked list
        public void addEdge(int startVertex, int endVertex, int weight)
        {
            if (!adjacencyList.ContainsKey(startVertex))
            {
                adjacencyList[startVertex] = new List<Tuple<int, int>>();
            }

            adjacencyList[startVertex].Add(new Tuple<int, int>(endVertex, weight));
            adjacencyList[startVertex].Sort((pair1, pair2) => pair2.Item2.CompareTo(pair1.Item2));
        }
        

        // Removes the first occurence of an edge and returns true
        // if there was any change in the collection, else false
        public bool removeEdge(int startVertex, int endVertex, int weight)
        {
            Tuple<int, int> edge = new Tuple<int, int>(endVertex, weight);

            return adjacencyList[startVertex].Remove(edge);
        }

        
    }
}
