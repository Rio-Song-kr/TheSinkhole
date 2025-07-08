using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Turret/TurretSo")]
public class TurretSo : ScriptableObject
{
    //Detail
    public GameObject turretPrefab;
    public Sprite TurretImg; // 터렛 이미지
    public string TurretName; //터렛 이름
    public string TurretDesc; // 터렛 설명
    public float buildingTime;//제작 준비 시간
    public RequireItemData[] RequireItems;

    //터렛 능력치 관련 세팅
    public float distance; //사거리
    public int Atk; //공격력
}
