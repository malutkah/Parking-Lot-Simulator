using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    /**
     * amount parking spaces
     * list of parking spaces
     * isFull?
     * FloorNo
     */

    public int FloorNumber;
    public List<GameObject> AllParkingSpaces;

    public bool IsFloorFull()
    {
        bool full = false;

        foreach (GameObject ps in AllParkingSpaces)
        {
            full = ps.GetComponent<ParkingSpace>().IsOccupied;
            if (!full)
            {
                return false;
            }
        }

        return full;
    }

    public string GetParkingSpaceName(int index)
    {
        ParkingSpace ps = AllParkingSpaces[index].GetComponent<ParkingSpace>();

        return ps.SpaceName;
    }

    public int SpaceCount() => AllParkingSpaces == null ? 0 : AllParkingSpaces.Count;
}
