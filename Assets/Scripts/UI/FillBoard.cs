using UnityEngine;

namespace MyRPG
{
    public class FillBoard : MonoBehaviour
    {
        private Transform cam;
        private void Start()
        {
            cam = Camera.main.transform;
        }
        private void Update()
        {
            transform.LookAt(transform.position + cam.rotation * Vector3.forward,cam.rotation * Vector3.up);
        }
    }
}