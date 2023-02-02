using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.Serialization;
using UnityEditor;
 
[Serializable]
public class Status
{
    [SerializeField] private int turns;

    [SerializeField] private int health;
    [SerializeField] private int healthDoT;
    
    [SerializeField] private int speed;
    [SerializeField] private int speedDoT;
    private int speedLost;
    
    [SerializeField] private int defence;
    [SerializeField] private int defenceDoT;
    private int defenceLost;
    
    [SerializeField] private int damage;
    [SerializeField] private int damageDoT;
    private int damageLost;

    public Status (int health = int.MinValue, int healthDoT = int.MinValue,
                   int speed = int.MinValue, int speedDoT = int.MinValue,
                   int defence = int.MinValue, int defenceDoT = int.MinValue,
                   int damage = int.MinValue, int damageDoT = int.MinValue) {
        this.health = health;
        this.healthDoT = healthDoT;
        this.speed = speed;
        this.speedDoT = speedDoT;
        this.defence = defence;
        this.defenceDoT = defenceDoT;
        this.damage = damage;
        this.damageDoT = damageDoT;
    }

    private void Apply(Character c) {
        c.Health.Boost -= health;
        c.Speed.Boost -= speed;
        c.Defence.Boost -= defence;
        c.Damage.Boost -= damage;

        speedLost = speed;
        defenceLost = defence;
        damageLost = damage;
    }

    public bool Tick(Character c) {
        if (turns > 0) {
            c.Health.Value -= healthDoT;
            c.Speed.Boost -= speedDoT;
            c.Defence.Boost -= defenceDoT;
            c.Damage.Boost -= damageDoT;

            speedLost += speedDoT;
            defenceLost += defenceDoT;
            damageDoT += damageDoT;
        }
        else {
            return Remove(c);
        }
        turns--;

        return true;
    }

    private bool Remove(Character c) {
        c.Health.Boost += health;
        c.Speed.Boost += speedLost;
        c.Defence.Boost += defenceLost;
        c.Damage.Boost += damageDoT;

        return false;
    }

    public virtual void Attack(Character e) {}
}
