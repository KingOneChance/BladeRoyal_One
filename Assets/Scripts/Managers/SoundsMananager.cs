using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectSoundType
{
    Jump,
    JumpSkill,
    Randing,
    Sheild,
    AttackSkill,
    Button,
}

public class SoundsMananager : MonoBehaviour
{
    #region singltone
    private static SoundsMananager _instance;
    public static SoundsMananager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundsMananager>();
                if (_instance == null)
                    _instance = new GameObject().AddComponent<SoundsMananager>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private AudioClip[] MonsterHitSounds = null;
    [SerializeField] private AudioClip[] stageBgm = null;
    [SerializeField] private AudioClip startBgm = null;
    [SerializeField] private AudioClip jumpSound = null;
    [SerializeField] private AudioClip randingSound = null;
    [SerializeField] private AudioClip shieldSound = null;
    [SerializeField] private AudioClip jumpSkillSound = null;
    [SerializeField] private AudioClip attackSkillSound = null;
    [SerializeField] private AudioClip buttonClickSount = null;

    AudioSource audioSource = null;
    Dictionary<EffectSoundType, AudioClip> soundSourec= new Dictionary<EffectSoundType, AudioClip>();
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void SetSoundSourceToDic()
    {
        soundSourec[EffectSoundType.Jump] = jumpSound;
        soundSourec[EffectSoundType.JumpSkill] = jumpSkillSound;
        soundSourec[EffectSoundType.Randing] = randingSound;
        soundSourec[EffectSoundType.Sheild] = shieldSound;
        soundSourec[EffectSoundType.AttackSkill] = attackSkillSound;
        soundSourec[EffectSoundType.Button] = buttonClickSount;
    }
    public void StartSceneBGM()
    {
        audioSource.clip = startBgm;
        audioSource.playOnAwake = false;
        audioSource.mute = false;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void TurnOnPlayerOnShot(EffectSoundType type) => audioSource.PlayOneShot(soundSourec[type]);
    public void TurnOnHitSounds(int num) => audioSource.PlayOneShot(MonsterHitSounds[num]);
    public void TurnOnStageBGM()
    {
        audioSource.Stop();
        audioSource.clip = stageBgm[GameManager.Instance.GetMapStageNum()];
        audioSource.loop = true;
        audioSource.Play();
    }
}
