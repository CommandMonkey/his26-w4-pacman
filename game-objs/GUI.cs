using SFML.Graphics;
using SFML.System;

namespace pacman;

public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth = 3;
    private int currentHealth;
    private int currentScore;

    public GUI() : base("pacman") // "pacman" is our texture name "pacman.txt"
    {
        scoreText = new Text();
        scoreText.CharacterSize *= 4;
        scoreText.Scale *= 0.25f;
    }

    public override bool Solid => false;

    private void OnLoseHealth(Scene scene, int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            scene.Loader.Reload();
    }

    private void OnGainScore(Scene scene, int amount)
    {
        currentScore += amount;
    }
    
    public override void Create(Scene scene)
    {
        base.Create(scene);

        scene.LoseHealth += OnLoseHealth;
        scene.GainScore += OnGainScore;
        
        sprite.TextureRect = new IntRect(72, 36, 18, 18); // Heart Full
        sprite.Scale *= 2;

        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.DisplayedString = "Score";
        currentHealth = maxHealth;
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.LoseHealth += OnLoseHealth;
        scene.GainScore += OnGainScore;
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
        
        sprite.Position = new Vector2f(36, 396);

        for (int i = 0; i < maxHealth; i++)
        {
            sprite.TextureRect = i < currentHealth
                ? new IntRect(72, 36, 18, 18) // Full Heart
                : new IntRect(72, 9, 18, 18); // Empty Heart
            
            base.Render(target);
            sprite.Position += new Vector2f(18 * 2, 0);
        }
        
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(
            414 - scoreText.GetGlobalBounds().Width, 396 // Right aligned text
        );
        target.Draw(scoreText);
    } 
}