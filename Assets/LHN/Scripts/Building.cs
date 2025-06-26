using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    #region Build Methuds

    // ������Ʈ�� ���� ��ġ�� ��ġ�� �� �ִ��� Ȯ��
    public bool CanBePlaced()
    {
        // ������Ʈ�� ���� ��ǥ�� �� ��ǥ�� ��ȯ
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);

        // ���� area ���� �����Ͽ� �ӽ� ��ü ����
        BoundsInt areaTemp = area;

        // �ӽ� ������ ��ġ�� ���� �� ��ǥ�� ����
        areaTemp.position = positionInt;

        // ������ ������ �׸��忡 ��ġ �������� Ȯ��
        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true; // ��ġ ����
        }

        return false; // ��ġ �Ұ���
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        
        // ��ġ�� ���·� ǥ��
        Placed = true;

        // �ش� ��ġ�� ���� ��ġ �Ǿ��ִٱ� ǥ��
        GridBuildingSystem.current.TakeArea(areaTemp);

    }


    #endregion

}
