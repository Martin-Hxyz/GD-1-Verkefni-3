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
            m_PlayerInSightRange = Physics.CheckSphere(transform.position, m_SightRange, playerMask);
            m_PlayerInAttackRange = Physics.CheckSphere(transform.position, m_AttackRange, playerMask);

            if (!m_PlayerInSightRange && !m_PlayerInAttackRange) Patrol();
            if (m_PlayerInSightRange && !m_PlayerInAttackRange) Chase();
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
            float x = Random.Range(-m_WalkPointRange, m_WalkPointRange);
            float z = Random.Range(-m_WalkPointRange, m_WalkPointRange);

            m_WalkPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            if (Physics.Raycast(m_WalkPoint, -transform.up, 2f, groundMask))
            {
                m_WalkPointSet = true;
            }
        }

        private void Chase()
        {
            m_Agent.SetDestination(m_Player.position);
            transform.LookAt(m_Player);
        }

        private void Attack()
        {
            m_Agent.SetDestination(transform.position);
            transform.LookAt(m_Player);

            Vector3 distance = transform.position - m_Player.position;

            if (distance.magnitude  <= m_AttackRange + 1)
            {
                if (!m_AlreadyAttacked)
                {
                     m_PlayerComp.ChangeHealth(-10);
                    m_AlreadyAttacked = true;
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