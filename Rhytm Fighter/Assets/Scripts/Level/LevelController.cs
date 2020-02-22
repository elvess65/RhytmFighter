using UnityEngine;
using RhytmFighter.Level.Data;

namespace RhytmFighter.Level
{
    public class LevelController 
    {
        private LevelDataBuilder m_LevelDataBuilder;
        //room builder
        //room visual builder
        //stateModel

        public LevelController()
        {
            m_LevelDataBuilder = new LevelDataBuilder();
        }

        public void GenerateLevel(int levelDepth)
        {
            Debug.Log("LevelController: Generate level");
            m_LevelDataBuilder.Build(levelDepth);

            //TODO:
            //Generate room
            //Save to current state
        }

        //TODO:
        //Create visual for current state
    }
}
