using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorScrollViewItem : MonoBehaviour
{
    private int floorNumber;
    private int maxFloors;
    private int freeFloors;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI freeSpacesText;
    public Floor floor;

    public void ItemOnClick()
    {
        // save floor in FloorScrollViewItem

        // if vehicle count > 0
        // show all vehicle in floor
        ParkingSpace space = null;

        if (floor.AllParkingSpaces.Count > 0)
        {

            for (int i = 0; i < floor.AllParkingSpaces.Count; i++)
            {
                space = floor.AllParkingSpaces[i].GetComponent<ParkingSpace>();
                ParkingGarageManager.instance.NewVehicleScrollViewItem(space.Vehicle.Platenumber, space.SpaceName);
            }
        }

        Debug.Log($"Selected Floor {floor.FloorNumber} with {floor.AllParkingSpaces.Count} spaces");
    }

    public void NewItem(int floorNo, int maxFloors, int freeFloors)
    {
        floorNumber = floorNo;
        this.maxFloors = maxFloors;
        this.freeFloors = freeFloors;

        SetPrefabText();
    }

    public void UpdateItem(int freeFloors)
    {
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }

    private void SetPrefabText()
    {
        floorText.text = $"Etage {floorNumber}";
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }
}
