using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class FreeForAll() : CustomCardModel(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (this.CombatState == null)
            return;
        
        foreach (Player player in this.CombatState.Players)
        {
            CardPile hand = PileType.Hand.GetPile(player);
            CardModel? card = hand.Cards.Where(model => model.EnergyCost.GetAmountToSpend() > 0).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>();

            if (card != null)
            {
                card.EnergyCost.SetThisTurn(0);
            }
        }
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}