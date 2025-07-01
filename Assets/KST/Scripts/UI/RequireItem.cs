using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    public void Set(Sprite sprite, string name)
    {
        icon.sprite = sprite;
        itemName.text = name;
    }
}