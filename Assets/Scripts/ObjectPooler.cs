using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    private List<List<GameObject>> objectPools;

    private void Awake()
    {
        Instance = this;

        objectPools = new List<List<GameObject>>();
        for (int i = 0; i < pools.Count; i++)
        {
            objectPools.Add(new List<GameObject>());
            for (int j = 0; j < pools[i].size; j++)
            {
                GameObject obj = Instantiate(pools[i].prefab, transform);
                obj.SetActive(false);
                objectPools[i].Add(obj);
            }
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].tag == tag)
            {
                for (int j = 0; j < objectPools[i].Count; j++)
                {
                    if (!objectPools[i][j].activeInHierarchy)
                    {
                        objectPools[i][j].SetActive(true);
                        objectPools[i][j].transform.position = position;
                        objectPools[i][j].transform.rotation = rotation;
                        return objectPools[i][j];
                    }
                }

                if (pools[i].prefab != null)
                {
                    GameObject obj = Instantiate(pools[i].prefab, transform);
                    obj.SetActive(true);
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    objectPools[i].Add(obj);
                    return obj;
                }

                break;
            }
        }

        return null;
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
    }
}
