using UnityEngine;

public class KebabReputation : MonoBehaviour
{
    [SerializeField]
    private GameObject _cloudParticlesPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        BirdMain bird = collision.collider.GetComponent<BirdMain>();

        if (bird != null)
        {
            Instantiate(_cloudParticlesPrefab, transform.position, Quaternion.identity);

            KebabController.Instance.AddKebab();
            Destroy(gameObject);
        }
    }
}
