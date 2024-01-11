using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIScrollToSelected : MonoBehaviour {
    
    [SerializeField] GameObject currentSelected;
    [SerializeField] GameObject previousSelected;
    [SerializeField] RectTransform currentSelectedTransform;
    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    void Update() {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected != null) {
            if (currentSelected != previousSelected) {
                previousSelected = currentSelected;
                currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                SnapTo(currentSelectedTransform);
            }
        }
    }

    void SnapTo(RectTransform target) {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) 
                            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        newPosition.x = 0;
        contentPanel.anchoredPosition = newPosition;
    }

}
