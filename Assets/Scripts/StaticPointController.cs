using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPointController : MonoBehaviour
{
    public GameObject m_Attractor;
    
    private float m_Speed;
    public Vector3 m_Direction;
    private Vector3 m_LastAdjustedDirection;

    private bool moved;
    private void Update()
    {
        // speed of attractor
        float attractorSpeed = Vector3.Magnitude(m_Direction) / Time.deltaTime;
        Vector3 normalizedDirection = m_Direction.normalized;
        Debug.Log(attractorSpeed);
        //Debug.Log("Position");
        // from here
        Vector3 nextPosition = transform.position;
        //Debug.Log(nextPosition);
        //Debug.Log(m_Speed);
        // move by speed
        nextPosition += m_LastAdjustedDirection * m_Speed * Time.deltaTime;
        //Debug.Log(nextPosition);
        // slow down by friction
        nextPosition -= m_LastAdjustedDirection * m_Speed * 0f * Time.deltaTime * Time.deltaTime;
        //Debug.Log(nextPosition);
        // speed up by attractor
        float distance = Vector3.Distance(transform.position, m_Attractor.transform.position);

        if (distance < 0.3f)
        {
            //Debug.Log("Speed");
            //Debug.Log(m_Speed);

            //Debug.Log(attractorSpeed);
            //Debug.Log(distance);
            //Debug.Log(0.1f / distance);
            //nextPosition += normalizedDirection * attractorSpeed * Time.deltaTime;
            nextPosition += normalizedDirection * attractorSpeed * Mathf.Clamp((0.3f / distance), 0.01f, 0.2f) * Time.deltaTime;
            //Debug.Log(nextPosition);
        }

        //float newSpeed = (m_Distance < 0.2f) ? (1 / m_Distance) : 0;
        //float reducedSpeed = m_Speed * (1 - Time.deltaTime / 5);
        //m_Speed = (newSpeed > reducedSpeed) ? newSpeed: reducedSpeed;
        //Debug.Log("FinalValues");

        // set speed
        m_Speed = Vector3.Distance(transform.position, nextPosition) / Time.deltaTime;
        //Debug.Log(transform.position);
        //Debug.Log(nextPosition);
        //Debug.Log(m_Speed);

        // set direction
        m_LastAdjustedDirection = nextPosition - transform.position;
        //Debug.Log(m_LastAdjustedDirection);

        transform.position = nextPosition;
        moved = true;
    }
}
