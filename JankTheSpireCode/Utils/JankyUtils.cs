using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace JankTheSpire.JankTheSpireCode.Utils;

public class JankyUtils
{
    
}

public class TrackedPowerVar(string name) : DynamicVar(name, 0M)
{
    private Func<PowerModel, Creature?, Decimal>? _trackedCalc;
    
    public override void SetOwner(AbstractModel owner)
    {
        base.SetOwner(owner);
        this.UpdateValues();
    }
    
    public TrackedPowerVar WithTracked(Func<PowerModel, Creature?, Decimal> trackedCalc)
    {
        if (this._trackedCalc != null)
            throw new InvalidOperationException($"Tried to set extra multiplier calc on {this} twice!");
        this._trackedCalc = !(trackedCalc.Target is AbstractModel) ? trackedCalc : throw new InvalidOperationException("Multiplier calc must be static!");
        return this;
    }
    
    public Decimal Calculate(Creature? target)
    {
        Decimal result = 0;
        if (this._trackedCalc == null)
            throw new InvalidOperationException("Extra multiplier calc must be specified!");
        if (this._owner != null)
        {
            PowerModel owner = (PowerModel) this._owner;
            result = !CombatManager.Instance.IsInProgress ? 0M : this._trackedCalc(owner, target);
        }
        return result;
    }
    
    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        this.PreviewValue = this.Calculate(target);
    }
    
    protected virtual DynamicVar GetBaseVar()
    {
        return (DynamicVar) (((PowerModel) this._owner!)!).DynamicVars.CalculationBase;
    }

    protected virtual DynamicVar GetExtraVar()
    {
        return (DynamicVar) (((PowerModel) this._owner!)!).DynamicVars.CalculationExtra;
    }
    
    protected override Decimal GetBaseValueForIConvertible() => this.Calculate(null);

    public override string ToString() => this.Calculate(null).ToString();
    
    private void UpdateValues()
    {
        if (this._owner == null)
            return;
        this.BaseValue = this.GetBaseVar().BaseValue;
    }
}