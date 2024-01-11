using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PiggyBank_CoinGroup : MonoBehaviour
{
    public Text txtCoin;
    // Start is called before the first frame update
    void Start()
    {
        Config.OnChange_PiggyBank_Coin += OnChange_PiggyBank_Coin;
        ShowCoin();
    }

    private void OnDestroy()
    {
        Config.OnChange_PiggyBank_Coin -= OnChange_PiggyBank_Coin;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChange_PiggyBank_Coin()
    {
        ShowCoin();
    }

    public void ShowCoin()
    {
        txtCoin.text = $"{Config.currPiggyBankCoin}";
    }
}
