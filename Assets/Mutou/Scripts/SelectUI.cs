using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUI : MonoBehaviour,
    IMoveHandler,
    IPointerEnterHandler
{
    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir != MoveDirection.None)
            SelectSe.Instance.PlaySelect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectSe.Instance.PlaySelect();
    }
}
