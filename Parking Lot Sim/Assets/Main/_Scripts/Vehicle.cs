using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum Vehicles { CAR, BIKE, OTHER }
public enum Driver { WOMEN, OTHER, DISABLED }

public class Vehicle : MonoBehaviour
{
    /**
     * platenumber : string
     * driver type : enum
     * vehicle type : enum
     * isInGarage : bool
     * parkingspot : ParkingSpot
     *
     */

    public string Platenumber;
    public bool IsInGarage;
    public Vehicles VehicleType;
    public Driver DriverType;
    public ParkingSpace Space;
    public float Speed;
    public GameObject Entrance;


    public string VehiclesAsShortString(Vehicles v) => v == Vehicles.CAR ? "C" : v == Vehicles.BIKE ? "B" : "O";

    public string DriverAsShortString(Driver d) => d == Driver.DISABLED ? "D" : d == Driver.OTHER ? "O" : "W";

    public string VehiclesAsString(Vehicles v) => v == Vehicles.CAR ? "Car" : v == Vehicles.BIKE ? "Bike" : "Other";

    public string DriverAsString(Driver d) => d == Driver.DISABLED ? "Disabled" : d == Driver.OTHER ? "Other" : "Woman";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Entrance")
        {
            IsInGarage = true;

            ParkingGarageManager.instance.VehicleEnteredGarage(gameObject);            

            Space = ParkingGarageManager.instance.ChooseParkingSpaceForVehicle();

            // disable vehicle
            gameObject.SetActive(!IsInGarage);
        }
    }
}
