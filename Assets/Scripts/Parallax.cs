using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxStrenght = 0.1f;
    [SerializeField] bool disableVerticalParallax;
    Vector3 targetPreviousPosition;


    // Start is called before the first frame update
    void Start()
    {
        if (!followingTarget) followingTarget = Camera.main.transform;

        targetPreviousPosition = followingTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        var delta = followingTarget.position - targetPreviousPosition;

        if (disableVerticalParallax) delta.y = 0;
        targetPreviousPosition = followingTarget.position;
        transform.position += delta * parallaxStrenght;

        float startpos = followingTarget.transform.position.x;

        //if (temp >= startpos + length) startpos += length;
        //else if (temp < startpos - length) startpos -= length;
    }

    /*private float length, startpos;
    [SerializeField] private GameObject cam;
    public float parallaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x *  parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp >= startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }*/


}
