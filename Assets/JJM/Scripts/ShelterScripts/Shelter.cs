using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class Shelter : MonoBehaviour, IDamageable
{
    public int LevelId;
    public int Level;
    public int Durability;
    public int MaxDurability;
    public string ShelterName;
    public GameObject WoodFence;
    public GameObject MetalFence;

    public Image durabilityImage;
    // public Shelter shelterObject; //���� ������Ʈ
    private Outlinable m_outline;

    private void Start()
    {
        LevelId = GameManager.Instance.Shelter.ShelterLevelToId[Level];
        Durability = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterDurability;
        MaxDurability = Durability;
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterName;
        m_outline = GetComponent<Outlinable>();
        m_outline.enabled = false;
    }

    private void Update()
    {
        ColorChange();
    }

    //�������̽� ����
    public void TakenDamage(int damage)
    {
        Durability -= damage;
        Debug.Log("���Ͱ� " + damage + "�� ���ظ� �޾ҽ��ϴ�. ���� ������: " + Durability);

        if (Durability <= 0)
        {
            Durability = 0;
            OnShelterDestroyed();
        }
    }

    public void Upgrade()
    {
        int currentMaxDurability = MaxDurability;

        Level++;
        LevelId = GameManager.Instance.Shelter.ShelterLevelToId[Level];
        MaxDurability = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterDurability;
        Durability = Durability + (MaxDurability - currentMaxDurability);
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterName;

        //todo Remove Item
        if (Level == 2)
        {
            WoodFence.SetActive(false);
            MetalFence.SetActive(true);
        }
    }

    public void ColorChange()
    {
        if (Durability >= MaxDurability * 0.7)
        {
            // durabilityImage.GetComponent<Renderer>().material.color = Color.green; //���
            durabilityImage.GetComponent<Image>().color = Color.green; //���
        }
        else if (Durability <= MaxDurability * 0.7 && Durability >= MaxDurability * 0.4)
        {
            // durabilityImage.GetComponent<Renderer>().material.color = Color.yellow; //�����
            durabilityImage.GetComponent<Image>().color = Color.yellow; //�����
        }
        else if (Durability < MaxDurability * 0.4)
        {
            durabilityImage.GetComponent<Image>().color = Color.red; //������
        }
    }

    private void OnShelterDestroyed()
    {
        Debug.Log("���Ͱ� �ı��Ǿ����ϴ�! �÷��̾ �׾����ϴ�");
        //���� ���� ó��

        //�ı� ����, ������Ʈ ��Ȱ��ȭ ó��
        //gameObject.SetActive(false);
        GameManager.Instance.SetGameOver();
    }

    public void SetOutline(bool isEnable) => m_outline.enabled = isEnable;
}