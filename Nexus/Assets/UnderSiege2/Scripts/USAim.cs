using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USAim : MonoBehaviour
    {
        public bool DownTurret;
        public float FireRate = 0.2f;
        public GameObject Bullet;
        public float AimSpeed = 5.0f;
        [Header("Note: non customizable below")]
        public GameObject GunObject;
        public GameObject BaseObject;
        public GameObject BsLooker;
        public GameObject HdLooker;
        public bool MltTurret;
        public GameObject ResetObject;
        public Animator PrimAnimat;
        public Animator SecAnimat;
        public bool HotGun;
        public string EmissiveString = "_EmissionColor"; //replace with _EmissiveColor for hdrp usage
        public GameObject HotGunObj1;
        public GameObject HotGunObj2;
        [ColorUsageAttribute(true, true)]
        public Color MaxBright;
        public float AnimationRate = 0.2f;
        public float VertOffset;
        public float HorizOffset = 0.7f;
        public GameObject VisualLazer;
        [HideInInspector]
        public USResponseArea ResponseArea;

        GameObject Target;
        int rnd;
        bool VisualLocked;
        Material _Material1;
        Material _Material2;
        Color TempColor;
        bool TargetFound;
        bool IsAimed;

        void Awake()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("FireRateTM");
            if (VisualLazer) VisualLazer.transform.localScale = new Vector3(0, 0, 0);
            if (HotGun)
            {
                if (HotGunObj1) _Material1 = HotGunObj1.transform.GetComponent<Renderer>().materials[0];
                if (HotGunObj2) _Material2 = HotGunObj2.transform.GetComponent<Renderer>().materials[0];
            }

            if (DownTurret)
            {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0, 0, 180);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (TargetFound)
            {
                if (Target != null)
                {
                    if (DownTurret)
                    {
                        if (Target.transform.position.y < BaseObject.transform.position.y)
                        {
                            Aim();
                        }
                        else
                        {
                            TargetFound = false;
                        }
                    }
                    else
                    {
                        if (Target.transform.position.y > BaseObject.transform.position.y)
                        {
                            Aim();
                        }
                        else
                        {
                            TargetFound = false;
                        }
                    }

                    if (FastDistance(transform.position, Target.transform.position, ResponseArea.TriggerSphereRadius + (ResponseArea.TriggerSphereRadius * 0.5f)))
                    {
                        TargetFound = false;
                    }
                }
                else
                {
                    TargetFound = false;
                }
            }
            else
            {
                GetNewTarget();
            }

            if (ResetObject)
            {
                ResetObject.transform.rotation = Quaternion.identity;
                if (DownTurret) ResetObject.transform.Rotate(0, 0, 180);
            }

            HotGunExec();
        }

        void Aim()
        {
            if (DownTurret)
            {
                BsLooker.transform.LookAt(new Vector3(Target.transform.position.x, BsLooker.transform.position.y, Target.transform.position.z));
                BsLooker.transform.Rotate(0, 0, 180);
                HdLooker.transform.LookAt(Target.transform);
                if (!MltTurret) HdLooker.transform.Rotate(270, 0, 180);
            }
            else
            {
                BsLooker.transform.LookAt(new Vector3(Target.transform.position.x, BsLooker.transform.position.y, Target.transform.position.z));
                HdLooker.transform.LookAt(Target.transform);
                if (!MltTurret) HdLooker.transform.Rotate(-270, 0, 0);
            }

            BaseObject.transform.localRotation = Quaternion.Slerp(BaseObject.transform.localRotation, BsLooker.transform.localRotation, AimSpeed * Time.deltaTime);
            GunObject.transform.localRotation = Quaternion.Slerp(GunObject.transform.localRotation, HdLooker.transform.localRotation, AimSpeed * Time.deltaTime);

            if (IsAimed)
            {
                FireBullet();
                IsAimed = false;
                StartCoroutine("FireRateTM");

                if (PrimAnimat)
                {
                    PrimAnimat.Rebind();
                    PrimAnimat.Play("Base Layer.Fire", 0, 0.1f);
                    PrimAnimat.speed = 4f;
                }
                if (SecAnimat)
                {
                    SecAnimat.Rebind();
                    SecAnimat.Play("Base Layer.Fire", 0, 0.1f);
                    SecAnimat.speed = 0.5f;
                }

                if (HotGun)
                {
                    TempColor = Color.Lerp(TempColor, MaxBright, 5.5f * Time.deltaTime);
                    if (HotGunObj1) _Material1.SetColor(EmissiveString, TempColor);
                    if (HotGunObj2) _Material2.SetColor(EmissiveString, TempColor);
                }
            }
        }

        void FireBullet()
        {
            if (TargetFound)
            {
                GameObject fire = Instantiate(Bullet);
                fire.transform.parent = GunObject.transform;
                fire.transform.localPosition = Vector3.zero;
                fire.transform.localPosition = fire.transform.TransformDirection(new Vector3(0, VertOffset, HorizOffset));
                fire.transform.localRotation = Quaternion.identity;
                fire.transform.parent = null;
                if (VisualLazer)
                {
                    if (!VisualLocked)
                    {
                        StartCoroutine(VisualLazerTime());
                        VisualLocked = true;
                    }
                }
            }
        }

        void GetNewTarget()
        {
            if (ResponseArea)
            {
                if (ResponseArea.USTargetShips.Length > 0)
                {
                    rnd = UnityEngine.Random.Range(0, ResponseArea.USTargetShips.Length - 1);
                    if (ResponseArea.USTargetShips[rnd] != null)
                    {
                        Target = ResponseArea.USTargetShips[rnd].transform.gameObject;
                        TargetFound = true;
                    }
                }
            }
        }

        IEnumerator VisualLazerTime()
        {
            VisualLazer.transform.localScale = new Vector3(0.8f, 0.8f, 14000);
            yield return new WaitForSeconds(0.5f);
            VisualLazer.transform.localScale = new Vector3(0, 0, 0);
            VisualLocked = false;
        }

        void HotGunExec()
        {
            if (HotGun)
            {
                if (!TargetFound)
                {
                    TempColor = Color.Lerp(TempColor, Color.black, 2.0f * Time.deltaTime);
                    if (HotGunObj1) _Material1.SetColor(EmissiveString, TempColor);
                    if (HotGunObj2) _Material2.SetColor(EmissiveString, TempColor);
                }
            }
        }

        bool FastDistance(Vector3 Self, Vector3 Target, float Radius)
        {
            bool Xpass = false;
            bool Ypass = false;
            bool Zpass = false;

            //x
            if ((Self.x >= 0 & Target.x >= 0) | (Self.x < 0 & Target.x < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.x) - Mathf.Abs(Target.x)) > Radius) Xpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.x) + Mathf.Abs(Target.x) > Radius) Xpass = true;
            }

            //y
            if ((Self.y >= 0 & Target.y >= 0) | (Self.y < 0 & Target.y < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.y) - Mathf.Abs(Target.y)) > Radius) Ypass = true;
            }
            else
            {
                if (Mathf.Abs(Self.y) + Mathf.Abs(Target.y) > Radius) Ypass = true;
            }

            //z
            if ((Self.z >= 0 & Target.z >= 0) | (Self.z < 0 & Target.z < 0))
            {
                if (Mathf.Abs(Mathf.Abs(Self.z) - Mathf.Abs(Target.z)) > Radius) Zpass = true;
            }
            else
            {
                if (Mathf.Abs(Self.z) + Mathf.Abs(Target.z) > Radius) Zpass = true;
            }

            if (Xpass | Ypass | Zpass) return true;
            else return false;
        }

        IEnumerator FireRateTM()
        {
            yield return new WaitForSeconds(FireRate);
            IsAimed = true;
        }
    }
}
