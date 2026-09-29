using SFML.Graphics;

namespace pacman;

public class Actor : Entity
{
    public virtual IntRect[][] texturePositions => null; // 2D Array of [ State[Frame:IntRect,...], ... ]
    
    private int _stateIdx;
    private int stateIdx
    {
        get => _stateIdx;
        set => _stateIdx = (value + texturePositions.Length) % texturePositions.Length;
    }
    
    private int frameIdx = 0;
    private float animTimer = 0.0f;
    public virtual float delayMs => 200.0f;
    
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

    public override void Create(Scene scene)
    {
        base.Create(scene);
        
        if (texturePositions.Length > 0 && texturePositions[0].Length > 0)
            sprite.TextureRect = texturePositions[0][0];
    }

    public override void Update(Scene scene, float deltaTime)
    {
        Animate(deltaTime);
        
        base.Update(scene, deltaTime);
    }
}