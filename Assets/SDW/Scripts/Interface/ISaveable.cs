public interface ISaveable
{
    string GetUniqueID();
    object GetSaveData();
    void LoadSaveDta(object data);
}