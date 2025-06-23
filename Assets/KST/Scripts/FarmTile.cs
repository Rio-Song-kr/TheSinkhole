using UnityEngine;

public class FarmTile : MonoBehaviour, Iinteractable
{
    public GameObject SeedPrefab; //씨앗 프리팹
    private GameObject m_plantedCrop; //작물 프리펩
    private bool m_isPlanted; //심어져 있으면 true, 아니면 false
    public void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None || m_isPlanted) return;

        if (toolType == ToolType.Hoe)
        {
            Debug.Log("작물 심어졌습니다.");
            //우선, 해당 위치에 씨앗프리펩 생성.
            m_plantedCrop = Instantiate(SeedPrefab, transform.position, Quaternion.identity);
            m_plantedCrop.GetComponent<Crop>().StartGrow();
            m_isPlanted = true;
        }
    }

    
}
