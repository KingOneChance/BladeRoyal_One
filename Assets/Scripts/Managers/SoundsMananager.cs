using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private AudioClip[] MonsterDieSounds = null; 
    [SerializeField] private AudioClip[] MonsterHitSounds = null;
    [SerializeField] private AudioClip[] stageBgm = null;
    [SerializeField] private AudioClip startBgm = null;
    AudioSource audioSource=null;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void StartSceneBGM()
    {
        audioSource.clip = startBgm;
        audioSource.playOnAwake = false;
        audioSource.mute = false;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void TurnOnDieSounds(int num)
    {
        audioSource.PlayOneShot(MonsterDieSounds[num]);
    }
    public void TurnOnHitSounds(int num)
    {
        audioSource.PlayOneShot(MonsterHitSounds[num]);
    }
    public void TurnOnStageBGM()
    {
        audioSource.Stop();
        audioSource.clip = stageBgm[GameManager.Instance.GetMapStageNum()];
        audioSource.loop = true;
        audioSource.Play();
    }
}
