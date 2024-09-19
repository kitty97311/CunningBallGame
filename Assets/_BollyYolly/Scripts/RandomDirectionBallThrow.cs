using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionBallThrow : MonoBehaviour
{
    [SerializeField] float randXPosRange = 2f;
    [SerializeField] float randZPosRange = 2f;
    [SerializeField] float yPos = 0.5f;
    [SerializeField] float forceToBall = 0.5f;
    [SerializeField] float timerToSpawnBall = 1f;
    [Tooltip("In radians")]
    [SerializeField] float maxAngleOfDeflection = 1.57f;
    [Tooltip("In radians")]
    [SerializeField] float minAngleOfDeflection = 1.04f;
    bool hasHitBall;
    float timer = 0f;
    float randAngleToThrow = 0f;
    float xPos, zPos = 0f;

    private void Start()
    {
        SelectRandomPositionForBox();
    }

    private void SelectRandomPositionForBox()
    {
        timer = timerToSpawnBall;
        xPos = Random.Range(-randXPosRange, randXPosRange);
        zPos = Random.Range(-randZPosRange, randZPosRange);
        transform.position = new Vector3(xPos, yPos, zPos);
    }
    private void Update()
    {
        if(hasHitBall)
        {
            timer -= Time.deltaTime; 
        }
        if(timer < 0f)
        {
            hasHitBall = false;
            SelectRandomPositionForBox();
            hasHitBall = false;
            GetComponent<BoxCollider>().enabled = true;
            timer = timerToSpawnBall;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        randAngleToThrow = Random.Range(minAngleOfDeflection, maxAngleOfDeflection); //taking the angle to deflect the ball
        Vector3 ballVelocityXZDirection = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0f, other.GetComponent<Rigidbody>().velocity.z).normalized;

        //formula for rotating a vector counterclockwise with an angle "randAngleToThrow"
        Vector3 randDirection = new Vector3(((ballVelocityXZDirection.x * Mathf.Cos(randAngleToThrow)) - (ballVelocityXZDirection.z * Mathf.Sin(randAngleToThrow))), 
                                            0f,
                                            ((ballVelocityXZDirection.x * Mathf.Sin(randAngleToThrow)) + (ballVelocityXZDirection.z * Mathf.Cos(randAngleToThrow))));
        other.GetComponent<Rigidbody>().velocity = randDirection * other.GetComponent<Rigidbody>().velocity.magnitude;
        hasHitBall = true;
        GetComponent<BoxCollider>().enabled = false;
    }

}
