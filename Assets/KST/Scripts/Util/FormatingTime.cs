using UnityEngine;
namespace Util
{
    public class FormatingTime
    {
        public static string FormatMinTime(float time)
        {
            int totalSeconds = Mathf.CeilToInt(time); 
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return $"{minutes:D2}:{seconds:D2}";
        }
        public static string FormatSecTime(float time)
        {
            int totalSeconds = Mathf.CeilToInt(time);
            return $"{totalSeconds}";
        }
    }
}