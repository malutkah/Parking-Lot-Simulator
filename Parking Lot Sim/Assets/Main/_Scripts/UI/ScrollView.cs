using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    public GameObject Content;

    public List<GameObject> items;

    #region Unity [Awake]
    private void Awake()
    {
        items = new List<GameObject>();
    }
    #endregion

    public bool ContainsPlatenumber(string platenumber)
    {
        bool contains = false;
        foreach (var item in items)
        {
            var s = item.GetComponent<VehicleScrollViewItem>();
            if (s.PlatenumberText.text == platenumber)
            {
                contains = true;
                break;
            }
        }

        return contains;
    }

    public int Count()
    {
        return items.Count;
    }

    /// <summary>
    /// Adds a GameObject to ScrollView
    /// </summary>
    public void Add(GameObject item)
    {
        item.transform.parent = Content.transform;
        items.Add(item);
    }

    /// <summary>
    /// Delete all items in ScrollView
    /// </summary>
    public void Clear()
    {
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject);
            }

            items.Clear();
        }
    }

    /// <summary>
    /// Deletes a specific item at a given index
    /// </summary>
    public void RemoveItem(int index)
    {
        Destroy(Content.transform.GetChild(index).gameObject);
        items.RemoveAt(index);
    }

    /// <summary>
    /// Deletes the given GameObject from the ScrollView
    /// </summary>
    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    /// <summary>
    /// Returns the FloorScrollViewItem Script
    /// </summary>
    public FloorScrollViewItem GetScrollViewItem(GameObject item) => item.GetComponent<FloorScrollViewItem>();
}
