using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Orbs;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(DefectCardPool))]
public class PoisonTheWell() : JankyCardModel(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OrbCmd.Channel<PoisonOrb>(choiceContext, this.Owner);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}