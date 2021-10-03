using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System;
using System.IO;

public class DrawAboutLogo : MonoBehaviour
//此为防盗版软件的DrawAbout，用以替换原有的DrawAbout
{
	private Texture2D logo, about, shuiyin, weifang,kongbai ;
	private bool drawAbout;
	private float size, logo_size, shuiyin_size;
	Texture2D SchoolLogo;
	private String expireTime;

	void Start()
	{
		logo = (Texture2D)Resources.Load("Textures/logo");
		about = (Texture2D)Resources.Load("Textures/bg_about");
		kongbai = (Texture2D)Resources.Load("Textures/KongBai");
		// shuiyin = (Texture2D)Resources.Load("Textures/shuiyin");
		drawAbout = false;
		SchoolLogo = new Texture2D(10, 10);
		SchoolLogo.LoadImage(Convert.FromBase64String(PlayerPrefs.GetString("Logo")));//流数据转换成Texture2D
		expireTime="到期时间："+ConvertDateTime(long.Parse(PlayerPrefs.GetString ("ExpireTime")))+"   序列号前五位："+PlayerPrefs.GetString("XuLieHaoFirst5");
	}

	void Update()
	{
		size = Screen.width < Screen.height ? Screen.width : Screen.height;
		logo_size = Screen.width > Screen.height ? Screen.width : Screen.height;
		shuiyin_size = size / 2;
	
//		Debug .Log(PlayerPrefs.GetString("Logo"));
	}

	void OnGUI()
	{
		//不要水印 将for注掉就好
		//for (int i = 0; i < Screen.width/shuiyin_size; i++) 
		//{
		//for(int j = 0;j<Screen.height/shuiyin_size;j++)
		//{
		//GUI.DrawTexture (new Rect (i*shuiyin_size,j*shuiyin_size, shuiyin_size, shuiyin_size), shuiyin, ScaleMode.StretchToFill);
		//}
		//}

		 if (drawAbout)
		 {
		 	GUI.DrawTexture(new Rect(Screen.width - size * 1.07f, Screen.height - size * 0.7f - Screen.width * 0.12f, size, size * 0.75f), about, ScaleMode.StretchToFill);
			string aa = expireTime;
			GUIStyle bb=new GUIStyle();
			bb.normal.background = null;    //设置背景填充的
			bb.normal.textColor=new Color(1,0,0);   //设置字体颜色
			bb.fontSize = 40;       //字体大小
			GUI.Label(new Rect(0, 0, 200, 200), aa,bb);
//			GUI.Label(new Rect(10,10,200,20),"Hello World!");
		 }
		 if (GUI.Button(new Rect(Screen.width - logo_size * 0.08f, Screen.height - logo_size * 0.08f, logo_size * 0.07f, logo_size * 0.07f), logo, GUI.skin.label))
		 {
		 	drawAbout = !drawAbout;
		 }

		if(PlayerPrefs.GetString("Logo")=="null"){
//			Debug .Log("没有图");
			GUI.Button(new Rect(Screen.width - logo_size * 0.15f, Screen.height - logo_size * 0.08f, logo_size * 0.07f, logo_size * 0.07f), kongbai, GUI.skin.label);
		}
		else{
			GUI.Button(new Rect(Screen.width - logo_size * 0.15f, Screen.height - logo_size * 0.08f, logo_size * 0.07f, logo_size * 0.07f), SchoolLogo, GUI.skin.label);
		}

	}

	//将时间戳转化为普通时间
	public static String ConvertDateTime(long time)
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		DateTime dt = startTime.AddSeconds (time);
		string t = dt.ToString ("yyyy/MM/dd HH:mm:ss");
		return t;
	}
}
