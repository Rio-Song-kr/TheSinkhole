using System;
using UnityEngine;

public class TestItemPickUp : MonoBehaviour
{
    /// <summary>
    /// 플레이어와 충돌 시 아이템을 스마트하게 인벤토리에 추가
    /// 마인크래프트 스타일로 기존 스택을 우선 채우고 최적 분배
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    // private void OnCollisionEnter(Collision other)
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Item")) return;

        var sceneItem = other.gameObject.GetComponent<SceneItem>();

        var inventory = GetComponent<Inventory>();
        if (!inventory) return;

        int remainingAmount = inventory.AddItemSmart(sceneItem.ItemDataSO, sceneItem.ItemAmount);

        //# 모든 아이템이 성공적으로 추가됨
        if (remainingAmount == 0)
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, sceneItem.ItemDataSO, sceneItem.ItemAmount);
            GameManager.Instance.Item.ItemPools[sceneItem.ItemDataSO.ItemData.ItemId].Pool.Release(sceneItem);
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
}