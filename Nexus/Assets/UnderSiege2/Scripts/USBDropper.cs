using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USBDropper : MonoBehaviour
    {
        public string GroundLayer = "GroundLayer";
        public float DetectRadius = 30f;
        public float DetectGroundRate = 0.3f;
        public GameObject DropObject;
        public bool ExcludeZeroChild;

        [HideInInspector]
        public Transform[] DropPoints;

        USBTraffic USBTrafficLink;

        int LayerMask1;
        bool IsDropped;
        bool locked;

        void Awake()
        {
            LayerMask1 = LayerMask.GetMask(GroundLayer);
            if (transform.childCount != 0)
            {
                DropPoints = new Transform[transform.childCount];
            }
            else
            {
                Debug.Log(" <color=yellow> No drop points! </color>");
                locked = true;
                return;
            }

            for (int k = 0; k < transform.childCount; k++)
            {
                DropPoints[k] = transform.GetChild(k);
            }

            if (!DropObject.GetComponent<USBDrill>())
            {
                Debug.Log(" <color=yellow> wrong drill object! (USBDrill.cs is required) </color>");
                locked = true;
                return;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!locked) StartCoroutine("DetectGround");
            USBTrafficLink = GetComponent<USEnemyShip>().USBTrafficLink;
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator DetectGround()
        {
            yield return new WaitForSeconds(DetectGroundRate);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectRadius, LayerMask1);
            if (hitColliders.Length > 0)
            {
                if (USBTrafficLink.DrillsCount < USBTrafficLink.MaxDrillsCount)
                {
                    int k00 = 0;
                    if (ExcludeZeroChild) k00 = 1;
                    for (int k0 = k00; k0 < DropPoints.Length; k0++)
                    {
                        GameObject obj = Instantiate(DropObject);
                        obj.transform.position = DropPoints[k0].transform.position;
                        obj.transform.localRotation = Quaternion.identity;
                        int rnd = 0;
                        if (hitColliders.Length > 1) rnd = UnityEngine.Random.Range(0, hitColliders.Length - 1);
                        obj.transform.GetComponent<USBDrill>().CurrentTarget = hitColliders[rnd].gameObject;
                        obj.transform.GetComponent<USEnemyShip>().USBTrafficLink = USBTrafficLink;
                        USBTrafficLink.DrillsCount++;
                    }
                    IsDropped = true;
                    //Debug.Log("dropper: ground detected");
                }
            }
            if (!IsDropped)
            {
                StartCoroutine("DetectGround");
            }
        }

        void OnDestroy()
        {
            USBTrafficLink.DroppersCount--;
        }
    }
}
