using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletManager : MonoBehaviour
{
    private float topBound = 30;
    [SerializeField] private float proyectileSpeed = 10.0f;
    [SerializeField] private float _proyectileRotationSpeed = 1.0f;
    private PlayerController playerController;
    private float _rotateXAxisRandom;
    private float _rotateYAxisRandom;
    private float _rotateZAxisRandom;
    private float _rotationX;
    private float _rotationY;
    private float _rotationZ;
    public ParticleSystem criticalParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        criticalParticles = GetComponent<ParticleSystem>();
        playerController = FindObjectOfType<PlayerController>();
        _rotateXAxisRandom = Random.value;
        _rotateYAxisRandom = Random.value;
        _rotateZAxisRandom = Random.value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerController.IsDead)
        {
            _rotationX += _rotateXAxisRandom * Time.deltaTime * _proyectileRotationSpeed;
            _rotationY += _rotateYAxisRandom * Time.deltaTime * _proyectileRotationSpeed;
            _rotationZ += _rotateZAxisRandom * Time.deltaTime * _proyectileRotationSpeed;

            if (transform.position.z > topBound)
            {
                DestroyBullet(gameObject);
            }
            else
            {
                transform.position += Vector3.forward * Time.deltaTime * proyectileSpeed;
                transform.Rotate(_rotationX, _rotationY, _rotationZ);
            }
        }
    }

    public void DestroyBullet(GameObject gameObject)
    {
        gameObject.SetActive(false);
        ObjectPooler.ProjectileCount++;
        GameManager.SharedInstance._projectileText.text = $"{ObjectPooler.ProjectileCount}";
        ResetRotation();
    }
    public void ResetRotation()
    {
        _rotationX = 0;
        _rotationY = 0;
        _rotationZ = 0;
        _rotateXAxisRandom = Random.value;
        _rotateYAxisRandom = Random.value;
        _rotateZAxisRandom = Random.value;
    }
}
