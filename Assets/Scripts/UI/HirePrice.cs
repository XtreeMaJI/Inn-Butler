using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HirePrice : MonoBehaviour
{
    private TMP_Text _MoneyCount;
    private TMP_Text _Salary;
    private Color _ColorBuf;

    private void Start()
    {
        _MoneyCount = transform.Find("MoneyCount").GetComponent<TMP_Text>();
        _Salary = transform.Find("Salary").GetComponent<TMP_Text>();

        _ColorBuf = _MoneyCount.color;

        string ParentName = transform.parent.name;
        
        switch(ParentName)
        {
            case "HireCookB":
                _MoneyCount.SetText(LevelManager.COOK_PRICE.ToString());
                break;
            case "HireHousemaidB":
                _MoneyCount.SetText(LevelManager.HOUSEMAID_PRICE.ToString());
                break;
            case "HireServantB":
                _MoneyCount.SetText(LevelManager.SERVANT_PRICE.ToString());
                break;
        }
        _Salary.SetText(LevelManager.BASE_SALARY.ToString());
    }

    public int get_Price()
    {
        int Price;
        int.TryParse(_MoneyCount.text, out Price);
        return Price;
    }

    public void to_twinkle_price_red()
    {
        _MoneyCount.color = Color.red;
        StartCoroutine("set_base_color");
    }

    private IEnumerator set_base_color()
    {
        yield return new WaitForSeconds(0.5f);
        _MoneyCount.color = _ColorBuf;
    }

    private void OnEnable()
    {
        if (_MoneyCount != null && _ColorBuf != null)
        {
            _MoneyCount.color = _ColorBuf;
        }
    }
}
