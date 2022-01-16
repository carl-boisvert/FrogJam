using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public MusicType _currentMusicType = MusicType.None;
    public bool isPainted = false;
    public PlantColor plantColor;
    
    [SerializeField] private GardenSlot _slot;
    [SerializeField] private PlantData _plantData;
    [SerializeField] private GameObject _deadPlantPrefab;
    [SerializeField] private int _plantStage;
    [SerializeField] public bool isDoneGrowing = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isWatered = false;
    [SerializeField] private DebugSettings _debug;
    [SerializeField] private PlantStatus _status;
    [SerializeField] private Transform _particleSpawnPoint;
    [SerializeField] private GameObject _likeParticlePrefab;
    [SerializeField] private GameObject _dislikeParticlePrefab;
    [SerializeField] private GameObject _currentParticle;
    [SerializeField] private GameObject _finishGrowingParticleSystem;
    [SerializeField] private List<PlantPaintMaterial> _plantPaintedMaterial;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _bloomingAudioClip;

    private GameObject _currentPlantGameObject;
    [SerializeField] private float _activeModifier = 1;
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
        _currentMusicType = MusicType.None;
        _status = PlantStatus.Neutral;

        if (slot)
        {
            _currentMusicType = slot.musicTypePlaying;
        }
        
        if (_debug.ImmediateGrowth)
        {
            _plantStage = _plantData.stages.Count - 1;
        }

        if (!_plantData.needWater)
        {
            Growth();
        }
        else
        {
            _currentPlantGameObject = Instantiate(_plantData.stages[_plantStage].prefab, transform);
        }
    }

    private void Update()
    {
        if (!isDead && !isDoneGrowing)
        {
            if (_plantData.needWater)
            {
                if (isWatered)
                {
                    SetPlantStatus();
                }
            }
            else
            {
                SetPlantStatus();
            }
        }
        else
        {
            Destroy(_currentParticle);
        }
    }

    private void SetPlantStatus()
    {
        if (_plantData.musicLikes.Contains(_currentMusicType))
        {
            _activeModifier = _plantData.musicLikeMultiplier;
            if (_status != PlantStatus.Happy)
            {
                Debug.Log("Spawning Happy");
                Destroy(_currentParticle);
                _currentParticle = Instantiate(_likeParticlePrefab, _particleSpawnPoint);
            }
            _status = PlantStatus.Happy;

        } else if (_plantData.musicDisikes.Contains(_currentMusicType))
        {
            if (_status != PlantStatus.Sad)
            {
                Debug.Log("Spawning Sad");
                Destroy(_currentParticle);
                _currentParticle = Instantiate(_dislikeParticlePrefab, _particleSpawnPoint);
            }

            if (!isDying)
            {
                StopCoroutine(_coroutine);
                StartCoroutine(Die());
                isDying = true;
            }
                
            _status = PlantStatus.Sad;
        }
        else
        {
            Debug.Log("Spawning Neutral");
            _status = PlantStatus.Neutral;
            Destroy(_currentParticle);
            _activeModifier = 1;
        }
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
            _currentPlantGameObject  = Instantiate(_plantData.stages[_plantStage].prefab, transform);
            //_currentPlantGameObject.GetComponentInChildren<MeshRenderer>().material.color = _plantData.color;
            Destroy(_currentParticle);
            Instantiate(_finishGrowingParticleSystem, _particleSpawnPoint);
            _audioSource.PlayOneShot(_bloomingAudioClip);
        }
    }

    public void PickedUp()
    {
        _currentMusicType = MusicType.None;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        if (_slot != null)
        {
            _slot.RemoveWater();
            _slot.hasSomething = false;
            _slot.plantController = null;
            _slot = null;
        }
    }
    
    private IEnumerator Die()
    {
        float time = Time.time;
        float dieTime = time + _plantData.musicDislikeTime;
        while (time < dieTime)
        {
            yield return new WaitForSeconds(1);
            
            if (!_plantData.musicDisikes.Contains(_currentMusicType))
            {
                Debug.Log("Rescued Plant");
                isDying = false;
                Destroy(_currentPlantGameObject);
                Growth();
                yield break;
            }
            time += 1;
        }
        
        isDead = true;
        Destroy(_currentPlantGameObject);
        Instantiate(_deadPlantPrefab, transform);
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
            time += 1 * _activeModifier;
            //Debug.Log(_activeModifier);
        }
        //Debug.Log($"Stop Growing phase {_plantStage} at {time}. Real time is {Time.time}");
        Destroy(_currentPlantGameObject);
        _plantStage++;
        Growth();
    }

    public void Watered()
    {
        if (_plantData.needWater && !isWatered)
        {
            isWatered = true;
            Destroy(_currentPlantGameObject);
            Growth();
        }
    }

    public void Paint(PlantColor paintColor)
    {
        isPainted = true;
        plantColor = paintColor;
        GrownPlant grownPlant = _currentPlantGameObject.GetComponentInChildren<GrownPlant>();
        grownPlant.renderer.material = _plantPaintedMaterial.Find(paint => paint.color == paintColor).material;
    }
}

[Serializable]
public class PlantPaintMaterial
{
    public PlantColor color;
    public Material material;
}


public enum PlantStatus
{
    Happy,
    Neutral,
    Sad
}
