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

    public Ghost() : base("pacman") // "pacman" is our texture name "pacman.txt"
    {
        // no-op
    }

    public override void Create(Scene scene)
    {
        direction = -1; // No direction
        speed = 100.0f;
        moving = true;
        
        base.Create(scene);
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
}