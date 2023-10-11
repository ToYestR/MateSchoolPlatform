using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//using DG.Tweening;
/// <summary>
/// OverLookMain 相机脚本
/// </summary>
public class OverLookMain : MonoBehaviour
{
	public static OverLookMain instance;
	public Transform target;
	public float Y_Min = 5.0f;
	public float Y_Max = 45.0f;
	public float MinDis = 15f;
	public float MaxDis = 25f;
	public float DisSensitivity = 5.0f;
	public float DragSensitivity = 0.3f;
	public float MoveSpeed = 0.2f;

	float InitDis;
	float saveDis;
	float DisVelocity;
	float TimeVelocityX;
	float TimeVelocityY;
	float Xsmooth;
	float Ysmooth;
	float Save_X;
	float Save_Y;
	float Temp_X;
	float Temp_Y;
	Vector2 lastTP1;
	Vector2 lastTP2;
	Vector3 OverLook_Pos;
	Quaternion OverLook_rot;
	private Vector3 tra;
	private Vector3 rot;
	static public float OverLook_X = 0.0f;   //相机纵向旋转值
	static public float OverLook_Y = 0.0f;   //相机横向旋转值
	static public float OverLook_Dis;             //相机远近值
	static public bool IO = false;           //
	public bool uIisCanMove = true;
	// 
	void Awake()
	{
		instance = this;
		OverLook_X = transform.eulerAngles.y;
		OverLook_Y = transform.eulerAngles.x;
		Xsmooth = OverLook_X;
		Ysmooth = OverLook_Y;

		InitDis = Vector3.Distance(transform.position, target.position);
		//脚本唤醒时给摄像机归位
		//		transform.rotation = Quaternion.Euler(OverLook_Y, OverLook_X, 0);
		//transform.position = new Vector3 (target.position.x,target.position.y,target.position.z-InitDis) ;

		OverLook_Dis = InitDis;
		saveDis = InitDis;
	}
	void OnEnable()
	{
		OverLook_X = transform.eulerAngles.y;
		OverLook_Y = transform.eulerAngles.x;
		Xsmooth = OverLook_X;
		Ysmooth = OverLook_Y;

		InitDis = Vector3.Distance(transform.position, target.position);
		//脚本唤醒时给摄像机归位
		transform.rotation = Quaternion.Euler(OverLook_Y, OverLook_X, 0);
		//transform.position = new Vector3 (target.position.x,target.position.y,target.position.z-InitDis) ;

		OverLook_Dis = InitDis;
		saveDis = InitDis;
	}
    //	
    private void Start()
    {
		tra = transform.localPosition;
		rot = transform.localRotation.eulerAngles;

	}
    public float speed = 20;
	public float rotation_H_speed = 1;
	public float rotation_V_speed = 1;
	// 
	void LateUpdate()
	{
		if (!uIisCanMove) return;


		if (target != null)
		{
			if (IO == false)
			{


				OverLook_Dis = Mathf.Clamp(OverLook_Dis -= Input.GetAxis("Mouse ScrollWheel") * DisSensitivity * 5f, MinDis, MaxDis);


                if (Input.GetMouseButtonDown(1))
                {
                    Temp_X = Input.mousePosition.x;
                    Temp_Y = Input.mousePosition.y;
                    Save_X = OverLook_X;
                    Save_Y = OverLook_Y;
                }
                if (Input.GetMouseButton(1))
				{
					OverLook_X = Save_X + (Input.mousePosition.x - Temp_X) * DragSensitivity;
					OverLook_Y = Save_Y + (Input.mousePosition.y - Temp_Y) * DragSensitivity * -1f;
					OverLook_Y = Mathf.Clamp(OverLook_Y, Y_Min, Y_Max);

					//}

				}
			}
			float New_Xsmooth = Mathf.SmoothDamp(Xsmooth, OverLook_X, ref TimeVelocityX, MoveSpeed);
			float New_Ysmooth = Mathf.SmoothDamp(Ysmooth, OverLook_Y, ref TimeVelocityY, MoveSpeed);
			Xsmooth = New_Xsmooth;
			Ysmooth = New_Ysmooth;

			OverLook_rot = Quaternion.Euler(Ysmooth, Xsmooth, 0);
			transform.rotation = OverLook_rot;

			float New_distance = Mathf.SmoothDamp(saveDis, OverLook_Dis, ref DisVelocity, MoveSpeed);
			saveDis = New_distance;
			Vector3 CamerainitDisance = new Vector3(0.0f, 0.0f, -saveDis);
			OverLook_Pos = transform.rotation * CamerainitDisance + target.position;
			transform.position = OverLook_Pos;

			#region 中键控制移动
			// 平移-YZL调整为左键
			if (Input.GetMouseButton(0))
			{
				target.transform.localRotation = Quaternion.Euler(new Vector3(90, transform.localRotation.eulerAngles.y, 0));

				//target.transform.Translate(Vector3.Lerp(Vector3.zero,new Vector3(Input.GetAxis("Mouse X") * rotation_H_speed * 5f,Input.GetAxis("Mouse Y") * rotation_V_speed * 5f,0), 2));
			
				target.transform.Translate(new Vector3(Input.GetAxis("Mouse X") * rotation_H_speed * -5f, Input.GetAxis("Mouse Y") * rotation_V_speed * -5f, 0));

				//if (target.transform.localPosition.x > 200)
    //            {
    //                target.transform.localPosition = new Vector3(200, target.transform.localPosition.y, target.transform.localPosition.z);

    //            }
    //            if (target.transform.localPosition.x < -200)
    //            {
    //                target.transform.localPosition = new Vector3(-200, target.transform.localPosition.y, target.transform.localPosition.z);

    //            }
    //            if (target.transform.localPosition.z > 200)
    //            {
    //                target.transform.localPosition = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, 200);

    //            }
    //            if (target.transform.localPosition.z < -200)
    //            {
    //                target.transform.localPosition = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, -200);

    //            }
            }
			#endregion
			#region 鼠标控制移动
			//         // 移动
			//         if (Input.GetKey(KeyCode.A)) //左移
			//{
			//	target.transform.Translate(-Vector3.left * speed * Time.deltaTime);
			//}
			//if (Input.GetKey(KeyCode.D)) //右移
			//{
			//	target.transform.Translate(-Vector3.right * speed * Time.deltaTime);

			//}
			//if (Input.GetKey(KeyCode.W)) //前移
			//{
			//	target.transform.Translate(-Vector3.forward * speed * Time.deltaTime);

			//}
			//if (Input.GetKey(KeyCode.S)) //后移
			//{
			//	target.transform.Translate(-Vector3.back * speed * Time.deltaTime);

			//}
			#endregion
		}
		else
		{
			Debug.LogWarning("目标物体未设置");
		}
        if (Input.GetKeyDown(KeyCode.R))
        {
			ResetMove();

		}
	}
	/// <summary>
	/// 相机移动
	/// </summary>
	/// <param name="tra"></param>
	public void Move(Transform tra)
	{
		//target.transform.DOMove(tra.position, 2f);
	}
	/// <summary>
	/// 相机复位
	/// </summary>
	public void ResetMove()
	{
		//DOTween.KillAll();
		//target.transform.DOLocalMove(Vector3.zero, 1f);
		target.transform.localPosition = Vector3.zero;
		transform.localPosition = tra;
		transform.localRotation = Quaternion.Euler(rot);
		OverLook_X = transform.eulerAngles.y;
		OverLook_Y = transform.eulerAngles.x;

		InitDis = 0;
		saveDis = 0;
		DisVelocity = 0;
		TimeVelocityX = 0;
		TimeVelocityY = 0;
		Xsmooth = 0;
		Ysmooth = 0;
		Save_X = 0;
		Save_Y = 0;
		Temp_X = 0;
		Temp_Y = 0;
		Xsmooth = OverLook_X;
		Ysmooth = OverLook_Y;

		InitDis = Vector3.Distance(transform.position, target.position);
		//脚本唤醒时给摄像机归位
		transform.rotation = Quaternion.Euler(OverLook_Y, OverLook_X, 0);
		//transform.position = new Vector3 (target.position.x,target.position.y,target.position.z-InitDis) ;

		OverLook_Dis = InitDis;
		saveDis = InitDis;
	}
}
