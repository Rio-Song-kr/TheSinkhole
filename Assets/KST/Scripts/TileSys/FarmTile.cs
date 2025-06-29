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
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;
    private bool m_isInteract;
    //농사창 UI
    // public GameObject FarmUIObj;

    [SerializeField] private CropDataSO growingCrop;
    public CropDataSO GetGrownCrop() => growingCrop;

    // 개척지가 있고, 괭이를 통해 농사지을 수 있고, 시간이 지나면 수확 알아서 가능하도록. 씨뿌리기 x, 타일 설치하고 일정시간 지나면 알아서 수확만 가능하도록.
    // void Start()
    // {
    //     m_isPlanted = true;

    // }
    void Awake() => m_awakeTime = Time.time;
        
    void OnEnable()
    {
        FarmUI.Instance.OnIsUIOpen +=SetInteraction;
    }
    void OnDisable()
    {
        FarmUI.Instance.OnIsUIOpen -=SetInteraction;
    }
    
    #region 상호작용 인터페이스 구현
    public interactType GetInteractType()
    {
        return interactType.PressE;
    }

    public bool CanInteract(ToolType toolType)
    {
        // return m_isPlayerOnFarmTile && !m_isPlanted && toolType == ToolType.Shovel;
        return m_isPlayerOnFarmTile && toolType == ToolType.Shovel;
    }
    public void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None) return;

        // FarmUIObj.SetActive(true);
        FarmUI.Instance.OpenUI();
        // m_isInteract = false;
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
    //수확 메서드
    public void HarvestingCrop()
    {
        m_isPlanted = false;
        growingCrop = null;
        
    }

    void SetInteraction(bool _status)
    {
        InteractUiText.SetActive(_status);
    }

    #region 충돌처리

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - m_awakeTime < m_interactDelay) return;
            //플레이어가 타일위에 있는지
            m_isPlayerOnFarmTile = true;
            // //플레이어가 상호작용을 통해 UI를 오픈했는지.
            // if (!FarmUI.Instance.GetActiveself())
            // {
            //     InteractUiText.SetActive(true);
            // }
            if (!FarmUI.Instance.GetActiveself())
            {
                InteractUiText.SetActive(true);
            }
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
