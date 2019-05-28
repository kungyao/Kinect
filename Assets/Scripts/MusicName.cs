using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicName : MonoBehaviour
{
    public AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        print(_clip.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
