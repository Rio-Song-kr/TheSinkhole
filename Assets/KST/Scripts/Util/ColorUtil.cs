using UnityEngine;
namespace Util
{
    public static class ColorUtil
    {
        //헥사 값만 사용할 경우
        public static Color Hexcode(string hex)
        {
            //헥사값과 기본값으로 하며, 파싱 실패시 색을 흰색으로 변경하기 위함.
            return Hexcode(hex, Color.white);
        }

        public static Color Hexcode(string hex, Color color)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var _color))
            {
                return _color;
            }
            else
            {
                Debug.LogWarning($"Hex 코드 파싱 실패: {hex}");
                return color;
            }
        }
    }
}