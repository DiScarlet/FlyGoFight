using UnityEngine;
using UnityEngine.EventSystems;

public class GreenBird : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
