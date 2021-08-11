using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicMgr : Singleton<MusicMgr>
{
    static string fileName = "settingData.data";
    static string savePath;
    public static string SavePath
    {
        get
        {
            if (string.IsNullOrEmpty(savePath))
            {
                savePath = UICommonUtil.GetSerSavePath(fileName);
            }
            return savePath;
        }
    }
    //默认背景音乐播放源
    public AudioSource defaultBGAudioSource;
    //默认音效播放源
    public AudioSource defaultEffectAudioSource;

    //所有音效字典
    private readonly Dictionary<string, AudioClip> allMusicDictionary = new Dictionary<string, AudioClip>();

    //所有音效的播放源
    private readonly List<AudioSource> allEffectAudioSourceList = new List<AudioSource>();

    /// <summary>
    /// 是否暂停背景音乐
    /// </summary>
    private bool isPauseBG;
    public bool IsPauseBG
    {
        get
        {
            return isPauseBG;
        }
        set
        {
            isPauseBG = value;
        }
    }


    /// <summary>
    /// 是否暂停音效
    /// </summary>
    private bool isPauseEffect;
    bool isCloseBG;
    bool isCloseEff;
    public bool IsCloseBG
    {
        get
        {
            return isCloseBG;
        }
        set
        {
            isCloseBG = value;
            if (isCloseBG)
                PauseBG();
            else
                ResumBG();
        }
    }
    public bool IsCloseEff { get => isCloseEff; set => isCloseEff = value; }

    /// <summary>
    /// 音效资源路径
    /// </summary>
    private readonly string soundAB_Name = AB_ResFilePath.SoundsFolderName.ToLower();

    public void Init()
    {
        LoadPlayerSetting();
        allEffectAudioSourceList.Add(defaultEffectAudioSource);
        AudioClip[] audioClips = AssetMgr.Instance.LoadAllAssets<AudioClip>(soundAB_Name);
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (!allMusicDictionary.ContainsKey(audioClips[i].name))
            {
                allMusicDictionary.Add(audioClips[i].name, audioClips[i]);
            }
            else
            {
                Debug.LogError($"  music {audioClips[i].name} repeat");
            }

        }

        ResumBG();
    }


    //播放背景音乐
    public void PlayMusicBG(string audioName, bool loop = true)
    {
        if (IsCloseBG)
        {
            return;
        }

        if (IsPauseBG)
        {
            return;
        }
        //如果没用找到缓存则加载
        if (!allMusicDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            Debug.LogError("找不到：" + audioName);
            return;
        }

        Debug.LogWarning("PlayMusicBG：" + audioName);

        if (defaultBGAudioSource != null)
        {
            defaultBGAudioSource.loop = loop;
            defaultBGAudioSource.clip = clip;
            defaultBGAudioSource.Play();
        }
        else
        {
            Debug.LogError("找不到默认播放源！");
        }
    }

    public bool IsPlayingMusic()
    {
        if (IsPauseBG)
        {
            return true;
        }

        return defaultBGAudioSource.isPlaying;
    }

    string PlayingAudioEff;
    //播放音效
    public void PlayMusicEff(string audioName, AudioSource otherAudioSource = null, bool loop = false, float pitch = 1)
    {
        if (IsCloseEff)
        {
            return;
        }

        if (isPauseEffect)
        {
            return;
        }
        if (!allMusicDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            Debug.LogError("找不到：" + audioName);
            return;
        }
        Debug.LogWarning("播放：" + audioName);

        if (otherAudioSource == null)
        {
            defaultEffectAudioSource.clip = clip;
            defaultEffectAudioSource.loop = loop;
            defaultEffectAudioSource.pitch = pitch;
            defaultEffectAudioSource.PlayOneShot(clip);
        }
        else
        {
            otherAudioSource.clip = clip;
            otherAudioSource.loop = loop;
            otherAudioSource.pitch = pitch;
            otherAudioSource.PlayOneShot(clip);
            if (!allEffectAudioSourceList.Contains(otherAudioSource))
            {
                allEffectAudioSourceList.Add(otherAudioSource);
            }
        }
    }



    public void PauseBG()
    {
        defaultBGAudioSource.Pause();
        IsPauseBG = true;
    }

    public void ResumBG()
    {
        defaultBGAudioSource.UnPause();
        IsPauseBG = false;
    }

    public void PauseEffect()
    {
        for (int i = 0; i < allEffectAudioSourceList.Count; i++)
        {
            allEffectAudioSourceList[i].Pause();
            Debug.Log("音效名字：" + allEffectAudioSourceList[i].name);
        }

        isPauseEffect = true;
    }

    public void ResumEffect()
    {
        for (int i = 0; i < allEffectAudioSourceList.Count; i++)
        {
            allEffectAudioSourceList[i].UnPause();
        }
        isPauseEffect = false;
    }

    public void PauseAll()
    {
        for (int i = 0; i < allEffectAudioSourceList.Count; i++)
        {
            allEffectAudioSourceList[i].Pause();
        }
        defaultBGAudioSource.Pause();

        IsPauseBG = true;
        isPauseEffect = true;
    }
    public void ResumAll()
    {
        for (int i = 0; i < allEffectAudioSourceList.Count; i++)
        {
            allEffectAudioSourceList[i].UnPause();
        }
        defaultBGAudioSource.UnPause();

        IsPauseBG = false;
        isPauseEffect = false;
    }

    /// <summary>
    /// 重载游戏清空游戏字典
    /// </summary>
    public void RestartMusic()
    {

        allEffectAudioSourceList.Clear();

        //allMusicDictionary.Clear();

    }

    public void LoadPlayerSetting()
    {
        bool isSuccess;
        SettingVoiceData data = SerializHelp.DeserializeFileToObj<SettingVoiceData>(SavePath, out isSuccess);
        if (isSuccess)
        {
            IsCloseBG = data.isPauseBGM;
            IsCloseEff = data.isPauseEff;
        }
        else
        {
            SavePlayerSetting();
        }
    }

    public void SavePlayerSetting()
    {
        SettingVoiceData data = new SettingVoiceData();
        data.isPauseBGM = IsCloseBG;
        data.isPauseEff = IsCloseEff;
        SerializHelp.SerializeFile<SettingVoiceData>(SavePath, data);
    }
}
