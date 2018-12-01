/*using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    // Variables to adjust basic camera speed
    public float interpVelocity;
    public float followSpeed;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;


    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }


    // Update is called once per second
    void FixedUpdate()
    {
        if (target)
        {
            
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            interpVelocity = targetDirection.magnitude * followSpeed;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);
            
        }
    }
}*/
using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    // Variables to adjust basic camera speed
    public float txPos, tyPos;
    public GameObject target;
    Vector3 targetPos;


    // Use this for initialization
    void Start()
    {
        txPos = transform.position.x;
        tyPos = transform.position.y;
        transform.position = new Vector3(txPos, tyPos, transform.position.z);
    }


    // Update is called once per second
    void FixedUpdate()
    {
        // Checks if the main character is farther than half the screen
        if (target.transform.position.x > 8)
        {
            txPos = target.transform.position.x;
        }
        // Checks if the main character is higher up than half the screen
        if (target.transform.position.y > 4.4)
        {
            tyPos = target.transform.position.y;
        }
        transform.position = new Vector3(txPos, tyPos, -10);
    }
}