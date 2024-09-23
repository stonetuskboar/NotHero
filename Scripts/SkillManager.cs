using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public priest Priest;
    public PlayerManager PlayerManager;
    public List<BasicSkill> SkillList;
    public List<Button> ButtonList;
    public GameObject rogueUI;
    public CanvasGroup rogueCanvasGroup;
    public SpeedSkill MoveSpeedSkill;
    public GameObject magicPrefab;
    public InputAction QKeyAction;
    public InputAction WKeyAction;
    public InputAction EKeyAction;
    public InputAction RKeyAction;
    public InputAction TKeyAction;
    public void Start()
    {
        rogueCanvasGroup.alpha = 0f;
        rogueCanvasGroup.interactable = false;
        rogueCanvasGroup.blocksRaycasts = false;
        for (int i = 0; i < SkillList.Count; i++)
        {
            SkillList[i].SkillId = i;
            SkillList[i].skillManager = this;
        }
        for (int i = 0; i < ButtonList.Count; i++)
        {
            int index = i;
            ButtonList[i].onClick.AddListener(() => { UpgradeThisSkill(index); });
            ButtonList[i].gameObject.SetActive(false);
        }

        QKeyAction.Enable();
        QKeyAction.performed += OnQkeyPressed;
        WKeyAction.Enable();
        WKeyAction.performed += OnWkeyPressed;
        EKeyAction.Enable();
        EKeyAction.performed += OnEkeyPressed;
        RKeyAction.Enable();
        RKeyAction.performed += OnRkeyPressed;
        TKeyAction.Enable();
        TKeyAction.performed += OnTkeyPressed;
    }

    public void CreateMagicEffectAt(Transform tf)
    {
       Instantiate(magicPrefab , tf);
    }

    public void ShowRandomThreeSkill()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            ButtonList[i].gameObject.SetActive(false);
        }
        List<int> randomValue = new List<int>();
        while(randomValue.Count <3) {
            int ran = Random.Range(0, ButtonList.Count);
            bool isUnique = true;
            for (int i = 0; i< randomValue.Count; i++)
            {
                if (randomValue[i] == ran) {  
                    isUnique = false; break; 
                }
            }
            if(isUnique == true)
            {
                randomValue.Add(ran);
            }
        }
        for (int i = 0; i < randomValue.Count; i++)
        {
            ButtonList[randomValue[i]].gameObject.SetActive(true);
        }
        StartCoroutine(ShowUI());
    }


    public void UpgradeThisSkill(int id) 
    {
        SkillList[id].Upgrade();
        StartCoroutine(HideUI());
    }
    IEnumerator ShowUI()
    {
        rogueCanvasGroup.alpha = 0f;
        rogueCanvasGroup.interactable = false;
        rogueCanvasGroup.blocksRaycasts = true;
        float time = 0;
        time += Time.deltaTime;
        while (time < 0.5f)
        {
            rogueCanvasGroup.alpha = time / 0.5f;
            yield return null;
            time += Time.deltaTime;
        }
        rogueCanvasGroup.alpha = 1f;
        rogueCanvasGroup.interactable = true;
        Time.timeScale = 0.001f;
    }
    IEnumerator HideUI()
    {
        Time.timeScale = 1f;
        rogueCanvasGroup.alpha = 1f;
        rogueCanvasGroup.interactable = false;
        rogueCanvasGroup.blocksRaycasts = true;
        float time = 0;
        time += Time.deltaTime;
        while (time < 0.5f)
        {
            rogueCanvasGroup.alpha = 1 - time / 0.5f;
            yield return null;
            time += Time.deltaTime;
        }
        rogueCanvasGroup.alpha = 0f;
        rogueCanvasGroup.blocksRaycasts = false;
    }

    public void OnQkeyPressed(InputAction.CallbackContext context)
    {
        SkillList[0].OnUseSkill();
    }
    public void OnWkeyPressed(InputAction.CallbackContext context)
    {
        SkillList[1].OnUseSkill();
    }
    public void OnEkeyPressed(InputAction.CallbackContext context)
    {
        SkillList[2].OnUseSkill();
    }
    public void OnRkeyPressed(InputAction.CallbackContext context)
    {
        SkillList[3].OnUseSkill();
    }
    public void OnTkeyPressed(InputAction.CallbackContext context)
    {
        SkillList[4].OnUseSkill();
    }
}
