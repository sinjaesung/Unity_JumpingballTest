using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlatformSpawner platformSpawner;
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Transform playerObject; //�������� Sphere ������Ʈ (y �̵�)
    [SerializeField]
    private float xSensitivity = 10.0f; //x�� �̵� ���� (Ŭ���� �� ���� ������ �����δ�)
    [SerializeField]
    private float moveTime = 1.0f; //y,z�̵� �ð�
    [SerializeField]
    private float minPositionY = 0.55f; //y�� �̵��� ���� �÷��̾��� �ּ� y��ġ ����
    private float gravity = -9.81f; //�߷�
    private float platformIndex = 0; //�÷��̾ �������� �̵��� �÷��� �ε���

    private RaycastHit hit;

    private IEnumerator Start()
    {
        //���콺 ���� ��ư�� ������ ������ �������� �ʰ� ���
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //���� ����
                gameController.GameStart();

                //�÷��̾��� y,z �̵� ����
                StartCoroutine("MoveLoop");
                //�÷��̾��� �̵� �ð� ����
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

            //�÷��̾��� y,z ��ġ ����
            yield return StartCoroutine(MoveToYZ(current, next));

            //�÷��̾ ���� �÷����� �������� ��
            //�÷��̾��� ���� ��ġ�� �÷����̸�
            if(hit.transform != null)
            {
                Debug.Log("Hit");
                gameController.IncreaseScore();
            }
            //�÷��̾��� ���� ������ ���������̸�
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
        //�Ʒ��� �������� ������ �߻��� ������ �ε����� ���� ������ hit�� ����
        Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f);

        //���콺 ���� ��ư(or ����� ��ġ)���� �÷��̾��� X ��ġ ����
        if (Input.GetMouseButton(0))
        {
            MoveToX();
        }
    }

    private void MoveToX()
    {
        float x = 0.0f;
        Vector3 position = transform.position;

        //���� �÷����� ����� �� ��
        if (Application.isMobilePlatform)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //0.0f~1.0f�� ������ ����� -0.5f�� �ϱ� ������
                //-0.5f~0.5f ������ ���� ���´�
                x = (touch.position.x / Screen.width) - 0.5f;
            }
        }
        //���� �÷����� PC�� ��
        else
        {
            //0.0f~1.0f�� ������ ����� -0.5f�� �ϱ� ������
            //-0.5f~0.5f ������ ���� ���´�
            x = (Input.mousePosition.x / Screen.width) - 0.5f;
        }

        //ȭ�� ���� ��ġ�� x���� -0.5f~0.5f�� ������ �Ѿ�� �ʵ���
        x = Mathf.Clamp(x, -0.5f, 0.5f);
        //�÷��̾��� x ��ġ ����
        position.x = Mathf.Lerp(position.x, x * xSensitivity, xSensitivity * Time.deltaTime);

        transform.position = position;
    }

    private IEnumerator MoveToYZ(float start, float end)
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity; //y������ �ʱ� �ӵ�

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            //�ð� ����� ���� ������Ʈ�� y ��ġ�� �ٲ��ش�
            //������ �: ������ġ + �ʱ�ӵ�*�ð� + �߷�*�ð�����
            float y = minPositionY + (v0 * percent) + (gravity * percent * percent);
            playerObject.position = new Vector3(playerObject.position.x, y, playerObject.position.z);

            //�ð� ���(�ִ� 1)�� ���� ������Ʈ�� z��ġ�� �ٲ��ش�
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

            //�÷��̾��� y,z �̵� �ð� ���� (���� ������ �̵�)
            moveTime -= 0.02f;

            //�̵� �ð��� 0.2f �����̸� �� �̻� ������ �ʴ´�
            if(moveTime <= 0.2f)
            {
                break;
            }
        }
    }
}
