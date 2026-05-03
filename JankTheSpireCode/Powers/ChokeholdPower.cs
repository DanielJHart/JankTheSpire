using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Powers;

public class ChokeholdPower : JankyPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private Creature? osty;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier != null 
            && applier.Player != null 
            && applier.Player.Osty != null)
        {
            osty = applier.Player.Osty;
        }
        
        return base.AfterApplied(applier, cardSource);
    }

    public override Task AfterAttack(AttackCommand command)
    {
        if (command.Attacker == osty)
        {
            PowerCmd.Remove(this);
        }
        
        return base.AfterAttack(command);
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature == osty)
        {
            PowerCmd.Remove(this);
        }
        
        return base.AfterDeath(choiceContext, creature, wasRemovalPrevented, deathAnimLength);
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Player)
        {
            await CreatureCmd.Damage(choiceContext, this.Owner, new DamageVar(this.Amount, ValueProp.Move), null, null);
            if (!this.Owner.IsAlive)
            {
                await Cmd.CustomScaledWait(0.1f, 0.25f);
            }
        }
        
        await base.BeforeTurnEnd(choiceContext, side);
    }
    
}