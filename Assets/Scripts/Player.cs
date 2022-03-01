using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float jumpPower = 8f;
    [SerializeField] //SerializeField는 유니티 인스펙터에 private 멤버변수를 노출해줍니다.
    float moveSpeed = 5f;
    Rigidbody2D getRigidbody;
    bool isGround = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Solid"))
        {
            isGround = true;
            print($"{collision.transform.tag}랑 충돌했습니다!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //리지드 바디를 받아옵니다. 마리오의 콜라이더는 2D이므로 Rigidbody2D로 받아옵니다
        getRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnJump();
        OnMove();
    }

    //moveDirection 변수를 MoveDirection가 업데이트해줍니다. 대소문자가 다릅니다!! 주의!!
    Vector2 MoveDirection
    {

        get
        {
            return (Vector2.right * Input.GetAxis("MoveX"));
        }
    }

    void OnJump()
    {
        if (isGround && Input.GetButtonDown("Jump")) //왜 이게 계속 트루지..
        {
            isGround = false;
            //Vector2.up * jumpPower는 Vector2.up의 크기를 jumpPower만큼 키워줘서 점프력을 구현한 것입니다. (벡터의 크기 * 실수 = 곱한 크기)
            getRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            print($"점프! {getRigidbody.velocity}");
            return;
        }

        if (Input.GetButtonUp("Jump"))
        {
            //유니티 물리방향에 (1, 0)을 곱해서 y * 0이 되어버리면 점프상승중에 y축 물리력이 0이 되서 멈추게됩니다.
            getRigidbody.velocity *= Vector2.right;
        }
    }

    void OnMove()
    {
        //크기가1인 방향을 가진 벡터 * n의 크기 = 크기가 n이고 방향을 가진 벡터
        getRigidbody.velocity = new Vector2(MoveDirection.x * moveSpeed, getRigidbody.velocity.y);
        print("걷는다!");
    }
}
