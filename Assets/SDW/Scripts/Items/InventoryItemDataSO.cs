using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory Item")]
public class InventoryItemDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public ItemType Type;
    public int ID;
    public string DisplayName;
    [TextArea(4, 4)] public string Description;

    [Header("Visual")]
    public Sprite Icon;

    [Header("Stack Settings")]
    public int MaxItemCount;

    //# UI에서 IsStackable이 false면 아이템 아이콘에 숫자를 표현하지 않음
    public bool IsStackable => MaxItemCount > 1;

    //# 에디터에서 유효성 검사
    private void OnValidate()
    {
        if (MaxItemCount < 1) MaxItemCount = 1;
        if (string.IsNullOrEmpty(DisplayName)) DisplayName = name;
    }
}