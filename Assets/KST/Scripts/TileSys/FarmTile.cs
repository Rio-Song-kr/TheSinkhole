using UnityEngine;

public class FarmTile : MonoBehaviour, Iinteractable
{
    private GameObject m_plantedCrop; //작물 프리펩
    private bool m_isPlanted; //심어져 있으면 true, 아니면 false
    private bool m_isPlayerOnFarmTile; //팜타일에 플레이어가 있으면 true, 없으면 false

    //UI
    //상호작용 UI
    public GameObject InteractUiText;
    //농사창 UI
    public GameObject FarmUI;

    // 개척지가 있고, 괭이를 통해 농사지을 수 있고, 시간이 지나면 수확 알아서 가능하도록. 씨뿌리기 x, 타일 설치하고 일정시간 지나면 알아서 수확만 가능하도록.
    void Start()
    {
        m_isPlanted = true;

    }
    public void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None || m_isPlanted) return;

        if (toolType == ToolType.Shovel)
        {
            // Debug.Log("작물 심어졌습니다.");
            // //우선, 해당 위치에 씨앗프리펩 생성.
            // m_plantedCrop.GetComponent<Crop>().StartGrow();
            // m_isPlanted = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnFarmTile = true;
            TestPlayerTool player = GetComponent<TestPlayerTool>();
            if (player.currentToolType == ToolType.Shovel && m_isPlayerOnFarmTile)
            {
                InteractUiText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    FarmUI.SetActive(true);
                    InteractUiText.SetActive(false);
                }
            }
            else
            {
                InteractUiText.SetActive(false);
                FarmUI.SetActive(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnFarmTile = false;
            InteractUiText.SetActive(false);

        }

    }


}
