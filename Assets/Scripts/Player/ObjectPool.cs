using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Drone
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        #region Singleton
        public static ObjectPool Instance;

        private void Awake()
        {
            Instance = this;
        }
        #endregion

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    Debug.LogError($"Failed to instantiate prefab for pool {pool.tag}");
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            Queue<GameObject> objectPool = poolDictionary[tag];

            if (objectPool.Count == 0)
            {
                // 풀이 비어있으면 새로운 오브젝트 생성
                Pool poolInfo = pools.Find(p => p.tag == tag);
                GameObject newObj = Instantiate(poolInfo.prefab);
                objectPool.Enqueue(newObj);
            }

            GameObject objectToSpawn = objectPool.Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            // 오브젝트를 다시 큐에 추가
            objectPool.Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void ReturnToPool(string tag, GameObject objectToReturn)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }

            objectToReturn.SetActive(false);
        }
    }
} 