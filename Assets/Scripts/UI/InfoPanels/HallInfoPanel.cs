using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HallInfoPanel : MonoBehaviour
{
    private TMP_Text _VisitorsCount;

    private void Start()
    {
        _VisitorsCount = transform.Find("VisitorsCount").GetComponent<TMP_Text>();
    }

    public void set_visitors_count(int newVisCount)
    {
        string NewText = "Посетители: " + newVisCount.ToString() + "/6";
        _VisitorsCount.SetText(NewText);
    }

}
