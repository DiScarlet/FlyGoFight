using UnityEngine;

public class SlingshotBands : MonoBehaviour
{
    private Transform bird;

    [Header("References")]
    [SerializeField] private LineRenderer leftBand;
    [SerializeField] private LineRenderer rightBand;
    [SerializeField] private Transform leftFork;
    [SerializeField] private Transform rightFork;


    public void SetBird(Transform birdTransform)
    {
        bird = birdTransform;
        SetVisible(true);
    }

    public void ClearBird()
    {
        bird = null;
        SetVisible(false);
    }

    private void LateUpdate()
    {
        if (bird == null)
        {
            return;
        }

        leftBand.SetPosition(0, leftFork.position);
        leftBand.SetPosition(1, bird.position);

        rightBand.SetPosition(0, rightFork.position);
        rightBand.SetPosition(1, bird.position);
    }

    private void SetVisible(bool visible)
    {
        leftBand.enabled = visible;
        rightBand.enabled = visible;
    }
}
