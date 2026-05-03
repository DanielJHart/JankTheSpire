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

[Pool(typeof(NecrobinderCardPool))]
public class Chokehold() : JankyCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new PowerVar<ChokeholdPower>(6m)
        ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null)
        {
            ChokeholdPower? power = await PowerCmd.Apply<ChokeholdPower>(play.Target, this.DynamicVars["ChokeholdPower"].BaseValue, this.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["ChokeholdPower"].UpgradeValueBy(3m);
    }
}