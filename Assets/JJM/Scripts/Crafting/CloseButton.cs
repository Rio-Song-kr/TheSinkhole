using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommonUI
{
    // ��� UI���� ��� ������ �ݱ�
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        [Header("���� ���(����θ� �θ� Canvas/Panel �ڵ� Ž��)")]
        public GameObject targetToClose;

        private static bool m_isUICloseKeyPressed = false;

        private void Awake()
        {
            // ��ư ������Ʈ ��������
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
            // ���� ����� �����Ǿ� ������ �� ������Ʈ�� ��Ȱ��ȭ
            if (targetToClose != null)
            {
                targetToClose.SetActive(false);
            }
            else
            {
                // ������ ������ �θ� �߿��� Canvas �Ǵ� Panel�� ã�Ƽ� ��Ȱ��ȭ
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