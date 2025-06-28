using UnityEngine;

public class ItemPickUpInteraction : MonoBehaviour
{
    [SerializeField] private GameObject m_itemPickUpObject;
    [SerializeField] private GameObject m_cursorObject;
    [SerializeField] private float m_intectionDistance;
    private static bool m_isInteractionKeyPressed;
    private SceneItem m_prevSceneItem = null;

    private void Update()
    {
        MouseInteraction();
    }

    private void MouseInteraction()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, m_intectionDistance) || !GameManager.Instance.IsCursorLocked)
        {
            m_itemPickUpObject.SetActive(false);

            if (GameManager.Instance.IsCursorLocked)
                m_cursorObject.SetActive(true);

            //# Outline Off
            if (m_prevSceneItem != null)
                m_prevSceneItem.SetOutline(false);
            m_prevSceneItem = null;
            return;
        }

        if (!hit.collider.gameObject.CompareTag("Item")) return;

        m_itemPickUpObject.SetActive(true);
        m_cursorObject.SetActive(false);

        var sceneItem = hit.collider.gameObject.GetComponent<SceneItem>();
        //# Outline On
        sceneItem.SetOutline(true);
        m_prevSceneItem = sceneItem;

        if (!m_isInteractionKeyPressed) return;

        var inventory = GetComponent<Inventory>();
        if (!inventory) return;

        int remainingAmount = inventory.AddItemSmart(sceneItem.ItemDataSO, sceneItem.ItemAmount);

        //# 모든 아이템이 성공적으로 추가됨
        if (remainingAmount == 0)
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, sceneItem.ItemDataSO, sceneItem.ItemAmount);
            GameManager.Instance.Item.ItemPools[sceneItem.ItemDataSO.ItemData.ItemId].Pool.Release(sceneItem);
            m_itemPickUpObject.SetActive(false);
        }
        else if (remainingAmount < sceneItem.ItemAmount)
        {
            //@ 일부만 추가됨 - 남은 수량으로 업데이트
            sceneItem.ItemAmount = remainingAmount;
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);

            //todo 아이템이 부분적으로 추가되었음을 시각적으로 표시
            //@ 예: 이펙트 재생, 사운드 등
        }
        else
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
        }
    }

    /// <summary>
    /// 플레이어와 충돌 시 아이템을 마인크래프트처럼 인벤토리에 추가
    /// 기존 스택을 우선 채우고 최적 분배
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    // private void OnTriggerStay(Collider other)
    // {
    //     if (!other.gameObject.CompareTag("Item")) return;
    //
    //     m_itemPickUpObject.SetActive(true);
    //
    //     if (!m_isInteractionKeyPressed) return;
    //
    //     //todo 아이템 획득 관련 메시지 작업을 띄워야 하며, 키를 눌렀을 때만 획득이 되어야 함
    //     var sceneItem = other.gameObject.GetComponent<SceneItem>();
    //
    //     var inventory = GetComponent<Inventory>();
    //     if (!inventory) return;
    //
    //     int remainingAmount = inventory.AddItemSmart(sceneItem.ItemDataSO, sceneItem.ItemAmount);
    //
    //     //# 모든 아이템이 성공적으로 추가됨
    //     if (remainingAmount == 0)
    //     {
    //         GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, sceneItem.ItemDataSO, sceneItem.ItemAmount);
    //         GameManager.Instance.Item.ItemPools[sceneItem.ItemDataSO.ItemData.ItemId].Pool.Release(sceneItem);
    //         m_itemPickUpObject.SetActive(false);
    //     }
    //     else if (remainingAmount < sceneItem.ItemAmount)
    //     {
    //         //@ 일부만 추가됨 - 남은 수량으로 업데이트
    //         sceneItem.ItemAmount = remainingAmount;
    //         GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
    //
    //         //todo 아이템이 부분적으로 추가되었음을 시각적으로 표시
    //         //@ 예: 이펙트 재생, 사운드 등
    //     }
    //     else
    //     {
    //         GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other) => m_itemPickUpObject.SetActive(false);
    public static void OnInteractionKeyPressed() => m_isInteractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isInteractionKeyPressed = false;
}