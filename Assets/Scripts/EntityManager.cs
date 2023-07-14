using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EntityManager : MonoBehaviour
{
    static public EntityManager Instance;

    [ReadOnly] public List<Enemy> Enemies = new List<Enemy>();
    [ReadOnly] public List<Entity> Entities = new List<Entity>();

    void Awake()
    {
        Instance = this;
    }

    public void AddEnemy(Enemy e)
    {
        Enemies.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        Enemies.Remove(e);
    }

    public void AddEntity(Entity e)
    {
        Entities.Add(e);
    }

    public void RemoveEntity(Entity e)
    {
        Entities.Remove(e);
    }

    void Start()
    {
        StartCoroutine(UpdateEnemiesPath());
    }

    IEnumerator UpdateEnemiesPath()
    {
        while (true)
        {
            List<Enemy> tmp = new List<Enemy>(Enemies);
            foreach (Enemy e in tmp)
            {
                if (e == null) continue;
                if (e.movement != null && !e.IsDead())
                {
                    e.movement.UpdatePostition();
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
