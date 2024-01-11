using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinGroup : MonoBehaviour
{
    public Text txtCoin;
    // Start is called before the first frame update
    void Start()
    {
        Config.OnChangeCoin += OnChangeCoin;
        ShowCoin();
    }

    private void OnDestroy()
    {
        Config.OnChangeCoin -= OnChangeCoin;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChangeCoin(int coinValue)
    {
        ShowCoin();
    }

    public void ShowCoin()
    {
        DOTween.Kill(txtCoin.transform);
        
        txtCoin.text = $"{Config.currCoin}";
        txtCoin.transform.localScale = Vector3.one;
        txtCoin.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 2f).SetEase(Ease.InOutBack).SetRelative(true).SetLoops(3,LoopType.Restart);
    }
}
