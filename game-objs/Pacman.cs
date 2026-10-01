using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace pacman;

public class Pacman : Actor
{
    // 2D Array of [ State[Frame:IntRect,...], ... ]
    public override IntRect[][] texturePositions => new [] {
        // 0 : RightState
        new [] {
            new IntRect(0, 0, 18, 18),
            new IntRect(18, 0, 18, 18)
        },
        // 1 : UpState
        new [] {
            new IntRect(0, 18, 18, 18),
            new IntRect(18, 18, 18, 18)
        },
        // 2 : LeftState
        new [] {
            new IntRect(0, 18*2, 18, 18),
            new IntRect(18, 18*2, 18, 18)
        },
        // 3 : DownState
        new [] {
            new IntRect(0, 18*3, 18, 18),
            new IntRect(18, 18*3, 18, 18)
        },
        // 4 : NotMovingState
        new []
        {
            new IntRect(18*2, 18*3, 18, 18),
        }
    };
    
    public Pacman() : base("pacman") // "pacman" is our texture name "pacman.txt"
    {
        // no-op
    }

    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    public override void Create(Scene scene)
    {
        speed = 100.0f;
        base.Create(scene);

        scene.Events.LoseHealth += OnLoseHealth;
    }
    
    public override void Destroy(Scene scene)
    {       
        scene.Events.LoseHealth -= OnLoseHealth;
        base.Destroy(scene);
    }

    protected override void Reset()
    {
        Console.WriteLine($"A  IsAligned: {IsAligned}; wasAligned: {wasAligned}; dir: {direction}; mov: {moving}");
        base.Reset();
        direction = -1;
        Console.WriteLine($"   IsAligned: {IsAligned}; wasAligned: {wasAligned}; dir: {direction}; mov: {moving}");
    }

    protected override int PickDirection(Scene scene)
    {
        int dir = direction;
        
        // Set direction
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed((Keyboard.Key.D)))
        {
            dir = 0;
            moving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed((Keyboard.Key.W)))
        {
            dir = 1;
            moving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed((Keyboard.Key.A)))
        {
            dir = 2;
            moving = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed((Keyboard.Key.S)))
        {
            dir = 3;
            moving = true;
        }
        
        // Update state
        stateIdx = dir < 0 ? 4 : dir; // This works because the texturePositions array is in the same order as the directions-int

        // If not free, don't move
        if (IsFree(scene, dir)) return dir;
        if (!IsFree(scene, direction)) moving = false;

        stateIdx = moving ? direction : 4; // 4 : NotMovingState
        return direction;
    }
}