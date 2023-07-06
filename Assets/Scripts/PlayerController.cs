using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour, IEntity
{
    public Rigidbody Rb;
    public Collider Cl;
    public GameObject Model;
    public Animator Anim;
    public Camera cam;
    public float MovementSpeed;

    Vector3 camDirection;

    public float longRunTimer = 2;
    float currentTimer;
    bool longRun;

    //PARTICLES
    public ParticleSystem dashParticle;
    public ParticleSystem trailParticle;

    //ATTACKS
    int attackCounter;
    float resetTime = 0.1f;
    float resetTimer;
    public Capacity[] CapacityPrefabs;
    public List<Capacity> Capacities = new List<Capacity>();

    int health;
    public int Health
    {
        get => health;
        set => health = value;
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
        }

        Anim.runtimeAnimatorController = overrideController;
    }

    void Update()
    {
        if (IsAttacking()) return;

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
            if (attackCounter < 1)
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
    }

    void UseCapacity()
    {
        //Detect and dash to enemies;
        Rb.velocity = Vector3.zero;

        Anim.Play("Capacity" + (attackCounter + 1));
        Capacities[attackCounter].Use();
    }
}