using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace pacman;

public class Actor : Entity
{
    public virtual IntRect[][] texturePositions => null; // 2D Array of [ State[Frame:IntRect,...], ... ]
    
    private int _stateIdx;
    protected int stateIdx
    {
        get => _stateIdx;
        set => _stateIdx = (value + texturePositions.Length) % texturePositions.Length;
    }
    
    private int frameIdx = 0;
    private float animTimer = 0.0f;
    public virtual float delayMs => 200.0f;

    private float graceTimer = 0.0f;
    public bool isGraced => graceTimer > 0.0f;

    public bool wasAligned; //TODO: private
    protected float speed;
    protected int direction = -1; // To avoid the int-default-value "0" which is mapped to right
    protected bool moving;
    protected Vector2f originalPosition;
    protected float originalSpeed;

    protected bool IsAligned => (int)MathF.Floor(Position.X) % 18 == 0 && 
                                (int)MathF.Floor(Position.Y) % 18 == 0;
    
    public Actor(string textureName) : base(textureName)
    {
        // no-op
    }
    
    public virtual void Animate(float deltaTime)
    {
        animTimer += deltaTime * 1000;

        if (animTimer > delayMs)
        {
            animTimer = 0;
            
            // Select frame in current state
            if (stateIdx < texturePositions.Length)
            {
                IntRect[] currentState = texturePositions[stateIdx];
                
                if (frameIdx + 1 < currentState.Length)
                    frameIdx++;
                else
                    frameIdx = 0;
                
                sprite.TextureRect = currentState[frameIdx];
            }
            
        }
    }

    protected virtual void Reset()
    {
        graceTimer = 1.0f; // Also works as a on-game-start grace timer
        
        wasAligned = false;
        Position = originalPosition;
        speed = originalSpeed;
    }

    protected bool IsFree(Scene scene, int direction) // Direction works like an enum
    {
        Vector2f at = Position + new Vector2f(9, 9); // We use 9,9 to center, since tileSize is 18x18
        at += 18 * ToVector(direction);
        FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);
        return !scene.FindIntersects(rect).Any(e => e.Solid);
    }
    
    protected static Vector2f ToVector(int direction)
    {
        // Switch case moves counter-clockwise
        switch (direction)
        {
            // Right
            case 0: 
                return new Vector2f(1, 0);
            // Up
            case 1:
                return new Vector2f(0, -1);
            // Left
            case 2:
                return new Vector2f(-1, 0);
            // Down
            case 3:
                return new Vector2f(0, 1);
            
            // All other
            default:
                return new Vector2f(0, 0);
        }
    }

    protected virtual int PickDirection(Scene scene)
    {
        return -1;
    }
    
    public override void Create(Scene scene)
    {
        base.Create(scene);

        originalPosition = Position;
        originalSpeed = speed;
        
        Reset();
        
        if (texturePositions.Length > 0 && texturePositions[0].Length > 0)
            sprite.TextureRect = texturePositions[0][0];
    }

    public override void Update(Scene scene, float deltaTime)
    {   
        base.Update(scene, deltaTime);
        
        // Invincible Timer
        if (isGraced)
        {
            graceTimer -= deltaTime;
            sprite.Color = new Color(255, 255, 255, 128);
        }
        else
        {
            sprite.Color = new Color(255, 255, 255, 255);
        }

        // Move until aligned
        if (IsAligned)
        {
            if (!wasAligned)
                direction = PickDirection(scene);

            if (moving)
                wasAligned = true;
        }
        else
        {
            wasAligned = false;
        }
        
        // Animate call incase PickDirection changed stateIdx
        Animate(deltaTime);
        
        // Movement
        if (!moving || isGraced) return;
        Position += ToVector(direction) * (speed * deltaTime);
        
        // Handle wrap-around
        switch (MathF.Floor(Position.X))
        {
            // 432 = (ScreenW / 2) + 18
            case < 0:
                Position = new Vector2f(432, Position.Y);
                break;
            case > 432:
                Position = new Vector2f(0, Position.Y);
                break;
        }
    }
}