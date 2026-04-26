using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace JankTheSpire.JankTheSpireCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class ImHelping() : JankyRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target,
        CardModel? cardSource)
    {
        if (power is StrengthPower && amount > 0 
                                   && giver == this.Owner.Creature 
                                   && giver.CombatState != null)
        {
            foreach (Player player in giver.CombatState.Players)
            {
                if (player != this.Owner)
                {
                    PowerCmd.Apply<StrengthPower>(player.Creature, amount, this.Owner.Creature, null);
                }
            }

            return 0;
        }
        
        return base.ModifyPowerAmountGiven(power, giver, amount, target, cardSource);
    }
}