using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterUpgradeUI : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject shelterCanvas;// Shelter UI ĵ����
    public TextMeshProUGUI timeText;// ���׷��̵� �ð� �ؽ�Ʈ
    public TextMeshProUGUI descriptionText;// ���׷��̵� ���� �ؽ�Ʈ

    [Header("��� ���� UI")]
    public List<IngreSlot> slots; //ReqIngre0~3����;

    [Header("���׷��̵� ��ư")]
    public Button enforceButton; // ���׷��̵� ��ư
    public TextMeshProUGUI enforceButtonText; // ���׷��̵� ��ư �ؽ�Ʈ

    private Dictionary<ItemEnName, List<int>> matDic;//��� ���� ����
    private ShelterUpgrade shelterUpgrade;// ShelterUpgrade ������Ʈ

    private void Start()
    {
        enforceButton.onClick.AddListener(OnClickUpgrade);
        Hide();//���۽� ����
    }

    //UI����
    public void Show(Dictionary<ItemEnName,List<int>> mat, ShelterUpgrade upgrade)
    {
        matDic = mat;
        shelterUpgrade = upgrade;

        shelterCanvas.SetActive(true);// Shelter UI ĵ���� Ȱ��ȭ
        RefreshSlots();//��� ǥ��
        SetTexts(); //��� �ؽ�Ʈ ����

    }

    //UI �ݱ�
    public void Hide()
    {
        shelterCanvas.SetActive(false);// Shelter UI ĵ���� ��Ȱ��ȭ
        timeText.text = "5 seconds";//�ð� �ʱ�ȭ
    }

    //��� �ؽ�Ʈ ����
    private void SetTexts()
    {
        int level = shelterUpgrade.GetCurrentLevel();
        
        descriptionText.text = $"���� ���� {level}\n"
                      + $"���� ������: {shelterUpgrade.GetCurrentDurability()} / {shelterUpgrade.GetMaxDurability()}\n"
                      + $"���� ���� {level + 1}\n"
                      + $"���� ������: {shelterUpgrade.GetCurrentDurability() + 1000} / {shelterUpgrade.GetMaxDurability() + 1000}\n"; 
                        // ���׷��̵� ���� �ؽ�Ʈ ����
    }
    
    
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
            var name = keyValue.Key.ToString();
            var have = keyValue.Value[0]; // ���� ��� ������
            var need = keyValue.Value[1]; // ���׷��̵忡 �ʿ��� ��ᷮ

            slots[i].Set(name, have, need); // ���Կ� ��� ���� ����
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

        Hide();// UI �ݱ�
        
    }

}
