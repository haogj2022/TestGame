using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int HP;
    protected int currentHP;
    protected bool isDead;

    private void Awake()
    {
        currentHP = HP;
    }

    public void SetHP(int value)
    {
        currentHP = value;
    }

    public int damage;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Dead();
        }
    }

    protected void Flip(float direction)
    {

        transform.rotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);

    }

    public virtual void Attack()
    {

    }

    public virtual void Dead()
    {
        isDead = true;
    }
}
