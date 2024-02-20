using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    int totalMoney = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalMoney = SaveSystem<int>.Load("totalMoney", totalMoney);
        UIManager.Instance.SetMoneyCount(totalMoney);
    }

    public void EarnMoney(int amount, Vector3 worldPos,bool isPosUI,UIManager.MoneySendingTypes moneySendingType)
    {
        if (amount < 1)
            return;

        totalMoney += amount;
        SaveSystem<int>.Save("totalMoney", totalMoney);
        UIManager.Instance.SendMoneyImagesToMoneyCounterImage(worldPos, isPosUI,1, 10, moneySendingType);
        UIManager.Instance.SetMoneyCount(totalMoney, 1, 0.25f);
    }

    public bool CheckMoney(float money)
    {
        return money <= totalMoney;
    }

    public void SpendMoney(int money)
    {
        if (CheckMoney(money))
        {
            totalMoney = Mathf.Max(0, totalMoney - money);
            SaveSystem<float>.Save("totalMoney", totalMoney);
            UIManager.Instance.SetMoneyCount(totalMoney, 0.5f, 0);

        }
    }

}
