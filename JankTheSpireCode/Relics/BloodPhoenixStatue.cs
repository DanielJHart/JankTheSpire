using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace JankTheSpire.JankTheSpireCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class BloodPhoenixStatue() : CustomRelicModel
{
    private bool _wasUsed = false;
    
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override bool IsUsedUp => this._wasUsed;
    
    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (!this.IsUsedUp)
        {
            if (creature == this.Owner.Creature
                && this.IsAtPercThreshold(25m, creature.CurrentHp, creature.MaxHp))
            {
                this._wasUsed = true;
                
                // number between 0 - 1.0
                var rand = this.Owner.RunState.Rng.Shuffle.NextFloat();
                if (rand > 0.5f)
                {
                    // Oops, time to die.
                    CreatureCmd.Kill(this.Owner.Creature);
                }
                else
                {
                    // Yay, time to heal.
                    CreatureCmd.Heal(this.Owner.Creature, this.Owner.Creature.MaxHp);
                }
            }
        }
        
        return base.AfterCurrentHpChanged(creature, delta);
    }

    private bool IsAtPercThreshold(decimal percent, int currentHealth, int maxHealth)
    {
        decimal perc = ((decimal)currentHealth / (decimal)maxHealth) * 100m;
        return perc <= percent;
    }
}