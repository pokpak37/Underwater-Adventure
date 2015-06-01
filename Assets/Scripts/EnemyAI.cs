using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    Transform _transform;

    public enum Habit
    {
        NotFight, Argressive, Protective
    }
    public Habit habit;

    public enum AttackType
    {
        Hit, Shoot, Charge
    }
    public AttackType attackType;
    public float protectionRange;
    public float detectRange;
    float retreatRange;
    public int hp = 15;
    public int hitDmg;

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
    public float timer;

    public GameObject myExplosionParticle;

    public Vector3 spawnPos;

    public enum Stage
    {
        Idle, Chase, Retreat
    }
    public Stage AIStage;

    public float lineOfSightRange = 3;
    public float fieldOfViewAngle = 110f;

    public Transform target;
    public Vector3 targetPos;
    public LayerMask playerLayer;

    Gun[] guns;

    bool gunsIsNotNull
    {
        get { return guns != null; }
    }
    bool isFlip;

    Quaternion rotation
    {
        get { return _transform.rotation; }
        set { _transform.rotation = value; }
    }

    void Awake()
    {
        _transform = transform;
        retreatRange = lineOfSightRange * 1.5f;
        preChargeMoveSpeed = idelMoveSpeed / 3f;
        chaseMoveSpeed = idelMoveSpeed * 3f;
        guns = GetComponentsInChildren<Gun>();
        if (gunsIsNotNull)
        {
            foreach (Gun gun in guns)
                gun.isEnemy = true;
        }
    }

    void Start()
    {
        //x1 = this.gameObject.GetComponentInChildren<Animator>();
        isFlip = false;
        AIStage = Stage.Idle;
        moveSpeed = idelMoveSpeed;
        //savePosition = transform.position;
        spawnPos = _transform.position;
        GetRandomPos();
    }

    void FixedUpdate()
    {
        switch (AIStage)
        {
            case Stage.Idle: StageIdle();
                break;
            case Stage.Chase: StageChase();
                break;
            case Stage.Retreat: StageRetreat();
                break;
            default: StageIdle();
                break;
        }
        transform.SetZ(0);
    }

    void StageIdle()
    {
        // move to anywhere in circle area around spawn point every 1.5-3.0 second
        timer += Time.fixedDeltaTime;

        FindingTarget();
        IdleMove();
        RandomizerTargetPosition();
    }

    private void RandomizerTargetPosition()
    {
        if (timer > maxDelay)
        {
            GetRandomPos();
            timer = 0;
        }
    }

    private void IdleMove()
    {
        if (Mathf.Abs(Vector2.Distance(_transform.position, targetPos)) > 0.3f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - _transform.position);
            //_rigidbody.MoveRotation (Quaternion.RotateTowards (_transform.rotation, targetRotation, 5));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
            _transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //_rigidbody.AddForce (transform.forward * moveSpeed * 3);
        }
    }

    void GetRandomPos()
    {
        if (habit == Habit.Protective)
        {
            targetPos = spawnPos;

            targetPos.x += Random.Range(-protectionRange, protectionRange);
            targetPos.y += Random.Range(-protectionRange, protectionRange);
            targetPos.z = 0;
        }
        else
        {
            targetPos = Random.insideUnitSphere * protectionRange;
            targetPos.z = 0;
            targetPos += transform.position;
        }
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
        bool playerIsInDetectRange = Physics.CheckSphere(_transform.position, detectRange, playerLayer);
        if (playerIsInDetectRange)
        {
            print("Player detected !!!");
            SeePlayer();
        }
    }

    void DetectingPlayerBySight()
    {
        bool playerIsInSight = Physics.CheckSphere(_transform.position, lineOfSightRange, playerLayer);
        if (playerIsInSight)
        {
            Vector3 direction = PlayerControl.instance.transform.position - _transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            float halfFieldOfViewAngle = fieldOfViewAngle / 2;

            if (angle < halfFieldOfViewAngle)
            {
                print("Player in Sight !!!");
                SeePlayer();
            }
        }
    }

    public void SeePlayer()
    {
        if (attackType == AttackType.Shoot)
        {
            ActiveGun(true);
        }
        target = PlayerControl.instance.transform;
        moveSpeed = chaseMoveSpeed;
        AIStage = Stage.Chase;
    }

    private void ActiveGun(bool active)
    {
        if (gunsIsNotNull)
        {
            foreach (Gun gun in guns)
                gun.enabled = active;
        }
    }

    void StageChase()
    {
        // shoot at player or move toward player

        float distance = Mathf.Abs(Vector2.Distance(transform.position, target.position));
        Quaternion targetRotation = Quaternion.LookRotation(target.position - _transform.position);

        if (distance > retreatRange)
            AIStage = Stage.Retreat;

        switch (attackType)
        {
            case AttackType.Hit:

                var rotationChange = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);

                bool isRight = rotationChange.y > 80f || rotationChange.y < 100f;
                bool isLeft = rotationChange.y > 260f || rotationChange.y < 280f;

                if (isRight)
                    OverAngleToFlip(true, 1f, rotationChange.x);
                else if (isLeft)
                    OverAngleToFlip(false, 1f, rotationChange.x);

                rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
                _transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

                break;
            case AttackType.Charge:
                float yDis = target.position.y - _transform.position.y;

                bool isRightSide = target.transform.position.x > _transform.position.x;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(isRightSide ? Vector3.right : Vector3.left),
                    Time.fixedDeltaTime * rotateSpeed * 3);

                if (yDis > 0.3f)
                {
                    _transform.Translate(Vector3.up * preChargeMoveSpeed);
                }
                else if (yDis < -0.3f)
                {
                    _transform.Translate(Vector3.down * preChargeMoveSpeed);
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > 2)
                        AIStage = Stage.Retreat;
                }
                break;
            case AttackType.Shoot:
                if (isLockPlayer)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
                }
                else
                {
                    if (target.transform.position.x > _transform.position.x)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.fixedDeltaTime * rotateSpeed);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.fixedDeltaTime * rotateSpeed);
                    }

                    timer += Time.fixedDeltaTime;
                    if (timer > maxDelay)
                    {
                        targetPos = GetRandomPosForShoot();
                        timer = 0;
                    }
                    float moveLeft = Mathf.Abs(Vector2.Distance(transform.position, targetPos));
                    if (moveLeft > 0.3f)
                    {
                        transform.Translate(moveDirection * moveSpeed / 2 * Time.deltaTime);
                    }
                }

                if (distance > detectRange * 1.5f)
                {
                    AIStage = Stage.Retreat;
                }
                else if (distance > detectRange)
                {
                    _transform.Translate(Vector3.forward * moveSpeed / 5 * Time.deltaTime);
                }
                else if (distance < detectRange * 0.5f)
                {
                    _transform.Translate(Vector3.back * moveSpeed / 5 * Time.deltaTime);
                }

                if (distance > protectionRange * 2f)
                {
                    AIStage = Stage.Retreat;
                }

                break;
        }
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
    Vector3 GetRandomPosForShoot()
    {
        Vector3 pos = transform.position;
        pos.y = target.position.y;
        if (pos.y > _transform.position.y)
        {
            moveDirection = Vector3.up;
        }
        else
        {
            moveDirection = Vector3.down;
        }
        return pos;
    }

    void StageRetreat()
    {
        float distance;
        Quaternion targetRotation;
        switch (attackType)
        {
            case AttackType.Hit:
                distance = Mathf.Abs(Vector2.Distance(transform.position, spawnPos));
                targetRotation = Quaternion.LookRotation(spawnPos - _transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed * 3);
                _transform.Translate(Vector3.forward * moveSpeed * 1.3f * Time.deltaTime);

                if (distance < 0.3f)
                {
                    moveSpeed = idelMoveSpeed;
                    AIStage = Stage.Idle;
                }
                break;
            case AttackType.Charge:
                _transform.Translate(Vector3.forward * moveSpeed * 1.5f * Time.deltaTime);
                break;
            case AttackType.Shoot:
                distance = Mathf.Abs(Vector2.Distance(transform.position, spawnPos));
                targetRotation = Quaternion.LookRotation(spawnPos - _transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed * 3);
                _transform.Translate(Vector3.forward * moveSpeed * 1.3f * Time.deltaTime);

                if (distance < 0.3f)
                {
                    Gun[] gun = GetComponentsInChildren<Gun>();
                    if (gun != null)
                    {
                        foreach (Gun aaa in gun)
                        {
                            aaa.enabled = false;
                        }
                    }
                    moveSpeed = idelMoveSpeed;
                    AIStage = Stage.Idle;
                }
                break;
        }

    }

    public void GetHit(int dmg)
    {
        hp -= dmg;
        print(name + " get hit : " + dmg + " HP : " + hp);
        if (hp <= 0)
        {
            Instantiate(myExplosionParticle, _transform.position, Quaternion.identity);
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
