using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickEvent : MonoBehaviour
{
    // (false = checkboard, true = rotateboard)
    public bool is_rot;
    public SongEditor se;
    //checkboard (left = 0 or right = 1)
    public int sort;
    //0 ~ 11
    public int b_id;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        if(is_rot)
            btn.onClick.AddListener(IsClick2);
        else
            btn.onClick.AddListener(IsClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //two checkboard button event
    private void IsClick()
    {
        se.UpdateCheckBoard(sort, b_id);
    }

    //mid rotate board button event
    private void IsClick2()
    {
        se.UpdateRotateBoard(b_id);
    }
}
