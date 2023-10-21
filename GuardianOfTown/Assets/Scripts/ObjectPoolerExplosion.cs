using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerExplosion : MonoBehaviour
{
    public static ObjectPoolerExplosion SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int AmountToPool {  get; private set; }
    public static int ProjectileCount { get; set; }

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePool();
    }

    public void InstantiatePool()
    {
        ProjectileCount = AmountToPool = DataPersistantManager.Instance.SavedPlayerBullets;
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < AmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager 
        }
    }

    public void InstantiatePool(int newNumberOfBullets)
    {
        ProjectileCount = AmountToPool = newNumberOfBullets;
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        GameManager.SharedInstance._projectileText.text = $"{ProjectileCount}";
        for (int i = 0; i < AmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject()
    {
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

}
