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
            if (m_PendingCommands.Count > 0)
                Debug.Log("Process command " + ticksSinceStart);

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
            private int m_CreationTick;

            public BattleCommand Command { get; private set; }


            public PendingCommand(BattleCommand command, int creationTick)
            {
                Command = command;
                m_CreationTick = creationTick;
                float viewLifeTime = command.ApplyDelay * (float)Rhytm.RhytmController.GetInstance().TickDurationSeconds;

                View = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
                View.Initialize(command.Sender.ProjectileSpawnPosition,
                                command.Target.ProjectileHitPosition,
                                viewLifeTime);

                //TODO: LeftTime = R.TimeToNextTick + ((ApplyDelay - 1) * TickDuration)
                //Target tick = R.TicksSinceStart + command.applyDelay

                Debug.Log("Command created. Tick " + m_CreationTick + " View life time: " + viewLifeTime + " Time to next tick: " +
                    Rhytm.RhytmController.GetInstance().TimeToNextTick);
            }

            public bool Process()
            {
                //TODO: If ticks since start == relaseTick
                return false;
            }

            public void PerformUpdate(float deltaTime) => View?.PerformUpdate(deltaTime);

            public void Dispose() => View.Dispose();
        }
    }
}
