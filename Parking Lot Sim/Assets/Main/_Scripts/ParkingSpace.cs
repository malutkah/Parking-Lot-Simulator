using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Space { WOMAN, NORMAL, DISABLED }

public class ParkingSpace : MonoBehaviour
{
    /**
     * space name (floor + overall space number, eg.: 3-56-W => floor 3, space 56, woman)
     * parking vehicle
     * is occupied?
     * space type (woman, normal, disabled
     * which floor
     */

    public string SpaceName;
    public Vehicle Vehicle;
    public bool IsOccupied;
    public int FloorNumber;
    public Space SpaceType;


    public string SpaceAsShortString(Space s) => s == Space.DISABLED ? "D" : s == Space.NORMAL ? "N" : "W";

    public string SpaceAsString(Space s) => s == Space.DISABLED ? "Disabled" : s == Space.NORMAL ? "Normal" : "Woman";

}
