using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour
{
    public  string name;
    public float health;
    public float maxHealth; // Default max health
    public Transform targetPosition;
    public bool isDeath = false;
    public bool isAttack = true;
    public Animator anim;
    public NavMeshAgent navMeshAgent;
    //private Timer damageTimer;
    public enum ZombieType
    {
       Hulk,Police,Small
    }

    public ZombieType zombieType;

    public EnemyData(string name, float health, Transform targetPosition, NavMeshAgent navMeshAgent, Animator enemyAnim, ZombieType zombieType)
    {
        this.name = name;
        this.health = health;
        this.maxHealth = health; // Set max health during initialization
        this.targetPosition = targetPosition;
        this.anim = enemyAnim;
        this.navMeshAgent = navMeshAgent;
        this.zombieType = zombieType;
    }

}
