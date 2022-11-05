using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorScrollViewItem : MonoBehaviour
{
    public int floorNumber;
    public int maxFloors;
    public int freeFloors;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI freeSpacesText;

    public void NewItem(int floorNo, int maxFloors, int freeFloors)
    {
        floorNumber = floorNo;
        this.maxFloors = maxFloors;
        this.freeFloors = freeFloors;

        SetPrefabText();
    }

    private void SetPrefabText()
    {
        floorText.text = $"Etage {floorNumber}";
        freeSpacesText.text = $"{freeFloors} / {maxFloors}";
    }
}
