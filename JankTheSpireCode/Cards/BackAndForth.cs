using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(SilentCardPool))]
public class BackAndForth() : JankyCardModel(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromCard<Back>(this.IsUpgraded),
        HoverTipFactory.FromCard<Forth>(this.IsUpgraded),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<CardModel> cardList = [ModelDb.Card<Back>(), ModelDb.Card<Forth>()];
        
        var other = ModelDb.CardPool<ColorlessCardPool>()
            .GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint);
        List<CardModel> list = CardFactory.GetDistinctForCombat(this.Owner, cardList, 2, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        
        if (this.IsUpgraded)
            CardCmd.Upgrade((IEnumerable<CardModel>) list, CardPreviewStyle.HorizontalLayout);
        
        CardModel? card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) list, this.Owner);
        
        if (card == null)
            return;
        
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, true);
    }
}