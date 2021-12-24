using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachDelay = 0.2f;

    private Rigidbody2D currentBallRb;
    private SpringJoint2D currentBallSpringJoint;

    private Camera mainCamera;
    private bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!Touchscreen.current.primaryTouch.press.isPressed) {

            
            if (isDragging)
            {
                LaunchBall();
                
            }

            isDragging=false;
        
            return;
        }

        isDragging=true;
        currentBallRb.isKinematic = true;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);

        currentBallRb.position = worldPos;
        
    }

    private void SpawnNewBall() 
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRb = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        currentBallRb.isKinematic = false;
        currentBallRb = null;

        Invoke("DetachBall",detachDelay);
        
    }

    private void DetachBall() 
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        
        Invoke(nameof(SpawnNewBall),respawnDelay);
    }
}
