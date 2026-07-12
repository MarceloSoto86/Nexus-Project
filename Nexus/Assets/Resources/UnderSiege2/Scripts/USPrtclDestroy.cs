using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILranch
{
    public class USPrtclDestroy : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Erase());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Erase()
        {
            yield return new WaitForSeconds(1);
            Destroy(this.gameObject);
        }
    }
}
