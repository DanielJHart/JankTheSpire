using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Helpers;

namespace JankTheSpire.JankTheSpireCode.Utils;

public class JankyOrbModel : CustomOrbModel
{
    public override decimal PassiveVal { get; }
    public override decimal EvokeVal { get; }
    public override Color DarkenedColor { get; }

    public override string? CustomIconPath
    {
        get
        {
            //var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".OrbImagePath();
            string path = ImageHelper.GetImagePath("orbs/lightning_orb.png");
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    public override string? CustomSpritePath
    {
        get
        {
            //var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.tscn".OrbImagePath();
            string path = SceneHelper.GetScenePath("orbs/orb_visuals/lightning_orb");
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
}