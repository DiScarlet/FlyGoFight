using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("CameraTargets")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Settings of Animation")]
    public float moveDuration = 1.5f;
    public float holdTime = 1f;

    [Header("Cinemachine Cameras")]
    public CinemachineCamera followCam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followCam.gameObject.SetActive(false);
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        //Hold
        yield return new WaitForSeconds(0.5f);

        //Move right
        yield return MoveCamera(endPoint.position.x);

        //Hold
        yield return new WaitForSeconds(holdTime);

        //Move left
        yield return MoveCamera(startPoint.position.x);

        followCam.gameObject.SetActive(true);
    }

    private IEnumerator MoveCamera(float targetX)
    {
        float initialX = transform.position.x;
        float t = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float newX = Mathf.Lerp(initialX, targetX, Mathf.SmoothStep(0f, 1f, t));
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            yield return null;
        }

       transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
