using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    #region Build Methuds

    // 오브젝트를 현재 위치에 배치할 수 있는지 확인
    public bool CanBePlaced()
    {
        // 오브젝트의 월드 좌표를 셀 좌표로 변환
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);

        // 기존 area 값을 복사하여 임시 객체 생성
        BoundsInt areaTemp = area;

        // 임시 영역의 위치를 현재 셀 좌표로 설정
        areaTemp.position = positionInt;

        // 지정된 영역이 그리드에 배치 가능한지 확인
        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true; // 배치 가능
        }

        return false; // 배치 불가능
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        
        // 배치된 상태로 표시
        Placed = true;

        // 해당 위치의 셀에 설치 되어있다교 표시
        GridBuildingSystem.current.TakeArea(areaTemp);

    }


    #endregion

}
