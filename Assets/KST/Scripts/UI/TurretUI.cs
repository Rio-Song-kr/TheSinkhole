using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : Singleton<TurretUI>
{
    [Header("Turret")]
    [SerializeField] private TurretSo selectedTurret;
    public TurretSo[] TurretList;

    [Header("UI")]
    public GameObject TurretUIGO;
    public bool GetActiveself() => TurretUIGO.activeSelf;

    //터렛 Detail
    public GameObject DetailGO;
    [SerializeField] private Image turretImg;
    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text turretDesc;
    [SerializeField] private TMP_Text turret;
    

    [SerializeField] private TMP_Text m_statusText; //상태 메세지
    public Image PrograssBarImg;
    public Button[] turretButtons;

    private float pressTimer = 0f;
    private float pressDuration = 5f;
    private bool isPressingE = false;
    //스크롤뷰
    [SerializeField] private GameObject turretBtnPrefab;
    [SerializeField] private Transform scrollViewContentPos; //스크롤뷰 컨텐츠 위치

    //오픈 관련 이벤트 처리
    public event Action<bool> OnIsUIOpen;


    [Header("Tile")]
    [SerializeField] private TurretTile currentTile;
    [SerializeField] private float builtTimer = 0f;


    void Start()
    {
        ScrollViewSetting();
        m_statusText.text = "";
        PrograssBarImg.fillAmount = 0f;
    }

    void Update()
    {
        if (currentTile == null) return;

        if (currentTile.IsBuild())
        {
            var buildingTr = currentTile.GetBuiltTurret();

            if (buildingTr == selectedTurret)
            {
                builtTimer -= Time.deltaTime;
                m_statusText.text = $"제작중 : {builtTimer}";
                if (builtTimer < 0f)
                {
                    builtTimer = 0f;
                    m_statusText.text = $"제작 완료! 설치하려면 [E]키를 누르세요";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Build();
                    }
                }

            }
            else
            {
                m_statusText.text = "설치할 수 없습니다.";
            }
            return;
        }
        if (selectedTurret == null) return;

        //설치 가능한 상태
        if (Input.GetKey(KeyCode.E))
        {
            isPressingE = true;
            pressTimer += Time.deltaTime;
            PrograssBarImg.fillAmount = pressTimer / pressDuration;
            m_statusText.text = $"제작 준비 중. . .";


            if (pressTimer >= pressDuration)
            {
                StartBuilding(selectedTurret);
            }
        }
        else if (pressTimer > 0f)
        {
            CancelBuilding();
        }
    }
    void CancelBuilding()
    {
        //상태 전부 초기화

        pressTimer = 0f;
        PrograssBarImg.fillAmount = 0f;
        isPressingE = false;
        m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";
    }
    public void SetTile(TurretTile tile)
    {
        currentTile = tile;
        //해당 타일이 이미 재배중이라면 해당 작물 표시
        if (tile.IsBuild())
        {
            DisplayTurretDetail(tile.GetBuiltTurret());
        }
        else
        {
            if (selectedTurret != null)
            {
                DisplayTurretDetail(selectedTurret);
                m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

                PrograssBarImg.fillAmount = 0f;
            }
        }
    }

    public void OpenUI()
    {
        TurretUIGO.SetActive(true);
        OnIsUIOpen?.Invoke(true);
        if (selectedTurret == null)
        {
            m_statusText.text = "";
            PrograssBarImg.fillAmount = 0f;
            DetailGO.SetActive(false);
            return;
        }

        DisplayTurretDetail(selectedTurret);


        if (currentTile != null && !currentTile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

            PrograssBarImg.fillAmount = 0f;
        }
    }
    public void CloseUI()
    {
        TurretUIGO.SetActive(false);
        OnIsUIOpen?.Invoke(false);
    }
    public void SelectTurret(TurretSo crop)
    {
        //설치 돼있다면 다른 터렛은 설치하지 못하도록
        if (currentTile != null && currentTile.IsBuild())
        {
            if (currentTile.GetBuiltTurret() != crop) return;
        }
        selectedTurret = crop;
        DisplayTurretDetail(crop);

        if (currentTile != null && !currentTile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

            PrograssBarImg.fillAmount = 0f;
        }
    }
    private void StartBuilding(TurretSo so)
    {
        currentTile.StartBuiltTurret(so);
        builtTimer = so.buildingTime;

        m_statusText.text = $"제작중 {builtTimer}";
        PrograssBarImg.fillAmount = 1f;
        foreach (var btn in turretButtons)
        {
            btn.interactable = false;
        }
    }
    public void DisplayTurretDetail(TurretSo data)
    {
        DetailGO.SetActive(true);

        turretImg.sprite = data.TurretImg;
        turretName.text = data.TurretName;
        turretDesc.text = $"타워 상세정보 : {data.TurretDesc}초 \n 공격력: {data.Atk} \n 사거리 : {data.distance} %";

    }
    public void ScrollViewSetting()
    {
        List<Button> btnList = new();
        foreach (Transform child in scrollViewContentPos)
        {
            Destroy(child.gameObject);
        }
        foreach (var turret in TurretList)
        {
            GameObject go = Instantiate(turretBtnPrefab, scrollViewContentPos);
            TurretBtn btn = go.GetComponent<TurretBtn>();
            btn.Init(turret);
            btnList.Add(btn.Btn);
        }
        turretButtons = btnList.ToArray();
    }

    //수확
    private void Build()
    {
        Instantiate(selectedTurret.turretPrefab, currentTile.transform.position + Vector3.up * 0.5f, Quaternion.identity);

        //초기화
        builtTimer = 0f;
        PrograssBarImg.fillAmount = 0f;
        m_statusText.text = $"";

        // foreach (var btn in turretButtons)
        // {
        //     btn.interactable = true;
        // }
    }

    
}