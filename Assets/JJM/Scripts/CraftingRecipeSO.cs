using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem {
    

    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe", order = 1)]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("�ʿ� ��� ���")]
        public CraftingItemInfo[] ingredients;//�ʿ���� ���

        [Header("���� �����")]
        public CraftingItemInfo result;//���� �����

        [Header("���� �ð�(��)")]
        public float craftingTime = 5f;//���۽ð� �⺻5��

        [Header("������(��ư ��)")]
        public Sprite icon; //���۰���� ������

        [Header("�����ǰ� �� ���ۼ�")]
        public CraftingStationType stationType; // �� �����ǰ� ��� ������ ���۴� Ÿ��

        [System.Serializable]
        public struct CraftingItemInfo
        {
            [Tooltip("������ ������ SO")]
            public ItemDataSO item; //ItemSO�� ���� ��������
            [Tooltip("�ʿ�/��� ����")]
            public int count;//�����ϴµ� �ʿ��� ����
        }
    }
}
