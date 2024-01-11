using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StarGroup : MonoBehaviour
{
    public List<Image> listStars = new List<Image>();
    public Sprite spriteStar_Normal;
    public Sprite spriteStar_Dark;

    public Image imgProgress;

    private int currStar = 3;
    const float SCORE_MAX = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Config.gameState == Config.GAME_STATE.PLAYING) {
            if (Config.CheckTutorial_1()) return;
            if (Config.CheckTutorial_2()) return;
            if (Config.CheckTutorial_3()) return;
            if (Config.CheckTutorial_4()) return;
            countDownTimeA += Time.deltaTime;
            //Debug.Log(countDownTimeA);
            if (isCountDownTime)
            {
                if (countDownTimeA > configLevelGame.timeA)
                {
                    score -= ConfigLevelGame.scoreInterval_Decrease * Time.deltaTime;
                    if (score <= 0)
                    {
                        score = 0;
                        //GamePlayManager.instance.SetGameLose();
                        isCountDownTime = false;
                    }
                    UpdateScore();

                    UpdateStar();
                }
            }
        }
    }
    private bool isCountDownTime = false;
    private ConfigLevelGame configLevelGame;
    public void InitStarGroup(ConfigLevelGame _configLevelGame) {
        configLevelGame = _configLevelGame;
        score = SCORE_MAX;
        isCountDownTime = true;
        for (int i = 0; i < listStars.Count; i++)
        {
            listStars[i].rectTransform.anchoredPosition = new Vector2(-286.5f + configLevelGame.listScrore_Stars[i+1] /100f * 573f,0f);
        }
    }

    public void Revive_InitStarGroup() {
        //isCountDownTime = true;
        //score = SCORE_MAX;
        //currStar = 3;
    }

    private float countDownTimeA;

    public float score = 100f;

    public void UpdateScore() {
        imgProgress.fillAmount = score / 100f;
    }


    public void UpdateStar() {
        if (Mathf.CeilToInt(score) < configLevelGame.listScrore_Stars[currStar]) {
            ChangeStar();
        }
    }

    public void ChangeStar() {
        currStar = currStar - 1;
        if (currStar >= 0)
        {
            Sequence sequenceChagneStar = DOTween.Sequence();
            sequenceChagneStar.Insert(0f, listStars[currStar].transform.DOScale(0f, 0.1f).SetEase(Ease.OutQuad));
            sequenceChagneStar.Insert(0f, listStars[currStar].transform.DORotate(new Vector3(0f, 0f, Random.Range(90f, 180f)), 0.1f).SetEase(Ease.OutQuad));
            sequenceChagneStar.InsertCallback(0.1f, () =>
            {
                listStars[currStar].sprite = spriteStar_Dark;
                listStars[currStar].SetNativeSize();
            });
            sequenceChagneStar.Insert(0.15f, listStars[currStar].transform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutBounce));
            sequenceChagneStar.Insert(0.15f, listStars[currStar].transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce));
        }
    }

    public void AddScore() {
        if (isCountDownTime)
        {
            countDownTimeA = 0;
            score += ConfigLevelGame.scoreInterval_Increase;
            if (score >= 100)
            {
                score = 100;
            }
            else if (currStar <= 2)
            {
                if (score >= configLevelGame.listScrore_Stars[currStar+1])
                {
                    score = configLevelGame.listScrore_Stars[currStar+1]-0.01f;
                }
            }
            DOTween.Kill(imgProgress);
            imgProgress.DOFillAmount(score / 100f, 0.2f).SetEase(Ease.Linear);
        }
    }

    public int GetCurrStar() {
        return currStar;
    }
}
