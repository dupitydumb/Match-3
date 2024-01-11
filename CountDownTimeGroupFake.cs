using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDownTimeGroupFake : MonoBehaviour
{
    public Text txtTime;

    public int TIME_START = 47 * 60 * 60 + 40 * 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnEnable()
    {
        StartCoroutine(UpdateTime_IEnumerator());
    }

    public IEnumerator UpdateTime_IEnumerator() {
        while (true) {
            int time = Mathf.FloorToInt(TIME_START - Time.realtimeSinceStartup);
            if (time <= 0)
            {
                time = TIME_START;
            }
            txtTime.text = Config.FormatTime(time);
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
