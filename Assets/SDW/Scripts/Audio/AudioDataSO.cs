using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioDataSO", menuName = "Scriptable Objects/AudioDataSo")]
public class AudioDataSO : ScriptableObject
{
    [Header("Audio Mixer Groups")]
    public AudioMixerGroup BGMAudioMixer;
    public AudioMixerGroup SFXAudioMixer;

    [Header("BGM Clips")]
    //# 낮
    public AudioClip A_Pioneer;
    //# 밤
    public AudioClip F_Night;

    [Header("SFX Clips")]

    //# 인트로
    public AudioClip Intro_Rain;
    public AudioClip M_Hole;
    public AudioClip RainHole;

    //# 캐릭터
    public AudioClip I_Click;
    public AudioClip C_Walk_C_RUN;
    public AudioClip I_Start;
    public AudioClip I_Duaring;
    public AudioClip I_Done;
    public AudioClip Swing;
    public AudioClip C_Drink;
    public AudioClip C_Eat;
    public AudioClip C_Low;
    public AudioClip C_Hurt_C_Pain;
    public AudioClip C_Thirst;
    public AudioClip C_Mind;
    public AudioClip C_Attack;
    public AudioClip C_Death;
    public AudioClip C_Hungry;
    public AudioClip I_Tobacco;
    public AudioClip I_Bandage;

    //#SFX
    public AudioClip TurretFire;
    public AudioClip ChangeTile;
    public AudioClip PopUp;
    public AudioClip Spider_Idle;
    public AudioClip Worm_Idle;
    public AudioClip Monster_Attack;
    public AudioClip Monster_Death;
}