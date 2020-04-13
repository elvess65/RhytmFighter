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

        private List<PeriodicPendingCommand> m_PeriodicCommands;
        private List<PendingCommand> m_SingleCommands;


        public static void AddCommand(AbstractBattleCommand command)
        {
            switch(command.Layer)
            {
                case CommandExecutionLayers.PeriodicExecution:
                    m_Instance.m_PeriodicCommands.Add(new PeriodicPendingCommand(command as AbstractPeriodicBattleCommand,
                                                                            Rhytm.RhytmController.GetInstance().CurrentTick,
                                                                            m_Instance.GetCommandViewFactory(command)));
                    break;
                case CommandExecutionLayers.SingleExecution:
                    m_Instance.m_SingleCommands.Add(new PendingCommand(command,
                                                                       Rhytm.RhytmController.GetInstance().CurrentTick,
                                                                       m_Instance.GetCommandViewFactory(command)));
                    break;
            }
        }


        public CommandsController()
        {
            m_Instance = this;

            m_PeriodicCommands = new List<PeriodicPendingCommand>();
            m_SingleCommands = new List<PendingCommand>();
        }

        public void ProcessPendingCommands(int currentTick)
        {
            Debug.Log(" - PROCESS COMMANDS " + currentTick + " -");

            //Apply periodic execution commands
            for (int i = 0; i < m_PeriodicCommands.Count; i++)
            {
                if (m_PeriodicCommands[i].CommandShouldBeApplied(currentTick))
                    ApplyCommand(m_PeriodicCommands[i].Command);
            }

            //Apply single execution commands
            for (int i = 0; i < m_SingleCommands.Count; i++)
            {
                if (m_SingleCommands[i].CommandShouldBeApplied(currentTick))
                {
                    ApplyCommand(m_SingleCommands[i].Command);
                    m_SingleCommands.RemoveAt(i--);
                }
            }

            //Release periodic execution commands
            for (int i = 0; i < m_PeriodicCommands.Count; i++)
            {
                if (m_PeriodicCommands[i].CommandShouldBeReleased(currentTick))
                {
                    ReleaseCommand(m_PeriodicCommands[i].Command);
                    m_PeriodicCommands.RemoveAt(i--);
                }
            }
        }

        public void PerformUpdate(float deltaTime)
        {
            if (m_PeriodicCommands.Count == 0 && m_SingleCommands.Count == 0)
                return;

            for (int i = 0; i < m_PeriodicCommands.Count; i++)
                m_PeriodicCommands[i].PerformUpdate(deltaTime);

            for (int i = 0; i < m_SingleCommands.Count; i++)
                m_SingleCommands[i].PerformUpdate(deltaTime);
        }


        private void ApplyCommand(AbstractBattleCommand command)
        {
            command.Target.ApplyCommand(command);
        }

        private void ReleaseCommand(AbstractBattleCommand command)
        {
            command.Target.ReleaseCommand(command);
        }


        private CommandViewFactory GetCommandViewFactory(AbstractBattleCommand command)
        {
            switch(command)
            {
                case AttackCommand attackCommand:
                    return new AttackCommandViewFactory();
                case DefenceCommand defenceCommand:
                    return new DefenceCommandViewFactory();
            }

            return null;
        }


        private class PendingCommand : iUpdatable
        {
            protected int m_ApplyTick;
            protected AbstractCommandView View;
            
            public AbstractBattleCommand Command { get; private set; }


            public PendingCommand(AbstractBattleCommand command, int creationTick, CommandViewFactory viewFactory)
            {
                //Initialize data
                Command = command;
                m_ApplyTick = creationTick + command.ApplyDelay;

                //Create view
                View = viewFactory.CreateView(command);
            }

            public bool CommandShouldBeApplied(int currentTick) => m_ApplyTick == currentTick;

            public void PerformUpdate(float deltaTime) => View?.PerformUpdate(deltaTime);
        }

        private class PeriodicPendingCommand : PendingCommand
        {
            protected int m_ReleaseTick;

            public PeriodicPendingCommand(AbstractPeriodicBattleCommand command, int creationTick, CommandViewFactory viewFactory) :
                base(command, creationTick, viewFactory)
            {
                m_ReleaseTick = m_ApplyTick + command.ReleaseDelay;
            }

            public bool CommandShouldBeReleased(int currentTick) => m_ReleaseTick == currentTick;
        }
    }
}
