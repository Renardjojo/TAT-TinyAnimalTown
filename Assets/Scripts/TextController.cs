using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextController : MonoBehaviour
{
    private Text mText;
    
    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();
    }

    public void SetTextWithRoundFloat(float value)
    {
        mText.text = System.Math.Round(value, 1).ToString();
    }
    
    public void SetTextWithFloat (float value)
    {
        mText.text = value.ToString();
    }
    
    public void SetValueAsTime (float seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        
        if (mText == null)
            mText = GetComponent<Text>();
        mText.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
    }
}
