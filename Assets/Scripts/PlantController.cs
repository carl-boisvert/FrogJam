using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private GardenSlot _slot;
    [SerializeField] private PlantData _plantData;
    [SerializeField] private int _plantStage;
    [SerializeField] public bool isDoneGrowing = false;
    [SerializeField] private DebugSettings _debug;

    private GameObject _currentPlantGameObject;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _debug = FindObjectOfType<DebugSettings>();
    }

    // Start is called before the first frame update
    public void Init(GardenSlot slot, PlantData data)
    {
        _slot = slot;
        _plantData = data;
        _plantStage = 0;
        
        if (_debug.ImmediateGrowth)
        {
            _plantStage = _plantData.stages.Count - 1;
        }

        
        Growth();
    }

    public PlantData GetPlantData()
    {
        return _plantData;
    }

    private void Growth()
    {
        if (_plantStage < _plantData.stages.Count - 1)
        {
            _coroutine = StartCoroutine(GrowPlant(_plantData.stages[_plantStage]));
        }
        else
        {
            isDoneGrowing = true;
            GameObject plant = Instantiate(_plantData.stages[_plantStage].prefab, transform);
            plant.GetComponentInChildren<MeshRenderer>().material.color = _plantData.color;
        }
    }

    public void PickedUp()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        if (_slot != null)
        {
            _slot.hasSomething = false;
            _slot = null;
        }
    }

    IEnumerator GrowPlant(PlantDataStage stage)
    {
        float time = Time.time;
        float stageTime = time + stage.time;
        _currentPlantGameObject = Instantiate(stage.prefab, transform);
        //Debug.Log($"Start Growing phase {_plantStage} at {time} and stopping at {stageTime}");
        while (time < stageTime)
        {
            yield return new WaitForSeconds(1);
            time = Time.time;
        }
        //Debug.Log($"Stop Growing phase {_plantStage} at {time}");
        Destroy(_currentPlantGameObject);
        _plantStage++;
        Growth();
    }
}
