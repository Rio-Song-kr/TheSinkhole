using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public class RecipeListController : MonoBehaviour
    {
        public Transform contentParent; // ������ ��ư���� �� Content
        public GameObject recipeButtonPrefab; // RecipeSelectButton ������
        public RecipePanelController panelController; // ����â ��Ʈ�ѷ�

        public CraftingRecipe[] recipes; // ��ü ������ ���

        void Start()
        {
            foreach (var recipe in recipes)
            {
                var go = Instantiate(recipeButtonPrefab, contentParent);
                var btn = go.GetComponent<RecipeSelectButton>();
                btn.Init(recipe, panelController);
            }
        }
    }
}
