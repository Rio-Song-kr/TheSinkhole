using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Image m_image;

    private void OnEnable() => Inventory.OnDynamicDisplayRequest += (inventor, isOpened) => OnUIStateChanged(isOpened);
    private void OnDisable() => Inventory.OnDynamicDisplayRequest -= (inventor, isOpened) => OnUIStateChanged(isOpened);

    private void OnUIStateChanged(bool isOpened) => m_image.gameObject.SetActive(!isOpened);
}