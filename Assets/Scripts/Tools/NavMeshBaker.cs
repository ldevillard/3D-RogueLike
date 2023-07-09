using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    List<GameObject> entityObjects = new List<GameObject>();

    [Button("Bake NavMesh")]
    private void BakeNavMesh()
    {
        // Trouver tous les objets avec le script Entity
        MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
        {
            if (monoBehaviour is IEntity)
            {
                entityObjects.Add(monoBehaviour.gameObject);
                monoBehaviour.gameObject.SetActive(false);
            }
        }

        // Faire le bake
        if (TryGetComponent<NavMeshSurface>(out NavMeshSurface navMeshSurface))
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("No NavMeshSurface component found!");
        }

        // Réactiver les objets et vider la liste
        foreach (GameObject obj in entityObjects)
        {
            obj.SetActive(true);
        }
        entityObjects.Clear();
    }
}
