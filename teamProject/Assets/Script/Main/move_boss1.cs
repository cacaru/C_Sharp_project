using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class move_boss1 : MonoBehaviour
{
    Animator animator;
    public GameObject textboss;
    public Vector3 v4 = new Vector3();
    public Vector3 v7 = new Vector3();
    float speed = 10.0f;
    string[] animations_name = { "attack01", "Run", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 3.2f;

    // Start is called before the first frame update
    void Start()
    {
        //초기 위치 설정
        v4.x = gameObject.transform.position.x;
        v4.z = gameObject.transform.position.z;
        v7.x = gameObject.transform.position.x;
        v7.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionum)
    {
        switch (motionum)
        {
            case 41:   //공격
                textboss.GetComponent<Text>().text = "Basic Attack";
                Boss_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 42:   //방어
                textboss.GetComponent<Text>().text = "Defend";
                Boss_hit();
                yield return new WaitForSeconds(2);
                break;
            case 43:    //심쿵
                textboss.GetComponent<Text>().text = "Heart Attack";
                Boss_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 44:    //회복
                textboss.GetComponent<Text>().text = "Recovery";
                Boss_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 45:    //함성
                textboss.GetComponent<Text>().text = "Cry";
                Boss_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 46:    //후려치기
                textboss.GetComponent<Text>().text = "Swing";
                Boss_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 74:   //왼쪽 1칸 이동
                v4.x -= 10;
                v4.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 84:   //오른쪽 1칸 이동
                v4.x += 10;
                v4.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 94:   //아래쪽 1칸 이동
                v4.x = gameObject.transform.position.x;
                v4.z -= 10;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 104:   //위쪽 1칸 이동
                v4.x = gameObject.transform.position.x;
                v4.z += 10;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 114:   //왼쪽 2칸 이동
                v4.x -= 20;
                v4.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 124:   //오른쪽 2칸 이동
                v4.x += 20;
                v4.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss1_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 134:   //쳐맞음
                Boss_hit();
                yield return new WaitForSeconds(2);
                break;
            case 144:   //쥬금
                Boss_dead();
                yield return new WaitForSeconds(5);
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();//불러오기
    }

    public void Boss_Attack1() {
        animator.SetTrigger("attack01");
    }

    public void Boss_hit()
    {
        animator.SetTrigger("hit");
    }

    public void Boss_dead()
    {
        animator.SetTrigger("dead");
    }
    
    void Boss1_go()
    {
        starttime = Time.time;

        InvokeRepeating("Boss1_move", 0, 0.02222225f);
    }

    void Boss1_move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v4, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v4;
            CancelInvoke("Boss1_move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("isRunning", false);
        }
    }

    void Boss1_go1()
    {
        starttime = Time.time;

        InvokeRepeating("Boss1_move", 0, 0.0123456f);
    }

    public bool isAnimeEnd(int i)
    {
        for (int k = 0; k < 10; k++)
        {
            animator.GetCurrentAnimatorStateInfo(0).IsName(animations_name[k]);
        }

        return true;
    }

    public void calculateVector3(float x, float y, float z)
    {
        v4.x = x;//백터 이동값
        v4.y = y;
        v4.z = z;
    }

    public void mylocationset()
    {
        v4.x = v7.x;
        v4.z = v7.z;
    }
}
