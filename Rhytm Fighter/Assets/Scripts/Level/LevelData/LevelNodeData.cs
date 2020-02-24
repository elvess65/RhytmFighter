using System.Text;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    public class LevelNodeData
    {
        public enum AddNoteResult { None, AddedToLeft, AddedToRight }

        public int ID { get; private set; }
        public int NodeSeed { get; private set; }

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

        void SetRightNode(LevelNodeData node)
        {
            RightNode = node;
            node.LeftInputNode = this;
        }

        void SetLeftNode(LevelNodeData node)
        {
            LeftNode = node;
            node.RightInputNode = node;
        }

        public AddNoteResult TryAddNodeRandomly(LevelNodeData node, bool debug = false)
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
            AddNoteResult result = TryAddNodeRandomly(node);
            Debug.LogError("add to " + ID + " node " + node.ID + " " + result);

            //Check
            if (result == AddNoteResult.AddedToLeft)
            {
                CheckLeftOutput_AddToLeftNode(node);
                CheckLeftInput_AddToLeftNode(node);
                CheckRightOutput_AddToLeftNode(node);
            }
            else if (result == AddNoteResult.AddedToRight)
            {
                CheckLeftOutput_AddToRightNode(node);
                CheckRightInput_AddToRightNode(node);
                CheckRightOutput_AddToRightNode(node);
            }
        }

        public void MergeNodeRecursively()
        {
            MergeNode();

            if (LeftNode != null)
                LeftNode.MergeNodeRecursively();

            if (RightNode != null)
                RightNode.MergeNodeRecursively();
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


        //AddToRightNode
        void CheckLeftOutput_AddToRightNode(LevelNodeData node)
        {
            if (LeftNode != null && LeftNode.RightNode != null)
            {
                Debug.LogError($"CREATE LEFT OUTPUT from {node.ID} to {LeftNode.RightNode.ID}");
                //node left = LeftNode.RightNode
                node.LeftNode = LeftNode.RightNode;
            }
        }

        void CheckRightInput_AddToRightNode(LevelNodeData node)
        {
            if (ParentNode != null && ParentNode.RightNode != null && ID != ParentNode.RightNode.ID)
            {
                Debug.LogError($"CREATE RIGHT INPUT from " + ParentNode.RightNode.ID + " to " + node.ID);
                // ParentNode.RightNode left = node
                ParentNode.RightNode.LeftNode = node;
            }
        }

        void CheckRightOutput_AddToRightNode(LevelNodeData node)
        {
            if (ParentNode != null && ParentNode.RightNode != null && ParentNode.RightNode.RightNode != null &&
                ParentNode.RightNode.RightNode.LeftNode != null &&
                node.ID != ParentNode.RightNode.RightNode.ID)
            {
                Debug.LogError($"CREATE RIGHT OUTPUT from {node.ID} to {ParentNode.RightNode.RightNode.LeftNode.ID}");
                //node right = ParentNode.RightNode.RightNode.LeftNode
                node.RightNode = ParentNode.RightNode.RightNode.LeftNode;
            }
        }


        //AddToLeftNode
        void CheckLeftOutput_AddToLeftNode(LevelNodeData node)
        {
            if (ParentNode != null && ParentNode.LeftNode != null && ParentNode.LeftNode.LeftNode != null && ParentNode.LeftNode.LeftNode.RightNode != null)
            {
                Debug.LogError($"CREATE LEFT OUTPUT from {node.ID} to {ParentNode.LeftNode.LeftNode.RightNode.ID}");
                //node right = ParentNode.LeftNode.LeftNode.RightNode
                node.LeftNode = ParentNode.LeftNode.LeftNode.RightNode;
            }
        }

        void CheckLeftInput_AddToLeftNode(LevelNodeData node)
        {
            if (ParentNode != null && ParentNode.LeftNode != null && ID != ParentNode.LeftNode.ID)
            {
                Debug.LogError($"CREATE LEFT INPUT from " + ParentNode.LeftNode.ID + " to " + node.ID);
                // ParentNode.LeftNode left = node
                ParentNode.LeftNode.RightNode = node;
            }
        }

        void CheckRightOutput_AddToLeftNode(LevelNodeData node)
        {
            if (RightNode != null && RightNode.LeftNode != null)
            {
                Debug.LogError($"CREATE RIGHT OUTPUT from {node.ID} to {RightNode.LeftNode.ID}");
                //node right = RightNode.LeftNode
                node.RightNode = RightNode.LeftNode;
            }
        }


        void MergeNode()
        {
            if (ParentNode == null)
                return;

            if (ParentNode.ParentNode == null)
                return;

            if (ParentNode.ParentNode.LeftNode != null)
            {
                if (ParentNode.ParentNode.LeftNode.RightNode == null)
                    ParentNode.ParentNode.LeftNode.RightNode = this;
            }

            if (ParentNode.ParentNode.RightNode != null)
            {
                if (ParentNode.ParentNode.RightNode.LeftNode == null)
                    ParentNode.ParentNode.RightNode.LeftNode = this;
            }
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
