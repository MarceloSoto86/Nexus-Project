using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USSineFlash : MonoBehaviour
    {
        public string EmissiveString = "_EmissionColor"; //replace with _EmissiveColor for hdrp usage
        public int MatId;
        [ColorUsageAttribute(true, true)]
        public Color MaxBright;
        public float Timing = 3.4f;
        Material _Material1;
        Color TempColor;

        // Start is called before the first frame update
        void Start()
        {
            _Material1 = transform.GetComponent<Renderer>().materials[MatId];
        }

        // Update is called once per frame
        void Update()
        {
            TempColor = new Color(MaxBright.r * Mathf.Sin(Time.time * Timing), MaxBright.g * Mathf.Sin(Time.time * Timing), MaxBright.b * Mathf.Sin(Time.time * Timing));
            if (TempColor.r > 0) _Material1.SetColor(EmissiveString, TempColor);
        }
    }
}
