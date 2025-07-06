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
        //# ���� �ƴ϶�� return;
        if (!GameTimer.IsDay) return;

        //# �������� Fence�� �ƴϸ� return;
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

        //# ��ȭ�� ���� E Ű�� ������ ǥ��
        SetTextObject(true, "��ȭ�Ϸ��� [E] Ű�� �����ּ���.");

        //# ���̰� Fence������ interaction Ű�� �ȴ��� ������ return;
        if (!m_interactionKeyClicked) return;
        m_interactionKeyClicked = false;

        //# ���̰�  Fence�̰� interaction Ű�� ���������� ������ �ظӰ� �ƴϸ� return;
        if (CurrentTool != ToolType.Hammer)
        {
            return;
        }

        // ���׷��̵� ������ ������Ʈ�� �ִ��� Ȯ��
        m_upgradeableObject = m_interaction.Hit.collider.GetComponent<UpgradeableObject>();

        if (m_upgradeableObject == null) return;

        m_materials = new Dictionary<ItemEnName, List<int>>();

        //todo UI Open�ؾ� ��
        //todo m_upgradableObject�� ���� ��� �ʿ�
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
}