using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToShowPrefab : MonoBehaviour
{
    public GameObject prefabToShow; // ������ �������� �����մϴ�.

    private void OnMouseDown()
    {
        GameObject instance = Instantiate(prefabToShow, transform.position, Quaternion.identity);
    }
}
