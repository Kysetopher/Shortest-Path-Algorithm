using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains {
    class Graph {

        public Dictionary<char, Dictionary<char, int>> pathTrees;
        public Dictionary<char, Dictionary<char, int>> routeStops;
        public Dictionary<char, LinkedList<Edge> > edgeMap;


        public Graph(int nCount) {
            edgeMap = new Dictionary<char, LinkedList<Edge> >();
            pathTrees = new Dictionary<char, Dictionary<char, int>>();
            routeStops = new Dictionary<char, Dictionary<char, int>> ();
        }

        public override string ToString() {

            string ret = "Graph: ";
            foreach (KeyValuePair< char , LinkedList <Edge>> station in edgeMap) {
                ret += "\nStation:" + station.Key;
                foreach (Edge e in station.Value) 
                    ret += e.ToString();
            }
            return ret;
        }

//---------------------------------------------| PUBLIC METHODS |--------------------------------------------------------

        public void addEdge( char src,  char dest, int wieght) {                 //To be used by the Graphical User Interface to generate edges for the graph based on characters 
           Edge e = new Edge(dest, wieght-48);
           edgeMap[src].AddFirst(e);
        }

        public void genPathTree(char root) {

            Dictionary<char, int> cmplxty = new Dictionary<char, int>();
            Dictionary<char , int> dstnc = new Dictionary<char , int>();
            Dictionary<char , bool> actv = new Dictionary<char , bool>();

            foreach( KeyValuePair < char , LinkedList<Edge> > pair in edgeMap) { 
                dstnc[pair.Key] = int.MaxValue;
                actv[pair.Key] = false;
                cmplxty[pair.Key] = int.MaxValue;
            }

            dstnc[root] = 0;

            for(int iteration = 0 ; iteration < edgeMap.Count - 1 ; iteration ++) {
                char src = nextShortestNode(dstnc, actv);

                if (src != ' '){
                    actv[src] = true;

                    foreach (KeyValuePair<char, LinkedList<Edge>> pair in edgeMap) 
                        if( !actv[pair.Key] && getEdge(src, pair.Key) != -1 && dstnc[src] + getEdge(src, pair.Key) < dstnc[pair.Key]) {
                            dstnc[pair.Key] = dstnc[src] + getEdge(src, pair.Key);
                            cmplxty[pair.Key] = cmplxty[src] + 1;
                        }

                }
            }

            foreach (KeyValuePair<char, LinkedList<Edge>> pair in edgeMap) {

                if (cmplxty[pair.Key] == int.MaxValue) cmplxty[pair.Key] = -1;

                if ( dstnc[pair.Key] == int.MaxValue) dstnc[pair.Key] = -1;
            }

            cmplxty[root] = -1;
            dstnc[root] = -1;

            routeStops[root] = cmplxty;
            pathTrees[root] = dstnc;
        }

        public int getEdge(char src, char dst) {
            if (edgeMap.ContainsKey(src)) foreach (Edge e in edgeMap[src]) {
                    if (e.getDst() == dst)
                        return e.getWght();
                }
            return -1;
        }

        public int RoutesByMaxStn(char src, char dest, int length) {            //to be used when there is a max stop number
            int count = 0;
            foreach( Edge e in edgeMap[src]){
                if (e.getDst() == dest) count++;
                if (length > 1) count += RoutesByMaxStn(e.getDst(), dest, length - 1);
            }
            return count;
        }

        public int RoutesByNumStn(char src, char dest, int length) {            //to be used when there is an exact stop number
            int count = 0;
            foreach (Edge e in edgeMap[src])
            {
                if (e.getDst() == dest && length == 1 ) count++;
                if (length > 1) count += RoutesByNumStn(e.getDst(), dest, length - 1);
            }
            return count;
        }

        public int RoutesByMaxDist(char src, char dest, int distance) {            //to be used when there is a max distance
            int count = 0;

            foreach (Edge e in edgeMap[src])
            {
                if (e.getDst() == dest && distance > e.getWght()) count++;

                if (distance > e.getWght()) count += RoutesByMaxDist(e.getDst(), dest, distance - e.getWght());
            }
            return count;
        }


        //---------------------------------------------| PRIVATE METHODS |--------------------------------------------------------

        private char nextShortestNode( Dictionary<char, int> dstnc, Dictionary<char, bool> actv) {
            int min = int.MaxValue;
            char index = ' ';

            foreach (KeyValuePair<char, LinkedList<Edge>> pair in edgeMap)
                if (!actv[pair.Key] && dstnc[pair.Key] <= min) {
                    min = dstnc[pair.Key];
                    index = pair.Key;
                }

            return index;
        }

    }
}
