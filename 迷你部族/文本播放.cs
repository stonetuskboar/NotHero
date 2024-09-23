using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 导入 TextMeshPro 命名空间

public class 文本播放 : MonoBehaviour
{

    public float 播放速度 = 0.01f; // 每个字符之间的间隔时间
    public string 完整文本 = "In the village of Dawnlight on the continent of Aetheran, a night raid by the Skeleton Sorcerer plunged the village into chaos. The village priest tried his best to protect the villagers, but his power was limited, and the village was destroyed.  After the disaster, the priest led the villagers to rebuild their homes, while also seeking the strength to fight against evil. A hero came to the village, warning the priest of the sorcerer's threat and inviting him to join the crusade. After some contemplation, the priest accepted the invitation, deciding to join the hero on a journey to avenge the village and uphold justice, setting out on the path to confront the Skeleton Sorcerer. ";
    public TextMeshProUGUI 文本UI; 

    // Start is called before the first frame update
    void Start()
    {
        if (文本UI != null)
        {
            StartCoroutine(逐字显示文本(完整文本));
        }
        else
        {
            Debug.LogError("未找到 TextMeshPro 组件，请确保场景中含有 TextMeshPro 组件。");
        }
    }

    private IEnumerator 逐字显示文本(string 文本)
    {
        文本UI.text = ""; // 清空文本 UI

        foreach (char 字符 in 文本) // 遍历每个字符
        {
            文本UI.text += 字符; // 将字符添加到文本 UI 中
            yield return new WaitForSeconds(播放速度); // 等待指定的时间再显示下一个字符
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
