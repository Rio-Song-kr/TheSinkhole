using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogBox
{
    #region ������ �ɼ�
    /// <summary>
    /// ���Ŀɼ�
    /// </summary>
    
    public enum Align
    {
        /// <summary>
        /// ����
        /// </summary>
        LEFT,
        /// <summary>
        /// ������
        /// </summary>
        RIGHT,
        /// <summary>
        /// ���
        /// </summary>
        CENTER,
        /// <summary>
        /// ��� Ȯ�� (�¿�� �ִ�ġ)
        /// </summary>
        ENPAND,
    }

    /// <summary>
    /// �Է��ʵ� �̺�Ʈ
    /// </summary>
     
    public enum InputFieldEvent
    {
        /// <summary>
        /// �Է°��� ����� ��
        /// </summary>

        OnValueChanged,

        /// <summary>
        /// ������ ������� (��Ŀ�� ����)
        /// </summary>

        OnEndEdit,

        /// <summary>
        /// ���õ� ��
        /// </summary>

        OnSelect,

        /// <summary>
        /// ������ ���� ��
        /// </summary>

        OnDeselect,
    }

    #endregion
    public class DialogBoxController : MonoBehaviour
    {
    
    }
}
