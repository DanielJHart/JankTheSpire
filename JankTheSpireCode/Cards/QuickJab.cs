using BaseLib.Abstracts;
using BaseLib.Utils;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(IroncladCardPool))]
public class QuickJab() : JankyCardModel(0, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    private bool _isPlayable;
    private List<Creature> _trackedCreatures = new List<Creature>();
    
    protected override bool IsPlayable =>  _isPlayable;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VulnerablePower>(play.Target, this.DynamicVars.Vulnerable.BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1m);
    }

    public override Task AfterCardGeneratedForCombat(CardModel card, bool addedByPlayer)
    {
        if (card == this)
        {
            foreach (Creature enemy in this.CombatState.Enemies)
            {
                enemy.PowerApplied += CreatureOnPowerApplied;
                _trackedCreatures.Add(enemy);
            }
        }
        
        return base.AfterCardGeneratedForCombat(card, addedByPlayer);
    }

    public override Task AfterCreatureAddedToCombat(Creature creature)
    {
        if (creature.IsMonster)
        {
            creature.PowerApplied += CreatureOnPowerApplied;
            _trackedCreatures.Add(creature);
        }
        
        return base.AfterCreatureAddedToCombat(creature);
    }

    private void CreatureOnPowerApplied(PowerModel obj)
    {
        if (obj is VulnerablePower)
        {
            _isPlayable = false;
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        foreach (var creature in _trackedCreatures)
        {
            creature.PowerApplied -= CreatureOnPowerApplied;
        }
        
        return base.AfterCombatEnd(room);
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (this.CombatState != null
            && this.CombatState.RoundNumber == 1)
        {
            foreach (Creature enemy in this.CombatState.Enemies)
            {
                enemy.PowerApplied += CreatureOnPowerApplied;
                _trackedCreatures.Add(enemy);
            }
        }
        
        _isPlayable = true;
        return base.AfterPlayerTurnStart(choiceContext, player);
    }
}