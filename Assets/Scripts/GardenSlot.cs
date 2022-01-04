using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenSlot : MonoBehaviour
{
    private bool _hasSomething = false;
    // Start is called before the first frame update
    public bool canPlant()
    {
        return !_hasSomething;
    }

    public void plant(GameObject go)
    {
        Instantiate(go, transform);
        _hasSomething = true;
    }
}
