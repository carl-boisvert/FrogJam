using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private GardenSlot _slot;
    [SerializeField] private PlantData _plantData;
    [SerializeField] private int _plantStage;
    [SerializeField] private bool isDoneGrowing = false;

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

    IEnumerator GrowPlant(PlantDataStage stage)
    {
        float time = Time.time;
        float stageTime = time + stage.time;
        _currentPlantGameObject = Instantiate(stage.prefab, transform);
        Debug.Log($"Start Growing phase {_plantStage} at {time} and stopping at {stageTime}");
        while (time < stageTime)
        {
            time = Time.time;
            yield return new WaitForSeconds(1);
        }
        Debug.Log($"Stop Growing phase {_plantStage} at {time}");
        Destroy(_currentPlantGameObject);
        _plantStage++;
        Growth();
    }
}
