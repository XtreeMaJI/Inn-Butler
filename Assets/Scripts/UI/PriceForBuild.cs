using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriceForBuild : MonoBehaviour
{
    private TMP_Text _MoneyCount;
    private TMP_Text _Rent;

    private const int BASE_RENT = 1;
    private const int BASE_PRICE = 10;

    private Color _ColorBuf; //Сохранение первоначального цвета(для мерцания)

    private void Start()
    {
        _MoneyCount = transform.Find("MoneyCount").GetComponent<TMP_Text>();
        _Rent = transform.Find("Rent").GetComponent<TMP_Text>();

        _ColorBuf = _MoneyCount.color;

        string NameOfParent = transform.parent.name;
        switch (NameOfParent)
        {
            case "FirstAddButton":
                _MoneyCount.SetText(BASE_PRICE.ToString());
                _Rent.SetText(BASE_RENT.ToString());
                break;
            case "SecondAddButton":
                _MoneyCount.SetText((1.5f * BASE_PRICE).ToString());
                _Rent.SetText((0).ToString());
                break;
            case "ThirdAddButton":
                _MoneyCount.SetText((2f * BASE_PRICE).ToString());
                _Rent.SetText((0).ToString());
                break;
            case "FourthAddButton":
                _MoneyCount.SetText((2.5f * BASE_PRICE).ToString());
                _Rent.SetText((0).ToString());
                break;
        }
    }

    //Увеличивается рента для новой комнаты и общая, которую платит игрок
    public void increase_rent()
    {
        int RentCost = 0;
        int.TryParse(_Rent.text, out RentCost);
        if (RentCost != 0)
        {
            _Rent.SetText((BASE_RENT + RentCost).ToString());
        }
    }

    public int get_Price()
    {
        int Price;
        int.TryParse(_MoneyCount.text, out Price);
        return Price;
    }

    public int get_Rent()
    {
        int Res;
        int.TryParse(_Rent.text, out Res);
        return Res;
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
