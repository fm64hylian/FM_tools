using UnityEngine;
using System.Collections;

namespace UnityMovementAI
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        Transform target;
        [SerializeField]
        Vector3 displacement;
        [SerializeField]
        Vector3 rotation;

        private void Awake()
        {
            //transform.position = target.position - displacement + (Vector3.up * displacement.y);
            transform.position = target.position + displacement;
            //transform.LookAt(target);
            transform.eulerAngles = rotation;
            
        }

        void Start()
        {
            //displacement = transform.position - target.position;
        }

        void LateUpdate()
        {
            if (target != null)
            {
                //if (Vector3.Distance(transform.position, target.position) > 8f) {
                //    transform.LookAt(target);
                //}

                transform.position = target.position + displacement;
            }
        }
    }
}