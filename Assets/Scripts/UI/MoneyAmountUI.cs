using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyAmountUI : MonoBehaviour
{
    private TMP_Text _MoneyCount;

    private void Start()
    {
        _MoneyCount = transform.Find("MoneyCount").GetComponent<TMP_Text>();
    }

    public void set_MoneyCount(int Money)
    {
        _MoneyCount.SetText(Money.ToString());
    }

}
