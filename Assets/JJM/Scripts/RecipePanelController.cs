using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class RecipePanelController : MonoBehaviour
    {
        [Header("UI 연결")]
        public Image resultItemImage;
        public TMP_Text resultItemName;
        // 재료 슬롯 생성 및 세팅 예시
        
        public TMP_Text itemDescription;
        public TMP_Text ingredientText;
        public Button craftingButton;
        public TMP_Text craftingButtonText;

        [Header("재료 스크롤뷰 Content")]
        public Transform ingredientContentParent; // Content 오브젝트

        [Header("슬롯 프리팹")]
        public GameObject ingredientSlotPrefab; // 재료 한 칸 프리팹

        [Header("연동")]
        public CraftingManager craftingManager;

        private CraftingRecipe currentRecipe;

        // 레시피 정보로 UI 갱신
        public void SetRecipe(CraftingRecipe recipe)
        {
            currentRecipe = recipe;

            // 결과 아이템 정보 표시
            resultItemImage.sprite = recipe.result.item.Icon;
            resultItemName.text = recipe.result.item.name;
            itemDescription.text = recipe.result.item.ItemText;

            // 재료 텍스트(간단)
            ingredientText.text = "필요 재료";

            // 기존 재료 슬롯 삭제
            foreach (Transform child in ingredientContentParent)
                Destroy(child.gameObject);

            // 재료 슬롯 생성
            foreach (var ing in recipe.ingredients)
            {
                var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);
                var slot = go.GetComponent<IngredientSlotUI>();
                if (slot != null)
                    slot.Set(ing.item, ing.count);
            }

            // 버튼 텍스트
            craftingButtonText.text = "Crafting";

            // 버튼 이벤트 연결
            craftingButton.onClick.RemoveAllListeners();
            craftingButton.onClick.AddListener(() =>
            {
                craftingManager.TryCraftWithDelay(recipe);
            });
        }
    }
}
