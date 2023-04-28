using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move_tanker : MonoBehaviour
{
    Animator animator;

    public Vector3 v2 = new Vector3();
    public Vector3 v10 = new Vector3();
    float speed = 10.0f;
    string[] animations_name = { "attack01", "attack02", "defense", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        //초기 위치 설정
        v2.x = gameObject.transform.position.x;
        v2.z = gameObject.transform.position.z;
        v10.x = gameObject.transform.position.x;
        v10.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionnum)
    {
        switch (motionnum)
        {
            case 31:    //단단해지기
                Tanker_Defense();
                yield return new WaitForSeconds(1);
                break;
            case 32:    //방깍
                Tanker_Attack1();
                yield return new WaitForSeconds(1);
                break;
            case 33:    //뚝배기
                Tanker_Attack1();
                yield return new WaitForSeconds(1);
                break;
            case 34:    //강타
                Tanker_Attack2();
                yield return new WaitForSeconds(1);
                break;
            case 35:    //방어력 증가
                Tanker_Defense();
                yield return new WaitForSeconds(1);
                break;
            case 73:   //1칸 왼쪽 이동
                v2.x -= 10;
                v2.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 83:   //1칸 오른쪽 이동
                v2.x += 10;
                v2.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 93:   //1칸 아래 이동
                v2.x = gameObject.transform.position.x;
                v2.z -= 10;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 103:   //1칸 위로 이동
                v2.x = gameObject.transform.position.x;
                v2.z += 10;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 113:   //2칸 왼쪽 이동
                v2.x -= 20;
                v2.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go1", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 123:   //2칸 오른쪽 이동
                v2.x += 20;
                v2.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Tanker_go1", 0.5f);
                yield return new WaitForSeconds(4);
                break;
            case 36:   //기본 공격
                Tanker_Attack1();
                yield return new WaitForSeconds(2);
                break;
            case 37:   //방어
                Tanker_Defense();
                yield return new WaitForSeconds(2);
                break;
            case 133: //쳐맞음
                Tanker_Hit();
                yield return new WaitForSeconds(2);
                break;
            case 143: //쥬금
                Tanker_Dead();
                yield return new WaitForSeconds(4);
                break;
            default:
                break;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();//불러오기
    }

    void Tanker_go()
    {
        starttime = Time.time;

        InvokeRepeating("Tanker_move", 0, 0.02222225f);
    }

    void Tanker_move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v2, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v2;
            CancelInvoke("Tanker_move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("isRunning", false);
        }
    }

    void Tanker_go1()
    {
        starttime = Time.time;

        InvokeRepeating("Tanker_move", 0, 0.0123456f);
    }



    public void Tanker_Run()
    {
        animator.SetBool("isRunning", true);
    }

    public void Tanker_Attack1()
    {
        animator.SetTrigger("attack01");
    }

    public void Tanker_Attack2()
    {
        animator.SetTrigger("attack02");
    }

    public void Tanker_Defense()
    {
        animator.SetTrigger("defenses");
    }

    public void Tanker_Hit()
    {
        animator.SetTrigger("hit");
    }

    public void Tanker_Dead()
    {
        animator.SetTrigger("isAlive");
    }

    public bool isAnimeEnd(int i)
    {
        for (int k = 0; k < 10; k++)
        {
            animator.GetCurrentAnimatorStateInfo(0).IsName(animations_name[k]);
        }
        return true;
    }

    public void mylocationset()
    {
        v2.x = v10.x;
        v2.z = v10.z;
    }

}
