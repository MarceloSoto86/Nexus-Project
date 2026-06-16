using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILranch
{
    [ExecuteInEditMode]
    public class USDamageRadiusVisual : MonoBehaviour
    {
        public bool VisualizeDamgaeRadius = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDrawGizmos()
        {
            if (VisualizeDamgaeRadius)
            {
                Gizmos.DrawWireSphere(transform.position, GetComponent<USBGround>().DamageRadius);
            }
        }
    }
}
