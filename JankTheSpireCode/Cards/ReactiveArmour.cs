using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Powers;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(IroncladCardPool))]
public class ReactiveArmour() : JankyCardModel(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cardSource = this;
        await PowerCmd.Apply<ReactiveArmourPower>(cardSource.Owner.Creature, 1, cardSource.Owner.Creature, cardSource);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}