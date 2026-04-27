using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;

namespace JankTheSpire.JankTheSpireCode.Utils;

public abstract class JankyRelicModel : CustomRelicModel
{
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
}