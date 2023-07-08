using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornSunlight : MonoBehaviour
{
    public GameObject sunlightPrefab;
    public float duration = 8;
    
    public Vector3 range_left;
    public Vector3 range_right;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if ( timer > duration ) {
            BornNewSunlight();
            timer = 0;
        }
    }

    private void BornNewSunlight() {
        float x = Random.Range(range_left.x, range_right.x);
        float y = Random.Range(range_left.y, range_right.y);
        float z = Random.Range(range_left.z, range_right.z);
        GameObject newSunlight = Instantiate(sunlightPrefab, new Vector3(x, y, z), Quaternion.identity);
        newSunlight.AddComponent<FallSunlight>();
    }
}
