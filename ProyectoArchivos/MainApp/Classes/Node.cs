using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArchivos.MainApp.Classes
{
    class Node
    {
        private char NodeType; // 1 byte
        public char type { get { return NodeType; } set { NodeType = value; } }

        private long NodeAddress; // 8 byte
        public long nodeAddress { get { return NodeAddress; } set { NodeAddress = value; } }

        public struct NodesPT
        {
            public long P; // 8 bytes
            public int K; // 4 bytes
        }

        private NodesPT[] node; // si n = 4 .: son 48 bytes
        public NodesPT[] nodePK { get { return (node); } set { node = value; } }

        private long nexNode; // 8 bytes
        public long next { get { return (nexNode); } set { nexNode = value; } }                   //Total son 65 bytes

        public Node(char type, long dir)
        {
            this.type = type;
            nodeAddress = dir;

            node = new NodesPT[4];

            for (int i = 0; i < 4; i++)
            {
                node[i].P = -1;
                node[i].K = -1;
            }

            next = -1;
        }
    }
}
