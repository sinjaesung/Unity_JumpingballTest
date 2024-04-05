using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target; //카메라가 추적하는 대상

    private void LateUpdate()
    {
        //target이 존재하지 않으면 실행 하지 않는다
        if (target == null) return;

        //카메라의 위치(Position) 정보 갱신
        Vector3 position = transform.position;
        position.z = target.position.z - 10;
        transform.position = position;
    }
}
