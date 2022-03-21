using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Hand : MonoBehaviour
{
    //Physics Movement
    [Space]
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] private float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] private LayerMask grabbableLayer;

    private Transform _followTarget;
    private Rigidbody _body;

    private bool _isGrabbing;
    private GameObject _heldObject;
    private Transform _grabPoint;
    private FixedJoint _joint1, _joint2;

    // Start is called before the first frame update
    void Start()
    {
        //Physics Movement
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;

        //INputs Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.cancelled += Release;

        //Teleport Hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        //Postition
        var postionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(_followTarget.position, transform.position);
        _body.velocity = (_followTarget.position - transform.position).normalized * (followSpeed * distance);
        //Rotation
        var RotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = _followTarget.rotation * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    private void Grab(InputAction.CallbackContext context)
    {
        if(_isGrabbing || _heldObject) return;
        
        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        if (grabbableColliders.Length < 1) return;

        var objectToGrab = grabbableColliders[0].transform.gameObject;

        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if(objectBody != null)
        {
            _heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent<RigidBody>();
            if
        }
    }
}
