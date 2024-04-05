using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlatformSpawner platformSpawner;
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Transform playerObject; //보여지는 Sphere 오브젝트 (y 이동)
    [SerializeField]
    private float xSensitivity = 10.0f; //x축 이동 감도 (클수록 더 많은 범위를 움직인다)
    [SerializeField]
    private float moveTime = 1.0f; //y,z이동 시간
    [SerializeField]
    private float minPositionY = 0.55f; //y축 이동을 위해 플레이어의 최소 y위치 설정
    private float gravity = -9.81f; //중력
    private float platformIndex = 0; //플레이어가 다음으로 이동할 플랫폼 인덱스

    private RaycastHit hit;

    private IEnumerator Start()
    {
        //마우스 왼쪽 버튼을 누르기 전까지 시작하지 않고 대기
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //게임 시작
                gameController.GameStart();

                //플레이어의 y,z 이동 제어
                StartCoroutine("MoveLoop");
                //플레이어의 이동 시간 감소
                StartCoroutine("DecreaseMoveTime");

                break;
            }

            yield return null;
        }
    }
    private IEnumerator MoveLoop()
    {
        while (true)
        {
            platformIndex++;

            float current = (platformIndex - 1) * platformSpawner.ZDistance;
            float next = platformIndex * platformSpawner.ZDistance;

            //플레이어의 y,z 위치 제어
            yield return StartCoroutine(MoveToYZ(current, next));

            //플레이어가 다음 플랫폼에 도착했을 때
            //플레이어의 도착 위치가 플랫폼이면
            if(hit.transform != null)
            {
                Debug.Log("Hit");
                gameController.IncreaseScore();
            }
            //플레이어의 도착 범위가 낭떠러지이면
            else
            {
                Debug.Log("GameOver");
                gameController.GameOver();
                break;
            }
        }
    }
    private void Update()
    {
        //아래쪽 방향으로 광선을 발사해 광선에 부딪히는 발판 정보를 hit에 저장
        Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f);

        //마우스 왼쪽 버튼(or 모바일 터치)으로 플레이어의 X 위치 제어
        if (Input.GetMouseButton(0))
        {
            MoveToX();
        }
    }

    private void MoveToX()
    {
        float x = 0.0f;
        Vector3 position = transform.position;

        //현재 플랫폼이 모바일 일 때
        if (Application.isMobilePlatform)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //0.0f~1.0f의 값으로 만들고 -0.5f를 하기 때문에
                //-0.5f~0.5f 사이의 값이 나온다
                x = (touch.position.x / Screen.width) - 0.5f;
            }
        }
        //현재 플랫폼이 PC일 때
        else
        {
            //0.0f~1.0f의 값으로 만들고 -0.5f를 하기 때문에
            //-0.5f~0.5f 사이의 값이 나온다
            x = (Input.mousePosition.x / Screen.width) - 0.5f;
        }

        //화면 밖을 터치해 x값이 -0.5f~0.5f의 범위를 넘어가지 않도록
        x = Mathf.Clamp(x, -0.5f, 0.5f);
        //플레이어의 x 위치 설정
        position.x = Mathf.Lerp(position.x, x * xSensitivity, xSensitivity * Time.deltaTime);

        transform.position = position;
    }

    private IEnumerator MoveToYZ(float start, float end)
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity; //y방향의 초기 속도

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            //시간 경과에 따라 오브젝트의 y 위치를 바꿔준다
            //포물선 운동: 시작위치 + 초기속도*시간 + 중력*시간제곱
            float y = minPositionY + (v0 * percent) + (gravity * percent * percent);
            playerObject.position = new Vector3(playerObject.position.x, y, playerObject.position.z);

            //시간 경과(최대 1)에 따라 오브젝트의 z위치를 바꿔준다
            float z = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);

            yield return null;
        }
    }

    private IEnumerator DecreaseMoveTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            //플레이어의 y,z 이동 시간 감소 (점점 빠르게 이동)
            moveTime -= 0.02f;

            //이동 시간이 0.2f 이하이면 더 이상 줄이지 않는다
            if(moveTime <= 0.2f)
            {
                break;
            }
        }
    }
}
