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

        private SchemeNodeView m_SelectedNode;
        private LevelController m_LevelController;

        [MenuItem("Level/Scheme")]
        public static void ShowWindow()
        {
            GetWindow<LevelSchemeBuilderEditorWindow>(false, "Level Scheme", true).Initialize();
        }


        public void Initialize()
        {
            Dispose();

            m_LevelController = new LevelController();

            Selection.selectionChanged += NodeSelectionChanged;
        }


        void HandleDefaultState()
        {
            if (GUILayout.Button("Create level"))
            {
                m_LevelController.GenerateLevel(4, 10);
                m_LevelController.LevelSchemeBuilder.Build(m_LevelController.StartNode);

                m_State = WindowStates.OberveLevel;

                if (m_LevelController.LevelSchemeBuilder.HasRooms)
                    SelectNode(m_LevelController.LevelSchemeBuilder[m_LevelController.StartNode.ID]);
            }
        }

        void HandleOberveLevelState()
        {
            if (GUILayout.Button("Back"))
            {
                Dispose();
                m_State = WindowStates.Default;
                return;
            }

            GUILayout.Label("Node Data", EditorStyles.boldLabel);
            GUI.enabled = m_SelectedNode != null;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Selected Node Index:", EditorStyles.boldLabel);
            GUILayout.Label(m_SelectedNode.NodeData.ID.ToString());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Selected Node Name:", EditorStyles.boldLabel);
            GUILayout.Label(m_SelectedNode.gameObject.name);
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            if (GUILayout.Button("Observe node"))
                m_State = WindowStates.ObserveNode;
        }

        void HandleObserveNodeState()
        {
            if (GUILayout.Button("Back"))
                m_State = WindowStates.OberveLevel;

            GUILayout.Label("Observe node with ID: " + m_SelectedNode.NodeData.ID, EditorStyles.boldLabel);
        }


        void SelectNode(SchemeNodeView node)
        {
            m_SelectedNode = node;

            Selection.activeGameObject = node.gameObject;
            SceneView.FrameLastActiveSceneView();

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
        }

        void OnDestroy()
        {
            Dispose();
        }
    }
}

