using UnityEngine;


public class TestTurretTile : MonoBehaviour
{

    [Header("UI")]
    //상호작용 UI
    public GameObject InteractUiText;

    //타워 프리펩
    [SerializeField] private GameObject turretPrefab;

    // 생성된 타워
    private GameObject builtTower;
    [SerializeField] private Turret turret;



    //플레이어가 콜라이더 내부에 있을 때
    [SerializeField] private bool isPlayerInside;
    [SerializeField] private bool CanBuildTurret = true;

    //머터리얼

    // PlayerTest player;

    public bool CanBuild() => CanBuildTurret;

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && isPlayerInside)
        {
            Debug.Log("F키 눌림");
            // TowerBuildUI.InvokeClose();
            // TurretZoneEvent.InvokeInteract(this);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("트리거 진입.");
            // PlayerModel player = collision.GetComponent<PlayerModel>();
            // player.IsCanInteract = true;
            isPlayerInside = true;
            // TowerBuildUI.InvokeOpen();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("트리거 탈출.");
            // PlayerTest player = collision.GetComponent<PlayerTest>();
            //TODO: 플레이어 상태 작성하는 코드에서 iscanInteract상태 받기
            // PlayerModel player = collision.GetComponent<PlayerModel>();
            // player.IsCanInteract = false;
            isPlayerInside = false;

            // TowerBuildUI.OnTextInteractClose?.Invoke();
            // TowerBuildUI.InvokeClose();
            // TowerZoneEvent.OnTowerExit?.Invoke();
            TurretZoneEvent.InvokeExit();
        }
    }


    public void BuildTower()
    {
        if (!CanBuildTurret) return;

        builtTower = Instantiate(turretPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        turret = builtTower.GetComponent<Turret>();
        // tower.towerZone = this;

        CanBuildTurret = false;
        // TowerZoneEvent.OnTowerExit?.Invoke();
        TurretZoneEvent.InvokeExit();

    }

    public void SellTower()
    {
        if (builtTower != null)
        {
            Destroy(builtTower);
            builtTower = null;
            turret = null;
            CanBuildTurret = true;

        }
    }
    public void TowerDestoried()
    {
        builtTower = null;
        turret = null;
        CanBuildTurret = true;

    }
    
}