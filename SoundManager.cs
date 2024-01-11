using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSound;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        audioSound = GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Click")]
    public AudioClip click1, click2, click3;


    public void PlaySound_Click() {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(click2);
        }
    }

    [Header("Block Click")]
    public AudioClip block_click;
    public void PlaySound_BlockClick()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(block_click);
        }
    }

    [Header("Block MoveFinish")]
    public AudioClip blockMoveFinish;
    public void PlaySound_BlockMoveFinish()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(blockMoveFinish);
        }
    }


    [Header("Block Cash")]
    public AudioClip blockCash;
    public void PlaySound_Cash()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(blockCash);
        }
    }

    [Header("Block Clear")]
    public AudioClip blockClear;
    public void PlaySound_Clear()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(blockClear);
        }
    }

    [Header("Voice Over")]
    public AudioClip[] voice;
    public void PlaySound_Voice(int index)
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(voice[index-1]);
        }
    }

    [Header("Block Free Block")]
    public AudioClip free_block;
    public void PlaySound_FreeBlock()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(free_block);
        }
    }

    [Header("Block Game Over")]
    public AudioClip game_over;
    public void PlaySound_GameOver()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(game_over);
        }
    }

    [Header("Gold")]
    public AudioClip gold;
    public void PlaySound_Gold()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(gold);
        }
    }

    [Header("Horeo")]
    public AudioClip horeo;
    public void PlaySound_Horeo()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(horeo);
        }
    }

    [Header("no_more_move")]
    public AudioClip no_more_move;
    public void PlaySound_NoMoreMove()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(no_more_move);
        }
    }

    [Header("phaogiay")]
    public AudioClip phaogiay;
    public void PlaySound_PhaoGiay()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(phaogiay);
        }
    }

    [Header("sfx_popup")]
    public AudioClip sfx_popup;
    public void PlaySound_Popup()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(sfx_popup);
        }
    }

    [Header("sfx_wind")]
    public AudioClip sfx_wind;
    public void PlaySound_Wind()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(sfx_wind);
        }
    }

    [Header("showBoard")]
    public AudioClip showBoard;
    public void PlaySound_ShowBoard()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(showBoard);
        }
    }

    [Header("win")]
    public AudioClip win;
    public void PlaySound_Win()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(win);
        }
    }

    [Header("showView")]
    public AudioClip showView;
    public void PlaySound_ShowView()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(showView);
        }
    }

    [Header("hideView")]
    public AudioClip hideView;
    public void PlaySound_HideView()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(hideView);
        }
    }

    [Header("winstarPop")]
    public AudioClip winstarPop;
    public void PlaySound_WinStarPop()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(winstarPop);
        }
    }

    [Header("notification")]
    public AudioClip notification;
    public void PlaySound_Notification()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(notification);
        }
    }

    [Header("reward")]
    public AudioClip reward;
    public void PlaySound_Reward()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(reward);
        }
    }

    [Header("Spin")]
    public AudioClip spin;
    public void PlaySound_Spin()
    {
        if (Config.isSound)
        {
            audioSound.PlayOneShot(spin);
        }
    }
}
