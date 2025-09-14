using UnityEngine;

namespace PlasmaDragon.Player
{
    public class SimpleCameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private string targetName = "Dragon_Controller";
        
        [Header("Follow Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0, 10, -20);
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private float lookAheadDistance = 5f;
        
        [Header("Rotation")]
        [SerializeField] private bool lookAtTarget = true;
        [SerializeField] private float rotationSmoothing = 5f;
        
        private void Start()
        {
            if (target == null)
            {
                GameObject targetObj = GameObject.Find(targetName);
                if (targetObj != null)
                {
                    target = targetObj.transform;
                }
            }
        }
        
        private void LateUpdate()
        {
            if (target == null) return;
            
            Vector3 desiredPosition = target.position + target.rotation * offset;
            
            if (target.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 velocity = rb.linearVelocity;
                if (velocity.magnitude > 1f)
                {
                    desiredPosition += velocity.normalized * lookAheadDistance;
                }
            }
            
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            if (lookAtTarget)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothing * Time.deltaTime);
            }
        }
        
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
        }
    }
}