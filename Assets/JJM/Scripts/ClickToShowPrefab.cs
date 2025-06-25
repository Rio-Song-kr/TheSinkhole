using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToShowPrefab : MonoBehaviour
{
    public GameObject prefabToShow; // 생성할 프리팹을 지정합니다.

    private void OnMouseDown()
    {
        GameObject instance = Instantiate(prefabToShow, transform.position, Quaternion.identity);
    }
}
