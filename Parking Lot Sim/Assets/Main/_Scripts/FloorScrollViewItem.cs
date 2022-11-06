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

    public void NewItem(int floorNo, int maxFloors, int freeFloors)
    {
        floorNumber = floorNo;
        this.maxFloors = maxFloors;
        this.freeFloors = freeFloors;

        SetPrefabText();
    }

    public void UpdateItem(int freeFloors) {
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }

    private void SetPrefabText()
    {
        floorText.text = $"Etage {floorNumber}";
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }
}
