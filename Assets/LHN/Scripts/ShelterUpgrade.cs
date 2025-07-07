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

        //# ���� �ƴ϶�� return;
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

        // ���׷��̵� ������ ������Ʈ�� �ִ��� Ȯ��
        m_upgradeableObject = hitObject.transform.parent.GetComponent<Shelter>();
        m_prevShelter = m_upgradeableObject;

        if (m_upgradeableObject.Level == 2) return;

        m_upgradeableObject.SetOutline(true);

        if (m_interaction.CurrentTool != ToolType.Hammer)
        {
            m_uiManager.SetInteractionUI(
                InteractionType.Shelter, true, "��ȭ�� ���ؼ��� �ٸ� ������ �ʿ��մϴ�", false
            );

            return;
        }
        m_uiManager.SetInteractionUI(
            InteractionType.Shelter, true, "��ȭ�Ϸ��� [E] Ű�� �����ּ���.", false
        );

        //# ���̰� Fence������ interaction Ű�� �ȴ��� ������ return;
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

    //todo UI ���� �ʿ��� - UI���� ������ �̸��� ���� ����, �ʿ� ������ ǥ��, presenter���� ���׷��̵� ���� ���� �Ǵ�
    /// <summary>
    /// Upgrade�� �ϱ� ���� ���� ������ Ȯ���ϱ� ���� �޼���
    /// Icon ������ ������ ItemDatabase���� ItemEnName���� �����;� ��
    /// </summary>
    /// <returns>������ ���� �̸��� ���� ����, �ʿ� ����(���� : List, 0 - ����, 1 - �ʿ�)�� ��ȯ</returns>
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

    //todo UI���� ���׷��̵� ���� �� ȣ��
    /// <summary>
    /// Upgrade�� ����
    /// </summary>
    public void ProgressUpgrade()
    {
        //todo Upgrade �����ؾ� ��
        m_upgradeableObject.Upgrade();

        foreach (var item in m_materials)
        {
            m_inventory.RemoveItemAmounts(item.Key, item.Value[1]);
        }
    }

    public void OnInteractionKeyPressed() => m_interactionKeyClicked = true;
    public void OnInteractionKeyReleased() => m_interactionKeyClicked = false;

    // ���� ���� ���� ��ȯ
    public int GetCurrentLevel() => m_upgradeableObject.Level;

    // ���� ������ ��ȯ
    public int GetCurrentDurability() => m_upgradeableObject.Durability;

    // �ִ� ������ ��ȯ
    public int GetMaxDurability() => m_upgradeableObject.MaxDurability;

    private void OutlineOff()
    {
        if (m_prevShelter != null)
            m_prevShelter.SetOutline(false);
        m_prevShelter = null;
    }

    private InteractionType GetCurrentInteractionType() => InteractionType.Shelter;
}