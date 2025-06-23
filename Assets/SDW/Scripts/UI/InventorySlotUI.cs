using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private InventorySlot m_inventorySlot;

    private Image m_itemSprite;
    private TextMeshProUGUI m_itemCount;
    private Button m_button;

    public InventorySlot InventorySlot => m_inventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        var itemSprites = GetComponentsInChildren<Image>();
        m_itemSprite = itemSprites[1];
        m_itemCount = GetComponentInChildren<TextMeshProUGUI>();

        ClearSlot();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        m_inventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemDataSO != null)
        {
            m_itemSprite.sprite = slot.ItemDataSO.Icon;
            m_itemSprite.color = Color.white;

            if (slot.ItemCount > 1) m_itemCount.text = slot.ItemCount.ToString();
            else m_itemCount.text = "";
        }
        else
            ClearSlot();
    }

    public void UpdateUISlot()
    {
        if (m_inventorySlot != null) UpdateUISlot(m_inventorySlot);
    }

    public void ClearSlot()
    {
        m_inventorySlot.ClearSlot();
        m_itemSprite.sprite = null;
        m_itemSprite.color = Color.clear;
        m_itemCount.text = "";
    }

    private void OnUISlotClick() => ParentDisplay?.OnSlotClicked(this);
    public void OnBeginDrag(PointerEventData eventData) => ParentDisplay?.OnBeginDragSlot(this, eventData);
    public void OnDrag(PointerEventData eventData) => ParentDisplay?.OnDragSlot(this, eventData);
    public void OnEndDrag(PointerEventData eventData) => ParentDisplay?.OnEndDragSlot(this, eventData);
}