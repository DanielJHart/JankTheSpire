namespace JankTheSpire.JankTheSpireCode.Utils;

public static class StringExtensions
{
    public static string CardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "cards", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "cards", "large", path);
    }
}