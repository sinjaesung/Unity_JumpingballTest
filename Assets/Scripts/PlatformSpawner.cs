using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private Color[] platformColors; //발판 색상 배열
    [SerializeField]
    private Material platformMaterial; //발판 메터리얼 (색상 설정)

    [SerializeField]
    private GameObject platformPrefab; //발판 프리팹
    [SerializeField]
    private int spawnPlatformCountAtStart = 8; //게임 시작 시 최초 생성되는 발판 개수
    [SerializeField]
    private float xRange = 4; //발판이 배치될 수 있는 x 위치 범위
    [SerializeField]
    private float zDistance = 5; //발판 사이의 거리
    private int platformIndex = 0; //발판 인댁스 (배치되는 발판의 z위치 연산에 사용)

    public float ZDistance => zDistance; //외부에서 발판 사이의 거리 값 Get

    private void Awake()
    {
        //최초 발판 색상은 platformColors[0] 색상으로 설정
        platformMaterial.color = platformColors[0];

        //spawnPlatformCountAtStart에 저장된 개수만큼 최초 플랫폼 생성
        for(int i=0; i<spawnPlatformCountAtStart; ++i)
        {
            SpawnPlatform();
        }
    }

    public void SpawnPlatform()
    {
        //새로운 발판 생성
        GameObject clone = Instantiate(platformPrefab);
        //발판 Setup() 메소드 호출
        clone.GetComponent<Platform>().Setup(this);
        //발판 위치 설정
        ResetPlatform(clone.transform);
    }

    public void ResetPlatform(Transform transform, float y=0)
    {
        platformIndex++;

        //발판이 배치되는 x위치를 임의로 설정 (-xRange~xRange)
        float x = Random.Range(-xRange, xRange);
        //발판이 배치되는 위치 설정(z축은 현재 발판 인덱스 * zDistance)
        transform.position = new Vector3(x, y, platformIndex * zDistance);
    }

    public void SetPlatformColor()
    {
        //화면에 등장하는 모든 플랫폼의 색상 설정
        int index = Random.Range(0, platformColors.Length);
        platformMaterial.color = platformColors[index];
    }
}
