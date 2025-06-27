using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Building buildingPrefab;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 포인터 위치를 감지
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                // 충돌된 지점의 컴포넌트를 tile 에 저장
                GridBuildingSystem tile = hitInfo.collider.GetComponent<GridBuildingSystem>();

                // 타일의 타입이 White라면 빌드 실행
                if (tile.type == TileType.White)
                {
                    tile.Build(buildingPrefab);
                }
                else
                {
                    Debug.Log("이미 설치 되어있음");
                }
            }
        }
    }
}
