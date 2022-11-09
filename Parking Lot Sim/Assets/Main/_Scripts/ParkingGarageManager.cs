using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public VehicleManger vehicleManger;
    public GameObject FloorScrollView;
    public GameObject VehicleScrollView;
    public GameObject GaragePrefab;
    public GameObject ScrollViewPrefab;
    public GameObject VehicleScrollViewPrefab;
    public GameObject ParkingSpacePrefab;
    public GameObject Building;
    public List<GameObject> AllVehicleInside;
    [HideInInspector] public GameObject currentlyEnteredVehicle;
    public int floorCount, parkingSpaceCountPerFloor;
    public int VehicleSpawnCount;
    public TextMeshProUGUI NewFloorCountInput;

    private ParkingSpace space;
    private GarageBuilding garageBuilding;
    private ScrollView floorScrollView;
    private ScrollView vehicleScrollView;
    private UiManager ui;
    private List<GameObject> allVehicles;
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
        vehicleScrollView = VehicleScrollView.GetComponent<ScrollView>();
        ui = gameObject.GetComponent<UiManager>();
        allVehicles = new List<GameObject>();
    }

    void Start()
    {
        canSpawnVehicle = true;
        vehicleCount = 0;
        CreateGarageBuilding(floorCount, parkingSpaceCountPerFloor);

        // set ui text
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

        ui.SetGarageInfoText(garageBuilding.AllFloors.Count, garageBuilding.GetTotalFreeSpaceCount(), vehicleCount, 0, parkingSpaceCountPerFloor * floorCount);
    }
    #endregion


    #region Button clicks
    public void ShowAllVehiclesInScrollView()
    {
        for (int i = 0; i < vehicleCount; i++)
        {
            // klappt nicht
            vehicleScrollView.Add(allVehicles[i]);
        }
    }

    public void ChangeFloorCountClick()
    {
        // save vehicle spaces and such
        // check if space/floor still exists, like when the floorCount is smaller than before (5 -> 2)
        // vehicles without valid space gotta leave

        int newFloorCount = int.Parse(NewFloorCountInput.text);


        CreateGarageBuilding(newFloorCount, parkingSpaceCountPerFloor);
        
    }

    #endregion

    private IEnumerator SpawnVehicle(int amount)
    {
        canSpawnVehicle = false;
        for (int i = 0; i < amount; i++)
        {
            allVehicles.Add(vehicleManger.CreateVehicle(space, vehicleCount));
            vehicleCount++;
            yield return new WaitForSeconds(1f);
        }

        canSpawnVehicle = true;
    }

    public Floor GetFloorFromNumber(int floorNumber)
    {
        return garageBuilding.AllFloors.Find(f => f.FloorNumber == floorNumber);
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
        GameObject item = NewFloorScrollViewItem(floorNo, parkingSpaceCountPerFloor, parkingSpaceCountPerFloor, floor);
        // add floor to ScrollView
        floorScrollView.Add(item);

        return garageFloor;
    }

    #endregion

    #region Scroll View
    
    private GameObject NewFloorScrollViewItem(int floorNo, int maxFloor, int freeFloors, Floor f)
    {
        GameObject newScrollViewItem = Instantiate(ScrollViewPrefab);
        FloorScrollViewItem item = newScrollViewItem.GetComponent<FloorScrollViewItem>();
        item.NewItem(floorNo, maxFloor, freeFloors);
        item.floor = f;
        return newScrollViewItem;
    }

    public void ClearVehicleScrollView()
    {
        vehicleScrollView.Clear();
    }

    public void RemoveVehicleScrollViewItem(GameObject v)
    {
        vehicleScrollView.RemoveItem(v);
        freeParkingSpaces -= 1;
    }

    // floor scroll view on item click 
    public void NewVehicleScrollViewItem(string platenumber, string spaceName, int index, GameObject v, FloorScrollViewItem floorScrollViewItem)
    {
        GameObject newScrollViewItem = Instantiate(VehicleScrollViewPrefab);
        VehicleScrollViewItem item = newScrollViewItem.GetComponent<VehicleScrollViewItem>();
        item.NewItem(platenumber, spaceName, index, v, floorScrollViewItem);
        vehicleScrollView.Add(newScrollViewItem);
    }
    #endregion

    #region Parking Space

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
        hasVehicleEntered = v != null;
    }

    public ParkingSpace ChooseParkingSpaceForVehicle(GameObject v)
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

        if (!garageBuilding.IsGarageFull())
        {
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
            GameObject spaceGO = randomFloor.AllParkingSpaces[sRng.Next(randomFloor.AllParkingSpaces.Count)];
            ParkingSpace space = spaceGO.GetComponent<ParkingSpace>();

            while (space.IsOccupied)
            {
                sRng = new System.Random();
                spaceGO = randomFloor.AllParkingSpaces[sRng.Next(randomFloor.AllParkingSpaces.Count)];
                space = spaceGO.GetComponent<ParkingSpace>();
            }

            space.Vehicle = v.GetComponent<Vehicle>();
            space.IsOccupied = true;
            space.VehicleGO = v;

            Debug.Log($"Vehicle [{v.GetComponent<Vehicle>().Platenumber}] is at Parking space [{space.SpaceName}]");

            freeParkingSpaces = CountFreeSpacesInFloor(randomFloor);

            Debug.Log($"{randomFloor.FloorNumber} has {freeParkingSpaces} left");

            floorScrollView.GetScrollViewItem(floorScrollView.items[randomFloor.FloorNumber - 1]).UpdateItem(freeParkingSpaces);

            return space;
        }
        else
        {
            vehicleManger.ExitVehicle(v);
            return null;
        }
    }

    private int CountFreeSpacesInFloor(Floor f)
    {
        int count = 0;

        foreach (GameObject space in f.AllParkingSpaces)
        {
            if (!f.GetSpace(space).IsOccupied)
            {
                count++;
            }
        }
        return count;
    }

    #endregion
}
