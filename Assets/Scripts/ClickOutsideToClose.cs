using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickOutsideToClose : MonoBehaviour
{
    [Header("UI element to detect clicks outside of")]
    public RectTransform targetPanel;

    [Header("Event fired when clicking outside")]
    public UnityEvent OnClickOutside;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                        targetPanel,
                        Input.mousePosition,
                        null))
                {
                    OnClickOutside?.Invoke();
                }
            }
            else
            {
                OnClickOutside?.Invoke();
            }
        }
    }
}
