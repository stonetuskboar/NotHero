using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public int id;
    public PlayerType nowPlayerType = PlayerType.fighter;
    public Image BackImage;
    public Image FrontImage;
    public CharacterSelect characterSelect;
    private Button button;
    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {

        if(characterSelect != null)
        {
            characterSelect.SelectThis(id);
        }
    }


    public void FadeBlack(Color color)
    {
        StartCoroutine( FadeToColor(color));
    }

    IEnumerator FadeToColor(Color color)
    {
        float time = 0f;
        Color ogcolor = BackImage.color;
        while(time < 0.3f) {
            time += Time.deltaTime;
            BackImage.color = ogcolor + (color - ogcolor)*time/0.3f;
            FrontImage.color = BackImage.color;
            yield return null;
        }
        BackImage.color = color;
        FrontImage.color = color;
    }


    public void FadeWhite()
    {
        StartCoroutine(FadeToColor(Color.white));
    }
}
