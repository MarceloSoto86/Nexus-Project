using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USBDrill : MonoBehaviour
    {
        public string GroundLayer = "GroundLayer";
        public float Speed = 5;
        public float Damage = 0.05f;
        public float DetectRadiusDef = 5;
        public float DetectRadiusBig = 15;
        public float DamageRadius = 1f;
        public ParticleSystem _Particle;
        [HideInInspector]
        public GameObject CurrentTarget;
        int LayerMask1;
        USEnemyShip _USBTrafficLink;

        void Awake()
        {
            LayerMask1 = LayerMask.GetMask(GroundLayer);
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
        }

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("DetectGroundSM", 0.0f, 0.5f);
            InvokeRepeating("DetectGroundBG", 0.0f, 1.0f);
            _Particle.gameObject.SetActive(false);
            _USBTrafficLink = GetComponent<USEnemyShip>();
        }

        void OnEnable()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentTarget)
            {
                if (FastDistance(transform, CurrentTarget.transform, CurrentTarget.GetComponent<USBGround>().DamageRadius))
                {
                    CurrentTarget.GetComponent<USBGround>().GroundHealth -= Damage;
                    if (CurrentTarget.GetComponent<USBGround>().GroundHealth <= 0)
                    {
                        CurrentTarget.layer = 0; //no more target
                        CurrentTarget.GetComponent<Rigidbody>().isKinematic = false; //fly away
                        CurrentTarget = null;
                        StopParticle();
                        return;
                    }
                    StartParticle();
                }
                else
                {
                    StopParticle();
                }
            }
            else
            {

            }
        }

        void FixedUpdate()
        {
            //look
            LookAtTarget();

            //move
            MoveToTarget();
        }

        void DetectGroundSM()
        {
            //if (!CurrentTarget)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectRadiusDef, LayerMask1);
                if (hitColliders.Length > 0)
                {
                    CurrentTarget = hitColliders[0].gameObject;
                }
            }
        }

        void DetectGroundBG()
        {
            transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            if (!CurrentTarget)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectRadiusBig, LayerMask1);
                if (hitColliders.Length > 0)
                {
                    CurrentTarget = hitColliders[UnityEngine.Random.Range(0, hitColliders.Length)].transform.gameObject;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }

        void LookAtTarget()
        {
            if (CurrentTarget)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(CurrentTarget.transform.position - transform.position), 6 * Time.deltaTime);
            }
        }

        void MoveToTarget()
        {
            Vector3 futur_pos = transform.TransformDirection(new Vector3(0, 0, Speed * Time.deltaTime));
            Vector3 step_pos = transform.position + futur_pos;
            GetComponent<Rigidbody>().MovePosition(step_pos);
        }

        bool FastDistance(Transform Self, Transform Target, float Radius)
        {
            bool Xpass = false;
            bool Zpass = false;
            bool Ypass = false;

            //x
            if ((Self.position.x >= 0 & Target.position.x >= 0) | (Self.position.x < 0 & Target.position.x < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.x) - Mathf.Abs(Target.position.x)) < Radius) Xpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.x) + Mathf.Abs(Target.position.x) < Radius) Xpass = true;
            }

            //y
            if ((Self.position.y >= 0 & Target.position.y >= 0) | (Self.position.y < 0 & Target.position.y < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.y) - Mathf.Abs(Target.position.y)) < Radius) Ypass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.y) + Mathf.Abs(Target.position.y) < Radius) Ypass = true;
            }

            //z
            if ((Self.position.z >= 0 & Target.position.z >= 0) | (Self.position.z < 0 & Target.position.z < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.position.z) - Mathf.Abs(Target.position.z)) < Radius) Zpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.position.z) + Mathf.Abs(Target.position.z) < Radius) Zpass = true;
            }

            if (Xpass & Zpass & Ypass) return true;
            else return false;
        }

        void OnDestroy()
        {
            _USBTrafficLink.USBTrafficLink.DrillsCount--;
            if (_Particle.gameObject.activeSelf) _USBTrafficLink.USBTrafficLink.DrillsPrtclsCount--;
        }

        void StartParticle()
        {
            //start particle
            if (!_Particle.gameObject.activeSelf)
            {
                if (_USBTrafficLink.USBTrafficLink.DrillsPrtclsCount < _USBTrafficLink.USBTrafficLink.MaxDrillsPrtcls)
                {
                    _Particle.gameObject.SetActive(true);
                    _USBTrafficLink.USBTrafficLink.DrillsPrtclsCount++;
                }
            }
        }

        void StopParticle()
        {
            //stop particle
            if (_Particle.gameObject.activeSelf)
            {
                _Particle.gameObject.SetActive(false);
                _USBTrafficLink.USBTrafficLink.DrillsPrtclsCount--;
            }
        }
    }
}
