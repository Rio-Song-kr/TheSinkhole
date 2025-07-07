using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

namespace CraftingSystem
{
    public class CraftingStation : MonoBehaviour
    {
        // �� ���۴��� Ÿ��(����/�Ҹ� ��)
        [Header("�� ���۴��� Ÿ��")]
        public CraftingStationType stationType;

        [Header("�� ���۴뿡�� ���� ������ ������ ���")]
        // �� ���۴뿡�� ���� ������ ������ ���
        public List<CraftingRecipe> availableRecipes;

        //[Header(" ��ü ������ ���")]
        //// ��ü ������ ���(���� �� ��� �����Ǹ� ������ �� ���, �ʿ�� ScriptableObject�� ����)
        //public List<CraftingRecipe> allRecipes;

        [Header(" ������ �г� ��Ʈ�ѷ�(UI) ����")]
        // ������ �г� ��Ʈ�ѷ�(UI) ����
        public ResultPanelController resourcesPanelController;

        private bool canInteraction = false; // �÷��̾ ���� ���� �ִ���
        private static bool m_isInteractionKeyPressed = false;
        private Outlinable m_outline;

        private void Awake()
        {
            m_outline = GetComponent<Outlinable>();
            m_outline.enabled = false;
        }

        // UI ����
        public void OpenStationUI()
        {
            if (resourcesPanelController != null)
            {
                // �� ���۴� Ÿ�Կ� �´� �����Ǹ� ����
                // var filtered = availableRecipes.FindAll(r => r.stationType == stationType);

                // //todo ItemRecipeManager���� station id�� recipe ��� �о����
                var recipes = new List<CraftingRecipe>();

                foreach (int recipeID in GameManager.Instance.Recipe.StationIdRecipeList[stationType])
                {
                    //# �ش� Recipe id�� ���Ե� ��� ������ ����� �ҷ���
                    var recipeDatas = GameManager.Instance.Recipe.RecipeIdData[recipeID];

                    var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();

                    var craftItem = new CraftingItemInfo();

                    int resultId = GameManager.Instance.Recipe.RecipeIdToItemId[recipeID];
                    var resultItemEnName = GameManager.Instance.Item.ItemIdEnName[resultId];
                    recipe.icon = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName].Icon;

                    //# result ����
                    craftItem.item = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName];
                    craftItem.count = 1;
                    recipe.result = craftItem;
                    recipe.craftingTime = 5f;

                    recipe.ingredients = new List<CraftingItemInfo>();

                    //# �� ������ ��� �����͸� �ϳ��� ��ȸ�ϸ鼭 Icon�� Result ����
                    foreach (var ingredientData in recipeDatas)
                    {
                        var ingredient = new CraftingItemInfo();
                        int ingredientItemId = ingredientData.MaterialId;
                        var ingredientItemEnName = GameManager.Instance.Item.ItemIdEnName[ingredientItemId];

                        ingredient.item = GameManager.Instance.Item.ItemEnDataSO[ingredientItemEnName];
                        ingredient.count = ingredientData.MaterialQuantity;

                        recipe.ingredients.Add(ingredient);
                    }
                    recipes.Add(recipe);
                }

                //todo recipe ��� -> CraftingRecipe ����(List�� �߰�) �� ����
                resourcesPanelController.SetRecipeList(recipes); // ResultPanelController�� �޼��� ���
                resourcesPanelController.gameObject.SetActive(true); // UI Ȱ��ȭ
            }
        }

        public void SetOutline(bool isEnable) => m_outline.enabled = isEnable;
    }
}