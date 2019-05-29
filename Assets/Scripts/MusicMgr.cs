using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr
{
    static public bool IsInit= false;
    static public int IndexNow = 0;

    static private List<AudioClip> _music = new List<AudioClip>();

    static public AudioClip GetMusic()
    {
        if (IndexNow >= _music.Count || IndexNow < 0)
            return (AudioClip)Resources.Load<AudioClip>("Way Back Home");
        return _music[IndexNow];
    }

    static public List<bool> LoadMusic(List<string> names, bool ifClear = true)
    {
        if (ifClear)
        {
            _music.Clear();
        }
        List<bool> checkBit = new List<bool>();
        foreach (string name in names)
        {
            AudioClip tmpMusic = (AudioClip)Resources.Load(name);
            if (tmpMusic)
            {
                checkBit.Add(true);
                _music.Add(tmpMusic);
            }
            else
            {
                checkBit.Add(false);
            }
        }
        return checkBit;
    }

    static public void UnloadMusic()
    {
        foreach (AudioClip clip in _music)
            clip.UnloadAudioData();
    }
}
