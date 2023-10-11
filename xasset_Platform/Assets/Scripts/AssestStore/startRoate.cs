using UnityEngine;

public class startRoate : MonoBehaviour
{
	private bool onDrag = false;  //�Ƿ���ק    
	public float speed = 6f;      //��ת�ٶ�    
	private float tempSpeed;      //�����ٶ� 
	private float axisX = 1;      //�����ˮƽ�����ƶ�������   
	private float axisY = 1;      //�������ֱ�����ƶ�������   
	private float cXY;

	void OnMouseDown()
	{
		//������갴�µ��¼�// 
		axisX = 0f; axisY = 0f;
	}
	void OnMouseDrag()     //�����קʱ�Ĳ���// 
	{
		onDrag = true;
		axisX = -Input.GetAxis("Mouse X");
		//���������� 
		axisY = Input.GetAxis("Mouse Y");
		cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY); //��������ƶ��ĳ���//
		if (cXY == 0f) { cXY = 1f; }
	}
	//���������ٶ�
	float Rigid()
	{
		if (onDrag)
		{
			tempSpeed = speed;
		}
		else
		{
			if (tempSpeed > 0)
			{
				//ͨ����������ƶ�����ʵ����קԽ���ٶȼ���Խ��
				tempSpeed -= speed * 2 * Time.deltaTime / cXY;
			}
			else
			{
				tempSpeed = 0;
			}
		}
		return tempSpeed;
	}

	void Update()
	{
		//������ǰ���֮ǰ����һֱ������ת
		this.transform.Rotate(new Vector3(axisY, axisX, 0) * Rigid(), Space.World);
	}
}
