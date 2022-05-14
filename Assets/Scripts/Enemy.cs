using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Verkefni3
{
    public class Enemy : MonoBehaviour
    {
        public LayerMask groundMask, playerMask;
        public float timeBetweenAttacks = 1.0f;
        public float maxHealth;

        public NavMeshAgent m_Agent;
        public Transform m_Player;
        public Vector3 m_WalkPoint;
        public float m_WalkPointRange;
        public bool m_WalkPointSet;
        public float m_SightRange, m_AttackRange;
        public bool m_PlayerInSightRange, m_PlayerInAttackRange;
        public bool m_AlreadyAttacked;
        private float m_Health;
        private Player m_PlayerComp;

        private void Start()
        {
            m_Player = GameObject.FindWithTag("Player").transform;
            m_PlayerComp = GameObject.FindWithTag("Player").GetComponent<Player>();
            m_Agent = GetComponent<NavMeshAgent>();
            m_Health = maxHealth;
        }

        private void Update()
        {
            // hringlaga raycast sem er notað til að tjekka hvort það sé player innan við m_SightRange units 
            m_PlayerInSightRange = Physics.CheckSphere(transform.position, m_SightRange, playerMask);
            
            // hringlaga raycast sem er notað til að checka hvort það sé player innan viðð m_AttackRange units
            m_PlayerInAttackRange = Physics.CheckSphere(transform.position, m_AttackRange, playerMask);
            
            // ógeðslega mikil einföldun á state pattern
            
            // ef ég sé ekki player og ég er ekki að gera árás á player, labba ég um svæðið
            if (!m_PlayerInSightRange && !m_PlayerInAttackRange) Patrol();
            
            // ef ég sé player en ég er ekki nógu nálægt til að meiða hann, labba ég að playernum
            if (m_PlayerInSightRange && !m_PlayerInAttackRange) Chase();
            
            // ef ég bæði sé og er nógu nálagt, þá ræðst ég á playerinn
            if (m_PlayerInSightRange && m_PlayerInAttackRange) Attack();
        }

        public void ChangeHealth(int amount)
        {
            m_Health = Math.Clamp(m_Health + amount, 0, maxHealth);

            if (m_Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }


        private void Patrol()
        {
            if (!m_WalkPointSet) SetWalkPoint();

            if (m_WalkPointSet)
            {
                m_Agent.SetDestination(m_WalkPoint);
            }

            Vector3 distance = transform.position - m_WalkPoint;

            if (distance.magnitude < 1f)
            {
                m_WalkPointSet = false;
            }
        }

        private void SetWalkPoint()
        {
            // velja tvö random hnit innan við -m_WalkpointRange og m_WalkPointRange
            float x = Random.Range(-m_WalkPointRange, m_WalkPointRange);
            float z = Random.Range(-m_WalkPointRange, m_WalkPointRange);
            
            // segir scriptuni að þetta sé næsti staður
            m_WalkPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            // auka tjekk til að gá hvort þetta sé ekki örugglega land sem við erum að fara labba á
            if (Physics.Raycast(m_WalkPoint, -transform.up, 2f, groundMask))
            {
                m_WalkPointSet = true;
            }
        }

        private void Chase()
        {
            // líta á og labba að player
            m_Agent.SetDestination(m_Player.position);
            transform.LookAt(m_Player);
        }

        private void Attack()
        {
            // stoppa 
            m_Agent.SetDestination(transform.position);
            
            // horfa á player
            transform.LookAt(m_Player);

            // range check
            Vector3 distance = transform.position - m_Player.position;
            if (distance.magnitude  <= m_AttackRange + 1)
            {
                // cooldown svo enemy lemji ekki milljón grilljón sinnum á sekúndu
                if (!m_AlreadyAttacked)
                {
                    // tekur 10 health af player
                    m_PlayerComp.ChangeHealth(-10);
                    m_AlreadyAttacked = true;
                    
                    // resettar cooldownið eftir timeBetweenAttacks sekúndur
                    // invoke er mjög nice það gerir svona hluti auðveldari. það semsagt kallar á fall eftir x sek
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }

        private void ResetAttack()
        {
            m_AlreadyAttacked = false;
        }
    }
}