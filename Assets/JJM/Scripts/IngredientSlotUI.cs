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

        public void Set(ItemDataSO item, int count)
        {
            iconImage.sprite = item.Icon;
            nameText.text = item.name;
            countText.text = $"x{count}";
        }
    }
}
