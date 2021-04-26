using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] Playlist;
    public float WaitTime;
    public bool PlayRandom;
    public bool IsPlaying;
    public float FadeOutTime;

    public AudioSource Player;

    private int _lastPlayedSongId;
    private float _waitTimer;
    private bool _realPlayState;
    private bool _isMuted;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.MusicPlayer = this;

        _lastPlayedSongId = -1;
        GetValues();
        GameManager.instance.UI.HandleAudioUI();
    }

    private void GetValues()
    {
        if (Player == null)
            Player = GetComponent<AudioSource>();

        //IsPlaying = GameManager.instance.MusicEnabled;
        //Player.volume = GameManager.instance.MusicVolume;
        _isMuted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            if (!Player.isPlaying)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer < 0f)
                {
                    PlayNextSong();
                }
            }
        }
        else
        {
            if (Player.isPlaying)
            {
                Player.Stop();
            }
        }

    }

    public void Mute()
    {
        _isMuted = true;
        _realPlayState = IsPlaying;
        IsPlaying = false;
        Player.Stop();
    }

    public void UnMute()
    {
        if (_isMuted)
        {
            IsPlaying = _realPlayState;
            _isMuted = false;
        }
    }

    public IEnumerator FadeOut(float fadeTime)
    {
        float startVolume = Player.volume;

        while (Player.volume > 0)
        {
            Player.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        Player.Stop();
        Player.volume = startVolume;
    }

    public void SetVolume(float val)
    {
        Player.volume = val;
    }

    public float GetVolume()
    {
        return Player.volume;
    }


    public void PlayNextSong()
    {
        int nextId = 0;
        if (PlayRandom)
        {
            nextId = _lastPlayedSongId;
            int i = 0;
            while (nextId == _lastPlayedSongId)
            {
                i++;
                nextId = UnityEngine.Random.Range(0, Playlist.Length);
                if (i > 100)
                    break;
            }
        }
        else
        {
            nextId = _lastPlayedSongId + 1;
            if (nextId == Playlist.Length)
                nextId = 0;
        }

        Player.clip = Playlist[nextId];
        Player.Play();
        _lastPlayedSongId = nextId;
    }
}
