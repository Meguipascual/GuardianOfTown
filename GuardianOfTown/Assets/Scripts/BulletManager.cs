using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float topBound = 30;
    [SerializeField] private float proyectileSpeed = 10.0f;
    private PlayerController playerManager;
    public ParticleSystem criticalParticles;
    // Start is called before the first frame update
    void Start()
    {
        criticalParticles = GetComponent<ParticleSystem>();
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
                ObjectPooler.ProjectileCount++;
                GameManager.SharedInstance.projectileText.text = $"Projectile: {ObjectPooler.ProjectileCount}";
            }
            else
            {
                transform.position += Vector3.forward * Time.deltaTime * proyectileSpeed;
            }
        }
    }


}
