using System.Collections.Generic;
using RhytmFighter.Battle.Command.Model;
using RhytmFighter.Battle.Command.Model.Modificator;
using RhytmFighter.Core.Enums;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class BattleCommandsModificatorProcessor
    {
        private Dictionary<CommandTypes, iCommandModificator> m_ActiveModificators;
        private List<CommandTypes> m_ContainersForModificatorsLastApply;

        public BattleCommandsModificatorProcessor()
        {
            m_ActiveModificators = new Dictionary<CommandTypes, iCommandModificator>();
            m_ContainersForModificatorsLastApply = new List<CommandTypes>();
        }

        public List<CommandTypes> ProcessApplyCommand(AbstractCommandModel inputCommandModel)
        {
            TryAddModificator(inputCommandModel);
            TryModifyCommand(inputCommandModel);

            return m_ContainersForModificatorsLastApply;
        }

        public void ProcessReleaseCommand(AbstractCommandModel inputCommandModel)
        {
            TryRemoveModificator(inputCommandModel);
        }

        
        private void TryAddModificator(AbstractCommandModel inputCommandModel)
        {
            if (inputCommandModel is iModificator modificatorCommand)
            {
                if (!HasModificator(inputCommandModel.Type))
                {
                    m_ActiveModificators.Add(inputCommandModel.Type, modificatorCommand.GetModificator());
                    Debug.Log("Modificator Processor: Add modificator " + inputCommandModel.Type);
                }
            }
        }

        private void TryRemoveModificator(AbstractCommandModel inputCommandModel)
        {
            if (HasModificator(inputCommandModel.Type))
            {
                m_ActiveModificators.Remove(inputCommandModel.Type);
                Debug.Log("Modificator Processor: Remove modificator: " + inputCommandModel.Type);
            }
        }

        private void TryModifyCommand(AbstractCommandModel inputCommandModel)
        {
            if (m_ContainersForModificatorsLastApply.Count > 0)
                m_ContainersForModificatorsLastApply.Clear();

            foreach (CommandTypes commandType in m_ActiveModificators.Keys)
            {
                bool modificationPerformed = m_ActiveModificators[commandType].TryModifyCommand(inputCommandModel);
                if (modificationPerformed)
                    m_ContainersForModificatorsLastApply.Add(commandType);
            }
        }

        private bool HasModificator(CommandTypes type)
        {
            return m_ActiveModificators.ContainsKey(type);
        }
    }
}
 