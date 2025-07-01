using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UI/DevelopSO")]
public class DevelopSO : ScriptableObject
{
    public string DevelopDesc;
    public float DevelopTime;
    public Sprite[] RequireImg;
    public string[] RequireName;
}
