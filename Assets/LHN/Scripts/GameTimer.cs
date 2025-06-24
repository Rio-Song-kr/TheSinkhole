using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    private const double GameMultiplier = 3600.0 / 30.0;
    private DateTime realStartTime;

    public TextMeshProUGUI gameTimeText; // UI�� ������ �ؽ�Ʈ

    void Start()
    {
        // ���� ���� ���� ���
        realStartTime = DateTime.Now;
    }

    void Update()
    {
        DateTime now = DateTime.Now;
        TimeSpan realElapsed = now - realStartTime;
        double gameSeconds = realElapsed.TotalSeconds * GameMultiplier;

        // ������ �Ǵ� 0�ú��� �帥 ���� �ð� ����
        DateTime gameTime = new DateTime(1, 1, 1, 6, 0, 0).AddSeconds(gameSeconds);

        string realTimeFormatted = now.ToString("tt hh:mm");
        string gameTimeFormatted = gameTime.ToString("tt hh:mm");

        gameTimeText.text = $"���� �ð�: {realTimeFormatted}\n���� �ð�: {gameTimeFormatted}";
    }
}
