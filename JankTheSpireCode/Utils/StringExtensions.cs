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
    
    public static string PowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "powers", path);
    }

    public static string OrbImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "orbs", path);
    }    
    
    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "relics", path);
    }
}