using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommonUI
{
    // 모든 UI에서 사용 가능한 닫기
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        [Header("닫을 대상(비워두면 부모 Canvas/Panel 자동 탐색)")]
        public GameObject targetToClose;

        private static bool m_isUICloseKeyPressed = false;

        private void Awake()
        {
            // 버튼 컴포넌트 가져오기
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(CloseTarget);
        }

        private void Update()
        {
            if (m_isUICloseKeyPressed)
            {
                m_isUICloseKeyPressed = false;
                // GameManager.Instance.SetCursorLock();
                CloseTarget();
            }
        }

        private void CloseTarget()
        {
            // 닫을 대상이 지정되어 있으면 그 오브젝트를 비활성화
            if (targetToClose != null)
            {
                targetToClose.SetActive(false);
            }
            else
            {
                // 지정이 없으면 부모 중에서 Canvas 또는 Panel을 찾아서 비활성화
                var parent = transform.parent;
                while (parent != null)
                {
                    if (parent.GetComponent<Canvas>() != null || parent.name.ToLower().Contains("panel"))
                    {
                        parent.gameObject.SetActive(false);
                        break;
                    }
                    parent = parent.parent;
                }
            }
            GameManager.Instance.SetCursorLock();
        }

        public static void OnUICloseKeyPressed()
        {
            m_isUICloseKeyPressed = true;
        }
    }
}