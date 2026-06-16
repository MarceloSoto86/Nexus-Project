using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USResponseArea : MonoBehaviour
    {
        [Header("'UI' and 'Water' layer used just for demo as built-in layers:")]
        public string EnemyLayer = "InteractLayer";
        public string TurretsLayer = "InteractLayer";
        public float TriggerSphereRadius = 5f;
        public float CollectRate = 0.5f;
        [HideInInspector]
        public Collider[] USTargetShips;
        int buttonsMask1;
        int buttonsMask2;

        void Awake()
        {
            buttonsMask1 = LayerMask.GetMask(EnemyLayer);
            buttonsMask2 = LayerMask.GetMask(TurretsLayer);
            InvokeRepeating("CollectTargets", 0.0f, CollectRate);
        }

        void Start()
        {
            //collect turrets
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, TriggerSphereRadius, buttonsMask2);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.transform.GetComponent<USAim>().ResponseArea = this.gameObject.transform.GetComponent<USResponseArea>();
            }
        }

        void Update()
        {

        }

        void CollectTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, TriggerSphereRadius, buttonsMask1);
            if (hitColliders != null)
            {
                USTargetShips = new Collider[hitColliders.Length];
                hitColliders.CopyTo(USTargetShips, 0);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, TriggerSphereRadius);
        }
    }
}
