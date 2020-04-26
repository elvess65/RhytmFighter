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


        public BattleCommandsModificatorProcessor()
        {
            m_ActiveModificators = new Dictionary<CommandTypes, iCommandModificator>();
        }

        public void ProcessApplyCommand(AbstractCommandModel inputCommandModel)
        {
            TryAddModificator(inputCommandModel);
            TryModifyCommand(inputCommandModel);
        }

        public void ProcessReleaseCommand(AbstractCommandModel inputCommandModel)
        {
            TryRemoveModificator(inputCommandModel);
        }

        public bool HasModificator(CommandTypes type)
        {
            return m_ActiveModificators.ContainsKey(type);
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
            foreach(iCommandModificator commandModificator in m_ActiveModificators.Values)
                commandModificator.TryModifyCommand(inputCommandModel);
        }
    }
}
 