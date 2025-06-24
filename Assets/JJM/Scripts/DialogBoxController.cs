using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogBox
{
    #region 열거형 옵션
    /// <summary>
    /// 정렬옵션
    /// </summary>
    
    public enum Align
    {
        /// <summary>
        /// 왼쪽
        /// </summary>
        LEFT,
        /// <summary>
        /// 오른쪽
        /// </summary>
        RIGHT,
        /// <summary>
        /// 가운데
        /// </summary>
        CENTER,
        /// <summary>
        /// 가운데 확장 (좌우로 최대치)
        /// </summary>
        ENPAND,
    }

    /// <summary>
    /// 입력필드 이벤트
    /// </summary>
     
    public enum InputFieldEvent
    {
        /// <summary>
        /// 입력값이 변경될 때
        /// </summary>

        OnValueChanged,

        /// <summary>
        /// 변경이 끝난경우 (포커스 해제)
        /// </summary>

        OnEndEdit,

        /// <summary>
        /// 선택될 때
        /// </summary>

        OnSelect,

        /// <summary>
        /// 선택이 끝날 때
        /// </summary>

        OnDeselect,
    }

    #endregion
    public class DialogBoxController : MonoBehaviour
    {
    
    }
}
