using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(IroncladCardPool))]
public class AllIn() : JankyCardModel(3, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(10M), new PowerVar<DexterityPower>(10M)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cardSource = this;
        
        IEnumerable<CardModel> cards = PileType.Hand.GetPile(this.Owner).Cards;
        var numCardsDiscarded = cards.Count();
        
        // Discard the cards
        await CardCmd.Discard(choiceContext, cards);

        // Calculate gain/loss
        var strengthGain = cardSource.DynamicVars["StrengthPower"].BaseValue * numCardsDiscarded;
        
        // Apply gain/loss
        await PowerCmd.Apply<AllInPower>(cardSource.Owner.Creature, strengthGain, cardSource.Owner.Creature, cardSource);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["StrengthPower"].UpgradeValueBy(10m);
        this.DynamicVars["DexterityPower"].UpgradeValueBy(10m);
    }
}