using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{

    // ������ �����ϴ� �Ŵ��� Ŭ����
    public class CraftingManager : MonoBehaviour
    {
        public Inventory playerInventory;// �÷��̾� �κ��丮 ����

        private bool isCrafting = false; // ���� ���� ������ ����

        public bool IsCrafting => isCrafting;

        private void Start()
        {
            // �ڵ� ����: "Player" �±װ� ���� ������Ʈ���� Inventory ������Ʈ ã��
            if (playerInventory == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                { 
                    playerInventory = playerObj.GetComponent<Inventory>();
                if (playerInventory != null)
                    Debug.Log("Player Inventory �ڵ� ���� ����");
                else
                    Debug.LogWarning("Player ������Ʈ�� Inventory ������Ʈ�� �����ϴ�.");
            }
            else
            {
                Debug.LogWarning("Player �±� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
            else
            {
                Debug.Log("Player Inventory�� �̹� Inspector���� ����Ǿ� �ֽ��ϴ�.");
            }
        }

        // ��� ����(���� �ð� ����)
        public void TryCraft(CraftingRecipe recipe)
        {
            //��� ���� ��� ���� �� �ۼ�
        }

        // ���� �ð� ���� (�ڷ�ƾ)
        public void TryCraftWithDelay(CraftingRecipe recipe)
        {
            if (isCrafting)
            {
                Debug.Log("���� ���Դϴ�. �ٸ� ������ �Ұ����մϴ�.");
                return;
            }
            if (CraftingHelper.CanCraft(recipe, playerInventory))
                StartCoroutine(CraftCoroutine(recipe));// �ڷ�ƾ���� ����

            else
                Debug.Log("��ᰡ �����մϴ�.");
        }

        // ���� ���� �ð� ���� �� ���� ����
        private IEnumerator CraftCoroutine(CraftingRecipe recipe)
        {
            isCrafting = true; // ���� ����
            Debug.Log($"{recipe.result.item.name} ���� ��... ({recipe.craftingTime}��)");
            yield return new WaitForSeconds(recipe.craftingTime);// ���� �ð� ���

            CraftingHelper.Craft(recipe, playerInventory);// ���� ����
            isCrafting = false; // ���� ��
        }
        
    }
}
