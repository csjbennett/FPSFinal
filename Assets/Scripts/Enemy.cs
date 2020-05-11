using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Player;
    public Transform PlayerPos;
    public int Health;
    public GameObject DeathEffect;
    public GameObject Head;
    private Animator Anm;
    public GameObject GunBarrel;
    public GameObject Bullet;
    public GameObject MuzzleFlash;
    private float Distance;
    private AudioSource Audio;
    public AudioClip ShootSound;
    public AudioSource Audio2;
    private float LoopTime;
    public bool PlayerSeen;
    public AudioClip StepSound;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent = GetComponent<NavMeshAgent>();
        Anm = GetComponent<Animator>();
        StartCoroutine(Shoot());
        Audio = GetComponent<AudioSource>();
        LoopTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSeen)
        {
            PlayerPos = Player.transform;
            Agent.SetDestination(PlayerPos.transform.position);

            Head.transform.LookAt(PlayerPos);

            if (Mathf.Abs(Agent.velocity.x) > 0 || Mathf.Abs(Agent.velocity.y) > 0)
                Anm.Play("Walk");
            else
                Anm.Play("Idle");

            Distance = Vector3.Distance(this.transform.position, PlayerPos.transform.position);

            if (Distance < 8)
                Stop();
            else
                Agent.isStopped = false;
        }
        else
            PlayerPos = this.transform;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Health -= 1;
        }
        if (Health == 0)
        {
            Instantiate(DeathEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(LoopTime);
        RaycastHit Hit;
        Ray hitRay = new Ray (Head.transform.position, Head.transform.forward);
        if (Physics.Raycast(hitRay, out Hit))
        {
            if (Hit.collider.tag == "Player")
            {
                Instantiate(Bullet, GunBarrel.transform.position, Head.transform.rotation);
                Instantiate(MuzzleFlash, GunBarrel.transform);
                Audio.PlayOneShot(ShootSound);
                LoopTime = Random.Range(0.5f, 1.5f);
            }
            else
                LoopTime = 0.1f;
        }
        StartCoroutine(Shoot());
    }

    private void Stop()
    {
        Agent.isStopped = true;
    }

    public void Step()
    {
        Audio2.PlayOneShot(StepSound);
    }
}
