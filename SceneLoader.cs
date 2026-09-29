using System.Text;
using SFML.System;

namespace pacman;

public class SceneLoader
{
    public static readonly string MapPath = "maps";
    
    private readonly Dictionary<char, Func<Entity>> loaders;
    private string currentScene = "";
    private string nextScene = "";

    public SceneLoader()
    {
        loaders = new Dictionary<char, Func<Entity>>
        {
            {'#', () => new Wall()},
            {'.', () => new Coin()},
            {'c', () => new Candy()},
            /*
            {'p', () => new Pacman()},
            {'g', () => new Ghost()}
            */
        };
    }

    private bool Create(char symbol, out Entity created)
    {
        if (loaders.TryGetValue(symbol, out Func<Entity> loader))
        {
            created = loader();
            return true;
        }

        created = null;
        return false;
    }

    public void HandleSceneLoad(Scene scene)
    {
        // Clear current scene
        if (nextScene == "") return;
        
        scene.Clear();
        
        // Parse file
        string path = $"{MapPath}/{nextScene}";
        if (File.Exists(path)) // Source: https://learn.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-10.0
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    // The create has a contains check so it will ignore ' ' and '|' 
                    if (Create(lines[y][x], out Entity valid))
                    {
                        valid.Position = new Vector2f(x*18, y*18); // Vector2f will implicit cast to float,float
                        scene.Spawn(valid);
                    }
                }
            }
        }

        // Change scene
        currentScene = nextScene;
        nextScene = "";
    }

    public void Load(string sceneName) => nextScene = sceneName;

    public void Reload() => nextScene = currentScene;
}