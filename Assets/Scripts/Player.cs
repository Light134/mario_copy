using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float jumpPower = 8f;
    [SerializeField] //SerializeField�� ����Ƽ �ν����Ϳ� private ��������� �������ݴϴ�.
    float moveSpeed = 5f;
    Rigidbody2D getRigidbody;
    bool isGround = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Solid"))
        {
            isGround = true;
            print($"{collision.transform.tag}�� �浹�߽��ϴ�!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //������ �ٵ� �޾ƿɴϴ�. �������� �ݶ��̴��� 2D�̹Ƿ� Rigidbody2D�� �޾ƿɴϴ�
        getRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnJump();
        OnMove();
    }

    //moveDirection ������ MoveDirection�� ������Ʈ���ݴϴ�. ��ҹ��ڰ� �ٸ��ϴ�!! ����!!
    Vector2 MoveDirection
    {

        get
        {
            return (Vector2.right * Input.GetAxis("MoveX"));
        }
    }

    void OnJump()
    {
        if (isGround && Input.GetButtonDown("Jump")) //�� �̰� ��� Ʈ����..
        {
            isGround = false;
            //Vector2.up * jumpPower�� Vector2.up�� ũ�⸦ jumpPower��ŭ Ű���༭ �������� ������ ���Դϴ�. (������ ũ�� * �Ǽ� = ���� ũ��)
            getRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            print($"����! {getRigidbody.velocity}");
            return;
        }

        if (Input.GetButtonUp("Jump"))
        {
            //����Ƽ �������⿡ (1, 0)�� ���ؼ� y * 0�� �Ǿ������ ��������߿� y�� �������� 0�� �Ǽ� ���߰Ե˴ϴ�.
            getRigidbody.velocity *= Vector2.right;
        }
    }

    void OnMove()
    {
        //ũ�Ⱑ1�� ������ ���� ���� * n�� ũ�� = ũ�Ⱑ n�̰� ������ ���� ����
        getRigidbody.velocity = new Vector2(MoveDirection.x * moveSpeed, getRigidbody.velocity.y);
        print("�ȴ´�!");
    }
}
