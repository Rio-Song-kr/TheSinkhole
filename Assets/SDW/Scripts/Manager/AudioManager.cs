using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioDataSO _audioData;
    public AudioDataSO AudioData => _audioData;

    //# BGM 전용 AudioSource, SFX는 ObjectPool을 사용
    private AudioSource _bgmAudioSource;

    private Dictionary<AudioClipName, AudioClip> _audioClips = new Dictionary<AudioClipName, AudioClip>(10);

    private void Awake()
    {
        InitAudioClip();
        InitBGMAudioSource();
    }

    private void Start() => PlayBGM(_audioClips[AudioClipName.M_Hole]);

    //# enum을 사용하여 Audio Clip을 Dictionary로 관리
    private void InitAudioClip()
    {
        _audioClips[AudioClipName.Intro_Rain] = _audioData.Intro_Rain;
        _audioClips[AudioClipName.M_Hole] = _audioData.M_Hole;
        _audioClips[AudioClipName.RainHole] = _audioData.RainHole;
        //# 캐릭터
        _audioClips[AudioClipName.I_Click] = _audioData.I_Click;
        _audioClips[AudioClipName.C_Walk_C_RUN] = _audioData.C_Walk_C_RUN;
        _audioClips[AudioClipName.I_Start] = _audioData.I_Start;
        _audioClips[AudioClipName.I_Duaring] = _audioData.I_Duaring;
        _audioClips[AudioClipName.I_Done] = _audioData.I_Done;
        _audioClips[AudioClipName.Swing] = _audioData.Swing;
        _audioClips[AudioClipName.C_Drink] = _audioData.C_Drink;
        _audioClips[AudioClipName.C_Eat] = _audioData.C_Eat;
        _audioClips[AudioClipName.C_Low] = _audioData.C_Low;
        _audioClips[AudioClipName.C_Hurt_C_Pain] = _audioData.C_Hurt_C_Pain;
        _audioClips[AudioClipName.C_Thirst] = _audioData.C_Thirst;
        _audioClips[AudioClipName.C_Mind] = _audioData.C_Mind;
        _audioClips[AudioClipName.C_Attack] = _audioData.C_Attack;
        _audioClips[AudioClipName.C_Death] = _audioData.C_Death;
        _audioClips[AudioClipName.C_Hungry] = _audioData.C_Hungry;
        _audioClips[AudioClipName.I_Tobacco] = _audioData.I_Tobacco;
        _audioClips[AudioClipName.I_Bandage] = _audioData.I_Bandage;
        _audioClips[AudioClipName.TurretFire] = _audioData.TurretFire;
        _audioClips[AudioClipName.ChangeTile] = _audioData.ChangeTile;
        _audioClips[AudioClipName.PopUp] = _audioData.PopUp;
        _audioClips[AudioClipName.Spider_Idle] = _audioData.Spider_Idle;
        _audioClips[AudioClipName.Worm_Idle] = _audioData.Worm_Idle;
        _audioClips[AudioClipName.Monster_Attack] = _audioData.Monster_Attack;
        _audioClips[AudioClipName.Monster_Death] = _audioData.Monster_Death;
    }

    private void InitBGMAudioSource()
    {
        if (_bgmAudioSource == null)
        {
            _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }

        //# BGM 설정
        _bgmAudioSource.outputAudioMixerGroup = _audioData.BGMAudioMixer;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.playOnAwake = false;

        //# 2D 사운드 - BGM 입체적으로 들릴 필요가 없음
        _bgmAudioSource.spatialBlend = 0f;
    }

    //# BGM 재생
    public void PlayBGM(AudioClipName clipName)
    {
        if (_audioClips.TryGetValue(clipName, out var clip))
        {
            _bgmAudioSource.clip = clip;
            _bgmAudioSource.Play();
        }
    }

    //# BGM 재생
    public void PlayBGM(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.Play();
    }

    //# SFX 재생 - Pool 사용
    public void PlaySFX(AudioClipName clipName, Vector3 position, bool isUI = false, float volume = 1f, float pitch = 1f)
    {
        if (_audioClips.TryGetValue(clipName, out var clip))
        {
            var audioController = AudioSourcePool.Instance.Pool.Get();

            // audioController.AudioSource.outputAudioMixerGroup = _audioData.SFXAudioMixer;

            // Pool에서 가져온 AudioSource 설정 강제 적용
            if (isUI)
            {
                audioController.AudioSource.spatialBlend = 0f;
            }
            else
            {
                audioController.AudioSource.spatialBlend = 1f;
                audioController.AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                audioController.AudioSource.minDistance = 5f;
                audioController.AudioSource.maxDistance = 30f; // 현재 설정과 맞춤
            }

            audioController.AudioSource.outputAudioMixerGroup = _audioData.SFXAudioMixer;
            audioController.PlayAudio(clip, position, volume, pitch);
        }
    }

    //# UI(Button Click) 재생
    public void Play2DSFX(AudioClipName clipName, float volume = 1f)
    {
        PlaySFX(clipName, Vector3.zero, true, volume);
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
}