using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyBoss_RockFish : EnemyAI
{
    public AttackType attackType1;
    public AttackType attackType2;

    Dictionary<string, List<GameObject>> dicComponents = new Dictionary<string, List<GameObject>>();

    public List<GameObject> breakAbleComponents = new List<GameObject>();
    public List<GameObject> unbreakAbleComponents = new List<GameObject>();
    public List<GameObject> weaknessComponents = new List<GameObject>();



    public override void SeePlayer()
    {
        RandomAtkType();
        base.SeePlayer();
    }
    /*
    public override IEnumerator AttackShoot()
    {
        
        float timer2 = 0;
        for(;;)
        {
            timer2 += Time.deltaTime;
            print("timer2 : " + timer2);
            if (timer2 > 6)
            {
                RandomAtkType();
                StartCoroutine(Chase());
            }
            yield return null;
        }
        //return base.AttackShoot();
    }
*/
    void RandomAtkType()
    {
        ThisAttackType = Random.Range(0, 2) == 0 ? attackType1 : attackType2;
        print(ThisAttackType.ToString());
    }
}