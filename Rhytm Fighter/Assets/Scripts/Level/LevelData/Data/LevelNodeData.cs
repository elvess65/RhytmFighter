using RhytmFighter.Persistant.Enums;
using System.Text;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    /// <summary>
    /// Node data
    /// </summary>
    public class LevelNodeData
    {
        public int ID { get; private set; }
        public int NodeSeed { get; private set; }
        public bool IsStartNode { get; set; }
        public bool IsFinishNode { get; set; }

        public LevelNodeData LeftNode;
        public LevelNodeData RightNode;
        public LevelNodeData ParentNode;
        public LevelNodeData LeftInputNode;
        public LevelNodeData RightInputNode;


        public LevelNodeData(int iD, int nodeSeed)
        {
            ID = iD;
            NodeSeed = nodeSeed;
            LeftNode = RightNode = ParentNode = LeftInputNode = RightInputNode = null;
        }

        public AddNoteResult TryAddNodeRandomly(LevelNodeData node)
        {
            AddNoteResult result = AddNoteResult.None;

            //Если RightNode != null && LeftNode != null не будет добавлено ни одного нода
            if (RightNode != null && LeftNode != null)
                return result;

            //Взять правый нод с вероятностью 50%. В противном случае взять левый
            bool takeRightNode = Random.Range(0, 100) > 50;
            if (takeRightNode)
            {
                //Правый нод свободен
                if (RightNode == null)
                {
                    SetRightNode(node);
                    result = AddNoteResult.AddedToRight;
                }
                //Правый нод занят - взять левый если он свободен
                else if (LeftNode == null)
                {
                    SetLeftNode(node);
                    result = AddNoteResult.AddedToLeft;
                }
            }
            else
            {
                //Левый нод свободен
                if (LeftNode == null)
                {
                    SetLeftNode(node);
                    result = AddNoteResult.AddedToLeft;
                }
                //Левый нод занят - взять правый если он свободен
                else if (RightNode == null)
                {
                    SetRightNode(node);
                    result = AddNoteResult.AddedToRight;
                }
            }

            if (result != AddNoteResult.None)
                node.ParentNode = this;

            return result;
        }

        public void AddAdditionalNode(LevelNodeData node)
        {
            //Add node
            TryAddNodeRandomly(node);

            //UpdateLeftInputNode
            if (node.RightInputNode != null && node.RightInputNode.LeftInputNode != null && node.RightInputNode.LeftInputNode.LeftNode != null)
                node.RightInputNode.LeftInputNode.LeftNode.SetRightNode(node);

            //UpdateLeftOutputNode
            if (node.LeftInputNode != null && node.LeftInputNode.RightInputNode != null && node.LeftInputNode.RightInputNode.RightNode != null)
                node.LeftInputNode.RightInputNode.RightNode.SetLeftNode(node);

            //UpdateRightInputNode
            if (node.LeftInputNode != null && node.LeftInputNode.LeftNode != null && node.LeftInputNode.LeftNode.RightNode != null)
                node.SetLeftNode(node.LeftInputNode.LeftNode.RightNode);

            //UpdateRightOutputNode
            if (node.RightInputNode != null && node.RightInputNode.RightNode != null && node.RightInputNode.RightNode.LeftNode != null)
                node.SetRightNode(node.RightInputNode.RightNode.LeftNode);
        }

        public void SetRightNode(LevelNodeData node)
        {
            RightNode = node;
            node.LeftInputNode = this;
        }

        public void SetLeftNode(LevelNodeData node)
        {
            LeftNode = node;
            node.RightInputNode = this;
        }


        public void PrintNodeDataRecursively()
        {
            PrintNodeData();

            if (LeftNode != null)
                LeftNode.PrintNodeDataRecursively();

            if (RightNode != null)
                RightNode.PrintNodeDataRecursively();
        }

        public void ForEachNodeRecursively(System.Action<LevelNodeData> action)
        {
            action?.Invoke(this);

            if (LeftNode != null)
                LeftNode.ForEachNodeRecursively(action);

            if (RightNode != null)
                RightNode.ForEachNodeRecursively(action);
        }


        void PrintNodeData()
        {
            StringBuilder b = new StringBuilder();
            b.Append($"ID: {ID}. Seed: {NodeSeed}. ");

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
