using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

[RequireComponent(typeof(ARRaycastManager))]
public class Object : MonoBehaviour
{
    public GameObject objectToInstantiate;

    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

    int count = 0;
    DateTime lastAddedTime = new DateTime();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }


    void Update()
    {
        var currentTime = System.DateTime.Now;
        if (lastAddedTime.AddSeconds(2) > currentTime)
            return;

        if(count <= 10)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }

            if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var spawnedObject = Instantiate(objectToInstantiate, hits[0].pose.position, Quaternion.identity);
                spawnedObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                spawnedObject.SetActive(true);
                spawnedObjects.Add(spawnedObject);

                count += 1;

                lastAddedTime = System.DateTime.Now;
            }
        }
    }
}
