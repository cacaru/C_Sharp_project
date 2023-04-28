using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class move_boss3 : MonoBehaviour
{
    Animator animator;
    public Text textboss;
    public Vector3 v6 = new Vector3();
    public Vector3 v9 = new Vector3();
    float speed = 10.0f;
    string[] animations_name = { "attack01", "attack02", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        v6.x = gameObject.transform.position.x;
        v6.z = gameObject.transform.position.z;
        v9.x = gameObject.transform.position.x;
        v9.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionnum)
    {
        switch (motionnum)
        {
            case 61:   //공격
                textboss.GetComponent<Text>().text = "Basic Attack";
                Boss_Attack1();
                yield return new WaitForSeconds(3);
                break;
            case 62:    //일자 공격
                textboss.GetComponent<Text>().text = "Power Attack";
                Boss_Attack2();
                yield return new WaitForSeconds(3);
                break;
            case 63:    //전체 공격
                textboss.GetComponent<Text>().text = "Full Attack";
                Boss_Attack4();
                yield return new WaitForSeconds(3);
                break;
            case 64:    //공격력 증가, 방어력 증가
                 textboss.GetComponent<Text>().text = "Rage";
                Boss_Attack6();
                yield return new WaitForSeconds(3);
                break;
            case 65:    //전방 공격
                 textboss.GetComponent<Text>().text = "Fire";
                Boss_Attack5();
                yield return new WaitForSeconds(3);
                break;
            case 66:    //범위 공격
                textboss.GetComponent<Text>().text = "End Burst";
                Boss_Attack7();
                yield return new WaitForSeconds(3);
                break;
            case 67:    //즉사기
                textboss.GetComponent<Text>().text = "The End Strike";
                Boss_Attack3();
                yield return new WaitForSeconds(3);
                break;
            case 76:   //왼쪽 1칸 이동
                v6.x -= 10;
                v6.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 86:   //오른쪽 1칸 이동
                v6.x += 10;
                v6.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 96:   //아래쪽 1칸 이동
                v6.x = gameObject.transform.position.x;
                v6.z -= 10;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 106:   //위쪽 1칸 이동
                v6.x = gameObject.transform.position.x;
                v6.z += 10;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 116:   //왼쪽 2칸 이동
                v6.x -= 20;
                v6.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 126:   //오른쪽 2칸 이동
                v6.x += 20;
                v6.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("boss3_go", 0.5f);
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

    public void Boss_Attack1()
    {
        animator.SetTrigger("attack1");
    }

    public void Boss_Attack2()
    {
        animator.SetTrigger("attack2");
    }

    public void Boss_Attack3()
    {
        animator.SetTrigger("attack3");
    }

    public void Boss_Attack4()
    {
        animator.SetTrigger("attack4");
    }

    public void Boss_Attack5()
    {
        animator.SetTrigger("attack5");
    }

    public void Boss_Attack6()
    {
        animator.SetTrigger("attack6");
    }

    public void Boss_Attack7()
    {
        animator.SetTrigger("attack7");
    }

    public void Boss_hit()
    {
        animator.SetTrigger("hit");
    }

    public void Boss_dead()
    {
        animator.SetTrigger("dead");
    }
    

    void boss3_go()
    {
        starttime = Time.time;

        InvokeRepeating("boss3_move", 0, 0.02222225f);
    }

    void boss3_move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v6, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v6;
            CancelInvoke("boss3_move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("isRunning", false);
        }
    }

    void boss3_go1()
    {
        starttime = Time.time;

        InvokeRepeating("boss3_move", 0, 0.0123456f);
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
        v6.x = v9.x;
        v6.z = v9.z;
    }
}
