using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;

public class GameTimer : MonoBehaviour
{
    private const double GameMultiplier = 3600.0 / 30.0;
    private DateTime realStartTime;
    int count = 1;

    public TextMeshProUGUI gameTimeText; // UI에 연결할 텍스트

    void Start()
    {
        // 게임 시작 시점 기록
        realStartTime = DateTime.Now;
    }

    DateTime beforeTime = new DateTime(1, 1, 1, 6, 0, 0);

    void Update()
    {
        DateTime now = DateTime.Now;
        TimeSpan realElapsed = now - realStartTime;
        double gameSeconds = realElapsed.TotalSeconds * GameMultiplier;

        // 기준이 되는 6시부터 흐른 게임 시간 생성
        DateTime gameTime = new DateTime(1, 1, 1, 6, 0, 0).AddSeconds(gameSeconds);

        string gameTimeFormatted = gameTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US"));

        if (gameTimeFormatted == "AM 06" && beforeTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US")) == "AM 05")
        {
            count += 1;
        }
        gameTimeText.text = $"DAY : {count}              {gameTimeFormatted}";
        // 조건문 시간 비교를 위한 이전 시간 저장
        beforeTime = gameTime;

    }
}
