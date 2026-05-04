using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class EveryoneForThemself() : JankyCardModel(3,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllAllies)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(25, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Decay>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, play);

        if (this.CombatState != null)
        {
            foreach (Creature ally in this.CombatState.Allies)
            {
                if (ally.Player != null && ally.Player != this.Owner)
                {
                    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<Decay>(ally.Player), PileType.Hand, true));
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(7m);
    }
}