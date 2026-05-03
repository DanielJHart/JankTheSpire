using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Powers;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(RegentCardPool))]
public class Supernova() : JankyCardModel(3, CardType.Skill,
    CardRarity.Rare, TargetType.Self)      
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new IntVar("ChargeTime", 3m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // Add Supernova power to player
        await PowerCmd.Apply<SupernovaPower>(this.Owner.Creature, this.DynamicVars.Damage.BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(1);
    }
}