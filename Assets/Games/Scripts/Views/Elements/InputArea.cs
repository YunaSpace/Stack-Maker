using UnityEngine;
using UnityEngine.EventSystems;

public class InputArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 startPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var endPosition = eventData.position;

        var delta = endPosition - startPosition;

        PlayerManager.OnPointerInputed?.Invoke(delta);
    }
}