using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public static int ProjectileCount { get; set; }

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProjectileCount = amountToPool;
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject()
    {
        ProjectileCount = 20;
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            ProjectileCount--;
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                GameManager.SharedInstance.projectileText.text = "Projectile: " + ProjectileCount;
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

}
