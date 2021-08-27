﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : BaseWorkerRoom, IRoomWithCarryables
{
    struct WineSpot
    {
        public Vector2 pos;
        public Wine item;
    }

    private const int NUM_OF_PLACES_FOR_WINE = 5;
    private const float SPACE_BETWEEN_BOTTLES = 0.15f;

    private GameObject _BuyWineB;
    private WineSpot[] PlacesForWine;

    public Wine WineInst;

    private void Start()
    {
        _BuyWineB = _Can.transform.Find("BuyWineB").gameObject;
        init_PlacesForWine();
    }

    private void init_PlacesForWine()
    {
        PlacesForWine = new WineSpot[NUM_OF_PLACES_FOR_WINE];
        PlacesForWine[0].pos = transform.Find("PlaceForWine").transform.position;
        PlacesForWine[0].item = null;
        for (int i = 1; i < NUM_OF_PLACES_FOR_WINE; i++)
        {
            PlacesForWine[i].pos = PlacesForWine[0].pos;
            PlacesForWine[i].pos.x += i*SPACE_BETWEEN_BOTTLES;
            PlacesForWine[i].item = null;
        }
    }

    protected override void enable_buttons()
    {
        if(_PC == null)
        {
            return;
        }
        if(_PC?.get_PlayerState() == PlayerController.StateOfPlayer.Carrying)
        {
            return;
        }
        _HammerB.SetActive(true);
        _BuyWineB.SetActive(true);
        _AddStaffB.SetActive(true);
    }

    protected override void disable_buttons()
    {
        _HammerB.SetActive(false);
        _BuyWineB.SetActive(false);
        _AddStaffB.SetActive(false);
    }

    public void press_BuyWineB()
    {
        add_Wine();
    }

    public void add_Wine()
    {
        for(int i=0; i<NUM_OF_PLACES_FOR_WINE; i++)
        {
            if(PlacesForWine[i].item == null)
            {
                PlacesForWine[i].item = Instantiate(WineInst, PlacesForWine[i].pos, new Quaternion());
                PlacesForWine[i].item.set_ParentRoom(this);
                return;
            }
        }
    }

    public void delete_item_from_room(Carryable item)
    {
        for(int i=0; i<NUM_OF_PLACES_FOR_WINE; i++)
        {
            if(ReferenceEquals(item, PlacesForWine[i].item))
            {
                PlacesForWine[i].item = null;
            }
        }

        if(_PC?.get_PlayerState() == PlayerController.StateOfPlayer.Carrying)
        {
            disable_buttons();
        }
    }

}