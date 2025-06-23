using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEditor;
using UnityEngine;

namespace CraftingSystem
{
    //�κ��丮 ������ �Ϸ�� �� ������ ���� ������ �����Ͽ� ������ ���������� �̸��� ����
    //��ũ���ͺ� ������Ʈ�� �����Ͽ� ������ �� �ֵ��� ��
    [CreateAssetMenu(fileName = "Recipe", menuName = "scriptable Object/Crafting Recipe", order = int.MaxValue)]
    public class CraftingRecipe : ScriptableObject
    {
        //CraftingItemInfo�� �迭�� ����Ͽ� ������ ���ۿ� �ʿ��� �����۵��� ����
        [Header("���ۿ� �ʿ��� ��� �����۵�")]//���ۿ� �ʿ��� ��������s
        [SerializeField] public CraftingItemInfo[] reqItems;

        //CraftingItemInfo�� ����Ͽ� ���� �� ���޵Ǵ� �������� ������ ����
        [Header("���� ����� ������")]//���� �Ϸ�� ���� ������
        [SerializeField] public CraftingItemInfo resultItem;

        [Header("���ۿ� �ɸ��� �ð�")]//���ۿ� �ɸ��� �ð�
        [SerializeField] public float craftingTime;

        [Header("�������� ǥ���� �̹���")]//�������� ǥ���� �̹���
        [SerializeField] public Sprite buttonSprite;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CraftingRecipe))]
    public class CraftingRecipeEditor : Editor
    {
        CraftingRecipe recipe;

        void OnEnable()
        {
            recipe = (CraftingRecipe)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //��ư ����
            if (GUILayout.Button("�̸� �ڵ� ����"))
            {
                //�̸� ����
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(recipe), $"RECIPE__{recipe.resultItem.item.ID.ToStirng()}");
            }
        }
    }
#endif
    //��� �� ��� �����ۿ� ���� ����
    //� ������ �� �������� ������ ��Ÿ��
    [System.Serializable]
    public struct CraftingItemInfo
    {
        //[SerializeField] public Item item; //������ (Item �κ��� �κ��丮�� �������� ��������� ����)
        //[SerializeField] public int amount; //������ ����
    }
}

