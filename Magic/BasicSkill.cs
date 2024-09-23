using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicSkill : MonoBehaviour
{
    public int SkillId = 0;
    public int SkillLevel = 0;
    public string SkillName = "";
    public SkillManager skillManager;
    public Image ShadeImage;
    public float CoolDownTime = 10f;
    protected float LeftCDTime = 0f;
    public float BuffTime = 10f;
    public float buffValue = 0.2f;
    public float UpgradeValue = 0.1f;
    protected Button button;
    protected BasicPlayer targetPlayer;
    protected CanvasGroup cg;
    public virtual void Start()
    {
        cg = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnUseSkill);
        ShadeImage.fillAmount = 0f;
        if(SkillLevel == 0)
        {
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
    public void Update()
    {
        if (LeftCDTime >= 0)
        {
            LeftCDTime -= Time.deltaTime;
            ShadeImage.fillAmount = LeftCDTime / CoolDownTime;
        }
    }

    public virtual void OnUseSkill()
    {
        if( LeftCDTime <= 0  && SkillLevel > 0)
        {
            bool isOk = skillManager.Priest.IsSpellOk();
            if(isOk == true)
            {
                targetPlayer = skillManager.Priest.targetPlayer;
                skillManager.Priest.PriestCastSpell();
                UseSkill();
                AudioManager.instance.PlayOneShotAt(3);
                StartCoroutine(OnBuff());
                LeftCDTime = CoolDownTime;
            }
        }
    }

    public virtual void Upgrade()
    {
        buffValue += UpgradeValue;
        SkillLevel++;
        if (SkillLevel == 1)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }

    public virtual void UseSkill()
    {

    }



    public IEnumerator OnBuff()
    {
        float time = 0f;
        while(time < BuffTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        OnBuffEnd();
    }


    public virtual void Buff(ref float damage)
    {
        damage *= 1 + buffValue;
    }

    public virtual void OnBuffEnd()
    {

    }
}
