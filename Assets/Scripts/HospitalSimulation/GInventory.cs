using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{

    public class GInventory
    {
        List<GameObject> items = new List<GameObject>();

        public void AddItem(GameObject item)
        {
            items.Add(item);
        }

        public GameObject FindItemWithTag(string tag)
        {
            foreach (GameObject item in items)
            {
                if (item.tag == tag)
                    return item;
            }
            return null;
        }

        public void RemoveItem(GameObject item)
        {
            int indToRemove = -1;
            foreach (GameObject g in items)
            {
                indToRemove++;
                if (g == item)
                        break;
            }
            if (indToRemove > -1)
                items.RemoveAt(indToRemove);
        }
    }
}