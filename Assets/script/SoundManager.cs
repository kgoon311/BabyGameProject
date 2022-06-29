using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SoundType
{
    SE,
    BGM,
    END
}
public class SoundManager : SingletonMono<SoundManager>
{
    Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    Dictionary<SoundType, float> Volumes = new Dictionary<SoundType, float>() { { SoundType.SE, 1 }, { SoundType.BGM, 1 } };
    Dictionary<SoundType, AudioSource> AudioSources = new Dictionary<SoundType, AudioSource>();
    [SerializeField] Slider BgmVolume;
    [SerializeField] Slider SeVolume;

    protected override void OnAwake()
    {
        GameObject Se = new GameObject();
        Se.transform.parent = transform;
        Se.AddComponent<AudioSource>();
        AudioSources[SoundType.SE] = Se.GetComponent<AudioSource>();

        GameObject Bgm = new GameObject();
        Bgm.transform.parent = transform;
        Bgm.AddComponent<AudioSource>().loop = true;
        AudioSources[SoundType.BGM] = Bgm.GetComponent<AudioSource>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        foreach (AudioClip clip in clips)
            sounds[clip.name] = clip;
    }
    public void PlaySound(string clipName, SoundType ClipType = SoundType.SE, float Volume = 1, float Pitch = 1)
    {
        if (ClipType == SoundType.BGM)
        {
            AudioSources[SoundType.BGM].clip = sounds[clipName];
            Volumes[SoundType.BGM] = Volume;
            AudioSources[SoundType.BGM].Play();
        }
        else
        {
            AudioSources[ClipType].pitch = Pitch;
            AudioSources[ClipType].PlayOneShot(sounds[clipName], Volume * SeVolume.value);
        }
    }
    private void Update()
    {
        AudioSources[SoundType.BGM].volume = Volumes[SoundType.BGM] * BgmVolume.value;
    }
}
