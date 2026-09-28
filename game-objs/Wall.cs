using SFML.Graphics;

namespace pacman;

public class Wall : Entity
{
    public Wall() : base("pacman") {} // "pacman" is our texture name "pacman.txt"

    public override bool Solid => true;

    public override void Create(Scene scene)
    {
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        // no-op
    }
}