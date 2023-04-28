using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move_ehealer : MonoBehaviour
{
    Animator animator;

    public Vector3 v1 = new Vector3();
    public Vector3 v11 = new Vector3();
    float speed = 10.0f;
    string[] animations_name = { "attack1", "attack2", "attack3", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //초기 위치 설정
        v1.x = gameObject.transform.position.x;
        v1.z = gameObject.transform.position.z;
        v11.x = gameObject.transform.position.x;
        v11.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionnum)
    {
        switch (motionnum)
        {
            case 21:    //힐
                Healer_Attack2();
                break;
            case 22:    //부활
                Healer_Attack3();
                break;
            case 23:    //공격력&치명타 버프
                Healer_Attack5();
                break;
            case 24:    //광역 힐
                Healer_Attack6();
                break;
            case 25:    //공격력 감소
                Healer_Attack7();
                break;
            case 72:   //1칸 왼쪽 이동
                v1.x += 10;
                v1.z = gameObject.transform.position.z;
                animator.SetBool("isRun", true);
                Invoke("Healer_go", 0.5f);
                break;
            case 82:   //1칸 오른쪽 이동
                v1.x -= 10;
                v1.z = gameObject.transform.position.z;
                animator.SetBool("isRun", true);
                Invoke("Healer_go", 0.5f);
                break;
            case 92:   //1칸 아래 이동
                v1.x = gameObject.transform.position.x;
                v1.z -= 10;
                animator.SetBool("isRun", true);
                Invoke("Healer_go", 0.5f);
                break;
            case 102:   //1칸 위 이동
                v1.x = gameObject.transform.position.x;
                v1.z += 10;
                animator.SetBool("isRun", true);
                Invoke("Healer_go", 0.5f);
                break;
            case 112:   //2칸 왼쪽 이동
                v1.x += 20;
                v1.z = gameObject.transform.position.z;
                animator.SetBool("isRun", true);
                Invoke("Healer_go1", 0.5f);
                break;
            case 122:   //2칸 오른쪽 이동
                v1.x -= 20;
                v1.z = gameObject.transform.position.z;
                animator.SetBool("isRun", true);
                Invoke("Healer_go1", 0.5f);
                break;
            case 26:   //기본 공격
                Healer_Attack1();
                break;
            case 27:   //방어
                Healer_defense();
                break;
            case 132:
                Healer_Hit();
                break;
            case 142:
                Healer_Dead();
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

    void Healer_go()
    {
        starttime = Time.time;
        InvokeRepeating("Healer_move", 0, 0.02222225f);
    }

    void Healer_move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v1, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v1;
            CancelInvoke("Healer_move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("isRun", false);
        }
    }

    void Healer_go1()
    {
        starttime = Time.time;

        InvokeRepeating("Healer_move", 0, 0.0123456f);
    }



    public void Healer_Run()
    {
        animator.SetBool("isRun", true);
    }

    public void Healer_Attack1()
    {
        animator.SetTrigger("attack1");
    }

    public void Healer_Attack2()
    {
        animator.SetTrigger("attack2");
    }

    public void Healer_Attack3()
    {
        animator.SetTrigger("attack3");
    }

    public void Healer_Attack5()
    {
        animator.SetTrigger("attack5");
    }

    public void Healer_Attack6()
    {
        animator.SetTrigger("attack6");
    }

    public void Healer_Attack7()
    {
        animator.SetTrigger("attack7");
    }

    public void Healer_defense()
    {
        animator.SetTrigger("defense");
    }

    public void Healer_Hit()
    {
        animator.SetTrigger("hit");
    }

    public void Healer_Dead()
    {
        animator.SetTrigger("dead");
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
        v1.x = x;//백터 이동값
        v1.y = y;
        v1.z = z;
    }

    public void mylocationset()
    {
        v1.x = v11.x;
        v1.z = v11.z;
    }
}
