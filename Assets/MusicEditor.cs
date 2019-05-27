using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicEditor : MonoBehaviour
{
    public MPMgr _pMgr;
    public Slider _slide;
    public Slider _mapScale;
    public Camera _cam;
    public AudioClip _music;
    //draw music track
    public LineRenderer _track;
    //draw edit track now
    public LineRenderer _trackNow;

    public float _amplitude;
    //[Range(1,100)]
    public float _constSampleScale = 1;

    private bool isInit = false;
    private float[] _data = null;
    private float _sampleScale = 1;
    private float _drawOffset = 0;
    private int _startIndex;
    private int _endIndex;

    void Start()
    {
        Initialize(_music);
    }

    void Update()
    {
        if (isInit)
        {
            _sampleScale = _constSampleScale * _mapScale.value;
            _drawOffset = _cam.orthographicSize / 1.5f;
            float half = _drawOffset  * _sampleScale;
            _slide.value = Mathf.RoundToInt(_slide.value);
            if (_slide.value < half)
            {
                _startIndex = Mathf.FloorToInt(0);
                _endIndex = Mathf.FloorToInt(half * 2);
            }
            else if (_slide.value + half >= _slide.maxValue)
            {
                _startIndex = Mathf.FloorToInt(_slide.maxValue - half * 2);
                _endIndex = Mathf.FloorToInt(_slide.maxValue);
            }
            else
            {
                _startIndex = Mathf.FloorToInt(_slide.value - half);
                _endIndex = Mathf.FloorToInt(_slide.value + half);
            }
            _startIndex = _startIndex < 0 ? 0 : _startIndex;
            _endIndex = _endIndex >= _data.Length ? _data.Length - 1 : _endIndex;

            _track.positionCount = _endIndex - _startIndex + 1;
            DrawLine();
        }
    }

    private void DrawLine()
    {
        if (isInit)
        {
            //print(_startIndex + "   " + _endIndex+"  "+ _data.Length);
            //print(_slide.value - _startIndex);
            float redLine = (_slide.value - _startIndex) / _sampleScale - _drawOffset;
            _trackNow.SetPosition(0, new Vector3(redLine, 100, 0));
            _trackNow.SetPosition(1, new Vector3(redLine, -100, 0));
            for (int i = _startIndex; i <= _endIndex; i++)
            {
                int posIndex = i - _startIndex;
                _track.SetPosition(i - _startIndex, new Vector3(posIndex / _sampleScale - _drawOffset, _data[i] * _amplitude, 0));
            }
        }
        else
        {
            _track.positionCount = 0;
            _trackNow.positionCount = 0;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (isInit)
    //    {
    //        Gizmos.color = Color.red;
    //        float redLine = (_slide.value - _startIndex) / _sampleScale - _drawOffset;
    //        Gizmos.DrawLine(new Vector2(redLine, 100), new Vector2(redLine, -100));
    //        Gizmos.color = Color.white;
    //        Vector2 pre = Vector2.zero;
    //        for (int i = _startIndex; i <= _endIndex; i++) 
    //        {
    //            if (i == _startIndex)
    //            {
    //                pre = new Vector2((i - _startIndex) / _sampleScale - _drawOffset, _data[i] * _amplitude);
    //            }
    //            else
    //            {
    //                Vector2 now = new Vector2((i - _startIndex) / _sampleScale - _drawOffset, _data[i] * _amplitude);
    //                Gizmos.DrawLine(pre, now);
    //                pre = now;
    //            }
    //        }
    //    }
    //}

    private void Initialize(AudioClip music)
    {
        _music = music;
        _sampleScale = _constSampleScale;
        _trackNow.positionCount = 2;
        int tmpMusicLength = Mathf.FloorToInt(_music.length * _constSampleScale);
        int tmpStep = Mathf.FloorToInt(_music.samples / tmpMusicLength);
        //fake data size (sample for each second)
        _data = new float[tmpMusicLength];
        float[] tmpData = new float[_music.samples];
        _music.GetData(tmpData, 0);
        //get fake music data per second
        for (int i = 0; i < _data.Length; i++)
        {
            int inverse = (_data.Length - i - 1) * tmpStep;
            if (inverse >= tmpData.Length)
                _data[i] = tmpData[tmpData.Length - 1];
            else
                _data[i] = tmpData[inverse];
        }
        _slide.minValue = 0;
        _slide.maxValue = Mathf.FloorToInt(_music.length * _constSampleScale);
        _slide.value = 0;
        isInit = true;
        _pMgr.Initialize(_constSampleScale, _music.length);
    }
}
