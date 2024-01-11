using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDownTimeGroup : MonoBehaviour
{
    //Đếm ngược thời gian
    [ToggleGroup("UseText")] [SerializeField] private bool UseText;
    [ToggleGroup("UseText")] [SerializeField] private bool isRound;
    [ToggleGroup("UseText")] [SerializeField] private bool isUse_S;
    [ToggleGroup("UseText")] [SerializeField] private Text timeTxt;

    [ToggleGroup("UseImage")] [SerializeField] private bool UseImage;
    [ToggleGroup("UseImage")] [SerializeField] private Image timeImg;

    private float initTime;
    private float countTime;
    private float lastCountDownTime;
    private bool isStartCountTime = false;

    private Action<Config.COUNTDOWN_TIME_TYPE> CountDownTimeGroup_Callback = delegate (Config.COUNTDOWN_TIME_TYPE countDownTimeType) { };
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitCallBack(Action<Config.COUNTDOWN_TIME_TYPE> _countDownTimeGroup_Callback) {
        CountDownTimeGroup_Callback = _countDownTimeGroup_Callback;
    }
    // Update is called once per frame
    void Update()
    {
        if (isStartCountTime)
        {
            //Debug.Log("isStartCountTime"+ isStartCountTime);
            countTime = countTime - Time.deltaTime;
            if (UseText)
            {
                if (lastCountDownTime - countTime >= 0.1f)
                {
                    lastCountDownTime = countTime;
                    ShowTime();
                }
            }

            if (UseImage)
            {
                ShowTime();
            }

            if (countTime <= 0)
            {
                countTime = 0;
                ShowTime();
                isStartCountTime = false;
                Debug.Log("AAAAAAAAAAAAAAAAAA:"+isStartCountTime);
                CountDownTimeGroup_Callback(Config.COUNTDOWN_TIME_TYPE.END);
            }
        }
    }

    public void InitCountDownTimeGroup(float _initTime) {
        initTime = _initTime;
        countTime = initTime;
        ShowTime();
    }

    public void StartCountDownTime() {
        isStartCountTime = true;
        lastCountDownTime = countTime;
    }

    public void StopCountDownTime()
    {
        isStartCountTime = false;
    }

    private void ShowTime() {
        if (UseText)
        {
            int hours = Mathf.FloorToInt(countTime / 3600);
            int countMinustes = Mathf.FloorToInt(countTime - hours * 3600);
            int minutes = Mathf.FloorToInt(countMinustes / 60);
            int seconds = Mathf.FloorToInt(countMinustes % 60);

            timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            //string textTime = countTime.ToString("0.0");
            //if (isRound) {
            //    textTime = Mathf.Round(countTime).ToString();
            //}
            //if (isUse_S)
            //{
            //    timeTxt.text = textTime + "s";
            //}
            //else {
            //    timeTxt.text = textTime + "";
            //}
        }

        if (UseImage) {
            timeImg.fillAmount = countTime / initTime;
        }
    }
    
}
