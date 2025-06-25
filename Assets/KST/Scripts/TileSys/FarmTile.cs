using UnityEngine;

public class FarmTile : MonoBehaviour, IToolInteractable
{
    [Header("Status")]
    [SerializeField]private bool m_isPlanted; //심어져 있으면 true, 아니면 false
    [SerializeField]private bool m_isPlayerOnFarmTile; //팜타일에 플레이어가 있으면 true, 없으면 false
    public bool IsPlanted() => m_isPlanted;
    public bool IsPlayerOnTile() => m_isPlayerOnFarmTile; 

    [Header("UI")]
    //상호작용 UI
    public GameObject InteractUiText;
    //농사창 UI
    public GameObject FarmUIObj;

    [SerializeField]private CropDataSO growingCrop;
    public CropDataSO GetGrownCrop() => growingCrop;

    // 개척지가 있고, 괭이를 통해 농사지을 수 있고, 시간이 지나면 수확 알아서 가능하도록. 씨뿌리기 x, 타일 설치하고 일정시간 지나면 알아서 수확만 가능하도록.
    // void Start()
    // {
    //     m_isPlanted = true;

    // }

    #region 상호작용 인터페이스 구현
    public interactType GetInteractType()
    {
        return interactType.PressE;
    }

    public bool CanInteract(ToolType toolType)
    {
        return m_isPlayerOnFarmTile && !m_isPlanted && toolType == ToolType.Shovel;
    }
    public void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None || m_isPlanted) return;

        FarmUIObj.SetActive(true);
        FarmUI.Instance.SetTile(this);
        InteractUiText.SetActive(false);

    }
    #endregion

    //심기 메서드
    public void StartPlanting(CropDataSO crop)
    {
        m_isPlanted = true;
        growingCrop = crop;
    }

    #region 충돌처리

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnFarmTile = true;
            InteractUiText.SetActive(true);
            Interaction player = other.GetComponent<Interaction>();
            player?.RegisterTrigger(this);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnFarmTile = false;
            InteractUiText.SetActive(false);
            Interaction player = other.GetComponent<Interaction>();
            player?.ClearTrigger(this);
        }

    }
    #endregion

}
