using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private GameObject coinObject; //현재 발판에 소속된 코인 오브젝트
    [SerializeField]
    private int coinSpawnPercent = 50; //코인 등장 확률

    private PlatformSpawner platformSpawner;
    private Camera mainCamera;
    private float yMoveTime = 0.5f; //재배치되는 플랫폼이 내려오는 이동 시간

    public void Setup(PlatformSpawner platformSpawner)
    {
        this.platformSpawner = platformSpawner;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //플랫폼이 카메라 뒤로 가서 보이지 않게 되면 플랫폼 재배치
        if(mainCamera.transform.position.z - transform.position.z > 0)
        {
            //플랫폼 위에 코인 활성/비활성 설정
            SpawnCoin();

            //플랫폼 위치 재설정
            platformSpawner.ResetPlatform(this.transform);
            //새로 등장할 때는 위에서 떨어지는 효과 적용
            StartCoroutine(MoveY(10, 0));
        }
    }

    private IEnumerator MoveY(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / yMoveTime;

            //시간 경과(최대 1)에 따라 오브젝트의 y 위치를 바꿔준다
            float y = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            yield return null;
        }
    }

    private void SpawnCoin()
    {
        //임의의 값 percent가 platformSpawner.SpawnCoinPercent보다 작으면 코인 생성
        int percent = Random.Range(0, 100);

        if(percent < coinSpawnPercent)
        {
            coinObject.SetActive(true);
        }
        else
        {
            coinObject.SetActive(false);
        }
    }
}
