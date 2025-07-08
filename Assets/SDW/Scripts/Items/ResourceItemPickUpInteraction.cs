using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemPickUpInteraction : PickUpInteraction
{
    private WaitForSeconds m_respawnTime;

    private void Start() => m_respawnTime = new WaitForSeconds(300);

    private void Update()
    {
        if (!MouseInteraction()) return;

        GetItem();
    }

    protected override InteractionType GetCurrentInteractionType() => InteractionType.ResourceItem;

    private void GetItem()
    {
        if (!GameManager.Instance.IsCursorLocked) return;

        var hitCollider = m_interaction.Hit.collider;

        if (hitCollider == null) return;
        var hitObject = hitCollider.gameObject;

        if (hitObject == null) return;

        if (!hitObject.CompareTag("ResourceItem"))
        {
            m_uiManager.ClearInteractionUI(InteractionType.ResourceItem);
            OutlineOff();
            return;
        }

        //# 이전에 활성화된 outline이 있으면서, 현재 바라보는 아이템과 다른 경우
        if (m_prevSceneItem != null && hitObject.GetComponent<SceneItem>().GetInstanceID() != m_prevSceneItem.GetInstanceID())
        {
            // m_uiManager.ClearInteractionUI(InteractionType.ResourceItem);
            OutlineOff();
            return;
        }

        HandleItems();
    }

    private void HandleItems()
    {
        m_uiManager.SetInteractionUI(InteractionType.ResourceItem, true, "아이템 획득은 [E] 키를 눌러주세요.", false);

        var sceneItem = m_interaction.Hit.collider.gameObject.GetComponent<SceneItem>();
        
        if (sceneItem != m_prevSceneItem)
            OutlineOff();

        m_prevSceneItem = sceneItem;
        //# Outline On
        sceneItem.SetOutline(true);

        if (!m_isInteractionKeyPressed) return;

        int remainingAmount = m_inventory.AddItemSmart(sceneItem.ItemDataSO, sceneItem.ItemAmount);

        //# 모든 아이템이 성공적으로 추가됨
        if (remainingAmount == 0)
        {
            sceneItem.SetOutline(false);
            sceneItem.gameObject.SetActive(false);
            StartCoroutine(Respawn(sceneItem.gameObject));
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

    private IEnumerator Respawn(GameObject sceneItemObject)
    {
        yield return m_respawnTime;
        sceneItemObject.SetActive(true);
    }
}