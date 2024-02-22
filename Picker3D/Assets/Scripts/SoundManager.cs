using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    public List<Sound> sounds = new List<Sound>();
    public Dictionary<string, AudioClip[]> clipPool;
    public Dictionary<string, AudioSource> sourcePool;
    bool soundOn = true;
    public Dictionary<string, Vector2> minMaxPitchPool;
    public Dictionary<string, bool> randomizedPitchPool;
    [SerializeField] float pitchInrementionValue = 0.1f;
    Dictionary<string, float> currentPitchValues = new Dictionary<string, float>();
    Dictionary<string, bool> pitchIncrementIncreasing = new Dictionary<string, bool>();

    private void Awake()
    {
        Instance = this;

        clipPool = new Dictionary<string, AudioClip[]>();
        sourcePool = new Dictionary<string, AudioSource>();
        minMaxPitchPool = new Dictionary<string, Vector2>();
        randomizedPitchPool = new Dictionary<string, bool>();

        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].clip.Length == 0 || sounds[i].audioSource == null)
            {
                continue;
            }
            clipPool.Add(sounds[i].tag, sounds[i].clip);
            sourcePool.Add(sounds[i].tag, sounds[i].audioSource);
            minMaxPitchPool.Add(sounds[i].tag, sounds[i].pitchMinMax);
            randomizedPitchPool.Add(sounds[i].tag, sounds[i].randomizedPitch);
        }
    }

    private void Start()
    {
        soundOn = SaveSystem<bool>.Load("sound", true);
    }

    public void Play(string tag)
    {
        if (!soundOn)
            return;

        if (!clipPool.ContainsKey(tag))
        {
            return;
        }

        if (clipPool[tag] != null)
        {
            AudioClip[] clips = clipPool[tag];
            SetPitch(tag);

            sourcePool[tag].PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }

    void SetPitch(string tag)
    {
        if (randomizedPitchPool[tag])
        {
            sourcePool[tag].pitch = Random.Range(minMaxPitchPool[tag].x, minMaxPitchPool[tag].y);
        }
        else
        {
            if (!currentPitchValues.ContainsKey(tag))
            {
                currentPitchValues.Add(tag, minMaxPitchPool[tag].x);
                pitchIncrementIncreasing.Add(tag, true);
            }
            sourcePool[tag].pitch = currentPitchValues[tag];

            currentPitchValues[tag] += pitchInrementionValue * (pitchIncrementIncreasing[tag] ? 1 : -1);
            if (currentPitchValues[tag] > minMaxPitchPool[tag].y)
            {
                currentPitchValues[tag] = minMaxPitchPool[tag].y;
                pitchIncrementIncreasing[tag] = false;
            }
            else if (currentPitchValues[tag] < minMaxPitchPool[tag].x)
            {
                currentPitchValues[tag] = minMaxPitchPool[tag].x;
                pitchIncrementIncreasing[tag] = true;
            }

        }
    }
}

[System.Serializable]
public class Sound
{
    public string tag;
    public AudioClip[] clip;
    public AudioSource audioSource;
    public Vector2 pitchMinMax = Vector2.one;
    public bool randomizedPitch;
}