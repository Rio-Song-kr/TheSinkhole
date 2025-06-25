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
}
