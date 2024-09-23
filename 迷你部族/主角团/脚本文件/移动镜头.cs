using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player; // 玩家对象的 Transform
    public Transform mainCamera;
    public float 跟随速度 = 4.0f; // 镜头跟随的速度
    private float 跟随范围 = 0.5f; // 默认跟随范围

    // Start is called before the first frame update
    void Start()
    {
        // 检查玩家对象是否已赋值
        if (player == null)
        {
            Debug.LogError("玩家对象未设置，请在检查器中设置玩家对象。");
        }
    }
    private void Update()
    {
        if (player != null)
        {
            // 获取当前镜头位置与玩家位置
            Vector3 目标位置 = new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z);

            // 计算镜头与玩家之间的距离
            float 距离 = Vector3.Distance(mainCamera.transform.position, player.position);

            // 根据距离动态调整跟随范围
            if (距离 > 跟随范围)
            {
                跟随范围 = 0.2f; // 距离大于基本跟随范围时，设置跟随范围为0.2f
            }
            else
            {
                跟随范围 = 0.5f; // 距离小于基本跟随范围时，设置跟随范围为0.5f
            }

            // 使镜头以一定速度向玩家位置移动
            if (距离 > 跟随范围) // 只有在超出范围时才移动
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, 目标位置, 跟随速度 * Time.deltaTime);
            }
        }
    }
}
