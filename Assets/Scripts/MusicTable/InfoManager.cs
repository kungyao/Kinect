using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class gameInfos
{
    public string music_name { get; set; }
    public double music_length { get; set; }
    public int music_samples { get; set; }
    public int music_bpm { get; set; }
    public List<gameInfo> game_infos { get; set; }

    public gameInfos()
    {

        music_name = "";
        music_length = 0;
        music_samples = 0;
        music_bpm = 0;
        game_infos = new List<gameInfo>();
    }
}

public class gameInfo
{
    public int _time;
    public int _start_grid;
    public int _end_grid;
    public int _rotateIndex;

    public gameInfo()
    {
        _time = 0;
        _start_grid = 0;
        _end_grid = 0;
        _rotateIndex = 0;
    }
    public gameInfo(int time, int s_grid, int e_grid, int rot_idx)
    {
        _time = time;
        _start_grid = s_grid;
        _end_grid = e_grid;
        _rotateIndex = rot_idx;
    }
}

public struct frameInfo
{
    public List<Vector2> relations;
    public List<int> rotateIndice;
    public frameInfo(List<Vector2> relations, List<int> rotateIndice)
    {
        this.relations = relations;
        this.rotateIndice = rotateIndice;
    }

    public void print()
    {
        foreach(Vector2 v in relations)
            Debug.Log(v);
        foreach(int i in rotateIndice)
            Debug.Log(i);
    }
};

public static class InfoManager
{
    public static Dictionary<int, frameInfo> allInfo = new Dictionary<int, frameInfo>();

    public static void Clear()
    {
        allInfo.Clear();
    }

    public static void insert(int time, List<Vector2> relations, List<int> rotateIndice)
    {
        frameInfo info = new frameInfo(relations, rotateIndice);
        allInfo[time] = info;
    }

    public static bool isExist(int time)
    {
        return allInfo.ContainsKey(time);
    }

    public static frameInfo getInfo(int time)
    {
        return allInfo[time];
    }

    public static void writeJson()
    {
        gameInfos infos = new gameInfos();
        AudioClip tmpClip = MusicMgr.GetMusic();
        infos.music_name = tmpClip.name;
        infos.music_length = tmpClip.length;
        infos.music_samples = MusicMgr.MusicSamples;

        foreach (KeyValuePair<int, frameInfo> f in allInfo)
        {         
            for(int i = 0; i < f.Value.relations.Count; i++)
            {
                frameInfo finfo = f.Value;
                gameInfo info = new gameInfo(f.Key, (int)finfo.relations[i][0], (int)finfo.relations[i][1], finfo.rotateIndice[i]);
                infos.game_infos.Add(info);
            }
        }
        string result = JsonMapper.ToJson(infos);

        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, tmpClip.name + ".json"));
        file.Write(result);
        file.Close();
    }
    
    public static gameInfos readJson(string musicName)
    {
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.streamingAssetsPath, musicName + ".json"));
        string data = file.ReadToEnd();
        gameInfos infos = JsonMapper.ToObject<gameInfos>(data);
        return infos;
    }
}
