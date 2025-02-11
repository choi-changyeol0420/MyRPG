using MyRPG.Player;
using UnityEngine;

namespace MyRPG.camera
{
    public class CameraControl : MonoBehaviour
    {
        #region Variables
        public Transform player;
        private Vector3 offset;
        #endregion

        // Start is called before the first frame update
        void Awake()
        {
            offset = transform.position - player.position;
        }

        //카메라가 플레이어 움직임에 한 템포 늦게 움직임을 준다
        void LateUpdate()
        {
            //플레이어의 위치와 카메라의 위치를 최초 저장한 위치 차이만큼 자동으로 유지시켜주게 됨
            transform.position = player.position + offset;
        }
        public Vector3 GetCameraOffset()
        {
            return offset;
        }
        public void UpdateCameraPosition(Vector3 savedOffset)
        {
            offset = savedOffset;
        }
    }
}