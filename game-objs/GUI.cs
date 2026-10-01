using System.Text;
using SFML.Graphics;
using SFML.System;

namespace pacman;

public class GUI : Entity
{
    private Font textFont;
    
    private const string HighScoreFile = "highscore.txt";
    private Text highScoreText;
    private int highScore;
    
    private Text scoreText;
    private int currentScore;

    private Text pauseHighScoreText;
    
    private int maxHealth = 3;
    private int currentHealth;
    
    public override bool RenderOnPause => true;

    public GUI() : base("pacman") // "pacman" is our texture name "pacman.txt"
    {
        highScoreText = new Text();
        highScoreText.CharacterSize /= 2;
        highScoreText.Scale *= 0.5f;
        
        scoreText = new Text();
        scoreText.CharacterSize *= 4;
        scoreText.Scale *= 0.25f;

        pauseHighScoreText = new Text();
    }

    public override bool Solid => false;

    private void LoadHighScore()
    {
        if (File.Exists(HighScoreFile)) // Source: https://learn.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-10.0
        {
            string text = File.ReadAllText(HighScoreFile, Encoding.UTF8).Trim();
            if (int.TryParse(text, out int result))
                highScore = result;
        }
    }

    public void SaveHighScore()
    {
        if (currentScore > highScore)
            File.WriteAllText(HighScoreFile, currentScore.ToString(), Encoding.UTF8); // Source: https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-10.0
    }
    
    private void OnLoseHealth(Scene scene, int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            DontDestroyOnLoad = false;
            LoadHighScore();
            Program.QueueReload(true);
        }
    }

    private void OnGainScore(Scene scene, int amount)
    {
        currentScore += amount;
        SaveHighScore();

        if (!scene.FindByType<Coin>(out _))
        {
            DontDestroyOnLoad = true;
            Program.QueueReload();
        }
    }
    
    public override void Create(Scene scene)
    {
        base.Create(scene);

        LoadHighScore(); // Loading this every reset is abit unneccesarry, had scores (or atleast highscore) been in Program it could have been loaded just once
        
        scene.Events.LoseHealth += OnLoseHealth;
        scene.Events.GainScore += OnGainScore;
        
        currentHealth = maxHealth;
        
        sprite.TextureRect = new IntRect(72, 36, 18, 18); // Heart Full
        sprite.Scale *= 2;
        
        textFont = scene.Assets.LoadFont("pixel-font");

        highScoreText.Font = textFont;
        scoreText.DisplayedString = "High Score";

        scoreText.Font = textFont;
        scoreText.DisplayedString = "Score";

        pauseHighScoreText.Font = textFont;
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.LoseHealth -= OnLoseHealth;
        scene.Events.GainScore -= OnGainScore;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        // no-op
    }

    public override void Render(RenderTarget target)
    {
        // 18  : TileSize
        // 36  : 18*2
        // 396 : (screenW/2) - 18
        // 414 : screenW/2
        
        // Pause GUI
        if (Program.GamePaused)
        {
            pauseHighScoreText.DisplayedString = $"High Score: {highScore}";
            pauseHighScoreText.Position = new Vector2f(
                (Program.viewSize.X/2)+18 - (pauseHighScoreText.GetGlobalBounds().Width/2), (Program.viewSize.Y/2) // Center aligned text (+18 since view if similarly offset for map reasons)
            );
            pauseHighScoreText.CharacterSize *= 4;
            pauseHighScoreText.Scale *= 0.25f;
            target.Draw(pauseHighScoreText);
            pauseHighScoreText.CharacterSize /= 4;
            
            pauseHighScoreText.DisplayedString = $"Press [space] to play again";
            pauseHighScoreText.Position = new Vector2f(
                (Program.viewSize.X/2)+18 - (pauseHighScoreText.GetGlobalBounds().Width/2), (3 * Program.viewSize.Y/4) 
            );
            target.Draw(pauseHighScoreText);
            pauseHighScoreText.Scale *= 4;

            return; // <-- Early Return
        }
        
        // Normal GUI
        sprite.Position = new Vector2f(36, 396);

        for (int i = 0; i < maxHealth; i++)
        {
            sprite.TextureRect = i < currentHealth
                ? new IntRect(72, 36, 18, 18) // Full Heart
                : new IntRect(72, 0, 18, 18); // Empty Heart
            
            base.Render(target);
            sprite.Position += new Vector2f(18 * 2, 0);
        }

        highScoreText.DisplayedString = $"High Score: {highScore}";
        highScoreText.Position = new Vector2f(
            414 - highScoreText.GetGlobalBounds().Width, 430 // Right aligned text
        );
        target.Draw(highScoreText);
        
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(
            414 - scoreText.GetGlobalBounds().Width, 396 // Right aligned text
        );
        target.Draw(scoreText);
    } 
}