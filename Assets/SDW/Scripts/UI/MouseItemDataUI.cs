using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MouseItemDataUI : MonoBehaviour
{
    private Image m_itemSprite;
    private TextMeshProUGUI m_itemCount;

    public InventorySlot InventorySlot;

    private void Awake()
    {
        m_itemSprite = GetComponentInChildren<Image>();
        m_itemCount = GetComponentInChildren<TextMeshProUGUI>();

        m_itemSprite.color = Color.clear;
        m_itemCount.text = "";

        InventorySlot = new InventorySlot();
    }

    public void UpdateMouseSlot(InventorySlot slot)
    {
        InventorySlot.AddItem(slot);

        m_itemSprite.sprite = slot.ItemDataSO.Icon;
        m_itemSprite.color = Color.white;

        m_itemCount.text = slot.ItemCount > 1 ? slot.ItemCount.ToString() : "";
    }

    private void Update()
    {
        if (InventorySlot.ItemDataSO != null)
        {
            transform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        InventorySlot.ClearSlot();
        m_itemSprite.color = Color.clear;
        m_itemSprite.sprite = null;
        m_itemCount.text = "";
    }

    public static bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}