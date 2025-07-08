using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterUpgradeUI : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject shelterCanvas; // Shelter UI ĵ����
    public TextMeshProUGUI timeText; // ���׷��̵� �ð� �ؽ�Ʈ
    public TextMeshProUGUI descriptionText; // ���׷��̵� ���� �ؽ�Ʈ

    [Header("��� ���� UI")]
    public List<IngreSlot> slots; //ReqIngre0~3����;

    [Header("���׷��̵� ��ư")]
    public Button enforceButton; // ���׷��̵� ��ư
    public TextMeshProUGUI enforceButtonText; // ���׷��̵� ��ư �ؽ�Ʈ

    private Dictionary<ItemEnName, List<int>> matDic; //��� ���� ����
    private ShelterUpgrade shelterUpgrade; // ShelterUpgrade ������Ʈ
    private static bool m_isUICloseButtonPressed;

    private void Start()
    {
        enforceButton.onClick.AddListener(OnClickUpgrade);
        Hide(); //���۽� ����
    }

    private void Update()
    {
        if (!m_isUICloseButtonPressed) return;
        m_isUICloseButtonPressed = false;

        Hide();
    }

    //UI����
    public void Show(Dictionary<ItemEnName, List<int>> mat, ShelterUpgrade upgrade)
    {
        matDic = mat;
        shelterUpgrade = upgrade;

        shelterCanvas.SetActive(true); // Shelter UI ĵ���� Ȱ��ȭ
        RefreshSlots(); //��� ǥ��
        SetTexts(); //��� �ؽ�Ʈ ����
    }

    //UI �ݱ�
    public void Hide()
    {
        shelterCanvas.SetActive(false); // Shelter UI ĵ���� ��Ȱ��ȭ
        timeText.text = "5 seconds"; //�ð� �ʱ�ȭ
    }

    //��� �ؽ�Ʈ ����
    private void SetTexts()
    {
        int level = shelterUpgrade.GetCurrentLevel();

        descriptionText.text = $"���� ���� {level}\n"
                               + $"���� ������: {shelterUpgrade.GetCurrentDurability()} / {shelterUpgrade.GetMaxDurability()}\n\n\n";

        int nextLevelId = GetNextLevelId(level);
        if (nextLevelId == -1) return;

        int nextMaxDurability = GetNextLevelMaxDurability(nextLevelId);
        int afterDurability = nextMaxDurability + shelterUpgrade.GetCurrentDurability() - shelterUpgrade.GetMaxDurability();
        descriptionText.text += $"���� ���� {level + 1}\n"
                                + $"���� ������: {afterDurability} / {nextMaxDurability}\n";
        // ���׷��̵� ���� �ؽ�Ʈ ����
    }

    private int GetNextLevelId(int levelId)
    {
        int nextLevel = shelterUpgrade.GetCurrentLevel() + 1;
        if (GameManager.Instance.Shelter.ShelterLevelToId.ContainsKey(nextLevel))
            return GameManager.Instance.Shelter.ShelterLevelToId[nextLevel];
        return -1;
    }

    private int GetNextLevelMaxDurability(int levelId) =>
        GameManager.Instance.Shelter.ShelterLevelData[levelId].ShelterDurability;

    private void RefreshSlots()
    {
        bool canUpgrade = true; // ���׷��̵� ���� ���� �ʱ�ȭ
        int i = 0;

        foreach (var keyValue in matDic)
        {
            if (i >= slots.Count)
            {
                break;
            }
            var enName = keyValue.Key;
            int have = keyValue.Value[0]; // ���� ��� ������
            int need = keyValue.Value[1]; // ���׷��̵忡 �ʿ��� ��ᷮ

            var itemData = GameManager.Instance.Item.ItemEnDataSO[enName];

            slots[i].Set(itemData.Icon, itemData.ItemData.ItemName, have, need); // ���Կ� ��� ���� ����
            slots[i].gameObject.SetActive(true);
            if (have < need) // �������� �ʿ��� �纸�� ������ ���׷��̵� �Ұ���
            {
                canUpgrade = false;
            }
            i++;
        }
        enforceButton.interactable = canUpgrade; // ���׷��̵� ��ư Ȱ��ȭ/��Ȱ��ȭ
    }

    //��ư Ŭ��
    private void OnClickUpgrade()
    {
        enforceButton.interactable = false; // ��ư ��Ȱ��ȭ
        enforceButtonText.text = "���׷��̵� ��..."; // ��ư �ؽ�Ʈ ����
        StartCoroutine(UpgradeCoroutine()); // ���׷��̵� �ڷ�ƾ ����
    }
    //5���� ��ȭ �Ϸ�
    private IEnumerator UpgradeCoroutine()
    {
        float wait = 5f; // ��� �ð� 5��
        while (wait > 0f)
        {
            timeText.text = $"{wait:F0} seconds"; // ���� �ð� ǥ��
            yield return new WaitForSeconds(1f); // 1�� ���
            wait -= 1f; // ���� �ð� ����
        }

        shelterUpgrade.ProgressUpgrade(); // ���׷��̵� ����
        enforceButtonText.text = "���׷��̵� �Ϸ�"; // ��ư �ؽ�Ʈ ����
        yield return new WaitForSeconds(1f); // 1�� ���
        enforceButtonText.text = "���׷��̵�"; // ��ư �ؽ�Ʈ �ʱ�ȭ

        GameManager.Instance.SetCursorLock();
        ;
        Hide(); // UI �ݱ�
    }

    public static void OnUICloseKeyPressed() => m_isUICloseButtonPressed = true;
    public static void OnUICloseKeyReleased() => m_isUICloseButtonPressed = false;
}