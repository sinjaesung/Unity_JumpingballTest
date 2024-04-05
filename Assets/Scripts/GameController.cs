using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlatformSpawner platformSpawner;

    [SerializeField]
    private TextMeshProUGUI textTapToPlay;

    [SerializeField]
    private TextMeshProUGUI textScore;
    private int score = 0;

    [SerializeField]
    private TextMeshProUGUI textHighScore;

    private void Awake()
    {
        //������ �÷��̿��� ȹ���ߴ� ���� �ҷ�����
        int score = PlayerPrefs.GetInt("LastScore");
        textScore.text = score.ToString();

        //������ ��ϵ� �ְ� ���� �ҷ�����
        int highScore = PlayerPrefs.GetInt("HighScore");
        textHighScore.text = $"High Score {highScore}";
    }

    public void GameStart()
    {
        //Tap To Play �ؽ�Ʈ�� ������ �ʰ� ����
        textTapToPlay.enabled = false;
        //�ְ� ���� �ؽ�Ʈ�� ������ �ʰ� ����
        textHighScore.enabled = false;
   
        //���� ���� ������ ������ ȹ�� ������ ��µǱ� ������ ���� ���� �� ���� �ؽ�Ʈ ����
        textScore.text = score.ToString();
    }

    public void IncreaseScore()
    {
        score++;
        textScore.text = score.ToString();

        //score�� ¦�� �� �� ���� ��� ���� ���� ����
        if(score % 2 == 0)
        {
            platformSpawner.SetPlatformColor();
        }
    }

    public void GameOver()
    {
        //������ ��ϵ� �ְ� ���� �ҷ�����
        int highScore = PlayerPrefs.GetInt("HighScore");
        //���� ������ �ְ� �������� ���� ��
        if(score > highScore)
        {
            //���� ������ �ְ� ���� ������ �����ϱ�
            PlayerPrefs.SetInt("HighScore", score);
        }

        //�������� ȹ���� ���� ���� (���� �ٽ� �ε� ���� �� ������ ���� ���)
        PlayerPrefs.SetInt("LastScore", score);

        //���� ���� �ٽ� �ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        //���α׷��� ������ �� LastScore�� 0���� ����
        PlayerPrefs.SetInt("LastScore", 0);
    }
}
