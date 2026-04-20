using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class WeeWoo() : JankyCardModel(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new CalculationBaseVar(30m),
            new ExtraDamageVar(-4m),
            new IntVar("ExtraDamageDescVar", _extraDamageDescVar),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature?, Decimal>) ((card, target) => CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Owner != card.Owner))))
        ];

    private decimal _extraDamageDescVar = 4m;

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.CalculationBase.UpgradeValueBy(10m);
    }
}