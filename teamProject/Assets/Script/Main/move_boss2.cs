using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class move_boss2 : MonoBehaviour
{
    Animator animator;
    public Text textboss;
    public Vector3 v5 = new Vector3();
    public Vector3 v8 = new Vector3();
    float speed = 10.0f;
    string[] animations_name = { "attack01", "Run", "hit", "dead" };

    private Vector3 from;
    private Vector3 to;
    private float starttime;
    private const float totalTime = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        v5.x = gameObject.transform.position.x;
        v5.z = gameObject.transform.position.z;
        v8.x = gameObject.transform.position.x;
        v8.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Motioning(int motionnum)
    {
        switch (motionnum)
        {
            case 51:   //공격
                textboss.GetComponent<Text>().text = "Basic Attack";
                Boss_Attack1();
                yield return new WaitForSeconds(2.5f);
                break;
            case 52:    //스킬1
                textboss.GetComponent<Text>().text = "Slash";
                Boss_Attack2();
                yield return new WaitForSeconds(2.5f);
                break;
            case 53:    //공격력 증가
                textboss.GetComponent<Text>().text = "Rage";
                Boss_Attack3();
                yield return new WaitForSeconds(2.5f);
                break;
            case 54:    //함성
                textboss.GetComponent<Text>().text = "War Cry";
                Boss_Attack4();
                yield return new WaitForSeconds(2.5f);
                break;
            case 55:    //스킬2
                textboss.GetComponent<Text>().text = "??";
                Boss_Attack5();
                yield return new WaitForSeconds(2.5f);
                break;
            case 56:    //스킬3
                textboss.GetComponent<Text>().text = "??";
                Boss_Attack6();
                yield return new WaitForSeconds(2.5f);
                break;
            case 75:   //왼쪽 1칸 이동
                v5.x -= 10;
                v5.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 85:   //오른쪽 1칸 이동
                v5.x += 10;
                v5.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 95:   //아래쪽 1칸 이동
                v5.x = gameObject.transform.position.x;
                v5.z -= 10;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 105:   //위쪽 1칸 이동
                v5.x = gameObject.transform.position.x;
                v5.z += 10;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 115:   //왼쪽 2칸 이동
                v5.x -= 20;
                v5.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
                yield return new WaitForSeconds(5);
                break;
            case 125:   //오른쪽 2칸 이동
                v5.x += 20;
                v5.z = gameObject.transform.position.z;
                animator.SetBool("isRunning", true);
                Invoke("Boss2_go", 0.5f);
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
        animator.SetTrigger("attack01");
    }

    public void Boss_Attack2() {
        animator.SetTrigger("attack02");
    }

    public void Boss_Attack3()
    {
        animator.SetTrigger("attack03");
    }

    public void Boss_Attack4()
    {
        animator.SetTrigger("attack04");
    }

    public void Boss_Attack5()
    {
        animator.SetTrigger("relax");
    }

    public void Boss_Attack6()
    {
        animator.SetTrigger("attack02");
    }
    
    void Boss2_go()
    {
        starttime = Time.time;

        InvokeRepeating("Boss2_move", 0, 0.02222225f);
    }

    void Boss2_move()
    {
        float deltaTime = Time.time - starttime;

        if (deltaTime < totalTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, v5, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = v5;
            CancelInvoke("Boss2_move");//애니메이션이 종료되면 invoke repeter 종료
            animator.SetBool("isRunning", false);
        }
    }

    void Boss2_go1()
    {
        starttime = Time.time;

        InvokeRepeating("Boss2_move", 0, 0.0123456f);
        
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
        v5.x = v8.x;
        v5.z = v8.z;
    }
}
