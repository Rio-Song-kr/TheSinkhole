using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // ������ �����ϴ� �Ŵ��� Ŭ����
    public class CraftingManager : MonoBehaviour
    {
        public Inventory playerInventory;// �÷��̾� �κ��丮 ����

        // ��� ����(���� �ð� ����)
        public void TryCraft(CraftingRecipe recipe)
        {
            if (CraftingHelper.Craft(recipe, playerInventory))
            {
                // ���� �� �߰� ����
            }
            else
            {
                // ���� �� �ȳ�
            }
        }

        // ���� �ð� ���� (�ڷ�ƾ)
        public void TryCraftWithDelay(CraftingRecipe recipe)
        {
            if (CraftingHelper.CanCraft(recipe, playerInventory))
                StartCoroutine(CraftCoroutine(recipe));// �ڷ�ƾ���� ����

            else
                Debug.Log("��ᰡ �����մϴ�.");
        }

        // ���� ���� �ð� ���� �� ���� ����
        private IEnumerator CraftCoroutine(CraftingRecipe recipe)
        {
            Debug.Log($"{recipe.result.item.name} ���� ��... ({recipe.craftingTime}��)");
            yield return new WaitForSeconds(recipe.craftingTime);// ���� �ð� ���

            CraftingHelper.Craft(recipe, playerInventory);// ���� ����
        }
    }
}
