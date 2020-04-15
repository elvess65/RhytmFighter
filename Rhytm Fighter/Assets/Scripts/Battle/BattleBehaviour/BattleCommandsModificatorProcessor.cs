using System.Collections.Generic;
using RhytmFighter.Battle.Command;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class BattleCommandsModificatorProcessor
    {
        private Dictionary<CommandTypes, iCommandModificator> m_ActiveModificators;


        public BattleCommandsModificatorProcessor()
        {
            m_ActiveModificators = new Dictionary<CommandTypes, iCommandModificator>();
        }

        public void ProcessApplyCommand(AbstractBattleCommand inputCommand)
        {
            TryAddModificator(inputCommand);
            TryModifyCommand(inputCommand);
        }

        public void ProcessReleaseCommand(AbstractBattleCommand inputCommand)
        {
            TryRemoveModificator(inputCommand);
        }


        private void TryAddModificator(AbstractBattleCommand inputCommand)
        {
            if (inputCommand is iModificator modificatorCommand)
            {
                if (!HasModificator(inputCommand.Type))
                {
                    m_ActiveModificators.Add(inputCommand.Type, modificatorCommand.GetModificator());
                    Debug.Log("Modificator Processor: Add modificator " + inputCommand.Type);
                }
            }
        }

        private void TryRemoveModificator(AbstractBattleCommand inputCommand)
        {
            if (HasModificator(inputCommand.Type))
            {
                m_ActiveModificators.Remove(inputCommand.Type);
                Debug.Log("Modificator Processor: Remove modificator: " + inputCommand.Type);
            }
        }

        private void TryModifyCommand(AbstractBattleCommand inputCommand)
        {
            foreach(iCommandModificator commandModificator in m_ActiveModificators.Values)
                commandModificator.TryModifyCommand(inputCommand);
        }

        private bool HasModificator(CommandTypes type)
        {
            return m_ActiveModificators.ContainsKey(type);
        }
    }
}
