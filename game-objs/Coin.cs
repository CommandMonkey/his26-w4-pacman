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

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);
    }
    
    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.PublishGainScore(amount);
            this.Dead = true;
        }
    }
}