using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleScrollViewItem : MonoBehaviour
{
    public TextMeshProUGUI PlatenumberText;
    public TextMeshProUGUI ParkingSpaceText;
    public Button ExitVehilceButton;

    [SerializeField] private GameObject vehicle;
    private string platenumber;
    private string parkingSpace;
    private FloorScrollViewItem floorScrollViewItem;

    #region Button

    public void ExitVehicleClick()
    {
        // update floor scroll view (free spaces)
        // get floor
        Vehicle v = vehicle.GetComponent<Vehicle>();
        Floor f = ParkingGarageManager.instance.GetFloorFromNumber(v.Space.FloorNumber);

        // floor.update(freeSpaces + 1)
        floorScrollViewItem.UpdateItem(f.CountFreeSpaces()+1);        

        ParkingGarageManager.instance.vehicleManger.ExitVehicle(vehicle);
        ParkingGarageManager.instance.RemoveVehicleScrollViewItem(gameObject);

        Destroy(gameObject);
    }

    #endregion


    public void NewItem(string pn, string ps, int index, GameObject v, FloorScrollViewItem fsvItem)
    {
        platenumber = pn;
        parkingSpace = ps;
        vehicle = v;
        floorScrollViewItem = fsvItem;

        gameObject.name = $"VehicleScrollViewItem_{index}";

        SetPrefabText();
    }
    private void SetPrefabText()
    {
        PlatenumberText.text = platenumber;
        ParkingSpaceText.text = parkingSpace;
    }
}
