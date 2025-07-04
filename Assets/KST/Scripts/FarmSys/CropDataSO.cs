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
    //런타임시 itemDataSo가 생성되기에 다른 방식으로 접근이 필요할듯 함.

    public ItemEnName seedItemSo; //작물 재배 시 필요한 아이템 주석처리 필요
    public RequireItemData[] RequireItems; //작물 재배 시 필요한 아이템 
    public ItemEnName harvestItemSo; // 작물 수확 시(결과물) 아이템
    public int harvestItemAmounts;//수확 아이템 개수
}
