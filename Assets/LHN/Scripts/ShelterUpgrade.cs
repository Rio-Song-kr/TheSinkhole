using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShelterUpgrade : MonoBehaviour
{
    [SerializeField] private Interaction m_interaction;
    [SerializeField] private Inventory m_inventory;

    [SerializeField] private GameObject m_crosshairObject;

    private bool m_interactionKeyClicked;
    private InteractionUIManager m_uiManager;

    private Dictionary<ItemEnName, List<int>> m_materials;

    private Shelter m_upgradeableObject;

    public ShelterUpgradeUI ShelterUpgradeUI;

    private Shelter m_prevShelter = null;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();
        m_interaction = GetComponent<Interaction>();
        m_uiManager = GetComponent<InteractionUIManager>();
    }

    private void Start() => m_interactionKeyClicked = false;

    private void Update()
    {
        if (!MouseInteraction()) return;

        //# 낮이 아니라면 return;
        if (!GameTimer.IsDay) return;

        var hitObject = m_interaction.Hit.collider;

        if (hitObject == null) return;

        if (!hitObject.CompareTag("Fence"))
        {
            m_uiManager.ClearInteractionUI(InteractionType.Shelter);

            m_interactionKeyClicked = false;
            OutlineOff();
            return;
        }

        // 업그레이드 가능한 컴포넌트가 있는지 확인
        m_upgradeableObject = hitObject.transform.parent.GetComponent<Shelter>();
        m_prevShelter = m_upgradeableObject;

        if (m_upgradeableObject.Level == 2) return;

        m_upgradeableObject.SetOutline(true);

        if (m_interaction.CurrentTool != ToolType.Hammer)
        {
            m_uiManager.SetInteractionUI(
                InteractionType.Shelter, true, "강화를 위해서는 다른 도구가 필요합니다", false
            );

            return;
        }
        m_uiManager.SetInteractionUI(
            InteractionType.Shelter, true, "강화하려면 [E] 키를 눌러주세요.", false
        );

        //# 낮이고 Fence이지만 interaction 키가 안눌려 졌으면 return;
        if (!m_interactionKeyClicked) return;
        m_interactionKeyClicked = false;


        if (m_upgradeableObject == null) return;

        m_materials = new Dictionary<ItemEnName, List<int>>();
        m_materials = GetMaterials();

        m_uiManager.ClearInteractionUI(InteractionType.Shelter);
        GameManager.Instance.SetCursorUnlock();


        ShelterUpgradeUI.gameObject.SetActive(true);
        ShelterUpgradeUI.Show(m_materials, this);
    }

    private bool MouseInteraction()
    {
        if (!m_interaction.IsDetected || !GameManager.Instance.IsCursorLocked)
        {
            var currentType = GetCurrentInteractionType();
            m_uiManager.ClearInteractionUI(currentType);

            //# Outline Off
            OutlineOff();
            return false;
        }

        return true;
    }

    //todo UI 연동 필요함 - UI에서 아이템 이름과 보유 수량, 필요 수량을 표시, presenter에서 업그레이드 가능 여부 판단
    /// <summary>
    /// Upgrade를 하기 위한 재료와 수량을 확인하기 위한 메서드
    /// Icon 정보는 별도로 ItemDatabase에서 ItemEnName으로 가져와야 함
    /// </summary>
    /// <returns>아이템 영어 이름과 보유 수량, 필요 수량(수량 : List, 0 - 보유, 1 - 필요)을 반환</returns>
    private Dictionary<ItemEnName, List<int>> GetMaterials()
    {
        foreach (var upgradeMaterials in GameManager.Instance.Shelter.ShelterUpgradeData[m_upgradeableObject.LevelId])
        {
            var quantity = new List<int>();
            var itemEnName = GameManager.Instance.Item.ItemIdEnName[upgradeMaterials.ItemId];
            quantity.Add(m_inventory.GetItemAmounts(itemEnName));
            quantity.Add(upgradeMaterials.MaterialQuantity);
            m_materials[itemEnName] = quantity;
        }
        return m_materials;
    }

    //todo UI에서 업그레이드 진행 시 호출
    /// <summary>
    /// Upgrade를 진행
    /// </summary>
    public void ProgressUpgrade()
    {
        //todo Upgrade 수정해야 함
        m_upgradeableObject.Upgrade();

        foreach (var item in m_materials)
        {
            m_inventory.RemoveItemAmounts(item.Key, item.Value[1]);
        }
    }

    public void OnInteractionKeyPressed() => m_interactionKeyClicked = true;
    public void OnInteractionKeyReleased() => m_interactionKeyClicked = false;

    // 현재 쉘터 레벨 반환
    public int GetCurrentLevel() => m_upgradeableObject.Level;

    // 현재 내구도 반환
    public int GetCurrentDurability() => m_upgradeableObject.Durability;

    // 최대 내구도 반환
    public int GetMaxDurability() => m_upgradeableObject.MaxDurability;

    private void OutlineOff()
    {
        if (m_prevShelter != null)
            m_prevShelter.SetOutline(false);
        m_prevShelter = null;
    }

    private InteractionType GetCurrentInteractionType() => InteractionType.Shelter;
}