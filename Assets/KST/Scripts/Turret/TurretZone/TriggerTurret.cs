using System;
using System.Collections;
using System.Collections.Generic;
using Test;
using TMPro;
using UnityEngine;


namespace Test
{
    public class TriggerTurret : MonoBehaviour
    {

        //타워 프리펩
        [SerializeField] private GameObject towerPrefab;

        // 생성된 타워
        private GameObject builtTower;
        [SerializeField] private Turret tower;



        //플레이어가 콜라이더 내부에 있을 때
        [SerializeField] private bool isPlayerInside;
        [SerializeField] private bool CanBuildTower = true;

        //머터리얼

        [SerializeField] private Renderer triggerRenderer;
        [SerializeField] private Material visibleMaterial;
        [SerializeField] private Material invisibleMaterial;

        // PlayerTest player;

        public bool CanBuild() => CanBuildTower;

        void Update()
        {
            if (Input.GetKey(KeyCode.F) && isPlayerInside)
            {
                Debug.Log("F키 눌림");
                // TowerBuildUI.InvokeClose();
                TurretZoneEvent.InvokeInteract(this);
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
            if (!CanBuildTower) return;

            builtTower = Instantiate(towerPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            tower = builtTower.GetComponent<Turret>();
            tower.towerZone = this;

            CanBuildTower = false;
            //투명색 머터리얼로 변경.
            triggerRenderer.material = invisibleMaterial;

            // TowerZoneEvent.OnTowerExit?.Invoke();
            TurretZoneEvent.InvokeExit();

        }

        public void SellTower()
        {
            if (builtTower != null)
            {
                Destroy(builtTower);
                builtTower = null;
                tower = null;
                CanBuildTower = true;

                //미설치 머터리얼로 변경.
                triggerRenderer.material = visibleMaterial;
            }
        }
        public void TowerDestoried()
        {
            builtTower = null;
            tower = null;
            CanBuildTower = true;

            //미설치 머터리얼로 변경.
            triggerRenderer.material = visibleMaterial;
        }
        
        public void UpgradeTower()
        {
            if (builtTower != null)
            {
                if (tower.Level.Value == tower.MaxLevel.Value)
                {
                    Debug.Log("타워가 최대 레벨입니다.");
                    return;
                }
                tower.LevelUp();
            }
        }



    }
}