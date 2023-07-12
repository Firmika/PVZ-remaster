using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public int GetLine()
    {
        return this.transform.parent.gameObject.name[4] - '0';
    }

    public int GetColumn()
    {
        return this.gameObject.name[3] - '0';
    }
}