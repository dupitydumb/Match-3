using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelPopup : MonoBehaviour
{
    public static LevelPopup instance;
    public BBUIButton btnClose;
    public GameObject popup;
    public GameObject lockGroup;
    public Text txtCounStar;

    //public ScrollRect scrollRect;
    //public Transform content;
    //public ItemLevel itemLevelPrefab;
    private void Awake()
    {
        instance = this;
    }

    enum LEVELPOPUP_ACTION_TYPE {
        SelectLevel,
        Close
    }

    LEVELPOPUP_ACTION_TYPE typeAction = LEVELPOPUP_ACTION_TYPE.Close;
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isShowView) {
        //    scrollRect.velocity = Vector2.zero;
        //}
    }

    public void ShowLevelPopup()
    {
        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        typeAction = LEVELPOPUP_ACTION_TYPE.Close;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
        if (GamePlayManager.instance != null && GamePlayManager.instance.isActiveAndEnabled)
        {
            GamePlayManager.instance.SetUnPause();
        }
    }

    private InfoLevel infoLevelSelect;
    public void SelectLevel(InfoLevel infoLevel) {
        infoLevelSelect = infoLevel;
        typeAction = LEVELPOPUP_ACTION_TYPE.SelectLevel;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void HidePopup_Finished()
    {
        if (typeAction == LEVELPOPUP_ACTION_TYPE.SelectLevel) {
            Config.isSelectLevel = true;
            Config.currSelectLevel = infoLevelSelect.level;
            if (AdmobManager.instance.isInterstititalAds_Avaiable())
            {
                AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
                {
                    SceneManager.LoadScene("Play");
                });
            }
            else
            {
                SceneManager.LoadScene("Play");
            }
           
        }
        gameObject.SetActive(false);
    }


    //bool isShowView = false;
    public void InitViews()
    {
        //isShowView = true;
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        txtCounStar.text = $"{Config.GetCountStars()}/{Config.MAX_LEVEL * 3}";
        //foreach (Transform child in content)
        //{
        //    Destroy(child.gameObject);
        //}

        InitView_ShowView();
       
    }


    public void InitLevels() {
        
        //for (int i = 0; i < Config.MAX_LEVEL; i++) {
        //    ItemLevel itemLevel = Instantiate(itemLevelPrefab, content);

        //    InfoLevel infoLevel = new InfoLevel(i + 1, 0);
        //    if (Config.currDic_LevelStars.ContainsKey(infoLevel.level)) {
        //        infoLevel.star = Config.currDic_LevelStars[infoLevel.level];
        //    }

        //    itemLevel.SetInitItemLevel(infoLevel);
        //}

        //scrollRect.verticalNormalizedPosition = 1f;
    }

    public void InitView_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();

            
        });
        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();

            InitLevels();
        });

        
    }
}
