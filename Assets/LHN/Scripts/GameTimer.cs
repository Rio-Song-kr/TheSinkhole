using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    private const double GameMultiplier = 3600.0 / 30.0;
    private DateTime realStartTime;

    public TextMeshProUGUI gameTimeText; // UI에 연결할 텍스트

    void Start()
    {
        // 게임 시작 시점 기록
        realStartTime = DateTime.Now;
    }

    void Update()
    {
        DateTime now = DateTime.Now;
        TimeSpan realElapsed = now - realStartTime;
        double gameSeconds = realElapsed.TotalSeconds * GameMultiplier;

        // 기준이 되는 0시부터 흐른 게임 시간 생성
        DateTime gameTime = new DateTime(1, 1, 1, 6, 0, 0).AddSeconds(gameSeconds);

        string realTimeFormatted = now.ToString("tt hh:mm");
        string gameTimeFormatted = gameTime.ToString("tt hh:mm");

        gameTimeText.text = $"현실 시간: {realTimeFormatted}\n게임 시간: {gameTimeFormatted}";
    }
}
