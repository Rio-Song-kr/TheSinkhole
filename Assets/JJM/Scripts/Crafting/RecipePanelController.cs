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

        [Header("RecipeSelectButton 연결")]
        public GameObject recipeButtonPrefab; // 레시피 버튼 프리팹

        [Header("재료 스크롤뷰 Content")]
        public Transform ingredientContentParent; // Content 오브젝트

        [Header("슬롯 프리팹")]
        public GameObject ingredientSlotPrefab; // 재료 한 칸 프리팹

        [Header("연동")]
        public CraftingManager craftingManager;

        private CraftingRecipe currentRecipe;
        private List<IngredientSlotUI> m_ingredientLists = new List<IngredientSlotUI>();

        public static bool NeedUpdateUI = false;

        private void Update()
        {
            if (NeedUpdateUI != true) return;

            NeedUpdateUI = false;

            var inventory = craftingManager.playerInventory;
            var ing = currentRecipe.ingredients;

            bool allEnough = true;
            bool allExact = true;

            for (int i = 0; i < ing.Count; i++)
            {
                allEnough = CheckEnough(ing[i], allEnough, ref allExact, i);
            }

            CanCrafting(currentRecipe, inventory, allEnough, allExact);
        }

        public void CloseRecipeUI() => gameObject.SetActive(false);

        // 레시피 정보로 UI 갱신
        public void SetRecipe(CraftingRecipe recipe)
        {
            // 1. UI 표시
            gameObject.SetActive(true);

            currentRecipe = recipe;

            // 결과 아이템 정보 표시
            resultItemImage.sprite = recipe.result.item.Icon;
            resultItemName.text = recipe.result.item.ItemData.ItemName;
            itemDescription.text = recipe.result.item.ItemText;

            // 재료 텍스트(간단)
            ingredientText.text = "필요 재료";

            // 기존 재료 슬롯 삭제
            foreach (Transform child in ingredientContentParent)
            {
                Destroy(child.gameObject);
            }

            bool allEnough = true;
            bool allExact = true;

            foreach (var ing in recipe.ingredients)
            {
                allEnough = CheckEnough(ing, allEnough, ref allExact);
            }

            CanCrafting(recipe, craftingManager.playerInventory, allEnough, allExact);


            // 버튼 텍스트
            craftingButtonText.text = "Crafting";

            // 버튼 이벤트 연결
            craftingButton.onClick.RemoveAllListeners();
            craftingButton.onClick.AddListener(() => { craftingManager.TryCraftWithDelay(recipe); });
        }

        private void CanCrafting(CraftingRecipe recipe, Inventory inventory, bool allEnough, bool allExact)
        {
            // 결과 아이템이 인벤토리에 이미 있는지 확인
            bool hasResultItem = inventory.GetItemAmounts(recipe.result.item.ItemEnName) > 0;

            // 인벤토리에 빈 슬롯이 있는지 확인
            bool hasEmptySlot = inventory.GetRemainingSlots() > 0;

            // 최종 제작 가능 조건
            bool canCraft;
            if (hasEmptySlot || hasResultItem)
            {
                // 기존 로직: 재료만 충분하면 제작 가능
                canCraft = allEnough;
            }
            else
            {
                // 빈 슬롯도 없고, 결과 아이템도 없음
                // 재료가 "정확히 필요량만큼" 있을 때만 제작 가능
                canCraft = allExact;
            }
            craftingButton.interactable = canCraft && !craftingManager.IsCrafting;
        }

        private bool CheckEnough(CraftingItemInfo ing, bool allEnough, ref bool allExact, int index = -1)
        {
            var inventory = craftingManager.playerInventory;
            int owned = inventory.GetItemAmounts(ing.item.ItemEnName);

            // 재료 슬롯 UI 생성 및 색상 처리
            IngredientSlotUI slotUI;
            if (index == -1)
            {
                var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);

                m_ingredientLists.Add(go.GetComponent<IngredientSlotUI>());

                slotUI = go.GetComponent<IngredientSlotUI>();
            }
            else
            {
                slotUI = m_ingredientLists[index];
            }
            if (slotUI != null)
            {
                // 보유 == 필요만 흰색, 그 외(작거나 많으면) 빨간색
                slotUI.Set(ing.item, ing.count, owned);
            }

            if (owned < ing.count)
                allEnough = false;
            if (owned != ing.count)
                allExact = false;
            return allEnough;
        }

        public IEnumerator CraftingButtonCountdownCoroutine(float time)
        {
            string originalText = craftingButtonText.text; // 기존 텍스트 저장

            int seconds = Mathf.CeilToInt(time);
            for (int i = seconds; i > 0; i--)
            {
                craftingButtonText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            craftingButtonText.text = "제작완료";
            yield return new WaitForSeconds(1f);

            craftingButtonText.text = originalText; // 원래 텍스트로 복구
            NeedUpdateUI = true;
        }

        public CraftingRecipe GetCurrentRecipe() => currentRecipe;
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}