using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Powers;

public sealed class ReactiveArmourPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target == this.Owner
            && (dealer != null && target == dealer)
            && this.CombatState.CurrentSide == this.Owner.Side
            && result.UnblockedDamage > 0)
        {
            await PowerCmd.Apply<PlatingPower>(target, result.UnblockedDamage, dealer, null);
        }
    }
}