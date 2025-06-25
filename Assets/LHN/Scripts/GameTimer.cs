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
    public TextMeshProUGUI gameTimeText; // UI�� ������ �ؽ�Ʈ

    void Start()
    {
        // ���� ���� ���� ���
        realStartTime = DateTime.Now;
    }
    DateTime beforeTime = new DateTime(1, 1, 1, 6, 0, 0);
    void Update()
    {
        DateTime now = DateTime.Now;
        TimeSpan realElapsed = now - realStartTime;
        double gameSeconds = realElapsed.TotalSeconds * GameMultiplier;

        // ������ �Ǵ� 6�ú��� �帥 ���� �ð� ����
        DateTime gameTime = new DateTime(1, 1, 1, 6, 0, 0).AddSeconds(gameSeconds);

        string gameTimeFormatted = gameTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US"));

        if (gameTimeFormatted == "AM 06" && beforeTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US")) == "AM 05")
        {
            count += 1;
        }
        gameTimeText.text = $"DAY : {count}, {gameTimeFormatted}";
        // ���ǹ� �ð� �񱳸� ���� ���� �ð� ����
        beforeTime = gameTime;
    }
}
