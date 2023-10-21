using System.Collections;
using UnityEngine;

public class DestroyExplosionInSeconds : MonoBehaviour
{
    [SerializeField] private float secondsToDestroy = 1f;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DeactivateExplosionInSeconds());
    }

    IEnumerator DeactivateExplosionInSeconds() 
    {
        yield return new WaitForSeconds(secondsToDestroy);
        gameObject.SetActive(false);
        ObjectPoolerExplosion.ProjectileCount++;
    }
}
