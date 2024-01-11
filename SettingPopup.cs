using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SettingPopup : MonoBehaviour
{
    public static SettingPopup instance;

    [Header("Popup")]
    public GameObject popup;
    public BBUIButton btnClose;
    public BBUIButton btnRate;
    [Header("Sound")]
    public BBUIButton btnSound;
    public Image icon_Sound;
    public Sprite sprite_SoundOn;
    public Sprite sprite_SoundOff;
    [Header("Music")]
    public BBUIButton btnMusic;
    public Image icon_Music;
    public Sprite sprite_MusicOn;
    public Sprite sprite_MusicOff;
    [Header("LockGroup")]
    public GameObject lockGroup;

    enum STATE_CLOSEPOPUP {
        CLOSE,RATE
    }

    STATE_CLOSEPOPUP stateClosePopup = STATE_CLOSEPOPUP.CLOSE;
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnSound.OnPointerClickCallBack_Completed.AddListener(TouchSound);
        btnMusic.OnPointerClickCallBack_Completed.AddListener(TouchMusic);
        btnRate.OnPointerClickCallBack_Completed.AddListener(TouchRate);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(PopupHideView_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void TouchClose() {
        stateClosePopup = STATE_CLOSEPOPUP.CLOSE;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }


    private void TouchSound() {
        Config.SetSound(!Config.isSound);
        ShowButtonSound();
    }

    private void TouchMusic() {
        Config.SetMusic(!Config.isMusic);
        if (Config.isMusic)
        {
            MusicManager.instance.PlayMusicBG();
        }
        else {
            MusicManager.instance.StopMusicBG();
        }
        ShowButtonMusic();
    }

    private void TouchRate() {
        stateClosePopup = STATE_CLOSEPOPUP.RATE;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    private void PopupHideView_Finished()
    {
        if (stateClosePopup == STATE_CLOSEPOPUP.RATE) {
            MenuManager.instance.OpenRatePopup();
        }
        gameObject.SetActive(false);
    }

    public void OpenSettingPopup()
    {
        gameObject.SetActive(true);
        InitViews();
    }

    private void InitViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);

        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        btnSound.gameObject.SetActive(true);
        btnMusic.gameObject.SetActive(true);
        btnRate.gameObject.SetActive(true);
        ShowButtonSound();
        ShowButtonMusic();
        

        InitViews_ShowView();
    }

    private void ShowButtonMusic() {
        if (Config.isMusic)
        {
            icon_Music.sprite = sprite_MusicOn;
        }
        else {
            icon_Music.sprite = sprite_MusicOff;
        }
        icon_Music.SetNativeSize();
    }

    private void ShowButtonSound()
    {
        if (Config.isSound)
        {
            icon_Sound.sprite = sprite_SoundOn;
        }
        else
        {
            icon_Sound.sprite = sprite_SoundOff;
        }
        icon_Music.SetNativeSize();
    }

    public void InitViews_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });

        //sequenceShowView.InsertCallback(0.6f, () =>
        //{
        //    btnMusic.gameObject.SetActive(true);
        //    btnMusic.GetComponent<BBUIView>().ShowView();
        //});

        //sequenceShowView.InsertCallback(0.8f, () =>
        //{
        //    btnSound.gameObject.SetActive(true);
        //    btnSound.GetComponent<BBUIView>().ShowView();
        //});

        //sequenceShowView.InsertCallback(1f, () =>
        //{
        //    btnRate.gameObject.SetActive(true);
        //    btnRate.GetComponent<BBUIView>().ShowView();
        //});


        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.GetComponent<BBUIView>().ShowView();
        });
    }
}
