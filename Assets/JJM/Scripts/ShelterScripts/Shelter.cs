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
    // public Shelter shelterObject; //쉘터 오브젝트
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

    //인터페이스 구현
    public void TakenDamage(int damage)
    {
        Durability -= damage;
        Debug.Log("쉘터가 " + damage + "의 피해를 받았습니다. 현재 내구도: " + Durability);

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
            // durabilityImage.GetComponent<Renderer>().material.color = Color.green; //녹색
            durabilityImage.GetComponent<Image>().color = Color.green; //녹색
        }
        else if (Durability <= MaxDurability * 0.7 && Durability >= MaxDurability * 0.4)
        {
            // durabilityImage.GetComponent<Renderer>().material.color = Color.yellow; //노란색
            durabilityImage.GetComponent<Image>().color = Color.yellow; //노란색
        }
        else if (Durability < MaxDurability * 0.4)
        {
            durabilityImage.GetComponent<Image>().color = Color.red; //빨간색
        }
    }

    private void OnShelterDestroyed()
    {
        Debug.Log("쉘터가 파괴되었습니다! 플레이어가 죽었습니다");
        //게임 오버 처리

        //파괴 연출, 오브젝트 비활성화 처리
        //gameObject.SetActive(false);
        GameManager.Instance.SetGameOver();
    }

    public void SetOutline(bool isEnable) => m_outline.enabled = isEnable;
}