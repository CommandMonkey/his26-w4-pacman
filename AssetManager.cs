using SFML.Graphics;

namespace pacman;

public class AssetManager
{
    public static readonly string AssetPath = "assets";

    private readonly Dictionary<string, Texture> textures;
    private readonly Dictionary<string, Font> fonts;

    public AssetManager()
    {
        textures = new Dictionary<string, Texture>();
        fonts    = new Dictionary<string, Font>();
    }

    public Texture LoadTexture(string name)
    {
        if (textures.TryGetValue(name, out Texture found))
            return found;

        Texture tex = new Texture($"{AssetPath}/{name}.png");
        textures.Add(name, tex);
        return tex;
    }

    public Font LoadFont(string name)
    {
        if (fonts.TryGetValue(name, out Font found))
            return found;

        Font font = new Font($"{AssetPath}/{name}.ttf");
        fonts.Add(name, font);
        return font;
    }
}