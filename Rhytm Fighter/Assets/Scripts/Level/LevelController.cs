using UnityEngine;
using RhytmFighter.Level.Data;
using RhytmFighter.Level.Scheme;

namespace RhytmFighter.Level
{
    public class LevelController 
    {
        private LevelDataBuilder m_LevelDataBuilder;
        private LevelSchemeBuilder m_LevelSchemeBuilder;
        //room builder
        //room visual builder
        //stateModel
        public LevelNodeData StartNode;

        public LevelSchemeBuilder LevelSchemeBuilder
        {
            get
            {
                if (m_LevelSchemeBuilder == null)
                    m_LevelSchemeBuilder = new LevelSchemeBuilder();

                return m_LevelSchemeBuilder;
            }
        }


        public LevelController()
        {
            m_LevelDataBuilder = new LevelDataBuilder();
        }

        public void GenerateLevel(int levelDepth, int levelSeed)
        {
            Debug.Log("LevelController: Build level data");

            StartNode = m_LevelDataBuilder.Build(levelDepth, levelSeed);

            //TODO:
            //Generate room
            //Save to current state
        }
    }
}
