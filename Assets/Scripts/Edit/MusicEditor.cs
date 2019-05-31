using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicEditor : MonoBehaviour
{
    public MPMgr _pMgr;
    public Slider _trackSlide;
    public Slider _trackScale;
    //use to draw track texture
    public Camera _cam;
    //frame editor
    public SongEditor _sEditor;
    public AudioSource _aSource;
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

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (isInit)
        {
            _sampleScale = _constSampleScale * _trackScale.value;
            _drawOffset = _cam.orthographicSize / 1.5f;
            float half = _drawOffset  * _sampleScale;
            _trackSlide.value = Mathf.RoundToInt(_trackSlide.value);
            if (_trackSlide.value < half)
            {
                _startIndex = Mathf.FloorToInt(0);
                _endIndex = Mathf.FloorToInt(half * 2);
            }
            else if (_trackSlide.value + half >= _trackSlide.maxValue)
            {
                _startIndex = Mathf.FloorToInt(_trackSlide.maxValue - half * 2);
                _endIndex = Mathf.FloorToInt(_trackSlide.maxValue);
            }
            else
            {
                _startIndex = Mathf.FloorToInt(_trackSlide.value - half);
                _endIndex = Mathf.FloorToInt(_trackSlide.value + half);
            }
            _startIndex = _startIndex < 0 ? 0 : _startIndex;
            _endIndex = _endIndex >= _data.Length ? _data.Length - 1 : _endIndex;

            _track.positionCount = _endIndex - _startIndex + 1;
            DrawLine();
        }
    }

    public void GoEditMode()
    {
        _sEditor.Initialize((int)_trackSlide.value);
    }

    private void DrawLine()
    {
        if (isInit)
        {
            //print(_startIndex + "   " + _endIndex+"  "+ _data.Length);
            //print(_slide.value - _startIndex);
            float redLine = (_trackSlide.value - _startIndex) / _sampleScale - _drawOffset;
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

    private void Initialize()
    {
        AudioClip music = MusicMgr.GetMusic();
        _aSource.clip = music;
        _aSource.Play();
        _sampleScale = _constSampleScale;
        _trackNow.positionCount = 2;
        int musicSamples = Mathf.FloorToInt(music.length * _constSampleScale);
        MusicMgr.MusicSamples = musicSamples;
        int tmpStep = Mathf.FloorToInt(music.samples / musicSamples);
        //fake data size (sample for each second)
        _data = new float[musicSamples];
        float[] tmpData = new float[music.samples];
        music.GetData(tmpData, 0);
        //get fake music data per second
        for (int i = 0; i < _data.Length; i++)
        {
            int inverse = (_data.Length - i - 1) * tmpStep;
            if (inverse >= tmpData.Length)
                _data[i] = tmpData[tmpData.Length - 1];
            else
                _data[i] = tmpData[inverse];
        }
        _trackSlide.minValue = 0;
        _trackSlide.maxValue = Mathf.FloorToInt(music.length * _constSampleScale);
        _trackSlide.value = 0;
        isInit = true;
        _pMgr.Initialize(_constSampleScale, music.length);
    }
}
