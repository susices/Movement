using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBall : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField]
    private float m_maxSpeed;
    [Range(0,100)]
    [SerializeField]
    private float m_maxAcceleration;
    [SerializeField]
    private Vector3 m_velocity;
    private Vector3 m_desiredVelocity;
    private Rigidbody m_rigidbody;
    private bool m_desiredJump;
    [SerializeField, Range(0f, 10f)] 
    private float m_jumpHeight = 2f;
    private bool m_onGround;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 playerInput;
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1);
        m_desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * m_maxSpeed;
        m_desiredJump |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        float maxSpeedChange = m_maxAcceleration * Time.deltaTime;
        m_velocity = m_rigidbody.velocity;
        m_velocity.x = Mathf.MoveTowards(m_velocity.x, m_desiredVelocity.x, maxSpeedChange);
        m_velocity.z = Mathf.MoveTowards(m_velocity.z, m_desiredVelocity.z, maxSpeedChange);
        if (m_desiredJump)
        {
            m_desiredJump = false;
            Jump();
        }
        
        m_rigidbody.velocity = m_velocity;
        m_onGround = false;
    }

    private void Jump()
    {
        if (!m_onGround)
        {
            return;
        }
        m_velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * m_jumpHeight);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_onGround = true;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        m_onGround = true;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,50,50),"Reset"))
        {
            this.m_velocity = Vector3.zero;
            this.transform.position = new Vector3(0f, 0.5f, 0f);
        }
    }
}
