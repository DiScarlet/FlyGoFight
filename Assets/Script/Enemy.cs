using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _cloudParticlesPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        BirdMain bird = collision.collider.GetComponent<BirdMain>();
        
        if (bird != null)
        {
            // Add particles / clouds boom
            Instantiate(_cloudParticlesPrefab, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
            return;
        }

        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            return;
        }

        if (collision.contacts[0].normal.y < - 0.5)
        {
            // Add particles / clouds boom
            Instantiate(_cloudParticlesPrefab, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
            return;
        }
    }
}
