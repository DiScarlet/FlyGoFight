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
            Debug.Log("From KR initiated additon of kebab");
            Instantiate(_cloudParticlesPrefab, transform.position, Quaternion.identity);

            KebabController.Instance.AddKebab();
            Destroy(gameObject);
        }
    }
}
