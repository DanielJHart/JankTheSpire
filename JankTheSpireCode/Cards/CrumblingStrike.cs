using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace JankTheSpire.JankTheSpireCode.Cards;

[Pool(typeof(IroncladCardPool))]
public class CrumblingStrike() : CustomCardModel(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(this.CurrentDamage, ValueProp.Move), new IntVar("Decrease", 4M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    private const string DecreaseKey = "Decrease";
    private int _currentDamage = 24;
    private int _decreasedDamage;
    
    [SavedProperty]
    public int CurrentDamage
    {
        get => this._currentDamage;
        set
        {
            this.AssertMutable();
            this._currentDamage = Math.Max(value, 0);
            this.DynamicVars.Damage.BaseValue = (Decimal) this._currentDamage;
        }
    }
    
    [SavedProperty]
    public int DecreasedDamage
    {
        get => this._decreasedDamage;
        set
        {
            this.AssertMutable();
            this._decreasedDamage = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        
        int intValue = this.DynamicVars["Decrease"].IntValue;
        this.DebuffFromPlay(intValue);
        if (!(this.DeckVersion is CrumblingStrike deckVersion))
            return;
        deckVersion.DebuffFromPlay(intValue);
    }

    private void DebuffFromPlay(int extraDamage)
    {
        this.DecreasedDamage += extraDamage;
        this.UpdateDamage();
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Decrease"].UpgradeValueBy(-2m);
    }

    private void UpdateDamage() => this.CurrentDamage -= this.DynamicVars["Decrease"].IntValue;
}