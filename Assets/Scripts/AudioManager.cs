using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    
    public AudioMixerGroup sfxMixer;
    [Header("BGM")]
    public AudioClip bgmBeginning;
    public AudioClip bgmEnding;
    public AudioClip bgmGameOver;
    [Header("UI")]
    public AudioClip uiButtonHighlight;
    public AudioClip uiButtonClick;
    public AudioClip uiToast;
    [Header("SFX")] 
    public AudioClip[] walk;
    public AudioClip enterCave;
    public AudioClip getKey;
    public AudioClip getSource;
    public AudioClip openLock;
    public AudioClip openBox;
    public AudioClip puzzleSolved;
    public AudioClip pressButton;
    public AudioClip pressPlate;
    public AudioClip useSwitch;
    public AudioClip litTorch;
    public AudioClip shutTorch;
    public AudioClip bombExplode;
    public AudioClip useSpare;
    public AudioClip useShoes;
    
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource playerSource;

    private int bgmState = 0;
    
    private void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxMixer;
        playerSource = gameObject.AddComponent<AudioSource>();
        playerSource.outputAudioMixerGroup = sfxMixer;
    }

    public void PlayBGM(int caveId)
    {
        if ((bgmState==1&&caveId==0)||(bgmState==0&&caveId==1)) return;
        if (caveId == bgmState) return;
        bgmState = caveId;
        bgmSource.DOFade(0, 1f).OnComplete(() =>
        {
            bgmSource.clip = GameManager.instance.caveInstance[caveId].caveMusic;
            bgmSource.Play();
            bgmSource.DOFade(1, 2f);
        });
    }
    
    public void PlayBGM(AudioClip clip)
    {
        bgmState = -1;
        bgmSource.DOFade(0, 1f).OnComplete(() =>
        {
            bgmSource.clip = clip;
            bgmSource.Play();
            bgmSource.DOFade(1, 2f);
        });
    }
    
    public void PlaySfx(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayStepsAudio()
    {
        var index = Random.Range(0, walk.Length);
        playerSource.clip = walk[index];
        playerSource.Play();
    }

    public IEnumerator PlayPuzzleSolved()
    {
        yield return new WaitForSeconds(0.5f);
        sfxSource.clip = puzzleSolved;
        sfxSource.Play();
    }
}
