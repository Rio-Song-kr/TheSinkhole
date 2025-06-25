using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CraftingSystem
{ 
    public class CraftingSlot : MonoBehaviour
    {
        //���� ������ �ϰ� �ִ��� Ȯ���ϱ� ���� ���� ����
        //������ �ߺ����� ���� �ʵ��� �ϱ�����
        private static bool misCrafting = false;
        public static bool IsCrafting => misCrafting;
        [Header("��� ���� ������")]
        [SerializeField] private GameObject mRecipeSlotPrefab;

        //���� �������� ����� ������ ����
        [Header("���� ��� �������� ����")]
        [SerializeField] private GameObject mResultItemSlot;
        //[SerializeField] private InventorySlot mResultItemSlot;

        //������ ���ۿ� �䱸�Ǵ� ��� �����۵��� ������ �� �䱸�Ǵ� �������� ������ ������� �� ���Կ� ��� ǥ���� �� ����
        //��ũ�� �並 ����Ͽ� ��ũ���� �Ͽ� � �������� �ʿ����� ��� Ȯ���� �� �ְ���
        //������ ������ �ν����� �� ��ġ��ų ��ũ�Ѻ��� ������ Ʈ�������� �̰��� ���
        [Header("���ۿ� �ʿ��� ��� �������� ��� �����̵� ������ Ʈ������")]
        [SerializeField] private Transform mRecipeContentTransform;

        //���� ��ư Ȱ��ȭ/��Ȱ��ȭ �ϱ� ���� ���
        [Header("���� ��ư")]
        [SerializeField] private Button mCraftingButton;

        //���ۿ� �ҿ�Ǵ� �ð� �� ���� �� ���� �ð��� ǥ���ϱ����� ��
        [Header("���� �ð� �ؽ�Ʈ ��")]
        [SerializeField] private TextMeshProUGUI mCraftingTimeLabel;

        //���� �� �������� ��ô���� ǥ���ϱ����� �̹���
        [Header("���� ���൵ �̹���")]
        [SerializeField] private Image mCraftingProgressImage;

        //������ �Ұ����� �� ���� ���� �� ������Ʈ�� ���� �ð������� ������ �Ұ��� �ϴٴ°��� �����ֱ� ���� ������Ʈ
        [Header("��Ȱ��ȭ ���½� ������ �̹��� ������Ʈ")]
        [SerializeField] private GameObject mDisableImageGo;

        /// <summary>
        /// ���� �ش� ������ ������� ������
        /// </summary>
        [HideInInspector] public CraftingRecipe CurrentRecipe;
        private Coroutine mCoCraftItem; //���� ���� �� �ð� ��� �ڷ�ƾ

        //���� ���̾�α� â�� �ݾ� ������ �� �̻� ���� �ʰų� �������� �������� ����Ҷ� ȣ���
        //���� �������� �ڷ�ƾ �ߴ� misCrafting �� false�� ����
        private void OnDisable()
        {
            if (mCoCraftItem != null)
            {
                StopCoroutine(mCoCraftItem);
            }
            misCrafting = false;
        }

        //������ ���� �ʱ�ȭ
        //�����Ǹ� �Ű������� ��� �����Ǹ� ������� �� ���� �ʱ�ȭ
        //�����ǿ� �ʿ��� �������� ������ŭ �κ��丮 ������ ���� �ش� ���Կ� ���
        //�κ��丮 ������ �̹� ������ �ش� ���� ����
        private Inventory mInventory;
        public void Init(CraftingRecipe recipe, Inventory inventory)
        {
            CurrentRecipe = recipe;

            mInventory = inventory;
            //���� Ȱ��ȭ
            gameObject.SetActive(true);

            //���� ��� �������� ���Կ� ���
            //mResultItemSlot.ClearSlot();
            //mResultItemSlot.AddItemToEmptySlot(recipe.resultItem.item, recipe.resultItem.count);

            //UI ��� �ʱ�ȭ
            mCraftingTimeLabel.text = $"{recipe.craftingTime:F1}s";
            mCraftingProgressImage.fillAmount = 1.0f;
            mCraftingButton.GetComponent<Image>().sprite = recipe.buttonSprite;

            //���� �������� ��� ������ ������ �����ϸ� ������ �°� �ν��Ͻ�
            for (int i = mRecipeContentTransform.childCount; i < recipe.reqItems.Length; i++)
            {
                Instantiate(mRecipeSlotPrefab, Vector3.zero, Quaternion.identity, mRecipeContentTransform);
            }

            //��� ��� ������ �ʱ�ȭ
            for (int i = 0; i < mRecipeContentTransform.childCount; i++)
            {
                Transform child = mRecipeContentTransform.GetChild(i);
                //���� ȹ��
                InventorySlot recipeSlot = mRecipeContentTransform.GetChild(i).GetComponent<InventorySlot>();

                //�������� ��� �������� ���� �ε��� ��ȣ�ϰ��
                if (i < recipe.reqItems.Length)
                {
                    //��� �������� ���Կ� ���
                    recipeSlot.ClearSlot();
                    recipeSlot.AddItemToEmptySlot(recipe.reqItems[i].item, recipe.reqItems[i].count);
                    child.gameObject.SetActive(true); // ���⼭ child�� GameObject�� Ȱ��ȭ
                    //recipeSlot.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false); // ��Ȱ��ȭ�� �����ϰ�
                    //recipeSlot.gameObject.SetActive(false);
                }
            }
        }

        //�ش� �������� ������ �� �ִ��� ���
        public void ToggleSlotState(bool isCraftable)
        {
            mDisableImageGo.SetActive(!isCraftable);
            mCraftingButton.interactable = isCraftable;
        }


        /// <summary>
        /// ��� �������� �����ϰ�, ��� �������� ȹ��
        /// </summary>

        //�������� ������ �ش� ������ ���� �ҿ�ð��� ������ �������� ��ȯ
        //�÷��̾��� �κ��丮���� ��� �������� �����ϰ� ���۰���������� �κ��丮�� ����
        private void RefreshItems()
        {
            // �κ��丮 �ý��� �ν��Ͻ� ���� (�̱��� �Ǵ� ���� ���� ��Ŀ� �°� ����)
            //var inventorySystem = mInventory.InventorySystem;

            // 1. ��� ����
            foreach (var info in CurrentRecipe.reqItems)
            {
                //RemoveItemFromInventory(inventorySystem, info.item, info.count);
            }

            // 2. ��� ������ ����
            //inventorySystem.AddItem(CurrentRecipe.resultItem.item, CurrentRecipe.resultItem.count);

            // 3. ���� ����
            CraftingManager.Instance.RefreshAllSlots();
        }
        // ���� ���Կ��� ������ �ʿ��� �� �����Ƿ� �Ʒ��� ���� ����
        private void RemoveItemFromInventory(InventorySystem inventorySystem, ItemDataSO itemDataSO, int amount)
        {
            int remaining = amount;
            if (inventorySystem.FindItemSlots(itemDataSO, out var slots))
            {
                foreach (var slot in slots)
                {
                    if (slot.ItemCount >= remaining)
                    {
                        slot.RemoveItem(remaining);
                        break;
                    }
                    else
                    {
                        remaining -= slot.ItemCount;
                        slot.RemoveItem(slot.ItemCount);
                    }
                }
            }
        }
        /*private void RefreshItems()
        {
            InventorySlot mainInventoryslot = null;

            //��� ������ ������ Ȯ���Ͽ� ���� �κ��丮�� �������� ����
            foreach(CraftingItemInfo info in CurrentRecipe.reqItems)
            {
                InventoryMain.Instance.HasItemInInventory(info.item.ID, out mainInventoryslot, info.count);
                mainInventoryslot.UpdateItemCount(-info.count);
            }

            //���� �� ��� �������� �κ��丮�� ȹ��
            InventoryMain.Instance.AcquireItem(CurrentRecipe.resultItem.item,CurrentRecipe.resultItem.count);

            //�������� ��ȯ �� ���Ե��� ������Ʈ
            CraftingManager.Instance.RefreshAllSlots();
        }*/

        //�������� �����ϴ� �ҿ�ð���ŭ UI��Ҹ� �����ϰ� �ð��� ����ߴٸ� ������ ��ȯ�� �ϱ����� �ڷ�ƾ
        private IEnumerator CoCraftItem()
        {
            misCrafting = true;

            // ���� ���
            //SoundManager.Instance.PlaySound2d("Craft Sound" + SoundManager.Range(1, 2));

            float process = 0f;
            while(process < 1f)
            {
                process += Time.deltaTime / CurrentRecipe.craftingTime;
                mCraftingProgressImage.fillAmount = Mathf.Lerp(0f,1f,process);

                yield return null;
            }
            //������ ȹ��
            RefreshItems();
            misCrafting = false;
        }
        #region Ui

        //��ư�� ���� ������ ���� �õ�
        //�̹� �������� �������̶�� ����, ���̾�α� �ڽ��� �̿��Ͽ� �˸��� �����
        public void BTN_Craft()
        {
            if(misCrafting)
            {
                //DialogBox.DialogBoxController dialogBox = DialogBoxGenerator.Instance.CreateSimpleDialogBox("�˸�",$"�̹� �������Դϴ�.", "Ȯ��",dialogBox.DialogBoxController.RESERVED_EVENT_CLOSE,null,160,100);
                //dialogBox.modifyBottomLayoutPadding(50, 50);
                return;
            }

            if(mCoCraftItem is not null)
            {
                StopCoroutine(mCoCraftItem);
            }
            mCoCraftItem = StartCoroutine(CoCraftItem());
        }
        #endregion

    }
}
