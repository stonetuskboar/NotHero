using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 玩家移动 : MonoBehaviour
{
    public float 移动速度 = 5.0f; // 玩家移动速度
    private Animator 动画控制器; // 存储 Animator 组件的引用
    private bool 面向右边 = true; // 默认面向右边

    // Start is called before the first frame update
    void Start()
    {
        // 获取 Animator 组件
        动画控制器 = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 获取水平输入
        float 水平移动 = Input.GetAxis("Horizontal");

        // 移动玩家
        transform.Translate(Vector3.right * 水平移动 * 移动速度 * Time.deltaTime);

        // 如果有水平移动，设置动画状态
        if (水平移动 != 0)
        {
            动画控制器.SetBool("walk", true); // 播放运动动画
        }
        else
        {
            动画控制器.SetBool("walk", false); // 停止运动动画
        }

        // 处理角色面向方向
        if (水平移动 > 0 && !面向右边)
        {
            Flip(); // 向右移动，角色朝向右边
        }
        else if (水平移动 < 0 && 面向右边)
        {
            Flip(); // 向左移动，角色朝向左边
        }
    }

    // 切换角色面向方向的方法
    void Flip()
    {
        面向右边 = !面向右边;
        Vector3 角色缩放 = transform.localScale;
        角色缩放.x *= -1; // 翻转 x 轴的缩放
        transform.localScale = 角色缩放;
    }
}
