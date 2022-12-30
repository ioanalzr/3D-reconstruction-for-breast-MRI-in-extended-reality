using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// determine the distance
public class DistanceChange : MonoBehaviour
{
    public GameObject HoloLens;
    public GameObject Hologram;

    public float Distance_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Distance_ = Vector3.Distance(HoloLens.transform.position, Hologram.transform.position);
    }
}
