using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _pivot;
    [SerializeField] private float _spawnDelay = 4;

    private bool _ballLaunched;
    private bool _canDrag;
    private bool _isDragging;
    private Camera _main;
    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    
    void Start()
    {
        _canDrag = true;
        _main = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody2D == null) return;
        
        DetectTouch();
        
        DetachBall();
    }

    private void DetectTouch()
    {
        // if screen isn't being touched
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            // but was...
            if (_isDragging)
            {
                LaunchPlayer();
            }
            _isDragging = false;
            return;
        }
        
        if (_canDrag)
        {
            _isDragging = true;
            _rigidbody2D.isKinematic = true;

            // Get position of touch in pixels
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            // Convert position from pixels to world
            Vector3 worldPosition = _main.ScreenToWorldPoint(touchPosition);
            worldPosition.z = transform.position.z;
            
            // Set player position to finger
            transform.position = worldPosition;
        }
    }

    private void LaunchPlayer()
    {
        _rigidbody2D.isKinematic = false;
        _canDrag = false;
    }
    
    private void DetachBall()
    {
        if (transform.position.x > _pivot.transform.position.x && !_isDragging)
        {
            // _ballLaunched = true;
            _springJoint2D.enabled = false;
            // _canDetach = false;
        }
            // StartCoroutine(ResetBallRoutine());
    }
    
    IEnumerator ResetBallRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        _ballLaunched = false;
        _canDrag = true;
        transform.position = _pivot.transform.position;
        _springJoint2D.enabled = true;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.rotation = 0;
    }
}
