using RhytmFighter.Battle.Command;
using System.Collections.Generic;

namespace RhytmFighter.Battle
{
    public class CommandsController 
    {
        private static CommandsController m_Instance;

        private List<PendingCommand> m_PendingCommands;


        public static void AddCommand(BattleCommand command) => m_Instance.m_PendingCommands.Add(new PendingCommand(command));

        public CommandsController()
        {
            m_Instance = this;
            m_PendingCommands = new List<PendingCommand>();
        }

       
        public void ProcessPendingCommands()
        {
            for(int i = 0; i < m_PendingCommands.Count; i++)
            {
                if (m_PendingCommands[i].Process())
                {
                    ReleaseCommand(m_PendingCommands[i].Command);
                    m_PendingCommands.RemoveAt(i--);
                }
            }
        }


        private void ReleaseCommand(BattleCommand command)
        {
            command.Target.ApplyCommand(command);
        }


        class PendingCommand
        {
            private int m_IterationsToRelease;

            public BattleCommand Command { get; private set; }


            public PendingCommand(BattleCommand command)
            {
                Command = command;
                m_IterationsToRelease = command.ApplyDelay;
            }

            public bool Process() => m_IterationsToRelease-- <= 0;
        }
    }
}
