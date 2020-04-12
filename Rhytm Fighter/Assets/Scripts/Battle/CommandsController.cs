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

        private List<PEPendingCommand> m_PEPCommands;
        private List<SEPendingCommand> m_SEPCommands;


        public static void AddCommand(AbstractBattleCommand command)
        {
            switch(command.Layer)
            {
                case CommandExecutionLayers.PeriodicExecution:
                    m_Instance.m_PEPCommands.Add(new PEPendingCommand(command as AbstractPeriodicBattleCommand, Rhytm.RhytmController.GetInstance().CurrentTick));
                    break;
                case CommandExecutionLayers.SingleExecution:
                    m_Instance.m_SEPCommands.Add(new SEPendingCommand(command, Rhytm.RhytmController.GetInstance().CurrentTick));
                    break;
            }
        }

        public CommandsController()
        {
            m_Instance = this;

            m_PEPCommands = new List<PEPendingCommand>();
            m_SEPCommands = new List<SEPendingCommand>();
        }

        public void ProcessPendingCommands(int currentTick)
        {
            Debug.Log(" - PROCESS COMMANDS " + currentTick + " -");

            //Apply periodic execution commands
            for (int i = 0; i < m_PEPCommands.Count; i++)
            {
                if (m_PEPCommands[i].CommandShouldBeApplied(currentTick))
                    ApplyCommand(m_PEPCommands[i].Command);
            }

            //Apply single execution commands
            for (int i = 0; i < m_SEPCommands.Count; i++)
            {
                if (m_SEPCommands[i].CommandShouldBeApplied(currentTick))
                {
                    ApplyCommand(m_SEPCommands[i].Command);
                    m_SEPCommands.RemoveAt(i--);
                }
            }

            //Release periodic execution commands
            for (int i = 0; i < m_PEPCommands.Count; i++)
            {
                if (m_PEPCommands[i].CommandSholdBeReleased(currentTick))
                {
                    ReleaseCommand(m_PEPCommands[i].Command);
                    m_PEPCommands.RemoveAt(i--);
                }
            }
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_PEPCommands.Count == 0 && m_SEPCommands.Count == 0)
                return;

            for (int i = 0; i < m_PEPCommands.Count; i++)
                m_PEPCommands[i].PerformUpdate(deltaTime);

            for (int i = 0; i < m_SEPCommands.Count; i++)
                m_SEPCommands[i].PerformUpdate(deltaTime);
        }


        private void ApplyCommand(AbstractBattleCommand command)
        {
            command.Target.ApplyCommand(command);
        }

        private void ReleaseCommand(AbstractBattleCommand command)
        {
            command.Target.ReleaseCommand(command);
        }


        class SEPendingCommand : iUpdatable
        {
            protected AbstractCommandView View;
            protected int m_ApplyTick;

            public AbstractBattleCommand Command { get; private set; }


            public SEPendingCommand(AbstractBattleCommand command, int creationTick)
            {
                //Initialize data
                Command = command;
                m_ApplyTick = creationTick + command.ApplyDelay;

                CreateView();
            }

            public bool CommandShouldBeApplied(int currentTick) => m_ApplyTick == currentTick;

            public void PerformUpdate(float deltaTime) => View?.PerformUpdate(deltaTime);


            protected void CreateView()
            {
                if (Command is AttackCommand)
                {
                    //TODO: Add view facotry
                    //Move to view factory
                    double viewLifeTime = Command.ApplyDelay * Rhytm.RhytmController.GetInstance().TickDurationSeconds +
                                                               (!Command.Sender.IsEnemy ? Rhytm.RhytmController.GetInstance().DeltaInput : 0);

                    //Initialize view
                    View = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ProjectilePrefab);
                    View.Initialize(Command.Sender.ProjectileSpawnPosition,
                                    Command.Target.ProjectileHitPosition,
                                    (float)viewLifeTime);
                }
                else
                {
                    Debug.LogWarning("Create view for defence command.");
                }
            }
        }

        class PEPendingCommand : SEPendingCommand
        {
            protected int m_ReleaseTick;

            public PEPendingCommand(AbstractPeriodicBattleCommand command, int creationTick) : base(command, creationTick)
            {
                m_ReleaseTick = m_ApplyTick + command.ReleaseDelay;
            }

            public bool CommandSholdBeReleased(int currentTick) => m_ReleaseTick == currentTick;
        }
    }
}
