using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectableObject : DetectorDown
{
    [SerializeField]
    Transform target = null;
    Animator animator = null;
    Rigidbody2D moveForce;
    int state = 0, moveDirection = 0;


    public override void OnDetectedDown()
    {
        if (state == 0)
        {
            print("밟힘!");
            animator.SetBool("IsCrushed", true);
            state++;
        }
        else
        {
            if (target.position.x > transform.position.x)
                moveDirection = -1;
            else
                moveDirection = 1;
            print($"{moveDirection}방향으로 이동할 예정");
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //obj에 한번 담아서 코드 중복을 줄여줍시다
        var obj = collision.transform;
        Animator contacttarget = obj.GetComponent<Animator>();
        Rigidbody2D contactrb = obj.GetComponent<Rigidbody2D>();

        if (collision.gameObject.layer == 9 && !contacttarget.GetBool("ShellContact"))
        {
            if (moveDirection != 0)
            {
                contacttarget.SetBool("ShellContact", true);
                contactrb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
            }
        }
    }

    //스크립트가 비활성화되면 자동으로 실행되는 이벤트입니다
    public void OnDisable()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        moveForce = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDirection != 0)
            moveForce.velocity = new Vector2(moveDirection * 10, 0);
    }

}
