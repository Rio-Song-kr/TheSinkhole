using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class ResultPanelController : MonoBehaviour
    {
        public Transform contentParent; // Button들이 들어갈 Content
        public GameObject resultButtonPrefab; // Button 프리팹 (아래 참고)
        public RecipePanelController recipePanelController; // 상세 패널

        public CraftingRecipe[] recipes; // 전체 레시피 목록

        void Start()
        {
            if (resultButtonPrefab == null) Debug.LogError("resultButtonPrefab이 null입니다!");
            if (contentParent == null) Debug.LogError("contentParent가 null입니다!");
            if (recipePanelController == null) Debug.LogError("recipePanelController가 null입니다!");
            if (recipes == null || recipes.Length == 0) Debug.LogError("recipes 배열이 비어있거나 null입니다!");

            foreach (var recipe in recipes)
            {
                //테스트
                if (recipe == null)
                {
                    Debug.LogError("recipes 배열에 null이 있습니다!");
                    continue;
                }
                if (recipe.result.item == null)
                {
                    Debug.LogError($"{recipe.name}의 result.item이 null입니다!");
                    continue;
                }
                //
                var go = Instantiate(resultButtonPrefab, contentParent);
                var btn = go.GetComponent<Button>();
                var nameText = go.GetComponentInChildren<TMP_Text>();
                var icon = go.GetComponentInChildren<Image>();

                //테스트
                if (btn == null)
                {
                    Debug.LogError("resultButtonPrefab에 Button 컴포넌트가 없습니다!");
                    continue;
                }
                if (nameText == null)
                {
                    Debug.LogError("resultButtonPrefab에 TMP_Text 컴포넌트가 없습니다!");
                }
                if (icon == null)
                {
                    Debug.LogError("resultButtonPrefab에 Image 컴포넌트가 없습니다!");
                }
                //
                if (nameText != null)
                    nameText.text = recipe.result.item.name;
                if (icon != null)
                    icon.sprite = recipe.result.item.Icon;

                btn.onClick.AddListener(() => recipePanelController.SetRecipe(recipe));
            }
        }
    }
}
