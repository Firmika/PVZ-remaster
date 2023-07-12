using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : Plant
{
    public float damage = 500f;
    public float attackDist = 120f;
    public float updateInterval = 0.5f;
    public float attackHorizontalOffset = 20;
    public float attackVerticalOffset = 120;
    public float moveSpeed = 300;
    private Vector3 upPos;
    private Vector3 downPos;
    private GameObject closestZombie = null;
    protected override void Start()
    {
        base.Start();
        // 攻击状态每0.5秒更新一次
        InvokeRepeating("UpdateAttackState", 0, updateInterval);
    }

    private void UpdateAttackState()
    {
        closestZombie = GameManager.instance.GetClosestZombie(this.gameObject);
        if (closestZombie == null || Vector2.Distance(transform.position, closestZombie.transform.position) > attackDist)
            return;
        // 监测到僵尸并发动攻击
        // 停止攻击状态更新
        CancelInvoke();
        // 不可被僵尸啃食
        GetComponent<Collider2D>().enabled = false;
        AudioManager.instance.PlaySE(Globals.SquashHmm);
        // 僵尸在左边
        if (transform.position.x > closestZombie.transform.position.x)
        {
            animator.SetTrigger("Left");
            upPos = closestZombie.transform.position + new Vector3(attackHorizontalOffset, attackVerticalOffset, 0);
            downPos = closestZombie.transform.position + new Vector3(attackHorizontalOffset, 0, 0);
        }
        // 僵尸在右边
        else
        {
            animator.SetTrigger("Right");
            upPos = closestZombie.transform.position + new Vector3(-attackHorizontalOffset, attackVerticalOffset, 0);
            downPos = closestZombie.transform.position + new Vector3(-attackHorizontalOffset, 0, 0);
        }
    }

    IEnumerator MoveTo(Vector3 tgtPos)
    {
        while (Vector3.Distance(transform.position, tgtPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, tgtPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void EndPeek()
    {
        animator.SetTrigger("AttackUp");
        StartCoroutine(MoveTo(upPos));
    }

    public void EndAttackUp()
    {
        animator.SetTrigger("AttackDown");
        StartCoroutine(MoveTo(downPos));
    }

    public void Attack()
    {
        if (closestZombie == null)
            return;
        AudioManager.instance.PlaySE(Globals.GargantuarThump);
        closestZombie.GetComponent<Zombie>().GetDamage(damage);
    }

    public void EndAttackDown()
    {
        DestroyPlant();
    }
}
