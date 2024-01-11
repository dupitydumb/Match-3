using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarChestButton : MonoBehaviour
{
    public BBUIButton btnChest;

    public Text txtChest;

    public Image imgChest;
    
    // Start is called before the first frame update
    void Start()
    {
        btnChest.OnPointerClickCallBack_Completed.AddListener(TouchChest);
        Init_Button();
    }

    private void OnDestroy()
    {
        btnChest.OnPointerClickCallBack_Completed.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init_Button()
    {
        txtChest.text = $"{Config.GetChestCountStar()}/{Config.CHEST_STAR_MAX}";
        imgChest.fillAmount = 0f;
        imgChest.DOFillAmount(Config.GetChestCountStar() * 1f / Config.CHEST_STAR_MAX, 0.2f);
        if (Config.GetChestCountStar() >= Config.CHEST_STAR_MAX)
        {
            btnChest.Interactable = true;
        }
        else
        {
            DOTween.Kill(btnChest.transform);
            btnChest.transform.localScale = Vector3.one;

            btnChest.Interactable = false;
        }
        
    }


    public void SetAnimation()
    {
        if (Config.GetChestCountStar() >= Config.CHEST_STAR_MAX)
        {
            btnChest.transform.DOScale(0.1f, 0.3f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void TouchChest()
    {
        //btnChest.Interactable = false;
        //Config.SetChestCountStar(0);
        
        //txtChest.text = $"{Config.GetChestCountStar()}/{Config.CHEST_STAR_MAX}";
        //imgChest.fillAmount = 0f;
       
        //DOTween.Kill(btnChest.transform);
        //MenuManager.instance.OpenChestPopup(true);
    }
}
