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

        public FullInventoryPopup inventoryFullPopup;

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

        private IEnumerator CraftCoroutine(CraftingRecipe recipe)
        {
            isCrafting = true;

            // ī��Ʈ�ٿ� ���� (��ư �ؽ�Ʈ ����)
            var panel = FindObjectOfType<RecipePanelController>();
            if (panel != null)
                yield return StartCoroutine(panel.CraftingButtonCountdownCoroutine(recipe.craftingTime));

            CraftingHelper.Craft(recipe, playerInventory);
            isCrafting = false;
        }

        // ��� ����(���� �ð� ����)
        //public void TryCraft(CraftingRecipe recipe)
        //{
        //    //��� ���� ��� ���� �� �ۼ�
        //}

        // ���� �ð� ���� (�ڷ�ƾ)
        public void TryCraftWithDelay(CraftingRecipe recipe)
        {
            if (isCrafting)
            {
                Debug.Log("���� ���Դϴ�. �ٸ� ������ �Ұ����մϴ�.");
                return;
            }

            // �κ��丮 ���� üũ (����)
            var invSys = playerInventory.DynamicInventorySystem;
            bool hasEmptySlot = invSys.HasEmptySlot();
            bool hasResultItem = invSys.FindItemSlots(recipe.result.item, out var resultSlots) && resultSlots.Count > 0;

            if (!hasEmptySlot && !hasResultItem)
            {
                if (inventoryFullPopup != null)
                    inventoryFullPopup.Show();
                else
                    Debug.LogWarning("InventoryFullPopup�� ����Ǿ� ���� �ʽ��ϴ�.");
                return;
            }

            StartCoroutine(CraftCoroutine(recipe));
        }

        
        
    }
}
