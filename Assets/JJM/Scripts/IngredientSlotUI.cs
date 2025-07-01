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
            Debug.Log($"Set 호출: {item?.name}, {ownedCount}/{needCount}");
            iconImage.sprite = item.Icon;
            nameText.text = item.name;
            countText.text = $"{ownedCount}/{needCount}";

            // 색상 변경: 부족하면 빨간색, 충분하면 흰색
            if (ownedCount < needCount)
                countText.color = Color.red;
            else
                countText.color = Color.white;
        }
    }
}
