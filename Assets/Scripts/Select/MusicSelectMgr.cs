using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSelectMgr : MonoBehaviour
{
    public List<Button> _mBtn = new List<Button>();
    public List<string> _data = new List<string>();

    private int[] _indexOffset = new int[5] { -2, -1, 0, 1, 2 };
    private List<Text> _mBtnText = new List<Text>();
    private int _indexNow = 0;
    // Start is called before the first frame update
    void Start()
    {
        List<bool> check = MusicMgr.LoadMusic(_data);
        int rmOffset = 0;
        //remove file which can't find
        for(int i = 0; i < check.Count; i++)
        {
            if (!check[i])
            {
                _data.RemoveAt(i - rmOffset);
                rmOffset++;
            }
        }
        foreach (Button btn in _mBtn)
        {
            _mBtnText.Add(btn.gameObject.GetComponentInChildren<Text>());
        }
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _indexNow = _indexNow == 0 ? 0 : _indexNow - 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int last = Mathf.Max(_data.Count - 1, 0);
            _indexNow = _indexNow >= last ? last : _indexNow + 1;
        }
        UpdateBtnState();
    }

    void UpdateBtnState()
    {
        //for(int i = 0; i < _mBtn.Count; i++)
        //{
        //    if (_mBtn[i].interactable)
        //    {
        //        _mBtnText[i].text = _data[_indexNow + _indexOffset[i]].ToString();
        //    }
        //    else
        //    {
        //        _mBtnText[i].text = "";
        //    }
        //}
        //1
        if (_indexNow - 2 >= 0)
        {
           _mBtn[0].interactable = true;
            _mBtnText[0].text = _data[_indexNow - 2].ToString();
        }
        else
        {
            _mBtn[0].interactable = false;
            _mBtnText[0].text = "";
        }
        //2
        if (_indexNow - 1 >= 0)
        {
            _mBtn[1].interactable = true;
            _mBtnText[1].text = _data[_indexNow - 1].ToString();
        }
        else
        {
            _mBtn[1].interactable = false;
            _mBtnText[1].text = "";
        }
        //3
        if (_data.Count != 0) 
        {
            _mBtn[2].interactable = true;
            _mBtnText[2].text = _data[_indexNow].ToString();
        }
        else
        {
            _mBtn[2].interactable = false;
            _mBtnText[2].text = "";
        }
        //4
        if (_indexNow + 1 < _data.Count) 
        {
            _mBtn[3].interactable = true;
            _mBtnText[3].text = _data[_indexNow + 1].ToString();
        }
        else
        {
            _mBtn[3].interactable = false;
            _mBtnText[3].text = "";
        }
        //5
        if (_indexNow + 2 < _data.Count) 
        {
            _mBtn[4].interactable = true;
            _mBtnText[4].text = _data[_indexNow + 2].ToString();
        }
        else
        {
            _mBtn[4].interactable = false;
            _mBtnText[4].text = "";
        }
    }

    private void Initialize()
    {
        MusicMgr.IsInit = true;
        //get music data
        _indexNow = MusicMgr.IndexNow;
    }

    public void OnBtnClick(int offset)
    {
        if (offset == 0)
        {
            MusicMgr.IndexNow = _indexNow;
            SceneManager.LoadSceneAsync("Test", LoadSceneMode.Single);
        }
        else
        {
            _indexNow += offset;
        }
    }
}
