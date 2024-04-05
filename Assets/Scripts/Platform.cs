using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private GameObject coinObject; //���� ���ǿ� �Ҽӵ� ���� ������Ʈ
    [SerializeField]
    private int coinSpawnPercent = 50; //���� ���� Ȯ��

    private PlatformSpawner platformSpawner;
    private Camera mainCamera;
    private float yMoveTime = 0.5f; //���ġ�Ǵ� �÷����� �������� �̵� �ð�

    public void Setup(PlatformSpawner platformSpawner)
    {
        this.platformSpawner = platformSpawner;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //�÷����� ī�޶� �ڷ� ���� ������ �ʰ� �Ǹ� �÷��� ���ġ
        if(mainCamera.transform.position.z - transform.position.z > 0)
        {
            //�÷��� ���� ���� Ȱ��/��Ȱ�� ����
            SpawnCoin();

            //�÷��� ��ġ �缳��
            platformSpawner.ResetPlatform(this.transform);
            //���� ������ ���� ������ �������� ȿ�� ����
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

            //�ð� ���(�ִ� 1)�� ���� ������Ʈ�� y ��ġ�� �ٲ��ش�
            float y = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            yield return null;
        }
    }

    private void SpawnCoin()
    {
        //������ �� percent�� platformSpawner.SpawnCoinPercent���� ������ ���� ����
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
