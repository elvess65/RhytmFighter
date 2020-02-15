using System.Text;
using UnityEngine;

namespace RhytmFighter.Level
{
    public class LevelNode
    {
        public int ID { get; private set; }
        public LevelNode LeftNode;
        public LevelNode RightNode;
        public LevelNode ParentNode;

        private int m_NodeSeed;


        public LevelNode(int iD, int nodeSeed)
        {
            ID = iD;
            m_NodeSeed = nodeSeed;
            LeftNode = RightNode = null;
        }

        public bool AddAdditionalNode(LevelNode node)
        {
            if (LeftNode == null)
            {
                return true;
            }

            if (RightNode == null)
            {
                return true;
            }

            if (LeftNode != null)
                LeftNode.AddAdditionalNode(node);

            if (RightNode != null)
                RightNode.AddAdditionalNode(node);

            return false;
        }

        public void PrintNodeDataRecursively()
        {
            PrintNodeData();

            if (LeftNode != null)
                LeftNode.PrintNodeDataRecursively();

            if (RightNode != null)
                RightNode.PrintNodeDataRecursively();
        }

        public void PrintNodeData()
        {
            StringBuilder b = new StringBuilder();
            b.Append($"ID: {ID}. Seed: {m_NodeSeed}. ");

            if (ParentNode != null)
                b.Append($"Parent ID {ParentNode.ID}. ");
            else
                b.Append("No parent. ");  

            if (LeftNode != null)
                b.Append($"Left ID: {LeftNode.ID}. ");

            if (RightNode != null)
                b.Append($"Right ID: {RightNode.ID}");

            Debug.Log(b.ToString());

        }
    }
}
