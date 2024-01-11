using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using System;

[Serializable]
public class ConfigLevelGame
{
    [Header("TINH DIEM")]
    [InfoBox("Mỗi A giây thì điểm bắt đầu giảm ( giá trị mặc định của A là 5)")]
    public float timeA = 5f;
    [InfoBox("Khi điểm giảm, mỗi 1s sẽ giảm X điểm (giá trị mặc định của X là 1)")]
    public const float scoreInterval_Decrease = 1f;
    [InfoBox("Khi người chơi đưa được 01 tile xuống sàn sẽ tăng Y điểm, Y mặc định là 2")]
    public const float scoreInterval_Increase = 3f;


    [Header("TINH SAO")]
    [InfoBox("Ở ngưỡng 0-25 người chơi sẽ được 0 sao")]
    [InfoBox("Ở ngưỡng 26-50 người chơi sẽ được 1 sao")]
    [InfoBox("Ở ngưỡng 50-75 người chơi sẽ được 2 sao")]
    [InfoBox("Ở ngưỡng 75-100 người chơi sẽ được 3 sao")]
    [InfoBox("Điểm tối đa ở mức 25,65,100 tuỳ vào việc trước đó đang ở mốc nào. Nếu đang ở 55 điểm thì tăng max là được 65 điểm.")]
    public List<float> listScrore_Stars = new List<float> { 0f, 25f, 50f,75f };

    [Header("PHAN THUONG")]
    [InfoBox("1sao - 10vang,2sao-12vang,3sao-15vang")]
    public List<int> listRewards_CoinValue = new List<int> { 2,1,2,3 };
    [InfoBox("xem video để được xReward")]
    public List<int> listXReward_Value = new List<int>() { 2, 3, 4, 5 };
    public List<int> listXReward_Percent = new List<int>() { 50, 20, 20, 10 };

}
