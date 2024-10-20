using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicLayer { Ambience, Drones, Battle, LowLife }

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance => _instance;

    private List<MusicFader> _musicFaders;

    private EventInstance _music;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        _musicFaders = new();

        _music = RuntimeManager.CreateInstance("event:/Music/BangekoKuraStems140BPM_FullAmbience");
        _music.start();
    }

    public void PlayMusicLayer(MusicLayer musicLayer, bool play)
    {
        float startVolume = play ? 0f : 1f;
        foreach (MusicFader fader in _musicFaders)
        {
            if (fader.MusicLayer == musicLayer && fader.Play == play) return;
            else if (fader.MusicLayer == musicLayer && fader.Play != play)
            {
                startVolume = fader.Volume;
                fader.Over = true;
                break;
            }
        }

        MusicFader newMusicFader = new(_music, musicLayer, play, startVolume);
        _musicFaders.Add(newMusicFader);
    }

    private void Update()
    {
        List<MusicFader> fadersToRemove = new();
        foreach (MusicFader fader in _musicFaders)
        {
            if (fader.Over)
            {
                fadersToRemove.Add(fader);
            }
        }

        foreach (MusicFader fader in fadersToRemove)
        {
            _musicFaders.Remove(fader);
        }

        foreach (MusicFader fader in _musicFaders)
        {
            fader.UpdateLogic();
        }
    }
}
