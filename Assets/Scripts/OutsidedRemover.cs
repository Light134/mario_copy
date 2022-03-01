using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsidedRemover : MonoBehaviour
{
    bool isEncount = false;
    [SerializeField]
    Camera cameraSensor = null; //��������Ʈ ���� �׳� ���⿡ �ν����Ϳ��� ��������
    // Start is called before the first frame update
    public bool IsBecameVisible(Transform target)
    {
        Vector3 screenPoint = cameraSensor.WorldToViewportPoint(target.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBecameVisible(transform)) //ȭ��ȿ� ����
        {
            isEncount = true; //����
            return;
        }
        else //ȭ��ۿ� ������
        {
            if (isEncount) //�ѹ��̶� ���������� ������
            {
                print($"�����: {isEncount}");
                Destroy(gameObject);
            }
        }
    }
}
