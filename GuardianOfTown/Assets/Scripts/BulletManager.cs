using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float topBound = 30;
    private float proyectileSpeed = 5.0f;
    private PlayerController playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerManager.IsDead)
        {
            if (transform.position.z > topBound)
            {
                gameObject.SetActive(false);
            }
            else
            {
                transform.position += Vector3.forward * Time.deltaTime * proyectileSpeed;
            }
        }
    }

}
