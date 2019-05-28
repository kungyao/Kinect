using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicEritorBackBtn : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadSceneAsync("MusicSelect", LoadSceneMode.Single);
    }
}
