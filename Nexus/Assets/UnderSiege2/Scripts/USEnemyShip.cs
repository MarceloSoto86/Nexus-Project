using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USEnemyShip : MonoBehaviour
    {
        public float Health = 5f;
        public float Speed = 0.05f;
        public float AbsLifeTime = 5.0f;
        public GameObject ExplodeEffect;
        public enum VehicleType
        {
            EnemyShip = 0,
            Dropper = 1,
            Drill = 2,
        }
        public VehicleType _VehicleType = VehicleType.EnemyShip;
        [HideInInspector]
        public USTraffic USTrafficLink;
        [HideInInspector]
        public USBTraffic USBTrafficLink;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LifeTime());
        }

        // Update is called once per frame
        void Update()
        {
            if (_VehicleType != VehicleType.Drill) transform.localPosition += transform.TransformDirection(Vector3.forward) * Speed * Time.deltaTime;
            if (Health <= 0)
            {
                if (ExplodeEffect)
                {
                    GameObject effect = Instantiate(ExplodeEffect);
                    effect.transform.position = transform.position;
                }
                Destroy(this.gameObject);
            }
        }

        IEnumerator LifeTime()
        {
            yield return new WaitForSeconds(AbsLifeTime);
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            if (_VehicleType == VehicleType.EnemyShip) USTrafficLink.VehicleCount--;
        }
    }
}
