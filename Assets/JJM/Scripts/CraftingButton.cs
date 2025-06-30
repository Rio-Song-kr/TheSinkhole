using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // UI ��ư���� ������ �����ϴ� ��ũ��Ʈ
    public class CraftingButton : MonoBehaviour
    {
        public RecipePanelController recipePanelController; // �г� ��Ʈ�ѷ� ����

        public CraftingManager craftingManager;// �Ŵ��� ����

        public void OnClickCraft()
        {
            var recipe = recipePanelController.GetCurrentRecipe();
            if (recipe != null)
                craftingManager.TryCraftWithDelay(recipe);
            else
                Debug.LogWarning("���õ� �����ǰ� �����ϴ�!");

        }
    }
}
