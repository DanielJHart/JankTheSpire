using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(RegentCardPool))]
public class RightfullyMine() : CustomCardModel(3,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override int CanonicalStarCost => 3;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        
        List<PowerModel> originalPowers = play.Target.Powers
            .Select<PowerModel, PowerModel>(
                (Func<PowerModel, PowerModel>)(p => (PowerModel)p.ClonePreservingMutability())).ToList<PowerModel>();

        foreach (PowerModel power in originalPowers)
        {
            PowerModel? powerById = play.Target.GetPowerById(power.Id);
            {
                PowerModel newPower = (PowerModel) power.ClonePreservingMutability();
                this.DoHackyThingsForSpecificPowers(newPower);
                await PowerCmd.Apply(newPower, this.Owner.Creature, (Decimal) power.Amount, this.Owner.Creature, this);
            }
        }
    }
    
    private void DoHackyThingsForSpecificPowers(PowerModel power)
    {
        if (!(power is ITemporaryPower temporaryPower))
            return;
        temporaryPower.IgnoreNextInstance();
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(1);
    }
}