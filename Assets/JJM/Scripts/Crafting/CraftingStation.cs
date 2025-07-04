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

        private void Update()
        {
            if (isPlayerInRange)
                Debug.Log("�÷��̾ ���ۼ� ���� �ȿ� ����");
            // �÷��̾ ���� ���� �ְ� EŰ�� ������ UI ����
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("EŰ �Է� ����, ���ۼ� UI ���� �õ�");
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
                resourcesPanelController.SetRecipeList(filtered); // ResultPanelController�� �޼��� ���
                resourcesPanelController.gameObject.SetActive(true); // UI Ȱ��ȭ
            }
        }
    }
}
