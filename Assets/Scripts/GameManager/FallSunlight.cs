using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSunlight : MonoBehaviour
{
    public float fallTime = 4;
    public float speed = 80;
    public Vector3 direction = Vector3.down;
    private float timer = 0;
    
    void Update()
    {
        timer += Time.deltaTime;
        if ( timer>fallTime ) {
            timer = 0;
            GetComponent<FallSunlight>().enabled = false;
        }
        transform.position += speed * direction * Time.deltaTime;
    }
}
