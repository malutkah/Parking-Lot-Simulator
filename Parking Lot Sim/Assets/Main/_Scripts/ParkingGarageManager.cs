using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingGarageManager : MonoBehaviour
{
    /**
     * create parking garage(floor count, parking space count)
     *      -> create floors(parking space count)
     *          -> create parking space
     *          
     * control vehicle spawning
     * 
     * instatiating new garage: y + 2
     */

    public VehicleManger vehicleManger;
    public GameObject GaragePrefab;
    public GameObject ParkingSpacePrefab;
    public GameObject Building;
    public int floorCount, parkingSpaceCountPerFloor;
    public int VehicleSpawnCount;

    private ParkingSpace space;
    private bool canSpawnVehicle;
    private GarageBuilding garageBuilding;

    #region Unity [Start, Update]

    private void Awake()
    {
        garageBuilding = Building.GetComponent<GarageBuilding>();
    }

    void Start()
    {
        CreateGarageBuilding(floorCount, parkingSpaceCountPerFloor);
        canSpawnVehicle = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canSpawnVehicle)
        {
            StartCoroutine(SpawnVehicle(VehicleSpawnCount));
        }
    }
    #endregion

    // todo: corotine later
    private IEnumerator SpawnVehicle(int amount)
    {
        canSpawnVehicle = false;
        for (int i = 0; i < amount; i++)
        {
            vehicleManger.CreateVehicle(space, i);
            yield return new WaitForSeconds(2f);
        }
        canSpawnVehicle = true;
    }

    #region Creating new Garage building
    private void CreateGarageBuilding(int floorCount, int parkingSpaceCountPerFloor)
    {
        string garagePrefabPath = "Prefabs/ParkingGarage/ParkingGarageFloor";
        ParkingGarage currentFloorParkingGarage;
        GameObject currentFloor;
        GameObject currentParkingSpace;

        for (int i = 0; i < floorCount; i++)
        {
            currentFloor = CreateFloor(garagePrefabPath, i + 1);
            currentFloorParkingGarage = currentFloor.GetComponent<ParkingGarage>();

            for (int j = 0; j < parkingSpaceCountPerFloor; j++)
            {
                currentParkingSpace = CreateParkingSpace(j + 1, i + 1, currentFloor);
                currentFloorParkingGarage.Floor.GetComponent<Floor>().AllParkingSpaces.Add(currentParkingSpace);
            }
        }
    }

    private GameObject CreateFloor(string garagePrefabPath, int floorNo)
    {
        ParkingGarage parkingGarage;
        // instantiate new garage prefab
        GameObject garageFloorToSpawn = LoadPrefab(garagePrefabPath);
        GameObject garageFloor = Instantiate(garageFloorToSpawn, new Vector3(garageFloorToSpawn.transform.position.x, garageFloorToSpawn.transform.position.y + (2 * floorNo - 2), garageFloorToSpawn.transform.position.z), Quaternion.identity);
        garageFloor.transform.parent = Building.transform;
        garageFloor.transform.GetChild(0).gameObject.name = $"Floor_{floorNo}";

        parkingGarage = garageFloor.GetComponent<ParkingGarage>();

        // current floor of current garage
        Floor floor = parkingGarage.Floor.GetComponent<Floor>();
        floor.FloorNumber = floorNo;

        garageBuilding.AllFloors.Add(floor);
        return garageFloor;
    }

    private GameObject CreateParkingSpace(int spaceNo, int floorNo, GameObject currentFloor)
    {
        string path = "Prefabs/ParkingGarage/ParkingSpace";
        GameObject spaceToSpawn = LoadPrefab(path);
        GameObject space = Instantiate(spaceToSpawn);
        space.transform.parent = currentFloor.transform.GetChild(0).transform;
        ParkingSpace parkingSpace = space.GetComponent<ParkingSpace>();

        parkingSpace.FloorNumber = floorNo;
        parkingSpace.SpaceType = GetRandomSpaceType();
        parkingSpace.IsOccupied = false;
        parkingSpace.SpaceName = GenerateParkingSpaceName(spaceNo, floorNo, parkingSpace.SpaceAsShortString(parkingSpace.SpaceType));

        return space;
    }

    private string GenerateParkingSpaceName(int spaceNo, int floorNo, string spaceType)
    {
        /**
         * space name = floorNo + spaceNo + spaceType
         * -> 5-456-W =    5         456       WOMAN
         * -> 1-5-N   =    1          5       NORMAL
         *
         * W = WOMAN
         * N = NORMAL
         * D = DISABLED
         */

        return $"{floorNo}-{spaceNo}-{spaceType}";
    }
    #endregion

    private Space GetRandomSpaceType()
    {
        Array sValues = Enum.GetValues(typeof(Space));
        System.Random sRng = new System.Random();
        Space sp = (Space)sValues.GetValue(sRng.Next(sValues.Length));
        return sp;
    }

    private GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
}
