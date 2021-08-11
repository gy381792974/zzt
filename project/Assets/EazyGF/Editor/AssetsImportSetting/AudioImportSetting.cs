using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioImportSetting : AssetPostprocessor
{
    //音乐文件导入之前调用
    public void OnPreprocessAudio()
    {
        AudioImporter audio = assetImporter as AudioImporter;
        if (audio != null)
        {
            audio.SetOverrideSampleSettings("Android", GetAndroidMusicSetting());
            audio.SetOverrideSampleSettings("iOS", GetIOSMusicSetting());
        }
    }

    private AudioImporterSampleSettings GetAndroidMusicSetting()
    {
        AudioImporterSampleSettings settings = new AudioImporterSampleSettings();
        settings.loadType = AudioClipLoadType.Streaming;
        settings.compressionFormat = AudioCompressionFormat.Vorbis;
        settings.quality = 100;
        settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        return settings;
    }

    private AudioImporterSampleSettings GetIOSMusicSetting()
    {
        AudioImporterSampleSettings settings = new AudioImporterSampleSettings();
        settings.loadType = AudioClipLoadType.Streaming;
        settings.compressionFormat = AudioCompressionFormat.Vorbis;
        settings.quality = 100;
        settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        return settings;
    }

    //音乐文件导入之后调用
    public void OnPostprocessAudio(AudioClip clip)
    {

    }
}
