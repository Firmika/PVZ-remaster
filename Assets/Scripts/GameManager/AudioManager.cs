using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    // 用于播放音乐
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> dictAudio;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        dictAudio = new Dictionary<string, AudioClip>();
    }

    // 辅助函数：加载音频，需要保证音频文件在Resources文件夹下
    private AudioClip LoadAudio(string path)
    {
        return (AudioClip)Resources.Load(path);
    }

    // 辅助函数：获取音频并缓存，避免重复加载
    private AudioClip GetAudio(string path)
    {
        if (!dictAudio.ContainsKey(path))
        {
            dictAudio[path] = LoadAudio(path);
        }
        return dictAudio[path];
    }

    public void PlayBGM(string name, float volume = 1.0f)
    {
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlaySE(string path, float volume = 1.0f)
    {
        // PlayOneShot可以叠加播放
        audioSource.PlayOneShot(GetAudio(path),volume);
    }

    public void PlaySE(AudioSource audioSource, string path, float volume = 1f)
    {
        audioSource.PlayOneShot(GetAudio(path),volume);
    }

}
