using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, 2021. ILonion32@gmail.com
namespace ILranch
{
    public class USBullet : MonoBehaviour
    {
        public string InteractLayer = "InteractLayer";
        public float Speed = 0.05f;
        public float Damage = 2.0f;
        public float AbsLifeTime = 3.0f;
        int buttonsMask;

        void Awake()
        {
            buttonsMask = LayerMask.NameToLayer(InteractLayer);
            StartCoroutine(LifeTime());
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(0, 0, Speed * Time.deltaTime);

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.2f, transform.TransformDirection(new Vector3(0, 0, 1)), out hit, Speed * Time.deltaTime))
            {
                if (hit.transform.gameObject.layer == buttonsMask)
                {
                    //target hit
                    hit.transform.gameObject.GetComponent<USEnemyShip>().Health -= Damage;
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }

        IEnumerator LifeTime()
        {
            yield return new WaitForSeconds(AbsLifeTime);
            Destroy(this.gameObject);
        }
    }
}
