using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(DefectCardPool))]
public class ControlV() : JankyCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public CardPlay? LastCardPlay { get; set; }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (LastCardPlay == null)
            return;
        
        CardPileAddResult addResult = await CardPileCmd.AddGeneratedCardToCombat(LastCardPlay.Card.CreateClone(), PileType.Hand, true);

        if (this.IsUpgraded)
        {
            addResult.cardAdded.SetToFreeThisTurn();
        }
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        LastCardPlay = cardPlay;
        return base.AfterCardPlayed(context, cardPlay);
    }

    protected override void OnUpgrade()
    {
    }
}