using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plant
{
    public float bornInterval = 5f;
    public GameObject sunlight;
    public Transform sunlightPosition;
    public Vector3 deviation_left = new Vector3(40, 0, 0);
    public Vector3 deviation_right = new Vector3(60, 0, 0);
    private float bornTimer = 0;
    
    private bool isGenerating = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerating) return;
        bornTimer += Time.deltaTime;
        if (bornTimer > bornInterval)
            GenSunlight();
    }

    public void GenSunlightOver()
    {
        int side = Random.Range(0, 2);
        side = side == 0 ? 1 : -1;
        float x = Random.Range(deviation_left.x, deviation_right.x);
        float y = Random.Range(deviation_left.y, deviation_right.y);
        float z = Random.Range(deviation_left.z, deviation_right.z);
        Vector3 deviation = new Vector3(x, y, z);
        Instantiate(sunlight, sunlightPosition.position + side * deviation, Quaternion.identity);
        animator.SetBool("Ready", false);
        bornTimer = 0;
        isGenerating = false;
    }

    private void GenSunlight()
    {
        animator.SetBool("Ready", true);
        isGenerating = true;
    }
}
