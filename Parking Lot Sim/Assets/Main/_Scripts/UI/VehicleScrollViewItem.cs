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

    private string platenumber;
    private string parkingSpace;

    #region Button

    public void ExitVehicleClick()
    {
        Debug.Log("Bye bye");
    }

    #endregion

    public void NewItem(string pn, string ps)
    {
        platenumber = pn;
        parkingSpace = ps;

        SetPrefabText();
    }
    private void SetPrefabText()
    {
        PlatenumberText.text = platenumber;
        ParkingSpaceText.text = parkingSpace;
    }
}
