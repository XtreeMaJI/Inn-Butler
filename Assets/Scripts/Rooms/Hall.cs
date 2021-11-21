using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hall : Room
{
    private struct PlacesForVisitors
    {
        public Visitor Vis;
        public Vector2 Place;
    }

    private List<PlacesForVisitors> _VisPlaces = new List<PlacesForVisitors>();
    public bool HasFreeSpace { get; private set; } //Есть ли место для ещё одного посетителя

    public GameObject[] VisPlacesPos = new GameObject[6];//Отсюда возьмём координаты мест для посетителей

    public HallInfoPanel InfoPanel;

    private void Start()
    {
        _LM.add_to_HallList(this);

        //Берём все элементы массива с позициями и переносим координаты этих элементов в массив мест для посетителей
        foreach(GameObject Pos in VisPlacesPos)
        {
            _VisPlaces.Add(new PlacesForVisitors { Vis = null, Place = Pos.transform.position });
        }

        HasFreeSpace = true;
    }

    public void enter_the_room(Visitor newVis)
    {
        PlacesForVisitors VisPlace = _VisPlaces.Find(Place => Place.Vis == newVis);
        newVis.transform.SetPositionAndRotation(VisPlace.Place, Quaternion.identity);
    }

    public void reserve_place_in_room(Visitor newVis)
    {
        if(HasFreeSpace == false)
        {
            return;
        }
        int Ind = _VisPlaces.FindIndex(Place => Place.Vis == null);
        PlacesForVisitors FreePlace = _VisPlaces[Ind];
        FreePlace.Vis = newVis;
        _VisPlaces[Ind] = FreePlace;

        int VisAmountInRoom = _VisPlaces.Where(Place => Place.Vis != null).Count();
        InfoPanel.set_visitors_count(VisAmountInRoom);

        foreach (PlacesForVisitors Place in _VisPlaces)
        {
            if(Place.Vis == null)
            {
                HasFreeSpace = true;
                return;
            }
        }

        HasFreeSpace = false;
    }

    public void free_space_in_room(Visitor curVis)
    {
        PlacesForVisitors VisPlace = _VisPlaces.Find(Place => Place.Vis == curVis);
        VisPlace.Vis = null;

        HasFreeSpace = true;

        int VisAmountInRoom = _VisPlaces.Where(Place => Place.Vis != null).Count();
        InfoPanel.set_visitors_count(VisAmountInRoom);
    }

}
