
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class EnemyManager : MonoBehaviour
{
    public ContactFilter2D enemyFilter;
    public PlayerManager PlayerManager;
    public DamageTextPool damageTextPool;
    public List<BasicEnemy> AliveEnemyList;
    public List<BasicEnemy> EnemyPool;
    private int enemyWholeAmount = 0;
    public List<GameObject> PrefabList;
    public GameObject enemyPrefab;
    public GameObject archeryEnemyPrefab;
    public EnemySo enemySo;
    private int enemyDeathAmount = 0;
    private int nowEnemyWave = 0;
    private int nowCreateEnemys = 0;
    private float enmeyWaveTimes = 10f;
    private float enemyCreateLeftTime = 0.5f;
    public Camera MainCamera;
    private bool isTraveling = false;

    public List<Transform> fleeLocations;
    public List<Vector3> FiveEnemyPositions;
    public GameObject EvilCircle;
    public GameObject evilBlust;

    public void Start()
    {
        enmeyWaveTimes = 15f;
    }

    public void Update()
    {
        enemyCreateLeftTime -= Time.deltaTime;
        if (enemyCreateLeftTime < 0)
        {
            if(isTraveling == true)
            {
                enemyCreateLeftTime = 3f;
                if (AliveEnemyList.Count <= 1)
                {
                    CreateEnemy(Random.Range(0, 2));
                }
            }
            else
            {
                enemyCreateLeftTime = enmeyWaveTimes;
                if (AliveEnemyList.Count < 25 && nowCreateEnemys < enemySo.EnemyWaves[nowEnemyWave].enemyAmount)
                {
                    CreateEnemysByWave();
                }
            }

        }
    }
    public void CreateFiveEnemy(Vector3 position)
    {
        GameObject circle = Instantiate(EvilCircle,position, Quaternion.identity);
        StartCoroutine(DeleteEvilCircle(circle));

        for(int i = 0; i < FiveEnemyPositions.Count; i++)
        {
            int rand = Random.Range(0, 2);
            BasicEnemy enemy =  GetEnemyFromPool(rand);
            enemy.transform.position = position + FiveEnemyPositions[i];
        }
        BasicEnemy elite = GetEnemyFromPool(2);
        elite.transform.position = position;
    }
    public void CreateEvilBlurst(Transform transform)
    {
        GameObject circle = Instantiate(evilBlust, transform);
    }
    IEnumerator DeleteEvilCircle(GameObject evilCircle)
    {
        float time = 0f;
        while(time < 3f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(evilCircle);
    }

    public void DecreaseEnemyWaveTime()
    {
        if(enmeyWaveTimes >= 2f)
        {
            enmeyWaveTimes -= 1f;
        }
    }

    public void CreateEnemysByWave()
    {
        EnemyWave  wave = enemySo.EnemyWaves[nowEnemyWave];
        for (int i = 0 ; i< wave.Enemys.Count; i++)
        { 
            for(int j = 0 ;  j < wave.Enemys[i]; j ++)
            {
                CreateEnemy(i);
            }
        }
    }
    public void CreateEnemy(int enemyId)
    {
        nowCreateEnemys++;
        BasicEnemy enemy = GetEnemyFromPool(enemyId);
        AliveEnemyList.Add(enemy);
        Vector3 screenTopRight = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, MainCamera.nearClipPlane));
        float x = screenTopRight.x + 1f; // ÔÚÆÁÄ»ÓÒ²àÉú³É
        float y = Random.Range(-2,2);
        enemy.transform.position = new Vector3(x,y, 0);
        enemy.FreshFromPool();
    }

    public BasicEnemy GetEnemyFromPool(int enemyId)
    {
        BasicEnemy enemy = null;
        for (int i = 0; i < EnemyPool.Count; i++)
        {
            if (EnemyPool[i].id == enemyId)
            {
                enemy = EnemyPool[i];
                EnemyPool.RemoveAt(i);
                return enemy;
            }
        }
        enemy = CreateNewEnemy(enemyId);
        return enemy;
    }


    public BasicEnemy CreateNewEnemy(int enemyId)
    {
        GameObject tmp;
        this.enemyWholeAmount++;
        tmp = Instantiate(PrefabList[enemyId], transform);
        tmp.name = tmp.name.Replace("(Clone)", this.enemyWholeAmount.ToString());
        BasicEnemy enemy = tmp.GetComponent<BasicEnemy>();
        enemy.Init(enemySo.EnemyDatas[enemyId], PlayerManager, this , damageTextPool);
        return enemy;
    }

    public BasicEnemy PoolAddNewEnemy(int enemyType)
    {
        BasicEnemy enemy = CreateNewEnemy(enemyType);
        EnemyPool.Add(enemy);
        return enemy;
    }

    public void TauntEnemyArea(float range , BasicPlayer player)
    {
        if (AliveEnemyList.Count <= 0)
        {
            return;
        }

        float distance;
        for (int i = 0; i < AliveEnemyList.Count; i++)
        {
            distance = (AliveEnemyList[i].transform.position - transform.position).magnitude;
            if (distance < range)
            {
                AliveEnemyList[i].SetTargetPlayer(player);
            }
        }
    }

    public BasicEnemy GetClosestEnemy(Transform transform)
    {
        if (AliveEnemyList.Count <= 0)
        {
            return null;
        }

        float MinDistance = 999999f;
        int index = -1;
        float distance;
        for (int i = 0; i < AliveEnemyList.Count; i++)
        {
            distance = (AliveEnemyList[i].transform.position - transform.position).magnitude;
            if (distance < MinDistance)
            {
                MinDistance = distance;
                index = i;
            }
        }
        return AliveEnemyList[index];
    }
    public void UnPoolThis(BasicEnemy basicEnemy)
    {
        enemyDeathAmount++;
        if (isTraveling == true)
        {
            if (enemyDeathAmount > 10 )
            {
                nowCreateEnemys = 0;
                isTraveling = false;
            }
        }
        else if (enemyDeathAmount >= enemySo.EnemyWaves[nowEnemyWave].enemyAmount)
        {
            enemyCreateLeftTime = 0f;
            nowCreateEnemys = 0;
            isTraveling = true;
            nowEnemyWave++;
            enemyDeathAmount = 0;
        }

        basicEnemy.gameObject.SetActive(false);
        AliveEnemyList.Remove(basicEnemy);
        EnemyPool.Add(basicEnemy);
    }


    public BasicEnemy GetEnemyByItsTransform(Transform EnemyTransform)
    {
        for (int i = 0; i < AliveEnemyList.Count; i++)
        {
            if (AliveEnemyList[i].transform == EnemyTransform)
            {
                return AliveEnemyList[i];
            }
        }
        return null;
    }
}
