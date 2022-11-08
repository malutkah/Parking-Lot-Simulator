using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageBuilding : MonoBehaviour
{
    public List<Floor> AllFloors;

    private void Awake()
    {
        AllFloors = new List<Floor>();
        //Floor = gameObject.transform.GetChild(0).gameObject;
    }

    public bool IsGarageFull()
    {
        bool full = false;

        foreach (Floor f in AllFloors)
        {
            full = f.IsFloorFull();
            if (!full)
            {
                return false;
            }
        }
        return full;
    }

    public int FloorCount() => AllFloors == null ? 0 : AllFloors.Count;

    public int GetTotalFreeSpaceCount()
    {
        int count = 0;

        foreach (Floor floor in AllFloors)
        {
            if (!floor.IsFloorFull())
            {
                count += floor.CountFreeSpaces();
            }
        }

        return count;
    }
}
