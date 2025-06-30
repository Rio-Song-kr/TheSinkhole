using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    [SerializeField] private int m_moveSpeed;
    [SerializeField] private int m_rotateSpeed;
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_rigidbody.velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            m_rigidbody.velocity = transform.forward * m_moveSpeed;
        if (Input.GetKey(KeyCode.S))
            m_rigidbody.velocity = -transform.forward * m_moveSpeed;
        if (Input.GetKey(KeyCode.A))
            m_rigidbody.rotation = Quaternion.Euler(0, m_rigidbody.rotation.eulerAngles.y - m_rotateSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            m_rigidbody.rotation = Quaternion.Euler(0, m_rigidbody.rotation.eulerAngles.y + m_rotateSpeed * Time.deltaTime, 0);
    }
}