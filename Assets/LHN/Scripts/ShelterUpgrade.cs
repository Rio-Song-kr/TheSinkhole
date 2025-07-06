using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShelterUpgrade : MonoBehaviour
{
    public ToolType CurrentTool = ToolType.None;
    [SerializeField] private Interaction m_interaction;
    [SerializeField] private Inventory m_inventory;

    [SerializeField] private GameObject m_crosshairObject;
    [SerializeField] private GameObject m_itemPickUpTextObject;
    [SerializeField] private TextMeshProUGUI m_itemPickUpText;

    private bool m_interactionKeyClicked;

    private Dictionary<ItemEnName, List<int>> m_materials;

    private UpgradeableObject m_upgradeableObject;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();
        m_interaction = GetComponent<Interaction>();
    }

    private void Start() => m_interactionKeyClicked = false;

    private void Update()
    {
        //# 낮이 아니라면 return;
        if (!GameTimer.IsDay) return;

        //# 낮이지만 Fence가 아니면 return;
        if (m_interaction.Hit.collider == null)
        {
            m_crosshairObject.SetActive(true);
            return;
        }
        if (!m_interaction.Hit.collider.CompareTag("Fence"))
        {
            m_crosshairObject.SetActive(true);
            m_interactionKeyClicked = false;
            return;
        }

        //# 강화를 위해 E 키를 누르는 표시
        SetTextObject(true, "강화하려면 [E] 키를 눌러주세요.");

        //# 낮이고 Fence이지만 interaction 키가 안눌려 졌으면 return;
        if (!m_interactionKeyClicked) return;
        m_interactionKeyClicked = false;

        //# 낮이고  Fence이고 interaction 키가 눌려져지만 도구가 해머가 아니면 return;
        if (CurrentTool != ToolType.Hammer)
        {
            return;
        }

        // 업그레이드 가능한 컴포넌트가 있는지 확인
        m_upgradeableObject = m_interaction.Hit.collider.GetComponent<UpgradeableObject>();

        if (m_upgradeableObject == null) return;

        m_materials = new Dictionary<ItemEnName, List<int>>();

        //todo UI Open해야 함
        //todo m_upgradableObject에 관한 고민 필요
        //# Test
        var testDict = GetMaterials();
        foreach (var test in testDict)
        {
            Debug.Log($"{test.Key}, {test.Value}");
        }
    }

    private void SetTextObject(bool isActive, string text = "")
    {
        m_crosshairObject.SetActive(false);
        m_itemPickUpTextObject.SetActive(isActive);
        m_itemPickUpText.text = text;
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
            m_inventory.RemoveItemAmounts(item.Key, item.Value[0]);
        }
    }

    public void OnInteraction() => m_interactionKeyClicked = true;
}