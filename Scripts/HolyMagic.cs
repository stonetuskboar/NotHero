using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyMagic : MonoBehaviour
{
    float time = 0f;

    public void Update()
    {
        time += Time.deltaTime;
        if(time >1.4f)
        {
            DestroyThisMagic();
        }
    }
    public void DestroyThisMagic()
    {
        Destroy(gameObject);
    }
}
