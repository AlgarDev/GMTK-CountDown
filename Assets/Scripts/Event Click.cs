using System.Collections;
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
    [SerializeField] private float graceDuration = 0.5f;
    private Coroutine graceCoroutine;
    private void Awake()
    {
        panel = GetComponentInParent<ControlPanel>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        onButtonDown.Invoke();
        if (isHovering)
            isDragging = true;
        if (graceCoroutine != null)
        {
            StopCoroutine(graceCoroutine);
            graceCoroutine = null;
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        onButtonUp.Invoke();
        StopGrace();
        isDragging = false;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        isHovering = true;
        if (graceCoroutine != null)
        {
            StopCoroutine(graceCoroutine);
            graceCoroutine = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        isHovering = false;
        if (isDragging)
        {
            if (graceCoroutine != null)
                StopCoroutine(graceCoroutine);

            graceCoroutine = StartCoroutine(GraceRelease());
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!CanInteract())
            return;
        if (panel != null && isDragging)
            panel.DragLever(eventData);
    }
    private IEnumerator GraceRelease()
    {
        yield return new WaitForSeconds(graceDuration);

        isDragging = false;
        graceCoroutine = null;

        //panel.SmoothLeverReset(false);
    }

    private void StopGrace()
    {
        if (graceCoroutine != null)
        {
            StopCoroutine(graceCoroutine);
            graceCoroutine = null;
        }
    }
    private bool CanInteract()
    {
        return panel.ship == null || panel.ship.IsDocked() && panel.countdownCoroutine == null && !panel.ship.isDead && MenuManager.Instance.beginPlay && !panel.ship.hasWon;
    }
}
