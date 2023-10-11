using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libx;
public class TeleportationEdit : MonoBehaviour
{
    [Header("�ڴ������ô���")]
    public int time = 100;    // �����ڶ���ʱ�䴫��
    [Header("Ҫ���͵��ĳ�������")]
    public string InScenceName; // Ҫ���͵��ĳ���
    [Header("Ҫ���͵��ĳ�������")]
    public string InScenceId;
    [Header("��ʾ����")]
    public string TitileName;   // ��ʾ�����ı�
    [Header("���ص�ַ")]
    public string url;
    [Space]
    public TextMesh title;
    private int _time;
    private bool isIn = false;
    private void Start()
    {
        title.text = TitileName;
    }
    private void Update()
    {
        if (Camera.main)
        {
            title.transform.LookAt(Camera.main.transform);
        }
        title.transform.Translate(Vector3.up * Mathf.Sin(Time.time) * Time.deltaTime * 0.2f, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        _time = 0;
        isIn = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _time++;
            if (_time > time && isIn == false)
            {
                // TODO �л�����
                GetComponent<Updater_Item>().LoadItem(url, InScenceId, InScenceName);
                Global.currentchildsceneid = InScenceId;
                isIn = true;
            }
        }
    }
    public void OnProgess(string str)
    {
        title.text = TitileName + str;
    }
    private void OnTriggerExit(Collider other)
    {
        _time = 0;
        isIn = false;
    }
}