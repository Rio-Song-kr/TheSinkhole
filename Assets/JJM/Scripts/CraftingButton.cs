using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // UI ��ư���� ������ �����ϴ� ��ũ��Ʈ
    public class CraftingButton : MonoBehaviour
    {
        public CraftingRecipe recipe;// ������ ������
        public CraftingManager craftingManager;// �Ŵ��� ����

        public void OnClickCraft()
        {
            craftingManager.TryCraftWithDelay(recipe);// ���� �õ�(���� �ð� ����)

        }
    }
}
