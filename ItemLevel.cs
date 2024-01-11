using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemLevel : MonoBehaviour
{
    public Text txtLevel;
    public Image iconLock;
    public List<Image> listStars;
    public Sprite spriteStar_On;
    public Sprite spriteStar_Off;
    public BBUIButton btnLevel;

    private InfoLevel infoLevel;
    // Start is called before the first frame update
    void Start()
    {
        btnLevel.OnPointerClickCallBack_Completed.AddListener(TouchLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void TouchLevel() {
        
        LevelPopup.instance.SelectLevel(infoLevel);
    }



    //infoLevel.x == indexLevel
    //infoLevel.y == star
    public void SetInitItemLevel(InfoLevel _infoLevel) {
        infoLevel = _infoLevel;

        ShowInfoLevel();
    }

    public void ShowInfoLevel() {
        if (infoLevel.level < Config.currLevel)
        {
            //Da mo khoa
            btnLevel.Interactable = true;
            iconLock.gameObject.SetActive(false);
            txtLevel.gameObject.SetActive(true);
            txtLevel.text = $"{infoLevel.level}";
            for (int i = 0; i < listStars.Count; i++)
            {
                listStars[i].gameObject.SetActive(true);
                listStars[i].sprite = spriteStar_Off;
            }
            if (infoLevel.star >= 3) infoLevel.star = 3;
            for (int i = 0; i < infoLevel.star; i++)
            {
                listStars[i].gameObject.SetActive(true);
                listStars[i].sprite = spriteStar_On;
            }
        }
        else if (infoLevel.level == Config.currLevel)
        {
            //CurrLevel
            btnLevel.Interactable = true;
            iconLock.gameObject.SetActive(false);
            txtLevel.gameObject.SetActive(true);
            txtLevel.text = $"{infoLevel.level}";
            for (int i = 0; i < listStars.Count; i++)
            {
                listStars[i].gameObject.SetActive(true);
                listStars[i].sprite = spriteStar_Off;
            }
        }
        else {
            if (Config.isHackMode)
            {
                btnLevel.Interactable = true;
                iconLock.gameObject.SetActive(false);
                txtLevel.gameObject.SetActive(true);
                txtLevel.text = $"{infoLevel.level}";
                for (int i = 0; i < listStars.Count; i++)
                {
                    listStars[i].gameObject.SetActive(true);
                    listStars[i].sprite = spriteStar_Off;
                }
                if (infoLevel.star >= 3) infoLevel.star = 3;
                for (int i = 0; i < infoLevel.star; i++)
                {
                    listStars[i].gameObject.SetActive(true);
                    listStars[i].sprite = spriteStar_On;
                }
            }
            else
            {
                btnLevel.Interactable = false;
                iconLock.gameObject.SetActive(true);
                txtLevel.gameObject.SetActive(false);
                txtLevel.text = $"{infoLevel.level}";
                for (int i = 0; i < listStars.Count; i++)
                {
                    listStars[i].gameObject.SetActive(false);
                    listStars[i].sprite = spriteStar_Off;
                }
            }
           
        }
    }
}
