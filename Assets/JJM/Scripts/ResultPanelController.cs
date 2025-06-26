using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class ResultPanelController : MonoBehaviour
    {
        public Transform contentParent; // Button���� �� Content
        public GameObject resultButtonPrefab; // Button ������ (�Ʒ� ����)
        public RecipePanelController recipePanelController; // �� �г�

        public CraftingRecipe[] recipes; // ��ü ������ ���

        void Start()
        {
            if (resultButtonPrefab == null) Debug.LogError("resultButtonPrefab�� null�Դϴ�!");
            if (contentParent == null) Debug.LogError("contentParent�� null�Դϴ�!");
            if (recipePanelController == null) Debug.LogError("recipePanelController�� null�Դϴ�!");
            if (recipes == null || recipes.Length == 0) Debug.LogError("recipes �迭�� ����ְų� null�Դϴ�!");

            foreach (var recipe in recipes)
            {
                //�׽�Ʈ
                if (recipe == null)
                {
                    Debug.LogError("recipes �迭�� null�� �ֽ��ϴ�!");
                    continue;
                }
                if (recipe.result.item == null)
                {
                    Debug.LogError($"{recipe.name}�� result.item�� null�Դϴ�!");
                    continue;
                }
                //
                var go = Instantiate(resultButtonPrefab, contentParent);
                var btn = go.GetComponent<Button>();
                var nameText = go.GetComponentInChildren<TMP_Text>();
                var icon = go.GetComponentInChildren<Image>();

                //�׽�Ʈ
                if (btn == null)
                {
                    Debug.LogError("resultButtonPrefab�� Button ������Ʈ�� �����ϴ�!");
                    continue;
                }
                if (nameText == null)
                {
                    Debug.LogError("resultButtonPrefab�� TMP_Text ������Ʈ�� �����ϴ�!");
                }
                if (icon == null)
                {
                    Debug.LogError("resultButtonPrefab�� Image ������Ʈ�� �����ϴ�!");
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
