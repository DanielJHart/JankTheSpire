using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class AvariceFAH : JankyRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    
    private bool _wasUsed = false;

    public override bool IsUsedUp => _wasUsed;

    public override Task AfterItemPurchased(Player player, MerchantEntry itemPurchased, int goldSpent)
    {
        if (player == this.Owner)
        {
            _wasUsed = true;
        }
        
        return base.AfterItemPurchased(player, itemPurchased, goldSpent);
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (!IsUsedUp && dealer != null && dealer.IsPlayer && dealer.Player == this.Owner)
        {
            return 1 + (0.1m * (int)(dealer.Player.Gold / 100));
            //return amount + (amount * );
        }
        
        return base.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource);
    }
}