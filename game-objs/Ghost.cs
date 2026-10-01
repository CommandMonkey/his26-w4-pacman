using System.Diagnostics.Contracts;
using SFML.Graphics;

namespace pacman;

public class Ghost : Actor
{
    // 2D Array of [ State[Frame:IntRect,...], ... ]  0:RedState[frame1,frame2],  1:BlueState[frame1,frame2]+
    public override IntRect[][] texturePositions => new [] {
        new [] {
            new IntRect(18*2, 0, 18, 18),
            new IntRect(18*3, 0, 18, 18)
        },
        new [] {
            new IntRect(18*2, 18, 18, 18),
            new IntRect(18*3, 18, 18, 18)
        }
    };

    private float frozenTimer;
    
    public Ghost() : base("pacman") // "pacman" is our texture name "pacman.txt"
    {
        // no-op
    }

    private void OnEatCandy(Scene scene, int amount)
    {
        frozenTimer = 5.0f;
    }
    
    public override void Create(Scene scene)
    {
        speed = 100.0f;
        moving = true;

        scene.Events.EatenCandy += OnEatCandy;
        
        base.Create(scene);
    }

    public override void Destroy(Scene scene)
    {
        scene.Events.EatenCandy -= OnEatCandy;
        base.Destroy(scene);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        frozenTimer = MathF.Max(frozenTimer - deltaTime, 0.0f);
        stateIdx = frozenTimer > 0.0f ? 1 : 0;
        
        base.Update(scene, deltaTime);
    }

    protected override int PickDirection(Scene scene)
    {
        List<int> validMoves = new List<int>();

        for (int i = 0; i < 4; i++)
        { 
            // Can't turn around 180 degrees
            if ((i + 2) % 4 == direction) continue;
            
            if (IsFree(scene, i)) validMoves.Add(i);
        }

        if (validMoves.Count == 0)
            return -1;
        
        int randIdx = new Random().Next(0, validMoves.Count);
        return validMoves[randIdx];
    }

    protected override void CollideWith(Scene scene, Entity e)
    {
        if (e is Pacman)
        {
            if (frozenTimer <= 0 && !((Actor)e).isGraced)
                scene.Events.PublishLoseHealth(1);
            
            // Frozen is already checked so this on blue-state pacman-collide
            if (!this.isGraced)
                Reset();
        }
    }
}