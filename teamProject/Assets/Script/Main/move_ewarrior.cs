using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move_ewarrior : MonoBehaviour
{
    Animator animator;

    public Vector3 v = new Vector3(); //벡터 이동값
    public Vector3 v12 = new Vector3(); // 첫위치
    float speed = 10.0f;    //이동 속도
    string[] animations_name = { "skill1", "skill2", "skill3", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 5.0f;

    void Start()
    {
        //초기 위치 설정
        v.x = gameObject.transform.position.x;
        v.z = gameObject.transform.position.z;
        v12.x = gameObject.transform.position.x;
        v12.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionnum)
    {
        switch (motionnum)
        {
            case 10: //back-step 제자리에 때렸다.
                Attack2();
                break;
            case 11: //발도
                Attack1();
                break;
            case 12:    //삼천세계 - case1 두칸 이동해서 때렸다.
                v.x -= 20;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go1", 0.5f);
                Attack1();
                break;
            case 13:    //back-step
                v.x += 10;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go", 0.5f);
                Attack2();
                break;
            case 14:    //가르기
                Attack3();
                break;
            case 15:    //분노
                Attack1();
                break;
            case 16:    //냥냥펀치
                Attack2();
                break;
            case 17:    //지면배기
                Attack3();
                break;
            case 18:    //삼천세계 case2   - 1칸 이동해서 때렸다.
                v.x -= 10;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go1", 0.5f);
                Attack1();
                break;
            case 19:    //삼천세계case3 - 제자리에서 때렸다.
                Attack1();
                break;
            case 71: // 1칸 왼쪽 이동
                v.x += 10;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go", 0.5f);
                break;
            case 81: // 1칸 오른쪽 이동
                v.x -= 10;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go", 0.5f);
                break;
            case 91:   //1칸 아래 이동
                v.x = gameObject.transform.position.x;
                v.z -= 10;
                animator.SetBool("runChk", true);
                Invoke("go", 0.5f);
                break;
            case 101:   // 1칸 위로 이동
                v.x = gameObject.transform.position.x;
                v.z += 10;
                animator.SetBool("runChk", true);
                Invoke("go", 0.5f);
                break;
            case 111:   //2칸 왼쪽 이동
                v.x += 20;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go1", 0.5f);
                break;
            case 121:   //2칸 오른쪽 이동
                v.x -= 20;
                v.z = gameObject.transform.position.z;
                animator.SetBool("runChk", true);
                Invoke("go1", 0.5f);
                break;
            case 7:   //기본 공격
                Attack1();
                break;
            case 8:   //방어
                Hit();
                break;
            case 131:
                Hit();
                break;
            case 141:   //죽을때
                Dead();
                break;
            case 151:   //부활
                Alive();
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(5);
    }


    void Awake()
    {
        animator = GetComponent<Animator>();//불러오기
    }

    void go()
    {
        starttime = Time.time;

        InvokeRepeating("move", 0, 0.02222225f);
    }

    void move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v;
            CancelInvoke("move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("runChk", false);
        }
    }

    void go1()
    {
        starttime = Time.time;
        InvokeRepeating("move", 0, 0.0123456f);
    }

    public void Run()
    {
        animator.SetBool("runChk", true);
    }

    public void Attack1()
    {
        animator.SetTrigger("skill1");
    }

    public void Attack2()
    {
        animator.SetTrigger("skill2");
    }

    public void Attack3()
    {
        animator.SetTrigger("skill3");
    }

    public void Hit()
    {
        animator.SetTrigger("hit");
    }

    public void Dead()
    {
        animator.SetBool("isAlive", false);
    }

    public void Alive()
    {
        animator.SetBool("isAlive", true);
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
        v.x = x;//백터 이동값
        v.y = y;
        v.z = z;
    }

    public void mylocationset()
    {
        v.x = v12.x;
        v.z = v12.z;
    }
}
