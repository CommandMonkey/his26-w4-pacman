using SFML.Graphics;

namespace pacman;

public class Candy : Entity
{
    public Candy() : base("pacman") {} // "pacman" is our texture name "pacman.txt"

    public override void Create(Scene scene)
    {
        base.Create(scene);
        sprite.TextureRect = new IntRect(54, 36, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            scene.Events.PublishEatCandy(1);
            this.Dead = true;
        }
    }
}