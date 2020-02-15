using UnityEngine;

namespace RhytmFighter.Level
{
    public class LevelCreator 
    {
        public void CreateLevel(int levelDepth)
        {
            Debug.Log("Create level with depth " + levelDepth);

            Random.InitState(10);

            int curDepthLevel = 0;
            int nodeIDCounter = 1;
            LevelNode startNode = null;
            LevelNode lastCreatedNode = null;

            //Создать базовую часть уровня
            while(curDepthLevel != levelDepth)
            {
                //Создать нод
                LevelNode node = CreateNode(nodeIDCounter++, nodeIDCounter * 120);

                //Запомнить начальный нод
                if (startNode == null)
                    startNode = node;

                //Если создан не первый нод - связать новый нод с предыдущем случайным образом
                if (lastCreatedNode != null)
                {
                    node.ParentNode = lastCreatedNode;

                    bool takeRightNode = Random.Range(0, 100) > 50;
                    if (takeRightNode)
                        lastCreatedNode.RightNode = node;
                    else
                        lastCreatedNode.LeftNode = node;
                }

                //Запомнить предыдущий нод
                lastCreatedNode = node;

                //Увеличить счетчик глубины
                curDepthLevel++;
            }

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

//   4
//     3
//   2
// 1
//Seed = 10

// 4
//   3
//     2
//   1
//Seed = 101
