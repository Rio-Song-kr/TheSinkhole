using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public static class CraftingHelper
    {
        // 제작 가능 여부 확인
        public static bool CanCraft(CraftingRecipe recipe, Inventory playerInventory)
        {
            //인벤토리 시스템 참조
            var inventory = playerInventory;

            // 재료 차감
            foreach (var ingredient in recipe.ingredients)
            {
                int remaining = ingredient.count; //남은 차감량
                //재료 아이템이 있는 인벤토리슬롯찾기

                int owned = inventory.GetItemAmounts(ingredient.item.ItemEnName);

                if (owned < ingredient.count)
                    return false;
            }

            //재료가 충분할때 제작
            return true;
        }

        // 실제 제작 실행
        public static bool Craft(CraftingRecipe recipe, Inventory playerInventory)
        {
            var inventory = playerInventory;

            //재료가 충분한지 최종으로 확인
            if (!CanCraft(recipe, playerInventory))
            {
                Debug.Log("재료가 부족합니다.");
                return false;
            }

            // 재료 차감
            foreach (var ingredient in recipe.ingredients)
            {
                int remaining = ingredient.count; //남은 차감량
                //재료 아이템이 있는 인벤토리슬롯찾기

                int owned = inventory.GetItemAmounts(ingredient.item.ItemEnName);

                if (inventory.RemoveItemAmounts(ingredient.item.ItemEnName, ingredient.count))
                {
                    RecipePanelController.NeedUpdateUI = true;
                }
            }

            // 결과 지급
            playerInventory.AddItemSmart(recipe.result.item, recipe.result.count);
            //inventorySystem.AddItem(recipe.result.item, recipe.result.count);
            Debug.Log("제작 완료!");
            return true;
        }
    }
}