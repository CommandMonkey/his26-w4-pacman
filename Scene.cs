using SFML.Graphics;

namespace pacman;

public class Scene
{
    private List<Entity> entities;
    public readonly SceneLoader Loader;
    public readonly AssetManager Assets;

    public Scene()
    {
        entities = new List<Entity>();
    }

    public void Spawn(Entity entity)
    {
        // TODO: Implement
    }

    public void Clear()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entities.RemoveAt(i);
            entity.Destroy(this);
        }
    }

    public void UpdateAll(float deltaTime)
    {
        // TODO: Implement
    }

    public void RenderAll(RenderTarget target)
    {
        // TODO: Implement
    }

    public bool FindByType<T>(out T found) where T : Entity
    {
        foreach (Entity entity in entities)
        {
            if (!entity.Dead && entity is T typed)
            {
                found = typed;
                return true;
            }
        }

        found = default(T);
        return false;
    }

    public IEnumerable<Entity> FindIntersects(FloatRect bounds)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            if (entity.Dead) continue;
            if (entity.Bounds.Intersects(bounds))
                yield return entity; // Yield pauses here after returning, i.e FindIntersects intenral for loop is resumed next time this is called, so it dosen't have to iterate the same already-checked entities again. (avoids returning true for the same entity every time this is called) 
        }
    }
}