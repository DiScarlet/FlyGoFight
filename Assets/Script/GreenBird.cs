using UnityEngine;
using UnityEngine.EventSystems;

public class GreenBird : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    Vector3 _initialPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        _initialPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = Color.red;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.color = Color.white;

        Vector2 directionToInitialPosition = _initialPosition - transform.position;
        //
        GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * 100);
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        worldPos.z = transform.position.z;
        transform.position = worldPos;
    }
}
