using SFML.Graphics;

namespace pacman;

public class Coin : Entity
{
    private int amount = 100;
    
    public Coin() : base("pacman") {} // "pacman" is our texture name "pacman.txt"

    public override void Create(Scene scene)
    {
        base.Create(scene);
        sprite.TextureRect = new IntRect(36, 36, 18, 18);
    }
    
    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.Events.PublishGainScore(amount);
            this.Dead = true;
        }
    }
}