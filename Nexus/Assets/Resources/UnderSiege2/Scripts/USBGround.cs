using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USBGround : MonoBehaviour
    {
        public float GroundHealth = 10;
        public float GroundMass = 10;
        public float DamageRadius = 3;
        public float HabitatRadius = 50;
        public bool AffectGravity;

        Vector3 StartPos;

        private void Awake()
        {
            StartPos = this.gameObject.transform.position;

            Rigidbody RigidBd = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            RigidBd.isKinematic = true;
            RigidBd.mass = GroundMass;
            RigidBd.interpolation = RigidbodyInterpolation.Interpolate;
            RigidBd.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            if (AffectGravity)
            {
                RigidBd.isKinematic = false;
                RigidBd.useGravity = true;
                RigidBd.Sleep();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("CheckHabitat", 0.0f, 1.0f);
        }

        // Update is called once per frame
        void Update()
        {
            ////Debug.log(this.gameObject.transform.position);
        }

        void CheckHabitat()
        {
            if (FastDistance(this.gameObject.transform, StartPos, HabitatRadius) == false)
            {
                Destroy(this.gameObject);
            }
        }

        bool FastDistance(Transform Self, Vector3 Target, float Radius)
        {
            bool Xpass = false;
            bool Zpass = false;
            bool Ypass = false;

            //x
            if ((Self.position.x >= 0 & Target.x >= 0) | (Self.position.x < 0 & Target.x < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.x) - Mathf.Abs(Target.x)) < Radius) Xpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.x) + Mathf.Abs(Target.x) < Radius) Xpass = true;
            }

            //y
            if ((Self.position.y >= 0 & Target.y >= 0) | (Self.position.y < 0 & Target.y < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.y) - Mathf.Abs(Target.y)) < Radius) Ypass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.y) + Mathf.Abs(Target.y) < Radius) Ypass = true;
            }

            //z
            if ((Self.position.z >= 0 & Target.z >= 0) | (Self.position.z < 0 & Target.z < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.z) - Mathf.Abs(Target.z)) < Radius) Zpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.z) + Mathf.Abs(Target.z) < Radius) Zpass = true;
            }

            if (Xpass & Zpass & Ypass) return true;
            else return false;
        }
    }
}
