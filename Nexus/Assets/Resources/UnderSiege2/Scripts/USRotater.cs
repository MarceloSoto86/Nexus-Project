using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILranch
{
    public class USRotater : MonoBehaviour
    {
        public bool x;
        public bool y;
        public bool z;
        public float Speed;
        public GameObject[] Rotate;

        void Awake()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                Speed *= -1;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Rotate != null)
            {
                if (x)
                {
                    foreach (GameObject obj in Rotate)
                    {
                        obj.transform.Rotate(Speed * Time.deltaTime, 0, 0);
                    }
                }
                if (y)
                {
                    foreach (GameObject obj in Rotate)
                    {
                        obj.transform.Rotate(0, Speed * Time.deltaTime, 0);
                    }
                }
                if (z)
                {
                    foreach (GameObject obj in Rotate)
                    {
                        obj.transform.Rotate(0, 0, Speed * Time.deltaTime);
                    }
                }
            }
        }
    }
}
