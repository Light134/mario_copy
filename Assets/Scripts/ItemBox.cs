using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : DetectorUp
{
    //����� ���� ������ ������Ʈ �����ø�ǿ�
    //�ν����Ϳ����� ISpawnableObject Ÿ�� ������ ������ �� ���⶧���� itemFinder�� GetComponent�ϰ� ��.
    [SerializeField]
    Transform itemFinder = null;
    [SerializeField]
    int haveItem = 1;
    Animator active = null;

    bool IsHave => haveItem > 0;

    //��¥ ������
    ISpawnableObject item;

    // Start is called before the first frame update
    void Start()
    {
        active = GetComponent<Animator>();
        //offset��ġ�� ������ ��ȯ.
        Vector3 spawnPosition = transform.position;
        var itemFinder = Instantiate(this.itemFinder, new Vector3(spawnPosition.x, spawnPosition.y, 1), Quaternion.Euler(0f, 0f, 0f));
        item = itemFinder.GetComponent<ISpawnableObject>();
    }

    public override void OnDetectedUp() // ���� ���ڵ� ���Ƴ�;;
    {
        //����
        if (IsHave)
        {
            haveItem--;
            item.OnSpawn();
            if(haveItem == 0)
            {
                active.SetBool("Active", false);
            }
        }
    }
}
