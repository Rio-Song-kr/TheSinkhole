using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public class CraftingStation : MonoBehaviour
    {
        //�ش� ���ۼҿ��� ������ ������ �����ǵ��� ��Ͻ�ŵ�ϴ�
        [Header("���� �����̼ǿ��� ������ ���� �����ǵ�")]
        [SerializeField] private CraftingRecipe[] mRecipes;

        //������ �������� ���� Ƽ���� ���ۼҸ� ������� �ʾƵ� ������ �� �ֽ��ϴ�
        //�۷ι� �����Ǹ� ����� �� �ִ� ����� �߰��� ����
        //[Header("�۷ι� �����Ǹ� ����ϴ°�?")]
        //[SerializeField] private bool mUseGlobalRecipes = true;

        ////���ۼ� ���̾�α� â�� ������ �� �ش� ���ۼ��� �̸��� UI�� ǥ���ϱ� ���� ���
        //[Header("���̾�α� â�� Ÿ��Ʋ �̸�")]
        //[SerializeField] private string mTitle = "CRAFTING STATION";

        //���� ���̾�α� â�� ��
        //CraftingManager���� ȣ���Ͽ� �ڽ��� �����ǿ� �۷ι������Ǹ� ����ϴ��� Ÿ��Ʋ �̸��� �Բ� �����Ͽ� â�� ������ ��
        public void TryOpenDialog()
        {
            //CraftingManager.Instance.TryOpenDialog(mRecipes,mUseGlobalRecipes,mTitle);
        }
    }
}

