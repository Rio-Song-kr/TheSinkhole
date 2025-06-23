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
        public static bool IsCrafting
        {
            get 
            { 
                return misCrafting; 
            }  
        }

        //���� �������� ����� ������ ����
        [Header("���� ��� �������� ����")]
        [SerializeField] private InventorySlot mResultItemSlot;

        //������ ������ �䱸�Ǵ� ��� �����۵��� ������ �� �䱸�Ǵ� �������� ������ ������� �� ���Կ� ��� ǥ���� �� ����
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
        private Coroutine? mCoCraftItem; //���� ���� �� �ð� ��� �ڷ�ƾ

        //���� ���̾�α� â�� �ݾ� ������ �� �̻� ���� �ʰų� �������� �������� ����Ҷ� ȣ���
        //���� �������� �ڷ�ƾ �ߴ� misCrafting �� false�� ����
        private void OnDisable()
        {
            if (mCoCraftItem is not null)
            {
                StopCoroutine(mCoCraftItem);
            }
            misCrafting = false;
        }

        //������ ���� �ʱ�ȭ
        //�����Ǹ� �Ű������� ��� �����Ǹ� ������� �� ���� �ʱ�ȭ
        //�����ǿ� �ʿ��� �������� ������ŭ �κ��丮 ������ ���� �ش� ���Կ� ���
        //�κ��丮 ������ �̹� ������ �ش� ���� ����
        public void Init(CraftingRecipe recipe)
        {
            CurrentRecipe = recipe;
            
            //���� Ȱ��ȭ
            gameObject.SetActive(true);

            //���� ��� �������� ���Կ� ���
            mResultItemSlot.ClearSlot();
            InventoryMain.Instance.AcquireItem(recipe.resultItem.item, mResultItemSlot,recipe.resultItem.count);

            //UI ��� �ʱ�ȭ
            mCraftingTimeLabel.text = $"{recipe.craftingTime.ToString("F1")}s";
            mCraftingProgressImage.fillAmount = 1.0f;
            mCraftingButton.GetComponent<Image>().sprite = recipe.buttonSprite;

            //���� �������� ��� ������ ������ �����ϸ� ������ �°� �ν��Ͻ�
            for (int i = mRecipeContentTransform.childCount; i < recipe.reqItems.Length; i++)
            {
                Instantiate(mResultItemSlot,Vector3.zero, Quaternion.identity, mRecipeContentTransform);
            }

            //��� ��� ������ �ʱ�ȭ
            for (int i = 0; i < mRecipeContentTransform.childCount; i++)
            {
                //���� ȹ��
                InventorySlot recipeSlot = mRecipeContentTransform.GetChild(i).GetComponent<InventorySlot>();

                //�������� ��� �������� ���� �ε��� ��ȣ�ϰ��
                if (i < recipe.reqItems.Length)
                {
                    //��� �������� ���Կ� ���
                    recipeSlot.ClearSlot();
                    InventoryMain.Instance.AcquireItem(recipe.reqItems[i].item, recipeSlot, recipe.reqItems[i].count);
                    recipeSlot.gameObject.SetActive(true);
                }
                else
                {
                    recipeSlot.gameObject.SetActive(false);
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
            CraftingManager.Instance.RefreshAllslots();
        }

        //�������� �����ϴ� �ҿ�ð���ŭ UI��Ҹ� �����ϰ� �ð��� ����ߴٸ� ������ ��ȯ�� �ϱ����� �ڷ�ƾ
        private IEnumerator CoCraftItem()
        {
            misCrafting = true;

            // ���� ���
            SoundManager.Instance.PlaySound2d("Craft Sound" + SoundManager.Range(1, 2));

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
                DialogBox.DialogBoxController dialogBox = DialogBoxGenerator.Instance.CreateSimpleDialogBox("�˸�",$"�̹� �������Դϴ�.", "Ȯ��",dialogBox.DialogBoxController.RESERVED_EVENT_CLOSE,null,160,100);
                dialogBox.modifyBottomLayoutPadding(50, 50);
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
