using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiPiracy : MonoBehaviour {
	//AR防盗版系统客户端获取时间逻辑
	/*
	上次启动的本地时间"NOW"
	激活时间"ActiveTIme"，为字符串
	激活状态“ActiveState”，为字符串
	到期时间“ExpireTime”，为字符串
	*/
	// public static string NetTime;    //网络时间
	public static long LocalTime;    //本地时间

	// Use this for initialization
	void Start () {
		//如果当前系统时间大于获取本地时间，则将当前系统时间写在内存中
		//如果当前系统时间小于上次打开的时间，设置状态为未激活，需要调整时间重新激活
		LocalTime=ConvertDateTimeInt (System.DateTime.Now);
		if(LocalTime<long.Parse (PlayerPrefs.GetString ("NOW"))){
			PlayerPrefs.SetInt ("ActiveState", 0);
			GameObject.Find("ImageCover").transform.localScale=new Vector3(0,0,0);
			GameObject.Find ("TanChuang").transform.localScale = Vector3.zero;
		}
		//如果当前系统时间大于过期时间，则软件失效
		if(LocalTime>long.Parse (PlayerPrefs.GetString ("ExpireTime"))){
			PlayerPrefs.SetInt ("ActiveState", 0);
			GameObject.Find ("TanChuang").transform.localScale = new Vector3 (4.5f,2.5f,4.5f); 
			Text txt1 = GameObject.Find("Attention").GetComponent<Text> ();
			txt1.text = "提醒您软件使用码("+PlayerPrefs.GetString("XuLieHaoFirst5")+"*)已经到期，到期时间:\n";
			Text txt2 = GameObject.Find("AttentionDate").GetComponent<Text> ();
			txt2.text =DrawAboutLogo.ConvertDateTime(long.Parse(PlayerPrefs.GetString ("ExpireTime")))+"\n请联系ar@bnu.edu.cn";
		}

	}
	
	// Update is called once per frame
	void Update () {

		//如果本地时间不在服务器端返回的有效期时间范围内则弹窗退出
		LocalTime=ConvertDateTimeInt (System.DateTime.Now);
		PlayerPrefs.SetString ("NOW", LocalTime.ToString());
	}

	//将普通时间转化为时间戳的方法
	public static long ConvertDateTimeInt(System.DateTime time)
	{
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1,0,0,0,0));
		long t = (time.Ticks - startTime.Ticks)/10000000; //除10000调整为13位
		return t;
	}
}
