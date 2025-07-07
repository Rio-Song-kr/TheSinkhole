using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ���
        private static bool m_isInteractionKeyPressed = false;

        private void Update()
        {
            // �÷��̾ ���� ���� �ְ� EŰ�� ������ UI ����
            if (isPlayerInRange && m_isInteractionKeyPressed)
            {
                GameManager.Instance.SetCursorUnlock();
                OpenStationUI();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("�÷��̾ ���ۼ� ������ ����");
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("�÷��̾ ���ۼ� �������� ����");
                isPlayerInRange = false;
            }
        }
        // UI ����
        public void OpenStationUI()
        {
            if (resourcesPanelController != null)
            {
                // �� ���۴� Ÿ�Կ� �´� �����Ǹ� ����
                var filtered = availableRecipes.FindAll(r => r.stationType == stationType);

                // //todo ItemRecipeManager���� station id�� recipe ��� �о����
                // var recipes = new List<CraftingRecipe>();
                //
                // foreach (int itemId in GameManager.Instance.Recipe.StationIdRecipeList[stationType])
                // {
                //     int recipeId = GameManager.Instance.Recipe.ItemIdToRecipeId[itemId];
                //     var recipeDatas = GameManager.Instance.Recipe.RecipeIdData[recipeId];
                //
                //     foreach (var recipeData in recipeDatas)
                //     {
                //         int resultId = GameManager.Instance.Recipe.ItemIdToRecipeId[recipeData.RecipeId];
                //         var resultItemEnName = GameManager.Instance.Item.ItemIdEnName[resultId];
                //
                //         var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
                //         recipe.craftingTime = 5f;
                //         recipe.icon = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName].Icon;
                //
                //         //todo ��Ÿ ������ �����ؾ� ��
                //
                //         recipes.Add(recipe);
                //     }
                // }

                //todo recipe ��� -> CraftingRecipe ����(List�� �߰�) �� ����
                resourcesPanelController.SetRecipeList(filtered); // ResultPanelController�� �޼��� ���
                resourcesPanelController.gameObject.SetActive(true); // UI Ȱ��ȭ
            }
        }

        public static void OnInteractionKeyPressed() => m_isInteractionKeyPressed = true;
        public static void OnInteractionKeyReleased() => m_isInteractionKeyPressed = false;
    }
}