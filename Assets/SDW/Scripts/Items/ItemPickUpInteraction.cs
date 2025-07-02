using UnityEngine;

public class ItemPickUpInteraction : MonoBehaviour
{
    private static bool m_isInteractionKeyPressed;
    private SceneItem m_prevSceneItem = null;
    private Interaction m_interaction;
    private Inventory m_inventory;

    private void Awake()
    {
        m_interaction = GetComponent<Interaction>();
        m_inventory = GetComponent<Inventory>();

        if (!m_interaction) Debug.Log("Player에 Interaction이 없습니다.");
        if (!m_inventory) Debug.Log("Player에 Inventory가 없습니다.");
    }

    private void Update() => MouseInteraction();

    private void MouseInteraction()
    {
        if (!m_interaction.IsDetected || !GameManager.Instance.IsCursorLocked)
        {
            m_interaction.SetTextObject(false);

            if (GameManager.Instance.IsCursorLocked)
                m_interaction.SetCrosshairObject(true);

            //# Outline Off
            OutlineOff();
            return;
        }

        var hitObject = m_interaction.Hit.collider.gameObject;

        if (!hitObject.CompareTag("Item"))
        {
            OutlineOff();
            return;
        }

        //# 이전에 활성화된 outline이 있으면서, 현재 바라보는 아이템과 다른 경우
        if (m_prevSceneItem != null && hitObject.GetComponent<SceneItem>().GetInstanceID() != m_prevSceneItem.GetInstanceID())
        {
            OutlineOff();
            return;
        }

        HandleItems();
    }

    private void OutlineOff()
    {
        if (m_prevSceneItem != null)
            m_prevSceneItem.SetOutline(false);
        m_prevSceneItem = null;
    }

    private void HandleItems()
    {
        m_interaction.SetTextObject(true, "아이템 획득은 [E] 키를 눌러주세요.");
        m_interaction.SetCrosshairObject(false);

        var sceneItem = m_interaction.Hit.collider.gameObject.GetComponent<SceneItem>();
        //# Outline On
        sceneItem.SetOutline(true);
        m_prevSceneItem = sceneItem;

        if (!m_isInteractionKeyPressed) return;

        int remainingAmount = m_inventory.AddItemSmart(sceneItem.ItemDataSO, sceneItem.ItemAmount);

        //# 모든 아이템이 성공적으로 추가됨
        if (remainingAmount == 0)
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, sceneItem.ItemDataSO, sceneItem.ItemAmount);
            GameManager.Instance.Item.ItemPools[sceneItem.ItemDataSO.ItemEnName].Pool.Release(sceneItem);
            m_interaction.SetTextObject(false);
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

    public static void OnInteractionKeyPressed() => m_isInteractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isInteractionKeyPressed = false;
}