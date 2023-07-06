using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Zombie : MonoBehaviour
{
    public Vector3 direction = Vector3.left;
    public float speed = 10f;
    public float HP = 100;
    public float lostHeadHP = 50;
    public float attackInterval = 0.5f;
    public float damage = 20f;
    private Animator animator;
    private GameObject head;
    private bool isWalking;
    private bool isLostHead;
    private bool isDead;
    private float curHP;
    private float attackTimer;
    void Start()
    {
        isWalking = true;
        isLostHead = false;
        isDead = false;
        curHP = HP;
        attackTimer = 0;
        animator = GetComponent<Animator>();
        head = transform.Find("ZombieHead").gameObject;
    }

    void Update()
    {
        if (isWalking)
            Walk();
    }

    public void GetDamage(float damage)
    {
        curHP -= damage;
        if (!isLostHead && curHP <= lostHeadHP)
            LoseHead();
        if (curHP <= 0)
            Die();
    }

    public void LoseHead()
    {
        isLostHead = true;
        animator.SetBool("LostHead", true);
        head.SetActive(true);
    }

    public void Die()
    {
        isWalking = false;
        isDead = true;
        animator.SetTrigger("Die");
    }

    public void DieAniOver()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.tag == "Plant")
        {
            isWalking = false;
            animator.SetBool("Walk", false);
            attackTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;
        if (other.tag == "Plant")
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                other.GetComponent<Plant>().GetDamage(damage);
                attackTimer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDead) return;
        if (other.tag == "Plant")
        {
            isWalking = true;
            animator.SetBool("Walk", true);
            attackTimer = 0;
        }
    }

    private void Walk()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
