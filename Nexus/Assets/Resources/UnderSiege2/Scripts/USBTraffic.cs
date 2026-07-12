using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USBTraffic : MonoBehaviour
    {
        [Header("maximum droppers in scene:")]
        public int MaxDroppersCount = 50;
        [Header("maximum drills in scene:")]
        public int MaxDrillsCount = 50;
        [Header("maximum drills particles:")]
        public int MaxDrillsPrtcls = 50;
        [Header("spawn frequency (min time):")]
        public float MinSpawnInterval = 0.3f;
        [Header("spawn frequency (max time):")]
        public float MaxSpawnInterval = 1.3f;
        [Header("start position scatter:")]
        public float MaxStartScatter = 3.0f;
        [Header("vehicle variety array:")]
        public GameObject[] DropperObjects;
        [Header("speed variety:")]
        public float LineSpeedVarMin = -2f;
        public float LineSpeedVarMax = 2f;

        [HideInInspector]
        public int DroppersCount;
        [HideInInspector]
        public int DrillsCount;
        [HideInInspector]
        public Transform[] SpawnPoints;
        [HideInInspector]
        public int DrillsPrtclsCount;

        bool locked;
        float[] CarSpeedVariations;
        GameObject VehicleContainer;

        void Awake()
        {
            if (transform.childCount != 0)
            {
                SpawnPoints = new Transform[transform.childCount];
            }
            else
            {
                Debug.Log(" <color=yellow> No traffic spawn points! </color>");
                locked = true;
                return;
            }

            for (int k = 0; k < transform.childCount; k++)
            {
                SpawnPoints[k] = transform.GetChild(k);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
            StartCoroutine(SpawnInterval(Random.Range(MinSpawnInterval, MaxSpawnInterval)));
            VehicleContainer = new GameObject { };
            VehicleContainer.name = "TrafficContainer";

            if (!locked)
            {
                CarSpeedVariations = new float[SpawnPoints.Length];
                for (int k0 = 0; k0 < CarSpeedVariations.Length; k0++)
                {
                    CarSpeedVariations[k0] = Random.Range(LineSpeedVarMin, LineSpeedVarMax);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("drp: " + DroppersCount + "  drl: " + DrillsCount + "  drl prtcls: " + DrillsPrtclsCount);
        }

        IEnumerator SpawnInterval(float rTime)
        {
            yield return new WaitForSeconds(rTime);

            if (!locked)
            {
                if (DroppersCount < MaxDroppersCount)
                {
                    for (int k0 = 0; k0 < SpawnPoints.Length; k0++)
                    {
                        GameObject obj = Instantiate(DropperObjects[Random.Range(0, DropperObjects.Length)]);

                        obj.transform.position = SpawnPoints[k0].position + Random.insideUnitSphere * MaxStartScatter;
                        obj.transform.localRotation = SpawnPoints[k0].localRotation;

                        if (obj.transform.gameObject.GetComponent<USEnemyShip>())
                        {
                            obj.transform.gameObject.GetComponent<USEnemyShip>().USBTrafficLink = transform.gameObject.GetComponent<USBTraffic>();
                            obj.transform.gameObject.GetComponent<USEnemyShip>().Speed += CarSpeedVariations[k0];
                        }
                        else
                        {
                            Debug.Log(" <color=yellow> Wrong vehicle! 'USBDropper' script is required </color>");
                            locked = true;
                            break;
                        }
                        obj.transform.parent = VehicleContainer.transform;
                        DroppersCount++;

                    }
                }
                StartCoroutine(SpawnInterval(Random.Range(MinSpawnInterval, MaxSpawnInterval)));
            }
        }

        void OnDrawGizmos()
        {
            for (int k0 = 0; k0 < transform.childCount; k0++)
            {
                Vector3 Pos = transform.GetChild(k0).transform.localPosition;
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(Pos, MaxStartScatter);
                Gizmos.DrawLine(transform.GetChild(k0).position, transform.GetChild(k0).position + transform.GetChild(k0).transform.TransformVector(Vector3.forward) * 130);
                Gizmos.color = Color.yellow;
                for (int k1 = 0; k1 < 4; k1++)
                {
                    Gizmos.DrawWireSphere(Pos += transform.GetChild(k0).transform.TransformVector(Vector3.forward) * 30, MaxStartScatter);
                }
            }
        }
    }
}
