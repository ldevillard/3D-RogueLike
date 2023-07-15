using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntitySpawner : MonoBehaviour
{
    /*
    This class allow us to spawn entity in the scene
    and then spawn again when the spawned entity died
    */

    public Entity entityToSpawn;
    public ParticleSystem spawnFx;

    public float spawnDelay = 1f;

    Entity current;

    void Start()
    {
        Invoke("Spawn", spawnDelay);
    }

    void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        spawnFx.Play();

        yield return new WaitForSeconds(0.5f);

        current = Instantiate(entityToSpawn, transform.position, transform.rotation);
        current.OnDie += HandleDie;

        current.transform.DOScale(1, 0.3f).From(0.1f).SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            spawnFx.Stop();
        });
    }

    void HandleDie(Entity e)
    {
        e.OnDie -= HandleDie;
        Invoke("Spawn", spawnDelay);
    }
}