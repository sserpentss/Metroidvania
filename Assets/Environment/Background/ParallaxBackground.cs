using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] layers;
    [SerializeField, Range(0f, 1f)] private float[] layersSpeed;

    void Update()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {        
        Vector3 cameraMove = new Vector3(mainCamera.transform.position.x - gameObject.transform.position.x, 
            mainCamera.transform.position.y - gameObject.transform.position.y, 0);
        gameObject.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);       
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].transform.position -= cameraMove * layersSpeed[i];
        }
    }
}
