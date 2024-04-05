using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target; //ī�޶� �����ϴ� ���

    private void LateUpdate()
    {
        //target�� �������� ������ ���� ���� �ʴ´�
        if (target == null) return;

        //ī�޶��� ��ġ(Position) ���� ����
        Vector3 position = transform.position;
        position.z = target.position.z - 10;
        transform.position = position;
    }
}
