using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI FloorCountText;
    public TextMeshProUGUI FreeSpacesCountText;
    public TextMeshProUGUI CarCountText;
    public TextMeshProUGUI BikeCountText;
    public TextMeshPro GarageSpaceText;

    public void SetGarageInfoText(int floorCount, int freeSpaces, int carCount, int bikeCount, int allSpaces)
    {
        FloorCountText.text = $"{floorCount}";
        FreeSpacesCountText.text = $"{freeSpaces}";
        CarCountText.text = $"{carCount}";
        BikeCountText.text = $"{bikeCount}";

        GarageSpaceText.text = $"{freeSpaces} / {allSpaces}";
    }
}
