﻿using RhytmFighter.Battle.Core.Abstract;
using RhytmFighter.Persistant.Enums;

namespace RhytmFighter.Battle.Command.Model
{
    public abstract class AbstractPeriodicCommandModel : AbstractCommandModel
    {
        /// <summary>
        /// Amount of ticks command should be released (ex. cancel shield on the 2nd tick after apply)
        /// </summary>
        public int ReleaseDelay { get; private set; }

        public AbstractPeriodicCommandModel(iBattleObject sender, iBattleObject target, int applyDelay, int releaseDelay) 
            : base(sender, target, applyDelay)
        {
            ReleaseDelay = releaseDelay;
            Layer = CommandExecutionLayers.PeriodicExecution;
        }
    }
}
