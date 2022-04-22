using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinMoneyAmount : MonoBehaviour
{
    public TMP_Text _MoneyAmountText;
    
    void Start()
    {
        SetAmountOfMoney(LevelManager.WIN_MONEY_AMOUNT);
    }

    private void SetAmountOfMoney(int NewNumOfMoney)
    {
        _MoneyAmountText.SetText(NewNumOfMoney.ToString());
    }
}
