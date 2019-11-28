using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    public class DragScript : MonoBehaviour
    {
        public Vector3 DragPoint { get; set; }
        public Vector3 DragAxis { get; set; }
        public Vector3 DragForce { get; private set; }

        public float DragClamp { get; set; }
        public Rigidbody myRigidbody;

        Vector3 dragPoint;
        Vector3 dragAxis;
        Vector3 dragDirection;
        float dragForce;

        void Awake()
        {
            if (StatMaster.isClient) return;

            myRigidbody = GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            if (StatMaster.isClient) return;

            dragPoint = transform.TransformPoint(DragPoint);
            dragAxis = DragAxis;

            dragDirection = Vector3.Scale(transform.InverseTransformDirection(myRigidbody.velocity), dragAxis);
            dragForce = Mathf.Clamp(myRigidbody.velocity.sqrMagnitude, 0, DragClamp);
            DragForce = transform.TransformDirection(dragDirection) * dragForce;

            myRigidbody.AddForceAtPosition(DragForce, dragPoint);
        }
    }
}
