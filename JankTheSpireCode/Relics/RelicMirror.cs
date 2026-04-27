using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace JankTheSpire.JankTheSpireCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class RelicMirror() : JankyRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        var validRelics = this.Owner.Relics.Where(relic => relic is { IsMelted: false, IsUsedUp: false } && relic.GetType() !=  typeof(RelicMirror)).ToList();

        if (!validRelics.Any())
        {
            return;
        }
        
        RelicModel? selectedRelic = this.Owner.PlayerRng.Rewards.NextItem(validRelics);

        if (selectedRelic != null)
        {
            await RelicCmd.Obtain(selectedRelic, this.Owner);
        }
    }
}