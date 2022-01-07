using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private GardenSlot _slot;
    [SerializeField] private PlantData _plantData;
    [SerializeField] private int _plantStage;
    [SerializeField] public bool isDoneGrowing = false;

    private GameObject _currentPlantGameObject;

    private Coroutine _coroutine;
    // Start is called before the first frame update
    public void Init(GardenSlot slot, PlantData data)
    {
        _slot = slot;
        _plantData = data;
        _plantStage = 0;
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
            Instantiate(_plantData.stages[_plantStage].prefab, transform);
        }
    }

    public void PickedUp()
    {
        StopCoroutine(_coroutine);
        _slot.hasSomething = false;
        _slot = null;
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
