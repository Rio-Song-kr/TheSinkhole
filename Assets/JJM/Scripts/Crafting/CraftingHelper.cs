using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public static class CraftingHelper
    {
        // ���� ���� ���� Ȯ��
        public static bool CanCraft(CraftingRecipe recipe, Inventory playerInventory)
        {
            //�κ��丮 �ý��� ����
            var inventory = playerInventory;

            // ��� ����
            foreach (var ingredient in recipe.ingredients)
            {
                int remaining = ingredient.count; //���� ������
                //��� �������� �ִ� �κ��丮����ã��

                int owned = inventory.GetItemAmounts(ingredient.item.ItemEnName);

                if (owned < ingredient.count)
                    return false;
            }

            //��ᰡ ����Ҷ� ����
            return true;
        }

        // ���� ���� ����
        public static bool Craft(CraftingRecipe recipe, Inventory playerInventory)
        {
            var inventory = playerInventory;

            //��ᰡ ������� �������� Ȯ��
            if (!CanCraft(recipe, playerInventory))
            {
                Debug.Log("��ᰡ �����մϴ�.");
                return false;
            }

            // ��� ����
            foreach (var ingredient in recipe.ingredients)
            {
                int remaining = ingredient.count; //���� ������
                //��� �������� �ִ� �κ��丮����ã��

                int owned = inventory.GetItemAmounts(ingredient.item.ItemEnName);

                if (inventory.RemoveItemAmounts(ingredient.item.ItemEnName, ingredient.count))
                {
                    RecipePanelController.NeedUpdateUI = true;
                }
            }

            // ��� ����
            playerInventory.AddItemSmart(recipe.result.item, recipe.result.count);
            //inventorySystem.AddItem(recipe.result.item, recipe.result.count);
            Debug.Log("���� �Ϸ�!");
            return true;
        }
    }
}