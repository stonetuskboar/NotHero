using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 修改地图坐标 : MonoBehaviour
{
    public Transform 玩家; // 玩家对象的 Transform
    public Transform 图片1; // 第一个图片对象的 Transform
    public Transform 图片2; // 第二个图片对象的 Transform

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 检查玩家的位置与第一个图片的 x 坐标的差值
        if (玩家.position.x - 图片1.position.x >= 28)
        {
            // 增加图片的 x 坐标
            Vector3 新位置 = 图片1.position;
            新位置.x += 56;
            图片1.position = 新位置;
        }

        // 检查玩家的位置与第二个图片的 x 坐标的差值
        if (玩家.position.x - 图片2.position.x >= 28)
        {
            // 增加图片的 x 坐标
            Vector3 新位置 = 图片2.position;
            新位置.x += 56;
            图片2.position = 新位置;
        }
        if (图片1.position.x - 玩家.position.x  >= 28)
        {
            // 增加图片的 x 坐标
            Vector3 新位置 = 图片1.position;
            新位置.x -= 56;
            图片1.position = 新位置;
        }

        // 检查玩家的位置与第二个图片的 x 坐标的差值
        if (图片2.position.x - 玩家.position.x  >= 28)
        {
            // 增加图片的 x 坐标
            Vector3 新位置 = 图片2.position;
            新位置.x -= 56;
            图片2.position = 新位置;
        }
    }
}
