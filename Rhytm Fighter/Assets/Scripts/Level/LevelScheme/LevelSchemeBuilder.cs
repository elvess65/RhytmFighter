using RhytmFighter.Level.Data;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level.Scheme
{
    /// <summary>
    /// Build scheme of the level
    /// </summary>
    public class LevelSchemeBuilder
    {
        private Dictionary<int, SchemeNodeView> m_RoomSchemes;

        private Vector3 m_INIT_POSITION = new Vector3(100, 100, 100);
        private Vector3 m_LEFT_NODE_OFFSET = new Vector3(-1, 0, 1);
        private Vector3 m_RIGHT_NODE_OFFSET = new Vector3(1, 0, 1);


        public SchemeNodeView this[int id] => m_RoomSchemes[id];

        public bool HasRooms => m_RoomSchemes != null && m_RoomSchemes.Count > 0;


        public LevelSchemeBuilder()
        {
            Dispose();
            m_RoomSchemes = new Dictionary<int, SchemeNodeView>();
        }

        public void Build(LevelNodeData startNodeData)
        {
            startNodeData.ForEachNodeRecursively(CreateSchemeForNode);
        }

        public void Dispose()
        {
            if (!HasRooms)
                return;

            foreach(SchemeNodeView nodeScheme in m_RoomSchemes.Values)
            {
                if (nodeScheme != null)
                    Object.DestroyImmediate(nodeScheme.gameObject);
            }

            SchemeNodeView[] nodesInScene = Object.FindObjectsOfType<SchemeNodeView>();
            if (nodesInScene.Length > 0)
            {
                foreach (SchemeNodeView nodeScheme in nodesInScene)
                {
                    if (nodeScheme != null)
                        Object.DestroyImmediate(nodeScheme.gameObject);
                }
            }

            m_RoomSchemes.Clear();
        }


        void CreateSchemeForNode(LevelNodeData nodeData)
        {
            //Если еще не добавили нод
            if (!m_RoomSchemes.ContainsKey(nodeData.ID))
            {
                //Начальная позиция
                Vector3 schemePos = m_INIT_POSITION;

                //Если у нода есть родитель 
                if (nodeData.ParentNode != null)
                {
                    //Если родителя еще нет в списке добавленый - пропустить нод
                    if (!m_RoomSchemes.ContainsKey(nodeData.ParentNode.ID))
                        return;

                    //Начальная позиция - позиция родителя
                    schemePos = m_RoomSchemes[nodeData.ParentNode.ID].transform.position;

                    //Если нод который питаемся добавить - левый нод
                    if (nodeData.ParentNode.LeftNode != null && nodeData.ID == nodeData.ParentNode.LeftNode.ID)
                        schemePos += m_LEFT_NODE_OFFSET;

                    //Если нод который питаемся добавить - правый нод
                    if (nodeData.ParentNode.RightNode != null && nodeData.ID == nodeData.ParentNode.RightNode.ID)
                        schemePos += m_RIGHT_NODE_OFFSET;
                }

                //Создать схему
                SchemeNodeView schemeNode = CreateRoomScheme(schemePos);
                schemeNode.Initialize(nodeData);

                //Создать отображение связей если есть соответсвующие ноды
                if (nodeData.LeftNode != null)
                    schemeNode.AddConnectionRenderer(schemeNode.transform.position + m_LEFT_NODE_OFFSET);

                if (nodeData.RightNode != null)
                    schemeNode.AddConnectionRenderer(schemeNode.transform.position + m_RIGHT_NODE_OFFSET);

                m_RoomSchemes.Add(nodeData.ID, schemeNode);
            }
        }

        SchemeNodeView CreateRoomScheme(Vector3 pos)
        {
            GameObject ob = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ob.transform.position = pos;

            SchemeNodeView schemeNode = ob.AddComponent<SchemeNodeView>();
            return schemeNode;
        }
    }
}
