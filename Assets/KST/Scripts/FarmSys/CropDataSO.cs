using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Farm/CropDataSO")]
public class CropDataSO : ScriptableObject
{
    public Sprite cropImg;
    public string cropName;
    public string cropDesc;
    public float cropEffect;
    public float growTime;

    //아이템
    public ItemDataSO seedItemSo; //작물 재배 시 필요한 아이템
    public ItemDataSO harvestItemSo; // 작물 수확 시 필요한 아이템
}
