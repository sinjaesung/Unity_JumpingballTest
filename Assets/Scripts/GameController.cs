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
        //마지막 플레이에서 획득했던 점수 불러오기
        int score = PlayerPrefs.GetInt("LastScore");
        textScore.text = score.ToString();

        //기존에 등록된 최고 점수 불러오기
        int highScore = PlayerPrefs.GetInt("HighScore");
        textHighScore.text = $"High Score {highScore}";
    }

    public void GameStart()
    {
        //Tap To Play 텍스트를 보이지 않게 설정
        textTapToPlay.enabled = false;
        //최고 점수 텍스트를 보이지 않게 설정
        textHighScore.enabled = false;
   
        //게임 시작 전에는 마지막 획득 점수가 출력되기 때문에 게임 시작 시 점수 텍스트 갱신
        textScore.text = score.ToString();
    }

    public void IncreaseScore()
    {
        score++;
        textScore.text = score.ToString();

        //score가 짝수 일 때 마다 모든 발판 색상 변경
        if(score % 2 == 0)
        {
            platformSpawner.SetPlatformColor();
        }
    }

    public void GameOver()
    {
        //기존에 등록된 최고 점수 불러오기
        int highScore = PlayerPrefs.GetInt("HighScore");
        //현재 점수가 최고 점수보다 높을 때
        if(score > highScore)
        {
            //현재 점수를 최고 점수 정보로 저장하기
            PlayerPrefs.SetInt("HighScore", score);
        }

        //마지막에 획득한 점수 저장 (씬을 다시 로드 했을 때 마지막 점수 출력)
        PlayerPrefs.SetInt("LastScore", score);

        //현재 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        //프로그램을 종료할 때 LastScore를 0으로 설정
        PlayerPrefs.SetInt("LastScore", 0);
    }
}
