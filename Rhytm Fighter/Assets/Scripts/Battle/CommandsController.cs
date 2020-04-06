using RhytmFighter.Assets;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.View;
using RhytmFighter.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class CommandsController : iUpdatable
    {
        private static CommandsController m_Instance;

        private double m_IterationTime;
        private List<PendingCommand> m_PendingCommands;


        public static void AddCommand(BattleCommand command) => m_Instance.m_PendingCommands.Add(new PendingCommand(command, m_Instance.m_IterationTime));

        public CommandsController(double iterationTime)
        {
            m_Instance = this;

            m_IterationTime = iterationTime;
            m_PendingCommands = new List<PendingCommand>();
        }

        public void ProcessPendingCommands()
        {
            for(int i = 0; i < m_PendingCommands.Count; i++)
            {
                if (m_PendingCommands[i].Process())
                {
                    ReleaseCommand(m_PendingCommands[i].Command);

                    m_PendingCommands[i].Dispose();
                    m_PendingCommands.RemoveAt(i--);
                }
            }
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_PendingCommands.Count == 0)
                return;

            for (int i = 0; i < m_PendingCommands.Count; i++)
                m_PendingCommands[i].PerformUpdate(deltaTime);
        }


        private void ReleaseCommand(BattleCommand command)
        {
            command.Target.ApplyCommand(command);
        }

        

        class PendingCommand : iUpdatable
        {
            private AbstractCommandView View;
            private int m_IterationsToRelease;

            public BattleCommand Command { get; private set; }


            public PendingCommand(BattleCommand command, double iterationTime)
            {
                Command = command;

                View = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
                View.Initialize(command.Sender.ProjectileSpawnPosition,
                                command.Target.ProjectileHitPosition,
                                command.ApplyDelay * (float)iterationTime);

                m_IterationsToRelease = command.ApplyDelay;
                Debug.Log(command.ApplyDelay);
            }

            public bool Process() => m_IterationsToRelease-- <= 0;

            public void PerformUpdate(float deltaTime) => View?.PerformUpdate(deltaTime);

            public void Dispose() => View.Dispose();
        }
    }
}
