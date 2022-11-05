using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    public GameObject ScrollViewItemPrefab;
    public GameObject Content;

    private List<GameObject> items;

    #region Unity [Awake]
    private void Awake()
    {
        items = new List<GameObject>();
    }
    #endregion

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
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(Content.transform.GetChild(i));
        }
        items.Clear();
    }

    /// <summary>
    /// Deletes a specific item at a given index
    /// </summary>
    public void RemoveItem(int index)
    {
        Destroy(Content.transform.GetChild(index));
    }

    /// <summary>
    /// Deletes the given GameObject from the ScrollView
    /// </summary>
    public void RemoveItem(GameObject item)
    {

    }
}
