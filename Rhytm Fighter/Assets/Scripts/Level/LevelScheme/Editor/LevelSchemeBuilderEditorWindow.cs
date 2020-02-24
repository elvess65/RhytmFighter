using UnityEditor;
using UnityEngine;

namespace RhytmFighter.Level.Scheme.Editor
{
    /// <summary>
    /// Editor window to observe level
    /// </summary>
    public class LevelSchemeBuilderEditorWindow : EditorWindow
    {
        private enum WindowStates { Default, OberveLevel, ObserveNode }
        private WindowStates m_State = WindowStates.Default;

        private const float m_MIN_LABEL_WIDTH = 120;

        //Default
        private bool m_UseManualData = true;
        private int m_LevelDepth = 4;
        private int m_LevelSeed = 10;
        private bool m_OnlyMainPath = true;

        private SchemeNodeView m_SelectedNode;
        private LevelController m_LevelController;

        [MenuItem("Level/Scheme")]
        public static void ShowWindow() => GetWindow<LevelSchemeBuilderEditorWindow>(false, "Level Scheme", true).Initialize();


        public void Initialize()
        {
            Dispose();

            m_LevelController = new LevelController();

            Selection.selectionChanged += NodeSelectionChanged;
        }


        void HandleDefaultState()
        {
            #region Level Generation Settings
            m_UseManualData = EditorGUILayout.BeginToggleGroup("Use Manual Settings", m_UseManualData);
            EditorGUILayout.EndToggleGroup();
            if (m_UseManualData)
            {
                m_LevelDepth = EditorGUILayout.IntField("Level Depth:", m_LevelDepth);
                m_LevelSeed = EditorGUILayout.IntField("Level Seed", m_LevelSeed);
                m_OnlyMainPath = EditorGUILayout.Toggle("Generate only main path", m_OnlyMainPath);
            }
            else
            {
                EditorGUILayout.TextField("Scriptable Object Path");
            }
            #endregion

            #region Create Level Button
            EditorGUILayout.Space();

            if (GUILayout.Button("Create level"))
            {
                int levelDepth = m_LevelDepth;
                int levelSeed = m_LevelSeed;

                m_LevelController.GenerateLevel(levelDepth, levelSeed, m_OnlyMainPath);
                m_LevelController.LevelSchemeBuilder.Build(m_LevelController.StartNode);

                if (m_LevelController.LevelSchemeBuilder.HasRooms)
                {
                    m_State = WindowStates.OberveLevel;
                    SelectNode(m_LevelController.LevelSchemeBuilder[m_LevelController.StartNode.ID]);

                    Selection.activeGameObject = m_SelectedNode.gameObject;
                    SceneView.FrameLastActiveSceneView();
                }
            }
            #endregion
        }

        void HandleOberveLevelState()
        {
            #region Button Back
            if (GUILayout.Button("Back"))
            {
                Dispose();
                m_State = WindowStates.Default;
                return;
            }
            #endregion

            #region Node Data
            EditorGUILayout.Space();

            if (m_SelectedNode != null)
            {
                //Title
                GUILayout.Label($"Node Data:", EditorStyles.boldLabel);

                //ID
                GUILayout.BeginHorizontal();
                    GUILayout.Label("ID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.ID.ToString());
                GUILayout.EndHorizontal();

                //Seed
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Seed:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.NodeSeed.ToString());
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Left Node
                GUI.contentColor = SchemeNodeView.LEFT_NODE_COLOR;
                GUILayout.BeginHorizontal();
                    GUILayout.Label("LeftNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.LeftNode != null ? m_SelectedNode.NodeData.LeftNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                GUI.contentColor = SchemeNodeView.LEFT_NODE_COLOR;
                GUILayout.BeginHorizontal();
                    GUILayout.Label("LeftInputNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.LeftInputNode != null ? m_SelectedNode.NodeData.LeftInputNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Right Node
                GUI.contentColor = SchemeNodeView.RIGHT_NODE_COLOR;
                GUILayout.BeginHorizontal();
                    GUILayout.Label("RightNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.RightNode != null ? m_SelectedNode.NodeData.RightNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                GUI.contentColor = SchemeNodeView.RIGHT_NODE_COLOR;
                GUILayout.BeginHorizontal();
                    GUILayout.Label("RightInputNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.RightInputNode != null ? m_SelectedNode.NodeData.RightInputNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Parent Node
                GUI.contentColor = SchemeNodeView.PARENT_NODE_COLOR;
                GUILayout.BeginHorizontal();
                    GUILayout.Label("ParentNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.ParentNode != null ? m_SelectedNode.NodeData.ParentNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                GUI.contentColor = Color.white;

                EditorGUILayout.Space();

                //Is Start Node
                GUILayout.Label("Is start node", EditorStyles.boldLabel);

                //Is Finish Node
                GUILayout.Label("Is finish node", EditorStyles.boldLabel);

                EditorGUILayout.Space();
            }
            #endregion

            #region Button Observe node
            if (GUILayout.Button("Observe node"))
                m_State = WindowStates.ObserveNode;
            #endregion
        }

        void HandleObserveNodeState()
        {
            if (GUILayout.Button("Back"))
                m_State = WindowStates.OberveLevel;

            GUILayout.Label("Observe node with ID: " + m_SelectedNode.NodeData.ID, EditorStyles.boldLabel);
        }

        void HandleAnyway()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Clear"))
            {
                Dispose();
                m_State = WindowStates.Default;
            }
        }


        void SelectNode(SchemeNodeView node)
        {
            m_SelectedNode = node;

            m_LevelController.LevelSchemeBuilder.ShowAllNodesAsNormal();
            m_SelectedNode.ShowAsCurrentNode();

            if (m_SelectedNode.NodeData.LeftNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.LeftNode.ID].ShowAsLinkedNode(true);

            if (m_SelectedNode.NodeData.RightNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.RightNode.ID].ShowAsLinkedNode(false);

            if (m_SelectedNode.NodeData.ParentNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.ParentNode.ID].ShowAsParentNode();

            Repaint();
        }

        void UnselectNode()
        {
            m_SelectedNode = null;
        }


        void NodeSelectionChanged()
        {
            if (Selection.activeObject != null)
            {
                SchemeNodeView roomScheme = (Selection.activeObject as GameObject).GetComponent<SchemeNodeView>();
                if (roomScheme != null)
                    SelectNode(roomScheme);
                else
                    UnselectNode();
            }
            else
                UnselectNode();
        }

        void Dispose()
        {
            Selection.selectionChanged -= NodeSelectionChanged;

            if (m_LevelController != null)
                m_LevelController.LevelSchemeBuilder.Dispose();
        }


        void OnGUI()
        {
            switch (m_State)
            {
                case WindowStates.Default:
                    HandleDefaultState();
                    break;
                case WindowStates.OberveLevel:
                    HandleOberveLevelState();
                    break;
                case WindowStates.ObserveNode:
                    HandleObserveNodeState();
                    break;
            }

            HandleAnyway();
        }

        void OnDestroy()
        {
            Dispose();
        }
    }
}

