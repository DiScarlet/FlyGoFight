using UnityEngine;

public class TrailTest : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }
}
