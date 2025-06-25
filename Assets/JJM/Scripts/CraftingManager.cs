using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



namespace CraftingSystem
{
    public class CraftingManager : Singleton<CraftingManager>
    {
        //���� ���� ���̾�α� â�� �����֤�����(���� �ý����� ��� ������) Ȯ���ϱ� ���� ����
        private static bool mIsDialogActive = false;
        public static bool IsDialogActive
        {
            get
            {
                return mIsDialogActive;
            }
        }

        //���ۼҿ��� ����� �� �ִ� �۷ι� ������
        //���ۼ� ������ �����ǿ��� �� �۷ι� �����Ǹ� ������ �� ����
        [Header("Crafting Station�� ��� ���� �������� ��� ������ ������")]
        [SerializeField] private CraftingRecipe[] mGlobalRecipes;

        //���� �����Ǹ� ������� ���� �� �ش� �����ǵ��� �ӽ÷� �Űܵ� Ʈ������
        [Header("���� �����Ǹ� ������� ���� �� ���Ե��� �ӽ÷� �� Ʈ������")]
        [SerializeField] private Transform mGlobalRecipesTemporaryPlacement;

        //���� ���̾�α� â�� Ȱ��ȭ �� ��Ȱ��ȭ �ϱ� ���� ���ӿ�����Ʈ
        [Header("Crafting ���̾�α� â ������Ʈ")]
        [SerializeField] private GameObject mCraftingDialogGo;

        //��ũ�� �信�� ���� ������ ������ ��ġ�ϱ� ���� ������ Ʈ������
        [Header("Crafting ���̾�α׿� �����Ǹ� ��ġ�� Content Ʈ������")]
        [SerializeField] private Transform mRecipeContentTransform;

        [Header("Crafting Slot ������")]
        [SerializeField] private GameObject mRecipeSlotPrefab;

        //UI ��ҵ��� ����
        //���� ������ �����۸� �� �� �ֵ��� �����ϴ� ��۰� ���̾�α� â�� Ÿ��Ʋ ��
        [Space(30)][Header("UI ��ҵ�")]
        [Header("���� ������ �����۸� ������ �ϴ� ���")]
        [SerializeField] private Toggle mViewCraftableOnlyToggle;
        [Header("���̾�α� â Ÿ��Ʋ")]
        [SerializeField] private TextMeshProUGUI mTitleLabel;

        CraftingSlot[] mGlobalRecipeSlots; //�۷ι� ������ ���Ե�
        List<CraftingSlot> mStationOnlyRecipeSlots = new List<CraftingSlot>(); //�����̼� ���� ������ ���Ե�

        private int mCurrentCraftingCount;//���� ���� �����̼��� ���� ������ ����

        private Inventory playerInventory;
        //private void Awake()
        //{
        //    //�ʱ�ȭ�� ���� Ȱ��ȭ ���� ����
        //    CraftingManager.mIsDialogActive = false;

        //    var playerObj = GameObject.FindWithTag("Player");
        //    if (playerObj != null)
        //        playerInventory = playerObj.GetComponent<Inventory>();

        //    Init();
        //}

        /// <summary>   
        /// ���� �����Ǹ� �ʱ�ȭ
        /// </summary>

        //���� �ε�Ǹ� �۷ι� �����Ǹ� �̸� �ε��ϰ� ������ȭ�Ͽ� ��� �غ� �Ϸ�
        //�ӽ÷� mGlobalRecipesTemporaryPlacement�� ��ġ�Ͽ� �۷ι� �����Ǹ� ����� ���� ������ ��ġ�� �̵�
        private void Init()
        {
            List<CraftingSlot> globalRecipeSlots = new List<CraftingSlot>();
            foreach (CraftingRecipe recipe in mGlobalRecipes)
            {
                //�ν��Ͻ� �� �ʱ�ȭ
                CraftingSlot craftingSlot = Instantiate(mRecipeSlotPrefab, Vector3.zero, Quaternion.identity, mGlobalRecipesTemporaryPlacement).GetComponent<CraftingSlot>();
                //craftingSlot.Init(recipe, playerInventory);

                //����Ʈ�� ����
                globalRecipeSlots.Add(craftingSlot);
            }
            mGlobalRecipeSlots = globalRecipeSlots.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipes">���̾�α׿� ���Խ�ų �����ǵ�</param>
        /// <param name="useGlobalRecipes">�۷ι� �����Ǹ� ������� ����</param>"

        //CraftingStation.cs �κ��� ȣ��Ǿ� �Ű������� �����ǿ� �۷ι� �����Ǹ� ����ϴ��� Ÿ��Ʋ�� �������� �Ѱ��ָ� �ʱ�ȭ�� �Ҽ� �ֵ��� ��
        //������ ������ ���� �������� �ν��Ͻ�ȭ�Ͽ� ������ ������ �°� ���� �� �ش� ���� �����տ� Init�� ȣ���Ͽ� �����Ǹ� ������
        public void TryOpenDialog(CraftingRecipe[] recipes, bool useGlobalRecipes, string title)
        {
            //�̹� ���̾�αװ� ����������
            if (mIsDialogActive)
            {
                return;
            }

            //������ ������ ���� ������Ʈ�� �ν��Ͻ� �� ����Ʈ�� ����
            for (int i = mStationOnlyRecipeSlots.Count; i < recipes.Length; i++)
            {
                CraftingSlot craftingSlot = Instantiate(mRecipeSlotPrefab, Vector3.zero, Quaternion.identity, mRecipeContentTransform).GetComponent<CraftingSlot>();
                mStationOnlyRecipeSlots.Add(craftingSlot);
            }

            //��� ������ �˻��Ͽ� Ȱ��ȭ �� ��Ȱ��ȭ
            for (int i = 0; i < mStationOnlyRecipeSlots.Count; i++)
            {
                //�������� �������� ���� �ε��� ��ȣ�ϰ��
                if (i < recipes.Length)
                {
                    mStationOnlyRecipeSlots[i].Init(recipes[i], playerInventory);
                }
                else
                {
                    mStationOnlyRecipeSlots[i].gameObject.SetActive(false);
                }
            }

            //�۷ι� ������ ��� ���� ����
            if (useGlobalRecipes)
            {
                foreach (CraftingSlot globalRecipe in mGlobalRecipeSlots)
                {
                    globalRecipe.transform.SetParent(mRecipeContentTransform);
                }
            }
            //���̾�α� �ڽ�Ȱ��ȭ
            mCraftingDialogGo.gameObject.SetActive(true);

            mTitleLabel.text = title;
            mCurrentCraftingCount = recipes.Length;
            mIsDialogActive = true;
            //UtilityManager.UnlockCursor();

            //��� ������ ����
            RefreshAllSlots();
        }
        /// <summary>
        /// ���̾�α׸� ����
        /// </summary>

        //���̾�α� â�� ��Ȱ��ȭ
        public void CloseDialog()
        {
            // �۷ι� �����Ǹ� ��� �ű�
            foreach (CraftingSlot globalRecipe in mGlobalRecipeSlots)
            {
                globalRecipe.transform.SetParent(mGlobalRecipesTemporaryPlacement);
            }
            //���̾�α� ��Ȱ��ȭ
            mCraftingDialogGo.gameObject.SetActive(false);

            mIsDialogActive = false;
            //UtilityManager.TryLockCursor();
        }

        /// <summary>
        /// �ش� ������ �̿� �������� �˻��Ͽ� ���Կ� ���¸� ����
        /// </summary>
        /// <param name="craftingslot"></param>

        //�ش� ������ ���� ������ �������� �˻��Ͽ� �˻� �� ���� ���θ� ���
        private void CheckCraftingSlot(CraftingSlot craftingSlot)
        {
            //������ Ȱ��ȭ
            craftingSlot.gameObject.SetActive(true);

            var inventorySystem = playerInventory.InventorySystem;

            //�䱸 �������� �÷��̾��� �κ��丮�� �ִ��� �˻�
            for (int i = 0; i < craftingSlot.CurrentRecipe.reqItems.Length; i++)
            {
                //�ϳ��� ������ ��ᰡ ���ٸ� ��Ȱ��ȭ ���·� ��ȯ
                //if (InventoryMain.Instance.HasItemInInventory(craftingSlot.CurrentRecipe.reqItems[i].item.ID, craftingSlot.CurrentRecipe.reqItems[i].count) == false)
                var reqInfo = craftingSlot.CurrentRecipe.reqItems[i];
                int requiredCount = reqInfo.count;
                int ownedCount = 0;

                if (inventorySystem.FindItemSlots(reqInfo.item, out var slots))
                {
                    foreach (var slot in slots)
                        ownedCount += slot.ItemCount;
                }

                //if (ownedCount < requiredCount)
                //{
                //    //������ �Ұ����� ���¿��� ViewCraftableOnly�� �����ִٸ�
                //    if (mViewCraftableOnlyToggle.isOn)
                //    {
                //        craftingSlot.gameObject.SetActive(false);//������Ʈ ��ü�� ��Ȱ��ȭ
                //    }
                //    else
                //    {
                //        craftingSlot.ToggleSlotState(false);//������ �Ұ����� ���·� ����������
                //    }
                //    return;
                //}
            }
            //�䱸 �������� ��� �ִ°�� Ȱ��ȭ ���·� ��ȭ
            craftingSlot.ToggleSlotState(true);
        }
        /// <summary>
        /// ��� ������ ����
        /// </summary>

        //Ư�� ���ǿ� ���� ��� ������ ������ �� �ʿ䰡 �ִٸ� ȣ���Ͽ� ����
        //���� ��� �÷��̾ ������ �Ͽ� ���� �������� �ٲ��� ȣ��
        public void RefreshAllSlots()
        {
            //���̾�αװ� ��Ȱ��ȭ ���¶��
            if (mIsDialogActive == false)
            {
                return;
            }
            for (int i = 0; i < mCurrentCraftingCount; i++)
            {
                CheckCraftingSlot(mStationOnlyRecipeSlots[i]);
            }

            //mGlobalRecipesTemporaryPlacement�� �ڽ� ������ 0
            if (mGlobalRecipesTemporaryPlacement.childCount == 0)
            {
                foreach (CraftingSlot globalRecipeSlot in mGlobalRecipeSlots)
                {
                    CheckCraftingSlot(globalRecipeSlot);
                }
            }
        }
        #region

        //��ۿ� ���� ȣ��Ǹ� ���� ������ ��� �� ������ ����
        public void TOGGLE_ViewCraftableOnly()
        {
            RefreshAllSlots();
        }
        #endregion
    }
}

