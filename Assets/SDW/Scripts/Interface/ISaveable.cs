/// <summary>
/// save/Load 기능을 위한 인터페이스
/// </summary>
public interface ISaveable
{
    string GetUniqueID();
    object GetSaveData();
    void LoadSaveDta(object data);
}