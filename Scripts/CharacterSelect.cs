using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public PlayerType nowPlayerType = PlayerType.archer;
    private int nowId = -1;
    public Color fadeBlackColor;
    public priest Priest;
    public List<CharacterButton> buttonList = new List<CharacterButton>();
    public List<GameObject> Circles = new List<GameObject>();
    public InputAction firstPlayerAction;
    public InputAction secondPlayerAction;
    public InputAction thirdPlayerAction;
    public void Start()
    {
        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].id = i;
            buttonList[i].characterSelect = this;
        }
        SelectThis(0);

        firstPlayerAction.Enable();
        firstPlayerAction.performed += SelectArcher;
        secondPlayerAction.Enable();
        secondPlayerAction.performed += SelectFighter;
        thirdPlayerAction.Enable();
        thirdPlayerAction.performed += SelectTank;
    }

    public void OnDisable()
    {
        firstPlayerAction.Disable();
        firstPlayerAction.performed -= SelectArcher;
        secondPlayerAction.Disable();
        secondPlayerAction.performed -= SelectFighter;
        thirdPlayerAction.Disable();
        thirdPlayerAction.performed -= SelectTank;
    }

    public void SelectArcher(InputAction.CallbackContext context)
    {
        SelectThis(0);
    }
    public void SelectFighter(InputAction.CallbackContext context)
    {
        SelectThis(1);
    }
    public void SelectTank(InputAction.CallbackContext context)
    {
        SelectThis(2);
    }
    public void SelectThis(int id)
    {
        for (int i = 0; i < Circles.Count; i++)
        {
            if(id != i)
            {
                Circles[i].SetActive(false);
            }
            else
            {
                Circles[i].SetActive(true);
            }
        }
        if (nowId != id)
        {
            nowId = id;
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (i != id)
                {
                    buttonList[i].FadeBlack(fadeBlackColor);
                }
                else
                {
                    nowPlayerType = buttonList[i].nowPlayerType;
                    buttonList[i].FadeWhite();
                }
            }
            Priest.TargetThisPlayer(nowPlayerType);

        }
    }
}
