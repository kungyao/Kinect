using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPMgr : MonoBehaviour {

    public Slider _slider;
    public Slider _scaleSlider;
    public InputField _field;

    private float _constSampleScale = 1;
    private float _musicLength = 1;
    private bool flag = false;

    private void Start()
    {
        _slider.onValueChanged.AddListener(delegate
        {
            if (!flag)
            {
                flag = true;
                UpdateInputFieldValue();
            }
            flag = false;
        });
        _field.onEndEdit.AddListener(delegate
        {
            if (!flag)
            {
                SetFieldValue();
                flag = true;
                UpdateSlideValue();
            }
            flag = false;
        });
    }

    public void Initialize(float css, float ml)
    {
        flag = true;
        _constSampleScale = css;
        _musicLength = ml;
    }

    public void UpdateSlideValue()
    {
        float val;
        if (float.TryParse(_field.text, out val))
        {
            if (val < 0)
            {
                _slider.value = 0;
                _field.text = "0";
            }
            else if (val > _musicLength)
            {
                _slider.value = _slider.maxValue;
                _field.text = Mathf.FloorToInt(_musicLength).ToString();
            }
            else
                _slider.value = val * _constSampleScale;
        }
    }

    public void UpdateInputFieldValue()
    {
        float musicTime = _slider.value / _constSampleScale;
        _field.text = musicTime.ToString();
    }

    private void SetFieldValue()
    {
        float val;
        float.TryParse(_field.text, out val);
        _field.text = ((float)Mathf.FloorToInt(val * 10) / 10).ToString();
    }
}
