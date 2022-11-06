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

    public static ParkingGarageManager instance;

    public GameObject FloorScrollView;
    public VehicleManger vehicleManger;
    public GameObject GaragePrefab;
    public GameObject ScrollViewPrefab;
    public GameObject ParkingSpacePrefab;
    public GameObject Building;
    public List<GameObject> AllVehicleInside;
    [HideInInspector] public GameObject currentlyEnteredVehicle;
    public int floorCount, parkingSpaceCountPerFloor;
    public int VehicleSpawnCount;

    private ParkingSpace space;
    private GarageBuilding garageBuilding;
    private ScrollView floorScrollView;
    private bool canSpawnVehicle;
    private bool hasVehicleEntered;
    private int vehicleCount;
    private int freeParkingSpaces;

    #region Unity [Awake, Start, Update]

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        garageBuilding = Building.GetComponent<GarageBuilding>();
        AllVehicleInside = new List<GameObject>();
        floorScrollView = FloorScrollView.GetComponent<ScrollView>();
    }

    void Start()
    {
        canSpawnVehicle = true;
        vehicleCount = 0;
        CreateGarageBuilding(floorCount, parkingSpaceCountPerFloor);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canSpawnVehicle)
        {
            StartCoroutine(SpawnVehicle(VehicleSpawnCount));
        }

        if (hasVehicleEntered)
        {
            //Debug.Log($"Vehicle [{currentlyEnteredVehicle.GetComponent<Vehicle>().Platenumber}] entered!");
            AllVehicleInside.Add(currentlyEnteredVehicle);
            hasVehicleEntered = false;
        }
    }
    #endregion

    private IEnumerator SpawnVehicle(int amount)
    {
        canSpawnVehicle = false;
        for (int i = 0; i < amount; i++)
        {
            vehicleManger.CreateVehicle(space, vehicleCount);
            vehicleCount++;
            yield return new WaitForSeconds(1f);
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

        var newY = 2 * floorNo - 2;
        var pos = new Vector3(garageFloorToSpawn.transform.position.x, garageFloorToSpawn.transform.position.y + newY, garageFloorToSpawn.transform.position.z);
        GameObject garageFloor = Instantiate(garageFloorToSpawn, pos, Quaternion.identity);
        
        garageFloor.transform.parent = Building.transform;
        garageFloor.transform.GetChild(0).gameObject.name = $"Floor_{floorNo}";

        parkingGarage = garageFloor.GetComponent<ParkingGarage>();

        // current floor of current garage
        Floor floor = parkingGarage.Floor.GetComponent<Floor>();
        floor.FloorNumber = floorNo;

        garageBuilding.AllFloors.Add(floor);

        // create new scroll view item
        GameObject item = NewFloorScrollViewItem(floorNo, parkingSpaceCountPerFloor, parkingSpaceCountPerFloor);
        // add floor to ScrollView
        floorScrollView.Add(item);
        
        return garageFloor;
    }

    private GameObject NewFloorScrollViewItem(int floorNo, int maxFloor, int freeFloors)
    {
        GameObject newScrollViewItem = Instantiate(ScrollViewPrefab);
        FloorScrollViewItem item = newScrollViewItem.GetComponent<FloorScrollViewItem>();
        item.NewItem(floorNo, maxFloor, freeFloors);
        return newScrollViewItem;
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

    public void VehicleEnteredGarage(GameObject v)
    {
        currentlyEnteredVehicle = v;
        hasVehicleEntered = currentlyEnteredVehicle != null;
    }

    public ParkingSpace ChooseParkingSpaceForVehicle()
    {
        /*
            - driver type W can ONLY park at W and N, never at D
            - driver type D can park only a D
            - driver type O can ONLY park at N
        */

        /*
            - check before, if garage is full
            - update free space of floor
        */

        // get a random floor that isn't full
        System.Random fRng = new System.Random();
        Floor randomFloor = garageBuilding.AllFloors[fRng.Next(garageBuilding.AllFloors.Count)];

        while (randomFloor.IsFloorFull())
        {
            // choose another floor
            fRng = new System.Random();
            randomFloor = garageBuilding.AllFloors[fRng.Next(garageBuilding.AllFloors.Count)];
        }

        // then get a random parking space that isn't occupied
        System.Random sRng = new System.Random();
        GameObject spaceGO = randomFloor.AllParkingSpaces[sRng.Next(garageBuilding.AllFloors.Count)];
        ParkingSpace space = spaceGO.GetComponent<ParkingSpace>();

        while (space.IsOccupied)
        {
            sRng = new System.Random();
            spaceGO = randomFloor.AllParkingSpaces[sRng.Next(randomFloor.AllParkingSpaces.Count)];
            space = spaceGO.GetComponent<ParkingSpace>();
        }

        space.Vehicle = currentlyEnteredVehicle.GetComponent<Vehicle>();
        space.IsOccupied = true;

        Debug.Log($"Vehicle [{currentlyEnteredVehicle.GetComponent<Vehicle>().Platenumber}] is at Parking space [{space.SpaceName}]");

        freeParkingSpaces = CountFreeSpacesInFloor(randomFloor);
        
        Debug.Log($"{randomFloor.FloorNumber} has {freeParkingSpaces} left");

        floorScrollView.GetScrollViewItem(floorScrollView.items[randomFloor.FloorNumber-1]).UpdateItem(freeParkingSpaces);
        

        return space;
    }

    private int CountFreeSpacesInFloor(Floor f)
    {
        int count = 0;

        foreach(GameObject space in f.AllParkingSpaces)
        {
            if (!f.GetSpace(space).IsOccupied) 
            {
                count++;
            }
        }
        return count;
    }

}
