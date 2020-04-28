using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Command.View;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using System.Collections.Generic;

namespace RhytmFighter.Battle.Command
{
    public class CommandsController : iUpdatable
    {
        private static CommandsController m_Instance;

        private List<AbstractCommandView> m_Views;
        private List<PendingCommand> m_SingleCommands;
        private List<PeriodicPendingCommand> m_PeriodicCommands;
        private Dictionary<CommandTypes, AbstractCommandViewFactory> m_ViewFactories;


        public static void AddCommand(AbstractCommandModel command)
        {
            switch(command.Layer)
            {
                case CommandExecutionLayers.PeriodicExecution:
                    m_Instance.m_PeriodicCommands.Add(new PeriodicPendingCommand(command as AbstractPeriodicCommandModel, Rhytm.RhytmController.GetInstance().CurrentTick));
                    break;
                case CommandExecutionLayers.SingleExecution:
                    m_Instance.m_SingleCommands.Add(new PendingCommand(command, Rhytm.RhytmController.GetInstance().CurrentTick));
                    break;
            }
        }

        public static void CreateViewForCommand(AbstractCommandModel command)
        {
            //Create view
            AbstractCommandView View = m_Instance.GetCommandViewFactory(command).CreateView(command);
            View.OnViewDisposed += m_Instance.ViewDestroyedHandler;
            m_Instance.ViewCreatedHandler(View);
        }


        public CommandsController()
        {
            m_Instance = this;

            m_Views = new List<AbstractCommandView>();
            m_SingleCommands = new List<PendingCommand>();
            m_PeriodicCommands = new List<PeriodicPendingCommand>();
            m_ViewFactories = new Dictionary<CommandTypes, AbstractCommandViewFactory>();
        }

        public void ProcessPendingCommands(int currentTick)
        {
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
            if (m_Views.Count == 0)
                return;

            for (int i = 0; i < m_Views.Count; i++)
                m_Views[i].PerformUpdate(deltaTime);
        }


        private void ApplyCommand(AbstractCommandModel command)
        {
            command.Target.ApplyCommand(command);
        }

        private void ReleaseCommand(AbstractCommandModel command)
        {
            command.Target.ReleaseCommand(command);
        }

        private AbstractCommandViewFactory GetCommandViewFactory(AbstractCommandModel command)
        {
            if (!m_ViewFactories.ContainsKey(command.Type))
            {
                switch (command.Type)
                {
                    case CommandTypes.Attack:
                        m_ViewFactories.Add(command.Type, new AttackCommandViewFactory());
                        break;

                    case CommandTypes.Defence:
                        m_ViewFactories.Add(command.Type, new DefenceCommandViewFactory());
                        break;
                }
            }

            return m_ViewFactories[command.Type];
        }


        private void ViewCreatedHandler(AbstractCommandView view)
        {
            m_Views.Add(view);
        }

        private void ViewDestroyedHandler(AbstractCommandView view)
        {
            if (m_Views.Contains(view))
                m_Views.Remove(view);
        }


        class PendingCommand
        {
            protected int m_ApplyTick;
            protected AbstractCommandView View;
            
            public AbstractCommandModel Command { get; private set; }


            public PendingCommand(AbstractCommandModel command, int creationTick)
            {
                //Initialize data
                Command = command;
                m_ApplyTick = creationTick + command.ApplyDelay;
            }

            public bool CommandShouldBeApplied(int currentTick) => m_ApplyTick == currentTick;
        }

        class PeriodicPendingCommand : PendingCommand
        {
            protected int m_ReleaseTick;

            public PeriodicPendingCommand(AbstractPeriodicCommandModel command, int creationTick) : base(command, creationTick)
            {
                m_ReleaseTick = m_ApplyTick + command.ReleaseDelay;
            }

            public bool CommandShouldBeReleased(int currentTick) => m_ReleaseTick == currentTick;
        }
    }
}
