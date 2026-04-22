using JankTheSpire.JankTheSpireCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace JankTheSpire.JankTheSpireCode.Utils;

public class BackAndForthUtility : SingletonModel
{
    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.GetType() == typeof(CardModel))
        {
            await TransformCard<Back>(cardPlay.Card);
        }
        else if (cardPlay.Card.GetType() == typeof(Back))
        {
            await TransformCard<Forth>(cardPlay.Card);
        }
    }

    private async Task TransformCard<A>(CardModel CardToTransform) where A : CardModel
    {
        CardModel newCard = CardToTransform.CombatState.CreateCard<Forth>(CardToTransform.Owner);
        if (CardToTransform.IsUpgraded)
            CardCmd.Upgrade(newCard);

        await CardCmd.Transform(CardToTransform, newCard);
    }
}