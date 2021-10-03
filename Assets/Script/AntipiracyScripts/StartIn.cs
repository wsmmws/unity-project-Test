using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartIn : MonoBehaviour {
	public  string SceneName;
	// Use this for initialization
	void Start () {

		//到期弹窗
		GameObject.Find ("TanChuang").transform.localScale = Vector3.zero;
		//激活之后直接进入第一个场景
		long LocalTime=ConvertDateTimeInt (System.DateTime.Now);
		if (PlayerPrefs.GetInt ("ActiveState") == 1) {
			if ((LocalTime +1)<= long.Parse (PlayerPrefs.GetString ("ExpireTime"))&&(LocalTime +1)>=long.Parse (PlayerPrefs.GetString ("ActiveTime"))) {
				GameObject.Find("ImageCover").transform.localScale=new Vector3(9,10,5);

				//到期时间小于3天时提示，否则直接进入
				if(long.Parse (PlayerPrefs.GetString ("ExpireTime"))-LocalTime<86400 * 3){
					GameObject.Find ("TanChuang").transform.localScale = new Vector3 (4.5f,2.5f,4.5f); 
					Text txt1 = GameObject.Find("Attention").GetComponent<Text> ();
					txt1.text = "提醒您软件使用码("+PlayerPrefs.GetString("XuLieHaoFirst5")+"*)即将到期，到期时间:\n";
					Text txt2 = GameObject.Find("AttentionDate").GetComponent<Text> ();
					txt2.text =ConvertDateTime(long.Parse(PlayerPrefs.GetString ("ExpireTime")))+"\n请联系ar@bnu.edu.cn";
				}else{
					Application.LoadLevel (SceneName);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetInt ("ActiveState") == 1) {
			//验证成功后必须等到图片下载完成才能进入场景，否则按钮为灰色
			if(Http.SchoolLogo!=null){
				GameObject.Find ("Start").GetComponent<Button> ().interactable = true; 
				GameObject.Find ("Button").GetComponent<Button> ().interactable = false; 
			}
		}
			//验证失败不能进入场景
		else {
			GameObject.Find ("Start").GetComponent<Button> ().interactable = false; 
			GameObject.Find ("Button").GetComponent<Button> ().interactable = true; 
		}
	}

	public void OnClick(){
		//如果当前为激活状态，则载入第一个场景
		if(PlayerPrefs.GetInt ("ActiveState") == 1){Application.LoadLevel (SceneName);}
		//如果当前为失效状态，则将弹窗隐藏
		if(PlayerPrefs.GetInt ("ActiveState") == 0){GameObject.Find ("TanChuang").transform.localScale = Vector3.zero;}
	}
	//将普通时间转化为时间戳的方法
	public static long ConvertDateTimeInt(System.DateTime time)
	{
		//double intResult = 0;
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1,0,0,0,0));
		//intResult = (time- startTime).TotalMilliseconds;
		long t = (time.Ticks - startTime.Ticks)/10000000; 
		return t;
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
