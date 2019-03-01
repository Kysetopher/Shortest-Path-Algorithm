using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains
{
   public class Edge
    {

        private int wght;
        private char dst;

        public Edge(char dstNode, int weight){
            dst = dstNode;
            wght = weight;
        }

        public override string ToString(){
            return "\n Weight: "+ wght +" Destination: " + dst;
        }

        public char getDst(){
            return dst;
        }

        public int getWght(){
            return wght;
        }


    }
}
