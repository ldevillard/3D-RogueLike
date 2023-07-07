using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class PlayerController : MonoBehaviour, IEntity
{
    static public PlayerController Instance;

    public Rigidbody Rb;
    public Collider Cl;
    public GameObject Model;
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
    [ReadOnly] public Entity Target;

    public int health;
    public int Health
    {
        get => health;
        set => health = value;
    }

    public event Action<int, int> OnHealthChanged;

    [ReadOnly] public int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        camDirection = (cam.transform.position - transform.position.SetY(cam.transform.position.y)).normalized;

        FetchCapacities();
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (IsDead()) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return Health <= 0;
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

    void Update()
    {
        if (IsAttacking())
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (moveHorizontal * cam.transform.right + moveVertical * -camDirection).normalized;

        Rb.velocity = (movement * (longRun ? MovementSpeed * 1.3f : MovementSpeed)).SetY(0);
        bool move = Rb.velocity.magnitude > 0.1f;

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

            if (Input.GetKeyDown(KeyCode.Space))
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
        if (Input.GetMouseButtonDown(0))
        {
            if (attackCounter < 3)
            {
                UseCapacity();

                attackCounter++;
                resetTimer = resetTime;
            }
        }

        if (attackCounter > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
                attackCounter = 0;
        }

        // Debugging the field of view
        Vector3 forward = Model.transform.forward;
        Vector3 right = Quaternion.Euler(0, 60, 0) * Model.transform.forward; // Rotate forward vector 45 degrees to the right
        Vector3 left = Quaternion.Euler(0, -60, 0) * Model.transform.forward; // Rotate forward vector 45 degrees to the left
        Debug.DrawRay(transform.position, forward * detectionRadius, Color.blue); // Draw forward ray
        Debug.DrawRay(transform.position, right * detectionRadius, Color.green); // Draw right limit of FOV
        Debug.DrawRay(transform.position, left * detectionRadius, Color.green); // Draw left limit of FOV

    }

    public void DamageTarget(int damage)
    {
        if (Target != null)
        {
            Target.Damage(damage);
        }
    }

    void UseCapacity()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Entity bestTarget = null;
        float bestScore = -Mathf.Infinity; // Le score initial est défini à -Infini pour garantir que la première entité sera sélectionnée.

        foreach (Collider item in colliders)
        {
            if (item.TryGetComponent<Entity>(out var e))
            {
                Vector3 dirToEntity = (e.transform.position - transform.position).normalized;
                float score = Vector3.Dot(Model.transform.forward, dirToEntity); // Calcul du score par produit scalaire

                //field of view condition
                if (score > Mathf.Cos(60 * Mathf.Deg2Rad)) // cos(90/2) = 0
                {
                    if (score > bestScore) // Si le score actuel est meilleur que le meilleur score jusqu'à présent,
                    {
                        bestTarget = e; // alors cette entité devient la meilleure cible
                        bestScore = score; // et son score devient le meilleur score
                    }
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