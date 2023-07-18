using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraking : MonoBehaviour
{
    public Transform target;
    public float smooth;

    private Vector3 direction;
    private float distance;
    void FixedUpdate()
    {
        direction = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, 0);
        distance = Vector3.Distance(target.transform.position, transform.position);
        transform.Translate(direction * Mathf.Sqrt(distance) * Time.deltaTime * smooth);
    }
}
