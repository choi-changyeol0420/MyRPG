using UnityEngine;

namespace MyRPG.Player
{
    public class InputManager : MonoBehaviour
    {
        private GameObject player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            CheckClick();
        }
        void CheckClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //카메라로부터 화면사의 좌표를 관통하는 가상의 선(레이)을 생성해서 리턴해 주는 함수
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                //Physics.Raycast(래이 타입 변수, out 레이 캐스트 히트 타입 변수) :
                //가상의 레이저선(레이)이 충돌체와 충돌하면, true(참) 값을 리턴하면서
                //동시에 레이캐스트 히트 변수에 충돌 대상의 정보를 담아주는 함수

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name == "Terrain")
                    {
                        //player.transform.position = hit.point;

                        player.GetComponent<PlayerFSM>().MoveTo(hit.point);
                    }
                    else if(hit.collider.gameObject.tag == "Enemy")
                    {
                        player.GetComponent<PlayerFSM>().AttackEnemy(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}