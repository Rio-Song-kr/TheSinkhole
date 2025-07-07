using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelter : MonoBehaviour, IDamageable
{
    public int LevelId;
    public int Level;
    public int Durability;
    public int MaxDurability;
    public string ShelterName;

    public Image durabilityImage;
    // public Shelter shelterObject; //���� ������Ʈ

    private void Start()
    {
        LevelId = GameManager.Instance.Shelter.ShelterLevelToId[Level];
        Durability = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterDurability;
        MaxDurability = Durability;
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterName;
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
        MaxDurability = GameManager.Instance.Shelter.ShelterLevelData[Level].ShelterDurability;
        Durability = Durability + (MaxDurability - currentMaxDurability);
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[Level].ShelterName;
    }

    public void ColorChange()
    {
        if (Durability >= MaxDurability * 0.7)
        {
            durabilityImage.GetComponent<Renderer>().material.color = Color.green; //���
        }
        else if (Durability <= MaxDurability * 0.7 && Durability >= MaxDurability * 0.4)
        {
            durabilityImage.GetComponent<Renderer>().material.color = Color.yellow; //�����
        }
        else if (Durability < MaxDurability * 0.4)
        {
            durabilityImage.GetComponent<Renderer>().material.color = Color.red; //������
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
}