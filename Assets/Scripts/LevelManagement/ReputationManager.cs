using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    private int Reputation = 0;
    private LevelManager _LM;
    private ReputationAmount[] _RepAmount;

    private void Start()
    {
        _LM = FindObjectOfType<LevelManager>();
        _RepAmount = FindObjectsOfType<ReputationAmount>();
    }

    public void increase_reputation(int RepAmount)
    {
        Reputation += RepAmount;
        if(_LM != null)
        {
            _LM.RepSelf = Reputation;
            foreach (var rep in _RepAmount)
            {
                rep.set_Rep_Count(Reputation);
            }
        }
    }

}
