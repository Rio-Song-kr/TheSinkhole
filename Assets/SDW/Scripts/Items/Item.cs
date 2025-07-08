/// <summary>
/// Item Effect를 가진 Item Class
/// JSON/CSV 등 파일로 저장 또는 불러오기 기능을 구현하기 위해 Scriptable Object와 별도 분리하여 구현
/// </summary>
[System.Serializable]
public class Item
{
    public int ItemId;
    public string ItemName;
    public EffectFileData ItemEffect;

    public Item()
    {
        ItemId = -1;
        ItemName = "";
    }
}