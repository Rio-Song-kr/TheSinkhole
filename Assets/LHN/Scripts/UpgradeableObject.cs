using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeableObject : MonoBehaviour
{
    public int LevelId;
    public int Level;
    public int Durability;
    public int MaxDurability;
    public string ShelterName;

    private void Start()
    {
        LevelId = GameManager.Instance.Shelter.ShelterLevelToId[Level];
        Durability = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterDurability;
        MaxDurability = Durability;
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[LevelId].ShelterName;
    }

    public void Upgrade()
    {
        int currentMaxDurability = MaxDurability;

        Level++;
        MaxDurability = GameManager.Instance.Shelter.ShelterLevelData[Level].ShelterDurability;
        Durability = Durability + (MaxDurability - currentMaxDurability);
        ShelterName = GameManager.Instance.Shelter.ShelterLevelData[Level].ShelterName;
    }
}