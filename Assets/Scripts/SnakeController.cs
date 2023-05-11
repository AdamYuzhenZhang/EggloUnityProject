using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private RandomPointCloud m_PointCloud;
    [SerializeField] private float Speed = 5f;
    private Vector3 m_Target;

    [SerializeField] private GameObject m_Route;
    private float m_MovedDistance;
    [SerializeField] private float m_DistanceInterval = 2f;
    private void Start()
    {
        // generate first target
        m_Target = m_PointCloud.RandomPointInBound();
        Reorient();
    }

    private void Update()
    {
        // while not at target move towards it
        if (!IsAtTarget())
        {
            Vector3 moveDirection = (m_Target - transform.position).normalized;
            transform.position += moveDirection * Speed * Time.deltaTime;
            m_MovedDistance += Speed * Time.deltaTime;
            if (m_MovedDistance >= m_DistanceInterval)
            {
                DrawPath();
                m_MovedDistance = 0f;
            }
            
        }
        // at target, update target
        else
        {
            m_Target = m_PointCloud.RandomPointInBound();
            Reorient();
        }
    }

    private bool IsAtTarget()
    {
        return Vector3.Distance(m_Target, transform.position) < 0.5f;
    }

    private void Reorient()
    {
        transform.LookAt(m_Target);
    }

    private void DrawPath()
    {
        Instantiate(m_Route, transform.position, transform.rotation);
    }
}
