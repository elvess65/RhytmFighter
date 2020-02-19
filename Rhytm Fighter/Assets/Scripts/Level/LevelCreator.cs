using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level
{
    public class LevelDataBuilder 
    {
        //TEMP
        private Dictionary<int, LevelNode> m_Nodes = new Dictionary<int, LevelNode>();

        public void Build(int levelDepth)
        {
            Debug.Log("Create level with depth " + levelDepth);

            Random.InitState(10);

            int curDepthLevel = 0;
            int nodeIDCounter = 1;
            LevelNode startNode = null;
            LevelNode lastCreatedNode = null;
            LevelNode finishNode = null;

            //Создать базовую часть уровня
            while (curDepthLevel != levelDepth)
            {
                //Создать нод
                LevelNode node = CreateNode(nodeIDCounter++, nodeIDCounter * 120);

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

            //TEMP Добавить дополнительные ноды
            LevelNode additionalNode = CreateNode(nodeIDCounter++, nodeIDCounter * 120);
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

            //Вывести в лог созданные ноды
            startNode.PrintNodeDataRecursively();
        }

        LevelNode CreateNode(int id, int nodeSeed)
        {
            LevelNode node = new LevelNode(id, nodeSeed);
            return node;
        }
    }
}
