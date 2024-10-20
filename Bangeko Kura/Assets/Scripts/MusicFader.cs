using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFader
{
    private EventInstance _music;
    private MusicLayer _musicLayer;
    private bool _play;
    private float _volume;
    private bool _over = false;

    public MusicLayer MusicLayer { get { return _musicLayer; } }
    public bool Play { get { return _play; } }
    public float Volume { get { return _volume; } }
    public bool Over {  get { return _over; } set { _over = value; } }

    public MusicFader(EventInstance music, MusicLayer musicLayer, bool play, float startVolume)
    {
        _music = music;
        _musicLayer = musicLayer;
        _play = play;

        _volume = startVolume;
    }

    public void UpdateLogic()
    {
        if (_play)
        {
            if (_volume < 1f)
            {
                _volume += Time.deltaTime;

                if (_volume >= 1f)
                {
                    _over = true;
                }
            }
        } else
        {
            if (_volume > 0f)
            {
                _volume -= Time.deltaTime;

                if (_volume <= 0f)
                {
                    _over = true;
                }
            }
        }

        _music.setParameterByName(_musicLayer.ToString() + " Volume", _volume);
    }
}
