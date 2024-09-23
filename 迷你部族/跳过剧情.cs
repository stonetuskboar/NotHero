using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 跳过剧情 : MonoBehaviour
{
    public GameObject 画布UI; 


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            关闭画布(); 
        }
    }

    private void 关闭画布()
    {
        if (画布UI != null)
        {
            画布UI.SetActive(false); 
        }
        else
        {
            Debug.LogWarning("画布UI 未设置，无法关闭");
        }
    }
}
