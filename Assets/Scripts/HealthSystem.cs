using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{

    private int maxHealthPoints;
    private int currentHealthPoints;

    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;

    public HealthSystem(int _maxHP)
    {
        maxHealthPoints = _maxHP;
        currentHealthPoints = maxHealthPoints;
    }

    public float GetCurrentHealthPercentage()
    {
        float x = maxHealthPoints;
        float y = currentHealthPoints;
        return y / x;
    }

    public void Damage(int damage)
    {
        currentHealthPoints -= damage;
        OnHealthChanged(this, EventArgs.Empty);

        if (currentHealthPoints <= 0)
            OnDead(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        currentHealthPoints += healAmount;

        if (currentHealthPoints > maxHealthPoints)
            currentHealthPoints = maxHealthPoints;

        OnHealthChanged(this, EventArgs.Empty);
    }
}
