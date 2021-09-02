using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ReputationAmount : MonoBehaviour
{
    private TMP_Text _RepCount;

    private void Start()
    {
        _RepCount = transform.Find("RepCount").GetComponent<TMP_Text>();
        _RepCount.SetText("0");
    }

    public void set_Rep_Count(int NewRep)
    {
        _RepCount.SetText(NewRep.ToString());
    }

}
