/************************************************************************************
Copyright : None

Developer : Jonathan de Canha Bala

Script Description : 
    This script is found on the camera root and allows this object to follow the vehicles position and rotation with
    the use of a lerp to smooth the transition, the x and y rotation of the object is clamped to 0 and quaternions are used to avoid gimbal locking
************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeXzXBlitZ {
    public class CameraFollow : MonoBehaviour {
        [Tooltip ("Slot in the desired object to be followed (The vehicle in this case)")]
        [SerializeField] Transform vehicle;
        [Tooltip ("The positional lerp speed)")]
        [SerializeField] float speed;
        [Tooltip ("The rotational lerp speed)")]
        [SerializeField] float rotationalSpeed;
        void FixedUpdate () {
            transform.position = Vector3.Lerp (transform.position, vehicle.transform.position, speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp (transform.rotation, vehicle.rotation, rotationalSpeed * Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
        }
    }
}