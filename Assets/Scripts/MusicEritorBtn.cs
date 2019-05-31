using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicEritorBtn : MonoBehaviour
{
    public GameObject _musicTrackGroup;
    public GameObject _frameEditorGroup;

    public void OnEditClick()
    {
        _musicTrackGroup.SetActive(false);
        _frameEditorGroup.SetActive(true);
    }

    public void OnReadClick()
    {
        InfoManager.readJson(MusicMgr.GetMusic().name);
    }

    public void OnSaveClick()
    {
        InfoManager.writeJson();
    }

    public void OnFrameBackClick()
    {
        _musicTrackGroup.SetActive(true);
        _frameEditorGroup.SetActive(false);
    }

    public void OnTotalBackClick()
    {
        InfoManager.Clear();
        SceneManager.LoadSceneAsync("MusicSelect", LoadSceneMode.Single);
    }
}
