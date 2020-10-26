using RhytmFighter.Data;
using RhytmFighter.Level.Scheme.View;
using RhytmFighter.Data.DataBase.Simulation;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
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
        private int m_LevelID = 1;
        private int m_LevelSeed = 10;
        private bool m_OnlyMainPath = false;

        private SchemeNodeView m_SelectedNode;
        private SchemeCellView m_SelectedCell;
        private LevelController m_LevelController;
        private InfoData m_InfoData;

        [MenuItem("Level/Scheme")]
        public static void ShowWindow() => GetWindow<LevelSchemeBuilderEditorWindow>(false, "Level Scheme", true).Initialize();


        public void Initialize()
        {
            Dispose();

            m_LevelController = new LevelController();
            m_InfoData = new InfoData(JsonUtility.ToJson(GameObject.FindObjectOfType<DBSimulation>().LevelsData));
        }


        void HandleDefaultState()
        {
            #region Level Generation Settings
            m_LevelID = EditorGUILayout.IntField("Level ID:", m_LevelID);
            m_OnlyMainPath = EditorGUILayout.Toggle("Generate only main path", m_OnlyMainPath);
            #endregion

            #region Create Level Button
            EditorGUILayout.Space();

            if (GUILayout.Button("Create level"))
                ButtonCreateLevel();
            #endregion
        }

        void HandleOberveLevelState()
        {
            #region Button Back
            if (GUILayout.Button("Back"))
            {
                ButtonBack_FromObserveLevel();
                return;
            }
            #endregion

            #region Node Data
            EditorGUILayout.Space();

            if (m_SelectedNode != null)
            {
                //Title
                GUILayout.Label($"Node Data: {(m_SelectedNode.NodeData.IsFinishNode ? "Is finish node" : string.Empty)}", EditorStyles.boldLabel);

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
                GUILayout.BeginHorizontal();
                    GUILayout.Label("LeftNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.LeftNode != null ? m_SelectedNode.NodeData.LeftNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                    GUILayout.Label("LeftInputNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.LeftInputNode != null ? m_SelectedNode.NodeData.LeftInputNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Right Node
                GUILayout.BeginHorizontal();
                    GUILayout.Label("RightNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.RightNode != null ? m_SelectedNode.NodeData.RightNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                    GUILayout.Label("RightInputNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.RightInputNode != null ? m_SelectedNode.NodeData.RightInputNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Parent Node
                GUILayout.BeginHorizontal();
                    GUILayout.Label("ParentNodeID:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedNode.NodeData.ParentNode != null ? m_SelectedNode.NodeData.ParentNode.ID.ToString() : "None");
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }
            #endregion

            #region Button Observe node
            if (GUILayout.Button("Observe node"))
                ButtonObserveNode();
            #endregion
        }

        void HandleObserveNodeState()
        {
            #region Button Back
            if (GUILayout.Button("Back"))
            {
                ButtonBack_FromObserveNode();
                return;
            }
            #endregion

            #region Cell Data
            if (m_SelectedNode != null && m_SelectedCell != null)
            {
                //Node
                GUILayout.Label("Node:", EditorStyles.boldLabel);
                GUILayout.Label($"ID: {m_SelectedNode.NodeData.ID}");
                GUILayout.Label($"Seed: {m_SelectedNode.NodeData.NodeSeed}");


                //Cell
                GUILayout.Label("Cell:", EditorStyles.boldLabel);

                // - Coord
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Coord:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label($"{m_SelectedCell.Cell.X} : {m_SelectedCell.Cell.Y}");
                GUILayout.EndHorizontal();

                // - Type
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Type:", GUILayout.MinWidth(m_MIN_LABEL_WIDTH));
                    GUILayout.Label(m_SelectedCell.Cell.CellType.ToString());
                GUILayout.EndHorizontal();
            }
            #endregion

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


        void ButtonCreateLevel()
        {
            LevelsData.LevelParams levelParams = m_InfoData.LevelsData.GetLevelParams(m_LevelID);

            m_LevelController.GenerateLevel(levelParams, m_OnlyMainPath, false, 0);
            m_LevelController.LevelSchemeBuilder.Build(m_LevelController.Model.StartNodeData);

            if (m_LevelController.LevelSchemeBuilder.HasData)
            {
                SelectNode(m_LevelController.LevelSchemeBuilder[m_LevelController.Model.StartNodeData.ID]);
                FocusAt(m_SelectedNode.gameObject);

                Selection.selectionChanged += NodeSelectionChanged;

                m_State = WindowStates.OberveLevel;
            }
        }

        void ButtonObserveNode()
        {
            m_LevelController.BuildRoomData(m_SelectedNode.NodeData, true, true);
            m_LevelController.RoomSchemeBuilder.Build(m_LevelController.Model.GetCurrenRoomData());

            if (m_LevelController.RoomSchemeBuilder.HasData)
            {
                Selection.selectionChanged -= NodeSelectionChanged;

                SelectCell(m_LevelController.RoomSchemeBuilder[0, 0]);
                FocusAt(m_SelectedCell.gameObject);

                Selection.selectionChanged += CellSelectionCHanged;

                m_State = WindowStates.ObserveNode;
            }
        }

        void ButtonBack_FromObserveLevel()
        {
            Dispose();
            m_State = WindowStates.Default;
        }

        void ButtonBack_FromObserveNode()
        {
            DisposeRoom();
            SelectNode(m_SelectedNode);
            FocusAt(m_SelectedNode.gameObject);

            Selection.selectionChanged += NodeSelectionChanged;

            m_State = WindowStates.OberveLevel;
        }


        void NodeSelectionChanged()
        {
            if (Selection.activeObject != null)
            {
                SchemeNodeView roomScheme = (Selection.activeObject as GameObject).GetComponent<SchemeNodeView>();
                if (roomScheme != null)
                    SelectNode(roomScheme);
            }
        }

        void SelectNode(SchemeNodeView node)
        {
            m_SelectedNode = node;

            m_LevelController.LevelSchemeBuilder.ShowAllAsNormal();
            m_SelectedNode.ShowAsCurrent();

            if (m_SelectedNode.NodeData.LeftNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.LeftNode.ID].ShowAsLinkedNode(true);

            if (m_SelectedNode.NodeData.RightNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.RightNode.ID].ShowAsLinkedNode(false);

            if (m_SelectedNode.NodeData.ParentNode != null)
                m_LevelController.LevelSchemeBuilder[m_SelectedNode.NodeData.ParentNode.ID].ShowAsParentNode();

            Repaint();
        }
        

        void CellSelectionCHanged()
        {
            if (Selection.activeObject != null)
            {
                SchemeCellView cellScheme = (Selection.activeObject as GameObject).GetComponent<SchemeCellView>();
                if (cellScheme != null)
                    SelectCell(cellScheme);
            }
        }

        void SelectCell(SchemeCellView cell)
        {
            m_LevelController.RoomSchemeBuilder.ShowAllAsNormal();

            m_SelectedCell = cell;
            m_SelectedCell.ShowAsCurrent();

            Repaint();
        }


        void FocusAt(GameObject ob)
        {
            Selection.activeGameObject = ob.gameObject;
            SceneView.FrameLastActiveSceneView();
        }


        void Dispose()
        {
            DisposeLevel();
            DisposeRoom();
        }

        void DisposeLevel()
        {
            Selection.selectionChanged -= NodeSelectionChanged;

            m_SelectedNode = null;

            if (m_LevelController != null)
                m_LevelController.LevelSchemeBuilder.Dispose();
        }

        void DisposeRoom()
        {
            Selection.selectionChanged -= CellSelectionCHanged;

            m_SelectedCell = null;

            if (m_LevelController != null)
            {
                m_LevelController.RoomSchemeBuilder.Dispose();
                m_LevelController.Model.RemoveRoom(m_LevelController.Model.CurrentRoomID);
            }
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

#endif