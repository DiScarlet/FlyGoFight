using UnityEngine;

public class IcePlank : MonoBehaviour
{
    [SerializeField] private GameObject _iceParticlesPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something eneterd ice");
        if (collision.gameObject.CompareTag("Bird"))
        {
            Debug.Log("Destroying ice");
            Instantiate(_iceParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
