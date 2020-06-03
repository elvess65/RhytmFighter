using RhytmFighter.Level.Data;
using RhytmFighter.Level.Scheme.View;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Level.Scheme.Builder
{
    /// <summary>
    /// Build scheme of the level
    /// </summary>
    public class LevelSchemeBuilder : AbstractSchemeBuilder
    {
        private Dictionary<int, SchemeNodeView> m_RoomSchemes;

        new private Vector3 m_INIT_POSITION = new Vector3(100, 100, 100);
        private Vector3 m_LEFT_NODE_OFFSET = new Vector3(-1, 0, 1);
        private Vector3 m_RIGHT_NODE_OFFSET = new Vector3(1, 0, 1);
        private Vector3 m_INPUT_NODE_OFFSET = new Vector3(0, 0.2f, 0);
        private Color m_NODE_CONNECTION_COLOR = Color.green;
        private Color m_INPUT_NODE_CONNECTION_COLOR = Color.yellow;
        private const float m_SCHEME_SCALE_MULTIPLAYER = 0.5f;


        public SchemeNodeView this[int id] => m_RoomSchemes[id];

        public override bool HasData => m_RoomSchemes != null && m_RoomSchemes.Count > 0;


        public LevelSchemeBuilder() : base()
        {
            m_RoomSchemes = new Dictionary<int, SchemeNodeView>();
        }

        public void Build(LevelNodeData startNodeData)
        {
            startNodeData.ForEachNodeRecursively(CreateSchemeForNode);

            DrawConnections();
        }

        public override void ShowAllAsNormal()
        {
            if (!HasData)
                return;

            foreach (SchemeNodeView nodeScheme in m_RoomSchemes.Values)
            {
                if (nodeScheme != null)
                    nodeScheme.ShowAsNormal();
            }
        }

        public override void Dispose()
        {
            //Clear rooms
            if (HasData)
            {
                foreach (SchemeNodeView nodeScheme in m_RoomSchemes.Values)
                {
                    if (nodeScheme != null)
                        Object.DestroyImmediate(nodeScheme.gameObject);
                }

                m_RoomSchemes.Clear();
            }

            //Clear other schemeNodeViews
            SchemeNodeView[] nodesInScene = Object.FindObjectsOfType<SchemeNodeView>();
            if (nodesInScene.Length > 0)
            {
                foreach (SchemeNodeView nodeScheme in nodesInScene)
                {
                    if (nodeScheme != null)
                        Object.DestroyImmediate(nodeScheme.gameObject);
                }
            }
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
                PrimitiveType type = PrimitiveType.Sphere;
                if (nodeData.IsStartNode)
                    type = PrimitiveType.Cube;
                else if (nodeData.IsFinishNode)
                    type = PrimitiveType.Capsule;

                SchemeNodeView schemeNode = CreateRoomSchemeView(schemePos, type);
                schemeNode.Initialize(nodeData);

                m_RoomSchemes.Add(nodeData.ID, schemeNode);
            }
        }

        void DrawConnections()
        {
            foreach (SchemeNodeView schemeNode in m_RoomSchemes.Values)
            {
                //Создать отображение связей если есть соответсвующие ноды
                if (schemeNode.NodeData.LeftNode != null)
                    schemeNode.AddConnectionRenderer(m_RoomSchemes[schemeNode.NodeData.LeftNode.ID].transform.position, m_NODE_CONNECTION_COLOR, Vector3.zero);

                if (schemeNode.NodeData.RightNode != null)
                    schemeNode.AddConnectionRenderer(m_RoomSchemes[schemeNode.NodeData.RightNode.ID].transform.position, m_NODE_CONNECTION_COLOR, Vector3.zero);

                if (schemeNode.NodeData.LeftInputNode != null)
                    schemeNode.AddConnectionRenderer(m_RoomSchemes[schemeNode.NodeData.LeftInputNode.ID].transform.position, m_INPUT_NODE_CONNECTION_COLOR, m_INPUT_NODE_OFFSET);

                if (schemeNode.NodeData.RightInputNode != null)
                    schemeNode.AddConnectionRenderer(m_RoomSchemes[schemeNode.NodeData.RightInputNode.ID].transform.position, m_INPUT_NODE_CONNECTION_COLOR, m_INPUT_NODE_OFFSET);
            }
        }

        SchemeNodeView CreateRoomSchemeView(Vector3 pos, PrimitiveType type)
        {
            GameObject ob = GameObject.CreatePrimitive(type);
            ob.transform.position = pos;
            ob.transform.localScale *= m_SCHEME_SCALE_MULTIPLAYER;

            SchemeNodeView schemeNode = ob.AddComponent<SchemeNodeView>();
            return schemeNode;
        }
    }
}
