using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeableObject : MonoBehaviour
{
    public int level = 1;
    public int helth = 300;

    public void Upgrade()
    {
        if (level < 3 )
        {
            level++;
            helth = helth + 300;

            Debug.Log("���׷��̵� �Ϸ�! ���� ����: " + level);
        }
    }
}
