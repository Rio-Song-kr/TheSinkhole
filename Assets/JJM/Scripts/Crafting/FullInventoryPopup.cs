using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullInventoryPopup : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
