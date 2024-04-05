using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private Color[] platformColors; //���� ���� �迭
    [SerializeField]
    private Material platformMaterial; //���� ���͸��� (���� ����)

    [SerializeField]
    private GameObject platformPrefab; //���� ������
    [SerializeField]
    private int spawnPlatformCountAtStart = 8; //���� ���� �� ���� �����Ǵ� ���� ����
    [SerializeField]
    private float xRange = 4; //������ ��ġ�� �� �ִ� x ��ġ ����
    [SerializeField]
    private float zDistance = 5; //���� ������ �Ÿ�
    private int platformIndex = 0; //���� �δ콺 (��ġ�Ǵ� ������ z��ġ ���꿡 ���)

    public float ZDistance => zDistance; //�ܺο��� ���� ������ �Ÿ� �� Get

    private void Awake()
    {
        //���� ���� ������ platformColors[0] �������� ����
        platformMaterial.color = platformColors[0];

        //spawnPlatformCountAtStart�� ����� ������ŭ ���� �÷��� ����
        for(int i=0; i<spawnPlatformCountAtStart; ++i)
        {
            SpawnPlatform();
        }
    }

    public void SpawnPlatform()
    {
        //���ο� ���� ����
        GameObject clone = Instantiate(platformPrefab);
        //���� Setup() �޼ҵ� ȣ��
        clone.GetComponent<Platform>().Setup(this);
        //���� ��ġ ����
        ResetPlatform(clone.transform);
    }

    public void ResetPlatform(Transform transform, float y=0)
    {
        platformIndex++;

        //������ ��ġ�Ǵ� x��ġ�� ���Ƿ� ���� (-xRange~xRange)
        float x = Random.Range(-xRange, xRange);
        //������ ��ġ�Ǵ� ��ġ ����(z���� ���� ���� �ε��� * zDistance)
        transform.position = new Vector3(x, y, platformIndex * zDistance);
    }

    public void SetPlatformColor()
    {
        //ȭ�鿡 �����ϴ� ��� �÷����� ���� ����
        int index = Random.Range(0, platformColors.Length);
        platformMaterial.color = platformColors[index];
    }
}
