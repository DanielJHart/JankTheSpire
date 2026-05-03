using BaseLib.Abstracts;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Orbs;

public class PoisonOrb : JankyOrbModel
{
    // protected override string PassiveSfx => "event:/sfx/characters/defect/defect_lightning_passive";
    // protected override string EvokeSfx => "event:/sfx/characters/defect/defect_lightning_evoke";
    // protected override string ChannelSfx => "event:/sfx/characters/defect/defect_lightning_channel";

    public override Decimal PassiveVal => this.ModifyOrbValue(1M);
    public override Decimal EvokeVal => this.ModifyOrbValue(3M);
    
    public override bool IncludeInRandomPool => true;
    
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await this.Passive(choiceContext, null);
    }

    public override Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        this.Trigger();
        Creature? randomTarget = GetRandomTarget();
        
        if (randomTarget != null)
        {
            return PowerCmd.Apply<PoisonPower>(randomTarget, this.PassiveVal, this.Owner.Creature, null);
        }
        
        return base.Passive(choiceContext, target);
    }

    public override Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        Creature? randomTarget = GetRandomTarget();
        
        if (randomTarget != null)
        {
            PowerCmd.Apply<PoisonPower>(randomTarget, this.EvokeVal, this.Owner.Creature, null);
        }
        
        return base.Evoke(playerChoiceContext);
    }

    private Creature? GetRandomTarget()
    {
        List<Creature> list = this.CombatState.GetOpponentsOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
        if (list.Count == 0)
        {
            return null;
        }
        
        return this.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(this.CombatState.HittableEnemies);
    }
}