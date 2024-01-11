using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SpinButtonManager : MonoBehaviour
{
    private BBUIButton buttonSpin;
    public Image icon;
    public CountDownTimeGroup countDownTimeGroup;
    private void Awake()
    {
        buttonSpin = GetComponent<BBUIButton>();
    }
    // Start is called before the first frame update
    void Start()
    {
        countDownTimeGroup.InitCallBack((Config.COUNTDOWN_TIME_TYPE countDownTimeType) =>
        {
            CountDownTimeGroup_CallBack(countDownTimeType);
        });

        Config.OnChangeSpin_LastTime += OnChangeSpin_LastTime;
        ShowSpinButton();
    }

    private void OnDestroy()
    {
        Config.OnChangeSpin_LastTime -= OnChangeSpin_LastTime;
    }

    public void OnChangeSpin_LastTime() {
        ShowSpinButton();
    }

    public void ShowSpinButton() {
        // long currTimeLastTime = Config.GetSpinLastTime();
        // if (currTimeLastTime + Config.TIME_SPIN > Config.GetTimeStamp())
        // {
        //     buttonSpin.Interactable = false;
        //     if (sequence_SpinButton != null)
        //     {
        //         sequence_SpinButton.Kill();
        //     }
        //     countDownTimeGroup.gameObject.SetActive(true);
        //     countDownTimeGroup.InitCountDownTimeGroup(currTimeLastTime + Config.TIME_SPIN - Config.GetTimeStamp());
        //     countDownTimeGroup.StartCountDownTime();
        //     icon.color = new Color(200f/255f, 200f / 255f, 200f / 255f, 200f / 255f);
        // }
        // else
        // {
        //     
        // }
        
        icon.color = new Color(1f, 1f, 1f, 1f);
        countDownTimeGroup.gameObject.SetActive(false);
        
        buttonSpin.Interactable = true;
        long currTimeLastTime = Config.GetSpinLastTime();
        if (currTimeLastTime + Config.TIME_SPIN > Config.GetTimeStamp())
        {
            if (sequence_SpinButton != null)
            {
                sequence_SpinButton.Kill();
            }
            countDownTimeGroup.gameObject.SetActive(true);
            countDownTimeGroup.InitCountDownTimeGroup(currTimeLastTime + Config.TIME_SPIN - Config.GetTimeStamp());
            countDownTimeGroup.StartCountDownTime();
        }
        else
        {
            ShowSpinButtonAvaiable();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountDownTimeGroup_CallBack(Config.COUNTDOWN_TIME_TYPE countDownTimeType) {
        ShowSpinButton();

    }
    Sequence sequence_SpinButton;
    public void ShowSpinButtonAvaiable() {
        //Debug.Log("ShowSpinButtonAvaiable");
        if (sequence_SpinButton != null) {
            sequence_SpinButton.Kill();
        }
        sequence_SpinButton = DOTween.Sequence();
        sequence_SpinButton.Insert(0f, icon.transform.DOPunchPosition(new Vector3(5f, 5f, 0.1f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InBounce));
        sequence_SpinButton.Insert(0f, icon.transform.DOPunchRotation(new Vector3(0f, 0f, 10f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InOutBack));
        sequence_SpinButton.Insert(0f, icon.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InBounce));
        sequence_SpinButton.Insert(2f, icon.transform.DOScale(1.1f, Random.Range(0.5f, 1f)).SetEase(Ease.Linear).SetLoops(Random.Range(2, 4), LoopType.Yoyo));
        sequence_SpinButton.SetLoops(-1, LoopType.Yoyo);
    }
}
