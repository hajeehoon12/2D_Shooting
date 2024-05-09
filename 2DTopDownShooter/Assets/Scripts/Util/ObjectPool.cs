using UnityEngine;
using System.Collections.Generic;
public class ObjectPool : MonoBehaviour
{

    [System.Serializable]

    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    
    }

    public List<Pool> pools = new List<Pool>(); // Many Types of Object(Arrow, skill, etc) with object pooling
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, this.transform); // GameManager Child instantiate prefab from Object in GameManager
                obj.SetActive(false);
                queue.Enqueue(obj);

            }

            PoolDictionary.Add(pool.tag, queue);

        }

    }

    public GameObject SpawnFromPool(string tag) // Object is spawned -> queue reorder , except setactive cause different place
    {
        if (!PoolDictionary.ContainsKey(tag)) // if tag is not contain in pool dictionary -> end
        {
            return null; 
        }

        GameObject obj = PoolDictionary[tag].Dequeue(); // queue get first one
        PoolDictionary[tag].Enqueue(obj);

        obj.SetActive(true);
        return obj;

    }

}