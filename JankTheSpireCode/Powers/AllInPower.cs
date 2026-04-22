using BaseLib.Abstracts;
using JankTheSpire.JankTheSpireCode.Cards;
using JankTheSpire.JankTheSpireCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace JankTheSpire.JankTheSpireCode.Powers;

public sealed class AllInStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<AllIn>();
}

public class AllInDexterityPower : TemporaryDexterityPower, ICustomPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<AllIn>();
    protected override bool IsPositive => false;
}

public sealed class AllInPower : JankyPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;

    public override async Task AfterBlockCleared(Creature creature)
    {
        AllInPower power = this;
        
        power.Flash();
        
        await PowerCmd.Apply<AllInStrengthPower>(power.Owner, power.Amount, power.Owner, null);
        await PowerCmd.Apply<AllInDexterityPower>(power.Owner, power.Amount, power.Owner, null);
        
        // This is a temp power to apply the strength, get rid of it now.
        await PowerCmd.Remove(power);
    }

    public class CardSource
    {
    }
}