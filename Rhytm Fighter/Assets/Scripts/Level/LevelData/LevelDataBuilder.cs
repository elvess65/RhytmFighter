using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level.Data
{
    public class LevelDataBuilder 
    {
        //TEMP
        private Dictionary<int, LevelNodeData> m_Nodes;


        public LevelNodeData Build(int levelDepth, int levelSeed, bool generateOnlyMainPath)
        {
            Debug.Log("LevelDataBuilder : Build level data. Level Depth: " + levelDepth + " Level seed: " + levelSeed);

            Random.InitState(levelSeed);

            int curDepthLevel = 0;
            int nodeIDCounter = 1;
            LevelNodeData startNode = null;
            LevelNodeData lastCreatedNode = null;
            LevelNodeData finishNode = null;

            //TEMP
            m_Nodes = new Dictionary<int, LevelNodeData>();

            //Создать базовую часть уровня
            while (curDepthLevel != levelDepth)
            {
                //Создать нод
                LevelNodeData node = CreateNode(nodeIDCounter++, nodeIDCounter * 120);

                //Запомнить начальный нод
                if (startNode == null)
                    startNode = node;

                //Если создан не первый нод - связать новый нод с предыдущем случайным образом
                if (lastCreatedNode != null)
                    lastCreatedNode.TryAddNodeRandomly(node);

                //Запомнить предыдущий нод
                lastCreatedNode = node;

                //Запомнить текущий нод как последний 
                finishNode = node;

                //TEMP
                m_Nodes.Add(node.ID, node);


                //Увеличить счетчик глубины
                curDepthLevel++;
            }

            if (!generateOnlyMainPath)
            {
                //TEMP Добавить дополнительные ноды
                LevelNodeData additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[1].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[3].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[5].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[7].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[4].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                //CHECK
                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[4].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);

                additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
                m_Nodes[5].AddAdditionalNode(additionalNode);
                m_Nodes.Add(additionalNode.ID, additionalNode);
                ///END TEMP
            }

            //Вывести в лог созданные ноды
            startNode.PrintNodeDataRecursively();

            return startNode;
        }


        LevelNodeData CreateNode(int id, int nodeSeed)
        {
            LevelNodeData node = new LevelNodeData(id, nodeSeed);
            return node;
        }
    }
}
