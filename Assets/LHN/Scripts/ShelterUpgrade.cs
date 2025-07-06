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

    private UpgradeableObject m_upgradeableObject;

   

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();
        m_interaction = GetComponent<Interaction>();
        m_uiManager = GetComponent<InteractionUIManager>();
    }

    private void Start() => m_interactionKeyClicked = false;

    private void Update()
    {
        //# ���� �ƴ϶�� return;
        if (!GameTimer.IsDay) return;

        var hitObject = m_interaction.Hit.collider;

        if (hitObject == null)
        {
            m_uiManager.SetInteractionUI(InteractionType.Shelter, false);
            return;
        }

        if (!m_interaction.Hit.collider.CompareTag("Fence"))
        {
            m_uiManager.ClearInteractionUI(InteractionType.Shelter);

            m_interactionKeyClicked = false;
            return;
        }

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

        // ���׷��̵� ������ ������Ʈ�� �ִ��� Ȯ��
        m_upgradeableObject = m_interaction.Hit.collider.GetComponent<UpgradeableObject>();

        if (m_upgradeableObject == null) return;

        m_materials = new Dictionary<ItemEnName, List<int>>();

        //todo UI Open�ؾ� ��
        
        
        
    //todo m_upgradableObject�� ���� ��� �ʿ�
    //todo UI���� ���׷��̵� ������ �̸�, ���� ����, �ʿ� ������ �������� ����
    //# Test
    var testDict = GetMaterials();
        foreach (var test in testDict)
        {
            //todo test.Value �� List��(0 - ���� ���� ����, 1 - �ʿ� ����)
            Debug.Log($"{test.Key}, {test.Value}");
            //todo ������ �������� �������� ����
            //GameManager.Instance.Item.ItemEnDataSO[test.Key].Icon;
        }
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
            m_inventory.RemoveItemAmounts(item.Key, item.Value[0]);
        }
    }

    public void OnInteraction() => m_interactionKeyClicked = true;

    // ���� ���� ���� ��ȯ
    public int GetCurrentLevel()
    {
        return m_upgradeableObject.Level;
    }

    // ���� ������ ��ȯ
    public int GetCurrentDurability()
    {
        return m_upgradeableObject.Durability;
    }

    // �ִ� ������ ��ȯ
    public int GetMaxDurability()
    {
        return m_upgradeableObject.MaxDurability;
    }
}