using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damage = 5;
    public LayerMask damagableLayers;

    [SerializeField] private float attackRate = 2f;
    private float attackTimer = 0f;

    void Awake()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.localPosition = new Vector3(0.25f, startPosition.y + 1 * Mathf.Sign(Input.GetAxis("Vertical")), startPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(0.25f + startPosition.x * Mathf.Sign(Input.GetAxis("Horizontal")), startPosition.y, startPosition.z);
        }

        if (Time.time >= attackTimer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                attackTimer = Time.time + 1f / attackRate;
            }
        }
    }

    private void Attack()
    {
        //������ �������� ����� ���


        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, damagableLayers);

        foreach (Collider2D hitObject in hitObjects)
        {
            hitObject.GetComponent<Damagable>().doDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
