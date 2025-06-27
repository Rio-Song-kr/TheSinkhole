using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class IngredientSlotUI : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text nameText;
        public TMP_Text countText;

        public void Set(ItemDataSO item, int needCount, int ownedCount)
        {
            Debug.Log($"Set »£√‚: {item?.name}, {ownedCount}/{needCount}");
            iconImage.sprite = item.Icon;
            nameText.text = item.name;
            countText.text = $"{ownedCount}/{needCount}";
        }
    }
}
