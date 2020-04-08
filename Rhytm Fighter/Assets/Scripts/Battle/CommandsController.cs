using RhytmFighter.Assets;
using RhytmFighter.Battle.Command;
using RhytmFighter.Battle.Command.View;
using RhytmFighter.Interfaces;
using RhytmFighter.Main;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class CommandsController : iUpdatable
    {
        private static CommandsController m_Instance;

        private List<PendingCommand> m_PendingCommands;


        public static void AddCommand(BattleCommand command)
        {
            m_Instance.m_PendingCommands.Add(new PendingCommand(command, Rhytm.RhytmController.GetInstance().CurrentTick));
        }

        public CommandsController()
        {
            m_Instance = this;
            m_PendingCommands = new List<PendingCommand>();
        }

        public void ProcessPendingCommands(int ticksSinceStart)
        {
            for(int i = 0; i < m_PendingCommands.Count; i++)
            {
                if (m_PendingCommands[i].CommandShouldBeReleased(ticksSinceStart))
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
            private int m_CreationTick;
            private int m_TargetTick;

            public BattleCommand Command { get; private set; }


            public PendingCommand(BattleCommand command, int creationTick)
            {
                //Initialize data
                Command = command;
                m_CreationTick = creationTick;
                m_TargetTick = m_CreationTick + command.ApplyDelay;
                double viewLifeTime = command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds +
                                                           Rhytm.RhytmController.GetInstance().DeltaInput;

                //Initialize view
                View = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
                View.Initialize(command.Sender.ProjectileSpawnPosition,
                                command.Target.ProjectileHitPosition,
                                (float)viewLifeTime);
            }

            public bool CommandShouldBeReleased(int ticksSinceStart) => m_TargetTick == ticksSinceStart;

            public void PerformUpdate(float deltaTime) => View?.PerformUpdate(deltaTime);

            public void Dispose() => View.Dispose();
        }
    }
}
