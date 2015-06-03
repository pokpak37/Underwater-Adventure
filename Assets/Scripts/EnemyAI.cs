using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    protected Transform _transform;
    
    public enum Habit
    {
        NotFight, Argressive, Protective
    }
    public Habit habit;

    public enum AttackType
    {
        Hit, Shoot, Charge
    }
    [SerializeField]
    private AttackType attackType;

    public AttackType ThisAttackType
    {
        get { return attackType; }
        set
        {
            attackType = value;

            switch (attackType)
            {
                case AttackType.Hit:
                    attackUpdate = AttackHit;
                    break;
                case AttackType.Charge:
                    attackUpdate = AttackCharge;
                    break;
                case AttackType.Shoot:
                    attackUpdate = AttackShoot;
                    break;
            }
        }
    }
    public float protectionRange;
    public float detectRange;
    float retreatRange;
    public float hp = 15;
    public float hitDmg;

    float distanceFollow;
    float distanceGoBack;

    Vector3 moveDirection;
    public bool isLockPlayer;
    public GameObject bullet;
    public float firerate;
    float moveSpeed;
    public float idelMoveSpeed;
    float chaseMoveSpeed;
    public float rotateSpeed;
    float preChargeMoveSpeed;

    public bool emitWhenDead;
    public GameObject emitObject;
    public int emitCount;

    public float minDelay = 1.5f;
    public float maxDelay = 3f;
    float delay;
    public float chargeDelay;
    public float timer;

    public GameObject myExplosionParticle;

    public Vector3 spawnPos;

    public enum Stage
    {
        Idle, Chase, Retreat
    }
    private Stage stage;

    public Stage ThisStage
    {
        get { return stage; }
        set
        {
            StopCoroutine(stage.ToString());
            stage = value;
            StartCoroutine(stage.ToString());
        }
    }

    public float lineOfSightRange = 3;
    public float fieldOfViewAngle = 110f;

    protected Transform target;
    public Vector3 targetPos;
    public LayerMask playerLayer;

    Gun[] guns;

    bool gunsIsNotNull
    {
        get { return guns != null; }
    }
    bool isFlip;

    Vector3 position
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }

    Quaternion rotation
    {
        get { return _transform.rotation; }
        set { _transform.rotation = value; }
    }

    delegate IEnumerator AttackUpdate();
    AttackUpdate attackUpdate;


    Quaternion targetRotation;

    void Awake()
    {
        _transform = transform;
        
        guns = GetComponentsInChildren<Gun>();
        if (gunsIsNotNull)
        {
            foreach (Gun gun in guns)
                gun.isEnemy = true;
        }
    }

    void SetStatsByGameLevel()
    {
       // EnemyAI thisAI = this.GetComponent<EnemyAI>();
       // LevelControl.instance.CalculateLevel(1, ref protectionRange, ref detectRange,
         //                       ref hp, ref hitDmg, ref lineOfSightRange);
    }

    void Start()
    {
        //x1 = this.gameObject.GetComponentInChildren<Animator>();
        SetStatsByGameLevel();
        retreatRange = lineOfSightRange * 1.5f;
        preChargeMoveSpeed = idelMoveSpeed / 1f;
        chaseMoveSpeed = idelMoveSpeed * 3f;
        distanceFollow = lineOfSightRange * 1.1f;
        distanceGoBack = lineOfSightRange * 0.9f;

        isFlip = false;
        moveSpeed = idelMoveSpeed;
        //savePosition = position;
        spawnPos = position;
        GetRandomPos();
        ThisStage = Stage.Idle;
        ThisAttackType = attackType;
    }

    IEnumerator UpdateZPosAndTargetRotation()
    {
        for(;;)
        {
            transform.SetZ(0f);
            if (target != null)
                targetRotation = Quaternion.LookRotation(target.position - position);
            yield return null;
        }        
    }

    //void Update()
    //{
    //    switch (AIStage)
    //    {
    //        case Stage.Idle: StageIdle();
    //            break;
    //        case Stage.Chase: StageChase();
    //            break;
    //        case Stage.Retreat: StageRetreat();
    //            break;
    //        default: StageIdle();
    //            break;
    //    }
    //    transform.SetZ(0);
    //}

    IEnumerator Idle()
    {
        float timer = 0f;
        for (; ; )
        {
            timer += Time.deltaTime;

            FindingTarget();
            IdleMove();
            RandomizerTargetPosition(ref timer);
            yield return null;
        }
    }

    private void RandomizerTargetPosition(ref float timer)
    {        
        if (timer > delay)
        {
            delay = Random.Range(minDelay, maxDelay);
            GetRandomPos();
            timer -= delay;
        }
    }

    private void IdleMove()
    {
        if (Mathf.Abs(Vector2.Distance(position, targetPos)) > 0.3f)
        {
            //Quaternion targetRotation = Quaternion.LookRotation(targetPos - position);
            //rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateSpeed);
            
            Vector3 targetDir = targetPos - position;
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(_transform.forward, targetDir, step, 0.0F);
            newDir.z = 0;
            rotation = Quaternion.LookRotation(newDir);
            _transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void GetRandomPos()
    {
        if (habit == Habit.Protective)
        {
            targetPos = spawnPos;

            targetPos.x += Random.Range(-protectionRange, protectionRange);
            targetPos.y += Random.Range(-protectionRange, protectionRange);
        }
        else
        {
            targetPos = Random.insideUnitSphere * protectionRange;
            targetPos += position;
        }
        targetPos.z = 0;
        delay = Random.Range(minDelay, maxDelay);

    }

    void FindingTarget()
    {
        if (habit != Habit.NotFight)
        {
            DetectingPlayerByRange();
            DetectingPlayerBySight();
        }
    }

    void DetectingPlayerByRange()
    {
        bool playerIsInDetectRange = Physics.CheckSphere(position, detectRange, playerLayer);
        if (playerIsInDetectRange)
        {
            print("Player detected !!!");
            SeePlayer();
        }
    }

    void DetectingPlayerBySight()
    {
        bool playerIsInSight = Physics.CheckSphere(position, lineOfSightRange, playerLayer);
        if (playerIsInSight)
        {
            Vector3 direction = PlayerControl.instance.transform.position - position;
            float angle = Vector3.Angle(direction, transform.forward);
            float halfFieldOfViewAngle = fieldOfViewAngle / 2;

            if (angle < halfFieldOfViewAngle)
            {
                print("Player in Sight !!!");
                SeePlayer();
            }
        }
    }

    public virtual void SeePlayer()
    {
        if (attackType == AttackType.Shoot)
            ActiveGun(true);
        target = PlayerControl.instance.transform;
        moveSpeed = chaseMoveSpeed;
        ThisStage = Stage.Chase;
        timer = 0f;
    }

    private void ActiveGun(bool active)
    {
        if (gunsIsNotNull)
            foreach (Gun gun in guns)
                gun.enabled = active;
    }

    IEnumerator AttackHit()
    {
        for (; ; )
        {
            var rotationChange = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateSpeed);

            bool isRight = rotationChange.y > 80f || rotationChange.y < 100f;
            bool isLeft = rotationChange.y > 260f || rotationChange.y < 280f;

            if (isRight)
                OverAngleToFlip(true, 1f, rotationChange.x);
            else if (isLeft)
                OverAngleToFlip(false, 1f, rotationChange.x);

            RotateTowardTarget(targetRotation);
            _transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator AttackCharge()
    {
        float timer = 0f;
        for (; ; )
        {
            float yDis = target.position.y - position.y;

            RotateToTargetSide();

            if (yDis > 0.3f)
                _transform.Translate(Vector3.up * preChargeMoveSpeed * Time.deltaTime);
            else if (yDis < -0.3f)
                _transform.Translate(Vector3.down * preChargeMoveSpeed * Time.deltaTime);
            else
            {
                timer += Time.deltaTime;
                if (timer > chargeDelay)
                {
                    yield return ChargeUpdate().MoveNext();
                    //StartCoroutine(ChargeUpdate());
                    //StopCoroutine(Chase());
                    //yield break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator ChargeUpdate()
    {
        while (X_Distance(target.position, position) < 10f)
        {
            _transform.Translate(Vector3.forward * moveSpeed * 1.5f * Time.deltaTime);
            yield return null;
        }
        //yield return StartCoroutine(Chase());
        yield return null;
    }

    IEnumerator AttackShoot()
    {
        for (; ; )
        {
            if (isLockPlayer)
                RotateTowardTarget(targetRotation);
            else
            {
                RotateToTargetSide();

                float yDis = target.position.y - position.y;
                if (yDis > 0.3f)
                    _transform.Translate(Vector3.up * idelMoveSpeed * Time.deltaTime);
                else if (yDis < -0.3f)
                    _transform.Translate(Vector3.down * idelMoveSpeed * Time.deltaTime);
            }

            float xDistance = X_Distance(position, target.position);

            if (xDistance < distanceGoBack)
                _transform.Translate(Vector3.back * idelMoveSpeed * Time.deltaTime);
            else if (xDistance > distanceFollow)
                _transform.Translate(Vector3.forward * idelMoveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator Chase()
    {
        // shoot at player or move toward player
        StartCoroutine(attackUpdate());

        for (; ; )
        {
            CheckDistanceMoreThanRetreatRange();
            yield return null;
        }
    }

    private void CheckDistanceMoreThanRetreatRange()
    {
        float distance = Mathf.Abs(Vector2.Distance(position, target.position));

        if (distance > retreatRange)
            ThisStage = Stage.Retreat;
    }

    float X_Distance(Vector3 vector1, Vector3 vector2)
    {
        if (vector1.x > vector2.x)
            return vector1.x - vector2.x;
        else
            return vector2.x - vector1.x;
    }

    private void RotateTowardTarget(Quaternion targetRotation)
    {
        //rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateSpeed);

        Vector3 targetDir = target.position - transform.position;
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        newDir.z = 0;
        rotation = Quaternion.LookRotation(newDir);
    }

    private void RotateToTargetSide()
    {
        bool isRightSide = target.position.x > position.x;
        rotation = Quaternion.Slerp(rotation,
            Quaternion.LookRotation(isRightSide ? Vector3.right : Vector3.left),
            Time.deltaTime * rotateSpeed * 3);
    }

    private void OverAngleToFlip(bool isFlipToLeft, float time, float xRotationChange)
    {
        bool isOverAngle = xRotationChange > 30f || xRotationChange < 330f;
        if (isOverAngle)
            if (!isFlip)
                Flip(isFlipToLeft, time);
    }

    private void Flip(bool isRight, float timeToFlip)
    {
        if (gunsIsNotNull)
            ActiveGun(false);

        StartCoroutine(UpdateFlip(isRight, timeToFlip));
    }

    private IEnumerator UpdateFlip(bool isRight, float timeToFlip)
    {
        isFlip = true;
        float yAxis = isRight ? 270f : 90f;
        Quaternion targetRotation = Quaternion.Euler(rotation.x, yAxis, 0f);

        do
        {
            Quaternion.Slerp(rotation, targetRotation, timeToFlip);
            yield return null;
        } while (rotation.y > yAxis - 5f || rotation.y < yAxis + 5);

        rotation = targetRotation;
        isFlip = false;
    }

    public virtual IEnumerator Retreat()
    {
        if (habit == Habit.Protective)
        {
            float distance;
            Quaternion targetRotation;
            ActiveGun(false);

            do
            {
                distance = Mathf.Abs(Vector2.Distance(position, spawnPos));
                targetRotation = Quaternion.LookRotation(spawnPos - position);
                rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateSpeed * 3);
                _transform.Translate(Vector3.forward * moveSpeed * 1.3f * Time.deltaTime);
                yield return null;
            } while (distance <= 0.3f);

            moveSpeed = idelMoveSpeed;
            stage = Stage.Idle;
        }
        else if (habit == Habit.Argressive)
        {
            StopCoroutine(UpdateZPosAndTargetRotation());
            do
            {
                targetRotation = Quaternion.LookRotation(Vector3.forward);
                rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateSpeed * 3);
                _transform.Translate(Vector3.forward * moveSpeed * 1.3f * Time.deltaTime);
                yield return null;
            } while (position.z < 15f);
            Destruct();
        }

    }

    public void GetHit(float dmg)
    {
        hp -= dmg;
        print(name + " get hit : " + dmg + " HP : " + hp);
        if (hp <= 0)
        {
            Instantiate(myExplosionParticle, position, Quaternion.identity);
            Destruct();
        }

        //Renderer rend = GetComponent<Renderer>();

        //rend.material.shader = Shader.Find("Custom/ToonSketch");

        //rend.material.SetColor ("Main Color", Color.blue);
    }

    void Destruct()
    {
        if (emitWhenDead)
        {
            //Instantiate(subBullet,
        }
        Destroy(gameObject);
        //PoolingManager.instance.enemyPooling.ReturnToPool(this);
    }

    

}
