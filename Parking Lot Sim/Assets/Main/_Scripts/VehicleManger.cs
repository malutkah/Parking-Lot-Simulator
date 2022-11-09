using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManger : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public GameObject GarageEntrance;
    public GameObject GarageExit;

    public GameObject CreateVehicle(ParkingSpace space, int index)
    {
        string vehiclePrefabPath = "Prefabs/Vehicles/Cars/";

        // isInGarage = false

        // pick random Vehicle type
        Vehicles vehicles = Vehicles.CAR; //GetRandomVehicleType();

        // pick random Driver type
        Driver driver = GetRandomDriverType();

        // choose car prefab (for path)
        int random = UnityEngine.Random.Range(1, 3);
        string carPrefabName = $"Car {random}";

        vehiclePrefabPath = vehicles == Vehicles.CAR ? vehiclePrefabPath + carPrefabName : vehicles == Vehicles.BIKE ? "blub" : "other";

        GameObject vehicle = LoadVehicle(vehiclePrefabPath, false, vehicles, driver, index, space);

        // generate platenumber
        string platenumber = GeneratePlatenumber(vehicle.gameObject.name, index);
        vehicle.GetComponent<Vehicle>().Platenumber = platenumber;

        //Debug.Log(vehicle.GetComponent<Vehicle>().Platenumber);

        SpawnVehicle(vehicle);

        return vehicle;
        //MoveVehicle(vehicle);
    }

    public void ExitVehicle(GameObject vehicleToExit)
    {
        // set active = true
        vehicleToExit.SetActive(true);

        // set vehicle position
        var pos = vehicleToExit.transform.position;

        vehicleToExit.transform.position = new Vector3(GarageExit.transform.position.x, pos.y, pos.z);
        vehicleToExit.transform.Rotate(0f, 0f, -180.0f, UnityEngine.Space.Self);

        // remove parking space from vehilce
        // update parking space
        var v = vehicleToExit.GetComponent<Vehicle>();

        // have to check this, bc if garage is full, the vehicle has no space
        if (v.Space != null)
        {
            v.Space.RemoveVehicle();
        }

        // move vehicle out
        MoveVehicle(vehicleToExit, -2f);
    }

    private Vehicles GetRandomVehicleType()
    {
        Array vValues = Enum.GetValues(typeof(Vehicles));
        System.Random vRng = new System.Random();
        Vehicles vehicles = (Vehicles)vValues.GetValue(vRng.Next(vValues.Length));
        return vehicles;
    }

    private Driver GetRandomDriverType()
    {
        Array dValues = Enum.GetValues(typeof(Driver));
        System.Random dRng = new System.Random();
        Driver driver = (Driver)dValues.GetValue(dRng.Next(dValues.Length));
        return driver;
    }

    private GameObject LoadVehicle(string path, bool inGarage, Vehicles vehicles, Driver driver, int index, ParkingSpace space)
    {
        var vehicle = Resources.Load<GameObject>(path);
        Vehicle v = vehicle.GetComponent<Vehicle>();
        v.IsInGarage = inGarage;
        v.VehicleType = vehicles;
        v.DriverType = driver;
        v.Space = space;

        vehicle.gameObject.name = $"{v.VehiclesAsString(vehicles)}-{v.DriverAsShortString(driver)}-{index + 1}";
        // vehicle + driver + (index + 1) -> CAR-W-3-hashCode
        // Car-O-4-3865
        // Car-W-1-15614

        return vehicle;
    }

    private void SpawnVehicle(GameObject vehicletoSpawn)
    {
        switch (vehicletoSpawn.GetComponent<Vehicle>().VehicleType)
        {
            case Vehicles.CAR:
                GameObject vehicle = Instantiate(vehicletoSpawn);
                MoveVehicle(vehicle, 1);
                break;
            case Vehicles.BIKE:
                Debug.Log("BIKE");
                break;
            case Vehicles.OTHER:
                Debug.Log("OTHER");
                break;
            default:
                break;
        }
    }

    private string GeneratePlatenumber(string carName, int index)
    {
        /**
         * carName[i] * 31^(length-1)
         */

        int code = 0;
        for (int i = 0; i < carName.Length; i++)
        {
            code += carName[i] * 31 ^ (carName.Length - 1);
        }

        return $"{carName}-{Mathf.FloorToInt(code / (index + 1))}";
    }

    private void MoveVehicle(GameObject vehicle, float dir)
    {
        vehicle.GetComponent<Rigidbody>().AddForce(0, 0, MoveSpeed * dir, ForceMode.Impulse);

    }
}
