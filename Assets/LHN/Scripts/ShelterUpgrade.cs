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
        // 사용자가 E 키를 눌렀을 때만 아래 코드 실행
        if (Input.GetKeyDown(KeyCode.E))
        {

            // 카메라 기준으로 마우스 커서 방향으로 레이 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 레이캐스트로 5f 거리 내의 충돌 오브젝트 탐지
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                if (CurrentTool == ToolType.Hammer)
                { 
                    SetTextObject(true, "개척하려면 [E] 키를 눌러주세요.");

                    // Fence 태그가 붙은 오브젝트인지 확인
                    if (hit.collider.CompareTag("Fence"))
                    {
                        // 업그레이드 가능한 컴포넌트가 있는지 확인
                        UpgradeableObject obj = hit.collider.GetComponent<UpgradeableObject>();
                        // 있으면 업그레이드 실행
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
