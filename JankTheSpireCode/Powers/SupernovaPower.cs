using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Powers;

public class SupernovaPower : JankyPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return (IEnumerable<DynamicVar>) [(DynamicVar) 
                new CalculationBaseVar(1m),
                new CalculationExtraVar(1m),
                new TrackedPowerVar("TurnsLeft").WithTracked((power, creature) =>  ((power as SupernovaPower)!).TurnsLeft)];
        }
    }

    private int TurnCount = 1;
    private int TurnsToCharge = 0;
    public int TurnsLeft { get; private set; } = 0;
    private int GatheredStars = 0;
    private int BaseDamageAmount = 0;

    private bool used = false;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        BaseDamageAmount = this.Amount;
        
        if (applier != null && applier == this.Owner)
        {
            Player? player = applier.Player;
            if (player != null && player.PlayerCombatState != null)
            {
                GatheredStars += player.PlayerCombatState.Stars;
                this.SetAmount(BaseDamageAmount * GatheredStars);
                PlayerCmd.SetStars(0, player);
            }
        }
        
        TurnsToCharge += 3;
        TurnsLeft = TurnsToCharge - TurnCount;
        
        return base.AfterApplied(applier, cardSource);
    }

    public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target,
        CardModel? cardSource)
    {
        if (power == this && target != null && target == this.Owner)
        {
            if (this.Amount > 0)
            {
                this.TurnsToCharge += 3;
                TurnsLeft = TurnsToCharge - TurnCount;
                return 0m;
            }
        }
        
        return base.ModifyPowerAmountGiven(power, giver, amount, target, cardSource);
    }

    public override Task AfterStarsGained(int amount, Player gainer)
    {
        if (gainer.Creature == this.Owner)
        {
            GatheredStars += amount;
            this.SetAmount(BaseDamageAmount * GatheredStars);
            PlayerCmd.SetStars(0, gainer);
        }
        
        return Task.CompletedTask;
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Player)
        {
            TurnCount++;
            TurnsLeft--;

            if (TurnCount == TurnsToCharge)
            {
                // Explode
                this.Flash();
                IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new BlockingPlayerChoiceContext(), (IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner, null);
                used = true;
            }
        }
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (used)
        {
            PowerCmd.Remove(this);
        }
        
        return base.AfterTurnEnd(choiceContext, side);
    }
}