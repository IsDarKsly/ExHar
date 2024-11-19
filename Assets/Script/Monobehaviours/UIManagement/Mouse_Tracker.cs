using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Tracker : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var vec = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        target.transform.position = cam.ScreenToWorldPoint(vec);
    }
}
