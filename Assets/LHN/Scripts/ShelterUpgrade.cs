using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterUpgrade : MonoBehaviour
{
    [SerializeField] private Inventory m_inventory;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();
    }
    private void Update()
    {
        // ����ڰ� E Ű�� ������ ���� �Ʒ� �ڵ� ����
        if (Input.GetKeyDown(KeyCode.E))
        {

            // ī�޶� �������� ���콺 Ŀ�� �������� ���� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ����ĳ��Ʈ�� 5f �Ÿ� ���� �浹 ������Ʈ Ž��
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                // Fence �±װ� ���� ������Ʈ���� Ȯ��
                if (hit.collider.CompareTag("Fence"))
                {
                    // ���׷��̵� ������ ������Ʈ�� �ִ��� Ȯ��
                    UpgradeableObject obj = hit.collider.GetComponent<UpgradeableObject>();
                    // ������ ���׷��̵� ����
                    if (obj != null)
                    {
                        obj.Upgrade();
                    }
                }
            }
        }
    }
}
