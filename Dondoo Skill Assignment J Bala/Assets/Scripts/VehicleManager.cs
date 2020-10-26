/************************************************************************************
Copyright : None

Developer : Jonathan de Canha Bala

Script Description : 
    This script handles the movement of the vehicle using Unitys built in wheel colliders.
    This script allows for front, rear and all wheel drive and can easily be changed inside the unity inspector.
    This script also simulates braking as well as engine braking.
************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeXzXBlitZ {
    public class VehicleManager : MonoBehaviour {
        enum DrivingType {
            FrontWheelDrive,
            RearWheelDrive,
            FourWheelDrive
        }

        [Tooltip ("The user can decide which wheel type they can drive with")]
        [Header ("Settings")]
        [SerializeField] DrivingType drivingType;
        public float accelerationFactor;
        public float engineBrakingFactor;
        public float brakingFactor;
        [SerializeField] float maxSteeringAngle;

        [Header ("Front WheelColliders")]
        [Space (20)]
        [SerializeField] WheelCollider frontDriverWheelCollider;
        [SerializeField] WheelCollider frontPassangerWheelCollider;

        [Header ("Rear WheelColliders")]
        [SerializeField] WheelCollider rearDriverWheelCollider;
        [SerializeField] WheelCollider rearPassangerWheelCollider;

        [Header ("Front Wheel Meshes")]
        [Space (20)]
        [SerializeField] Transform frontDriverWheelMesh;
        [SerializeField] Transform frontPassangerWheelMesh;

        [Header ("Rear Wheel Meshes")]
        [SerializeField] Transform rearDriverWheelMesh;
        [SerializeField] Transform rearPassangerWheelMesh;
        public static VehicleManager instance;
        Rigidbody rb;

        // Creates the singleton
        private void Awake () {
            instance = this;
            rb = GetComponent<Rigidbody> ();
        }

        // The use of fixed update puts less strain on the system as less calculates are made per second
        private void FixedUpdate () {
            // Input handler, using the old unity input system, get the vertical and horizontal axis
            float horizontalInput = Input.GetAxis ("Horizontal");
            float verticalInput = Input.GetAxis ("Vertical");

            // Steering handler, based on the horizontal input, one can turn the vehicle in the desired direction
            float steeringAngle = maxSteeringAngle * horizontalInput;
            frontDriverWheelCollider.steerAngle = steeringAngle;
            frontPassangerWheelCollider.steerAngle = steeringAngle;

            // Acceleration handler
            switch (drivingType) {
                case DrivingType.FrontWheelDrive:
                    // Enable front wheel drive, thereofore only spinng the front wheels
                    frontDriverWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    frontPassangerWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    break;
                case DrivingType.RearWheelDrive:
                    // Enable rear wheel drive, therefore only spinning the rear wheels
                    rearDriverWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    rearPassangerWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    break;
                case DrivingType.FourWheelDrive:
                    // Enable four wheel drive, therefore spinning all four wheels
                    frontDriverWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    frontPassangerWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    rearDriverWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    rearPassangerWheelCollider.motorTorque = verticalInput * accelerationFactor;
                    break;
            }

            // See if the vehicle is in movement before tyring to brake
            if (rb.velocity.magnitude > 1) {
                // This allows for engine braking when there is no vertical input 
                if (verticalInput == 0) {
                    frontDriverWheelCollider.brakeTorque = engineBrakingFactor;
                    frontPassangerWheelCollider.brakeTorque = engineBrakingFactor;
                    rearDriverWheelCollider.brakeTorque = engineBrakingFactor;
                    rearPassangerWheelCollider.brakeTorque = engineBrakingFactor;
                }
                // Allows the ability to brake
                else if (verticalInput < 0) {
                    frontDriverWheelCollider.brakeTorque = brakingFactor;
                    frontPassangerWheelCollider.brakeTorque = brakingFactor;
                    rearDriverWheelCollider.brakeTorque = brakingFactor;
                    rearPassangerWheelCollider.brakeTorque = brakingFactor;
                }
            } else {
                // Brake locking fail safe
                frontDriverWheelCollider.brakeTorque = 0;
                frontPassangerWheelCollider.brakeTorque = 0;
                rearDriverWheelCollider.brakeTorque = 0;
                rearPassangerWheelCollider.brakeTorque = 0;
            }

            //Update the wheel meshes to reflect what happening with each respective collider
            UpdateWheelPositions (frontDriverWheelCollider, frontDriverWheelMesh);
            UpdateWheelPositions (frontPassangerWheelCollider, frontPassangerWheelMesh);
            UpdateWheelPositions (rearDriverWheelCollider, rearDriverWheelMesh);
            UpdateWheelPositions (rearPassangerWheelCollider, rearPassangerWheelMesh);
        }

        void UpdateWheelPositions (WheelCollider _collider, Transform _transform) {
            Vector3 _pos = _transform.position;
            Quaternion _quat = _transform.rotation;

            // Out the desired colliders position and rotation to a local variable that is then assigned to the mesh 
            _collider.GetWorldPose (out _pos, out _quat);
            _transform.position = _pos;
            _transform.rotation = _quat;
        }
    }
}