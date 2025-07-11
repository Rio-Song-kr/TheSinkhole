using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.EventSystems;
using System.Collections;
using Util;

public class GameTimer : MonoBehaviour
{
    [Header("Day Night Icons")]
    [SerializeField] private Image m_icon;
    [SerializeField] private Sprite m_daySprite;
    [SerializeField] private Sprite m_nightSprite;

    private const double GameMultiplier = 3600.0 / 30.0;
    // private const double GameMultiplier = 3600.0 / 5.0;
    // private const double GameMultiplier = 3600.0 / 1.0;
    private DateTime realStartTime;
    private int count = 1;

    public TextMeshProUGUI gameTimeText; // UI에 연결할 텍스트
    public static int Day;
    public static bool IsDay = true;

    public Light m_light;
    public Color m_dayColor;
    public Color m_nightColor;

    private float m_RealtimeCount = 0;

    private void Start()
    {
        // 게임 시작 시점 기록
        realStartTime = DateTime.Now;
        m_icon.sprite = m_daySprite;
    }

    private DateTime beforeTime = new DateTime(1, 1, 1, 6, 0, 0);

    private void Update()
    {
        var now = DateTime.Now;
        var realElapsed = now - realStartTime;
        double gameSeconds = realElapsed.TotalSeconds * GameMultiplier;
        m_RealtimeCount += Time.deltaTime;

        // 기준이 되는 6시부터 흐른 게임 시간 생성
        var gameTime = new DateTime(1, 1, 1, 6, 0, 0).AddSeconds(gameSeconds);

        string gameTimeFormatted = gameTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US"));

        if (gameTimeFormatted == "AM 06" && beforeTime.ToString("tt hh", CultureInfo.GetCultureInfo("en-US")) == "AM 05")
        {
            count += 1;
            Day = count;
        }

        if (gameTimeFormatted == "AM 06")
        {
            if (m_icon.sprite != m_daySprite)
                m_icon.sprite = m_daySprite;
            IsDay = true;
            m_light.color = m_dayColor;

            GameManager.Instance.Audio.PlayBGM(AudioClipName.A_Pioneer);
        }
        else if (gameTimeFormatted == "PM 06")
        {
            if (m_icon.sprite != m_nightSprite)
                m_icon.sprite = m_nightSprite;
            IsDay = false;
            m_light.color = m_nightColor;
            GameManager.Instance.Audio.PlayBGM(AudioClipName.F_Night);
        }

        if (m_RealtimeCount >= 60f)
        {
            PlayerStatus.Instance.OnRealTimeEffect();
            m_RealtimeCount = 0;
        }

        gameTimeText.text = $"DAY {count:D2}       {gameTimeFormatted}";
        // 조건문 시간 비교를 위한 이전 시간 저장
        beforeTime = gameTime;
    }
}