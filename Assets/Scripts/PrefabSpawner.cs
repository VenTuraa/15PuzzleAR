using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager m_ARPlaneManager;

    private List<ARRaycastHit> m_hits = new List<ARRaycastHit>();

    [SerializeField]
    private Camera arCamera;

    private GameObject spawnedObject;

    private void Awake()
    {
        SetAllPlanesActive(true);
        m_hits.Clear();
    }

    private void Start()
    {
        spawnedObject = null;
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
        if (raycastManager.Raycast(Input.GetTouch(0).position, m_hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.gameObject.CompareTag("Spawnable"))
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(m_hits[0].pose.position);
                    }
                }
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPositioon)
    {
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = spawnPositioon;
        }
        else
        {
            spawnedObject = Instantiate(raycastManager.raycastPrefab, spawnPositioon, Quaternion.identity);
            SetAllPlanesActive(false);
        }
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);

        m_ARPlaneManager.enabled = value;
    }
}
