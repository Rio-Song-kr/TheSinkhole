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
            var inventorySystem = playerInventory.DynamicInventorySystem;

            //��ᰡ �ִ��� �˻�
            foreach (var ingredient in recipe.ingredients)
            {
                int owned = 0;//�������� 
                //��� �������� �ִ� �κ��丮 ���� ã��
                if (inventorySystem.FindItemSlots(ingredient.item, out var slots))
                {
                    foreach (var slot in slots)
                        owned += slot.ItemCount;
                }
                //���� ������ �ʿ䰳������ ������ ���� �Ұ�
                if (owned < ingredient.count)
                    return false;
            }
            //��ᰡ ����Ҷ� ����
            return true;
        }

        // ���� ���� ����
        public static bool Craft(CraftingRecipe recipe, Inventory playerInventory)
        {
            var inventorySystem = playerInventory.DynamicInventorySystem;

            //��ᰡ ������� �������� Ȯ��
            if (!CanCraft(recipe, playerInventory))
            {
                Debug.Log("��ᰡ �����մϴ�.");
                return false;
            }

            // ��� ����
            foreach (var ingredient in recipe.ingredients)
            {
                int remaining = ingredient.count;//���� ������
                //��� �������� �ִ� �κ��丮����ã��
                if (inventorySystem.FindItemSlots(ingredient.item, out var slots))
                {
                    foreach (var slot in slots)
                    {
                        if (slot.ItemCount >= remaining)
                        {
                            slot.RemoveItem(remaining);// �� ���Կ��� ��� ���� ����

                            break;
                        }
                        else
                        {
                            remaining -= slot.ItemCount;// ���� ������ ����

                            slot.RemoveItem(slot.ItemCount);// �ش� ���� ���� ����

                        }
                    }
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
