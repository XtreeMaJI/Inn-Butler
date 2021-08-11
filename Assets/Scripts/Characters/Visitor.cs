using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : BaseCharacter
{
    public enum StateOfVisitor: int
    {
        Idle = 0,
        MoveToReception = 1,
        MoveToRoom = 2,
        FollowPerson = 3
    }

    public LevelManager.TypeOfVisitor VisitorType { get; private set; }

    public int LeaseTerm; //Время, на которое посетитель займёт комнату

    protected void Start()
    {
        Reception Rec = Object.FindObjectOfType<Reception>();
        change_state(StateOfCharacter.MoveToReception, Rec);

    }

}
