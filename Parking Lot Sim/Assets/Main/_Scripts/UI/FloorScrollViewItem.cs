using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FloorScrollViewItem : MonoBehaviour, IPointerClickHandler
{
    private int floorNumber;
    private int maxFloors;
    private int freeFloors;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI freeSpacesText;
    public Floor floor;

    public void OnPointerClick(PointerEventData eventData)
    {
        // remove
        //ParkingGarageManager.instance.RemoveFloorScrollViewItem(gameObject, floorNumber);
    }

    public void ItemOnClick()
    {
        // clear vehicle scroll view
        ParkingGarageManager.instance.ClearVehicleScrollView();

        if (floor.AllParkingSpaces.Count > 0)
        {
            for (int i = 0; i < floor.AllParkingSpaces.Count; i++)
            {
                ParkingSpace space = floor.AllParkingSpaces[i].GetComponent<ParkingSpace>();

                if (space.IsOccupied)
                    ParkingGarageManager.instance.NewVehicleScrollViewItem(space.Vehicle.Platenumber, space.SpaceName, i, space.VehicleGO, this);
            }
        }

        //Debug.Log($"Selected Floor {floor.FloorNumber} with {floor.AllParkingSpaces.Count} spaces");
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

    public void UpdateItem(int freeFloors, int floorNo)
    {
        floorText.text = $"Etage {floorNo}";
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }

    private void SetPrefabText()
    {
        floorText.text = $"Etage {floorNumber}";
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }
}
