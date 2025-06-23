using UnityEngine;

public enum ToolType
{
    None,
    Shovel, //삽
    Hoe, //괭이
    Book, // 책
    Drill // 농사땅 부수는 용도
}
public class ToolControl : MonoBehaviour
{
    public ToolType CurrentTool = ToolType.None;
    [SerializeField] private Transform m_rayTransform; //레이 쏘는 위치(캐릭터의 카메라.)
    public float InteractionDistance = 2.3f; //적당한 거리로 세팅바람.(상호작용 가능한 적당한 레이 길이 필요.)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //좌클시(우클은 1)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new(m_rayTransform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance))
        {
            if (hit.collider.TryGetComponent(out Iinteractable interactable))
            {
                interactable.OnInteract(CurrentTool);
            }
        }
    }
}
