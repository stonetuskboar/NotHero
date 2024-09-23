using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    public GameObject TextPrefab;
    public Transform CanvasTransform;
    public List<TextMeshProUGUI> TextList;
    public float floatTime = 1f;

    public TextMeshProUGUI CreatePrefab()
    {
        GameObject obj = Instantiate(TextPrefab, CanvasTransform);
        obj.gameObject.SetActive(false);
        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        TextList.Add(tmp);
        return tmp;
    }

    public TextMeshProUGUI GetTmp()
    {
        for(int i = 0; i < TextList.Count; i++)
        {
            if (TextList[i].gameObject.activeSelf == false)
            {
                return TextList[i];
            }
        }
        return CreatePrefab();
    }

    public void CreateTextAt(string text, Vector3 Position)
    {
        TextMeshProUGUI tmp = GetTmp();
        Color color = tmp.color;
        color.a = 1f;
        tmp.color = color;
        tmp.text = text;
        tmp.transform.position = Position;
        tmp.gameObject.SetActive (true);
        StartCoroutine(TextFloat(tmp));
    }

    IEnumerator TextFloat(TextMeshProUGUI text)
    {

        float time = 0;
        float halfTime = floatTime / 2;
        while(time < halfTime)
        {
            time += Time.deltaTime;
            text.transform.position += 2 * Time.deltaTime * Vector3.up;
            yield return null;
        }
        time = 0;
        Color color = text.color;
        while (time < halfTime)
        {
            time += Time.deltaTime;
            color.a = 1 - time / halfTime;
            text.color = color;
            text.transform.position += 0.5f * Time.deltaTime * Vector3.up;
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
}
