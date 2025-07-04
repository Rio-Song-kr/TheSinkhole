using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShelterUpgrade : MonoBehaviour
{
    public ToolType CurrentTool = ToolType.None;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private GameObject m_itemPickUpTextObject;
    [SerializeField] private TextMeshProUGUI m_itemPickUpText;

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
                if (CurrentTool == ToolType.Hammer)
                { 
                    SetTextObject(true, "��ô�Ϸ��� [E] Ű�� �����ּ���.");

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
    public void SetTextObject(bool isActive, string text = "")
    {
        m_itemPickUpTextObject.SetActive(isActive);
        m_itemPickUpText.text = text;
    }
}
