using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectWithTag : MonoBehaviour
{
    [SerializeField] private float secondToRespawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Radio" || other.gameObject.tag == "WaterSpray")
        {
            StartCoroutine(ResetPosition(other.gameObject));
        }
    }

    IEnumerator ResetPosition(GameObject gameObject)
    {
        float time = Time.time;
        float respawnTime = time + secondToRespawn;
        yield return new WaitForSeconds(respawnTime - time);
        if (gameObject.gameObject.tag == "Radio")
        {
            RadioDataController radioDataController = gameObject.gameObject.GetComponent<RadioDataController>();
            radioDataController.ResetPosition();
        }

        if (gameObject.gameObject.tag == "WaterSpray")
        {
            WaterSpray waterSpray = gameObject.gameObject.GetComponent<WaterSpray>();
            waterSpray.ResetPosition();
        }
    }
}
