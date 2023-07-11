using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class PlayerController : Entity
{
    static public PlayerController Instance;

    PlayerControls controls;

    public Rigidbody Rb;
    public Animator Anim;
    public Camera cam;
    public float MovementSpeed;

    public PlayerBody body;

    Vector3 camDirection;

    public float longRunTimer = 2;
    float currentTimer;
    bool longRun;

    //PARTICLES
    public ParticleSystem dashParticle;
    public ParticleSystem trailParticle;

    //ATTACKS
    public float detectionRadius = 5;
    int attackCounter;
    float resetTime = 0.5f;
    float resetTimer;
    public Capacity[] CapacityPrefabs;
    [ReadOnly] public List<Capacity> Capacities = new List<Capacity>();
    [ReadOnly] public Enemy Target;

    Vector2 direction;
    bool move;

    void Awake()
    {
        Instance = this;

        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => direction = Vector2.zero;
        controls.Gameplay.Dash.performed += ctx => Dash();
        controls.Gameplay.Attack.performed += ctx => Attack();
    }

    protected override void Start()
    {
        camDirection = (cam.transform.position - transform.position.SetY(cam.transform.position.y)).normalized;
        FetchCapacities();
        base.Start();
    }

    protected override void Init()
    {
        EntityManager.Instance.AddEntity(this);
        base.Init();
    }

    public bool IsAttacking()
    {
        foreach (Capacity item in Capacities)
        {
            if (item.InUse)
                return true;
        }
        return false;
    }

    public void FetchCapacities()
    {
        foreach (var item in CapacityPrefabs)
            Capacities.Add(Instantiate(item, transform));

        AnimatorOverrideController overrideController = new AnimatorOverrideController(Anim.runtimeAnimatorController);

        for (int i = 0; i < Capacities.Count; i++)
        {
            overrideController["Capacity" + (i + 1)] = Capacities[i].clip;
            Anim.SetFloat("Capacity" + (i + 1) + "Speed", Capacities[i].clipSpeed);
        }


        Anim.runtimeAnimatorController = overrideController;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Dash()
    {
        if (IsAttacking())
            return;

        if (move)
        {
            Anim.Play("Dash");
            dashParticle.Play();
            trailParticle.Play();
            transform.DOMove(transform.position + Rb.velocity.normalized * 7.5f, 0.3f)
            .OnComplete(() =>
            {
                dashParticle.Stop();
                trailParticle.Stop();
                if (longRun)
                    Anim.Play("LongRun");
                else
                    Anim.Play("Idle");
            });
        }
    }

    void Attack()
    {
        if (IsAttacking())
            return;

        if (attackCounter < 3)
        {
            UseCapacity();

            attackCounter++;
            resetTimer = resetTime;
        }
        else
        {
            attackCounter = 0;
            resetTimer = resetTime;
        }
    }

    void Update()
    {
        if (IsAttacking())
            return;

        float moveHorizontal = direction.x;
        float moveVertical = direction.y;

        Vector3 movement = (moveHorizontal * cam.transform.right + moveVertical * -camDirection).normalized;

        Rb.velocity = (movement * (longRun ? MovementSpeed * 1.3f : MovementSpeed)).SetY(0);
        move = Rb.velocity.magnitude > 0.1f;

        if (!move) Rb.velocity = Vector3.zero.SetY(0);
        else Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, Quaternion.LookRotation(Rb.velocity.SetY(0)), Time.deltaTime * 30f);

        if (move)
        {
            if (!longRun)
            {
                currentTimer += Time.deltaTime;
                if (currentTimer >= longRunTimer)
                {
                    longRun = true;
                    Anim.SetBool("LongRun", true);
                }
            }

            if (Physics.Raycast(transform.position, Vector3.down * 10, out RaycastHit hit))
                transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * 10f);
        }
        else
        {
            currentTimer = 0;
            if (longRun)
            {
                longRun = false;
                Anim.SetBool("LongRun", false);
            }
        }

        Anim.SetFloat("Blend", Mathf.Lerp(Anim.GetFloat("Blend"), move ? 1 : 0, Time.deltaTime * 30f));

        //ATTACKS

        if (attackCounter > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
                attackCounter = 0;
        }

        // Debugging the field of view
        Vector3 forward = Model.transform.forward;
        Vector3 right = Quaternion.Euler(0, 50, 0) * Model.transform.forward; // Rotate forward vector 45 degrees to the right
        Vector3 left = Quaternion.Euler(0, -50, 0) * Model.transform.forward; // Rotate forward vector 45 degrees to the left
        Debug.DrawRay(transform.position, forward * detectionRadius, Color.blue); // Draw forward ray
        Debug.DrawRay(transform.position, right * detectionRadius, Color.green); // Draw right limit of FOV
        Debug.DrawRay(transform.position, left * detectionRadius, Color.green); // Draw left limit of FOV

    }

    public void DamageTarget(int damage)
    {
        if (Target != null && !Target.IsDead())
        {
            Target.Damage(damage);
        }
    }

    void UseCapacity()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Enemy bestTarget = null;
        float bestScore = -Mathf.Infinity; // Le score initial est défini à -Infini pour garantir que la première entité sera sélectionnée.

        foreach (Collider item in colliders)
        {
            if (item.TryGetComponent<Enemy>(out var e))
            {
                if (Vector2.Distance(transform.position, e.transform.position) > 1.5f)
                {
                    Vector3 dirToEntity = (e.transform.position - transform.position).normalized;
                    float score = Vector3.Dot(Model.transform.forward, dirToEntity); // Calcul du score par produit scalaire

                    //field of view condition
                    if (score > Mathf.Cos(50 * Mathf.Deg2Rad)) // cos(90/2) = 0
                    {
                        if (score > bestScore) // Si le score actuel est meilleur que le meilleur score jusqu'à présent,
                        {
                            bestTarget = e; // alors cette entité devient la meilleure cible
                            bestScore = score; // et son score devient le meilleur score
                        }
                    }
                }
                else
                {
                    bestTarget = e;
                    break;
                }
            }
        }

        Target = bestTarget;
        if (Target != null)
        {
            Vector3 dir = (Target.transform.position - transform.position).normalized.SetY(0);
            Model.transform.rotation = Quaternion.LookRotation(dir);
            if (Vector3.Distance(Target.transform.position, transform.position) > 1.5f)
            {
                dashParticle.Play();
                trailParticle.Play();
            }
            transform.DOMove((Target.transform.position - dir * 2).SetY(transform.position.y), 0.2f)
            .OnComplete(() =>
            {
                dashParticle.Stop();
                trailParticle.Stop();
            });
        }

        Rb.velocity = Vector3.zero;

        Anim.Play("Capacity" + (attackCounter + 1));
        Capacities[attackCounter].Use();
    }
}