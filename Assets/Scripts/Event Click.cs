using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private UnityEvent onButtonDown;
    [SerializeField] private UnityEvent onButtonUp;
    private ControlPanel panel;
    private bool isHovering;
    private bool isDragging;
    private void Awake()
    {
        panel = GetComponentInParent<ControlPanel>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onButtonDown.Invoke();
        if (isHovering)
            isDragging = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onButtonUp.Invoke();
        isDragging = false;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (isDragging)
            isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (panel != null && isDragging)
            panel.DragLever(eventData);
    }
}
