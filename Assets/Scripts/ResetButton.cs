using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public List<GameObject> ObjectsToSpawn;

    private List<GameObject> SpawnedObjects;

    public void Awake()
    {
        SpawnedObjects = new List<GameObject>();
        foreach (GameObject o in ObjectsToSpawn)
            SpawnedObjects.Add(Instantiate(o));
    }

    public void Reset()
    {
        // SceneManager.LoadScene(0);

        foreach (GameObject o in SpawnedObjects)
        {
            Destroy(o);
            
            GameObject fragmentParent = GameObject.Find($"{o.name}Fragments");
            if (fragmentParent != null)
            {
                foreach (Transform fragment in fragmentParent.transform)
                    Destroy(fragment.gameObject);
                Destroy(fragmentParent);
            }
        }
        SpawnedObjects.Clear();
        foreach (GameObject o in ObjectsToSpawn)
            SpawnedObjects.Add(Instantiate(o));
    }
}
