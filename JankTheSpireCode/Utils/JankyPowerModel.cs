using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;

namespace JankTheSpire.JankTheSpireCode.Utils;

public abstract class JankyPowerModel : CustomPowerModel
{
    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }    
    
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
}