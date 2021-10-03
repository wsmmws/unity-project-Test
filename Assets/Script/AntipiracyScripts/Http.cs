using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using UnityEngine.UI; 
using System;
using System.IO;



public class Http : MonoBehaviour {
	//与服务器端进行通讯，并且读取返回的JSON，结果UI显示
	public TestJSON testJSON;
	public TestJSON1 testJSON1;//加密JSON

	public string json;
	string state;
	public static string license;


	string filePath;  //保存的文件路径
	public static Texture2D SchoolLogo;         //下载的图片
	static string PicUrl;//用于存放图片的网址



	// Use this for initialization
	void Start () {
		//调试用，初始状态为未激活
		//	PlayerPrefs.SetInt ("ActiveState", 0);
	}

	void Update () {
		//刚进入场景时的UI显示
		Text txt1 = GameObject.Find("State").GetComponent<Text> ();
		if (PlayerPrefs.GetInt ("ActiveState") == 0) {
			txt1.text = "当前激活状态：未激活";
			GameObject.Find("ImageCover").transform.localScale=new Vector3(0,0,0);
		} else {
			state = "已激活";
			txt1.text = "当前激活状态："+state+"\n"+
				"激活时间："+ConvertDateTime(long.Parse(PlayerPrefs.GetString ("ActiveTime")))+"\n"
				+"到期时间："+ConvertDateTime(long.Parse(PlayerPrefs.GetString ("ExpireTime")));
		}


	}
	//当点击按钮时才会与服务器通讯
	public void OnClick(){

		StartCoroutine(RequestServer());

		//如果当网络不可用，则弹出对话框
		if (Application.internetReachability== NetworkReachability.NotReachable)              
		{ 
			Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
			txt1.text ="请连接网络";
		}

		//读取输入的序列号前五位并存储
		InputField input1=GameObject.Find("InputField1").GetComponent<InputField>();
		PlayerPrefs.SetString("XuLieHaoFirst5",input1.textComponent.text.Substring(0,5));
//		Debug.Log (PlayerPrefs.GetString("XuLieHaoFirst5"));
	}

	public IEnumerator RequestServer()
	{
		testJSON1 = new TestJSON1();//加密过后的JSON
		//		testJSON = new TestJSON();//明文JSON
		json = JsonUtility.ToJson(testJSON1);
		Debug.Log (json);
		string url = "https://aps.arvreducation.org/licensepack/verifyLicense";

		WWWForm form = new WWWForm(); 
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers["Content-Type"] = "application/json"; 
		headers["Accept"] = "application/json"; 

		WWW www = new WWW (url,Encoding.UTF8.GetBytes(json),headers);
		yield return www;


		if (string.IsNullOrEmpty (www.error)) {
			string resultData = www.text;

//			string jsondata = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);

			//获取key对应的value
			JsonData ORGjsonData = JsonMapper.ToObject(www.text);
			String  StrjsonData = Base64Decode (ReverseA (ORGjsonData["data"].ToString ()).Substring (8));
			Debug.Log (StrjsonData);

			JsonData jsonData=JsonMapper.ToObject(StrjsonData);

			//如果传到服务器端的序列号正确
			if (jsonData["code"].ToString()=="0200")
		    {
								//如果是未激活的状态，将激活时间写入内存
								if(PlayerPrefs.GetInt("ActiveState")==0){
									PlayerPrefs.SetString ("ActiveTime",jsonData["active_time"].ToString());
									PlayerPrefs.SetString ("ExpireTime",jsonData["expire_time"].ToString());
									PlayerPrefs.SetInt ("ActiveState", 1);
								}

				//将logo所在地址补全
				PicUrl = "https://"+jsonData ["logo"].ToString ();

//				如果本地时间在激活日期和失效时期之间，返回可以使用，否则不在有效期内
				Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
				long LocalTime=ConvertDateTimeInt (System.DateTime.Now);
				if ((LocalTime + 3) <= long.Parse (PlayerPrefs.GetString ("ExpireTime")) && (LocalTime + 3) >= long.Parse (PlayerPrefs.GetString ("ActiveTime"))) {
					txt1.text = "当前时间：" + System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss") + "在有效期内，可以使用";
					// Debug.Log(LocalTime);
				} else {
					txt1.text ="当前时间："+System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")+ "不在有效期内，已过期";
					PlayerPrefs.SetInt ("ActiveState", 0);
					//	GameObject.Find("ImageCover").transform.localScale=new Vector3(0,0,0);
				}


	
				//从服务器端下载学校Logo并保存在本地
				//资源在服务器的路径上
				WWW date = new WWW (PicUrl);
				//等待下载完
				yield return date;
				//下载完，得到所下载的图像的贴图
				SchoolLogo = date.texture;
				byte[] bytes = SchoolLogo.EncodeToPNG();

				//如果服务器端没有上传图片，返回值为null时在DrawAbout中用空白值代替
				if (jsonData ["logo"].ToString () == "aps.arvreducation.org/null") {
					PlayerPrefs.SetString("Logo","null");
				} else {
					PlayerPrefs.SetString ("Logo", Convert.ToBase64String (bytes));
				}
				Debug.Log("图片下载完成");
			}

	
			//如果传到服务器端的序列号错误
			if (jsonData ["code"].ToString () == "0404") {
//				Debug.Log ("XXX");
				InputField input1=GameObject.Find("InputField1").GetComponent<InputField>();
				if(input1.textComponent.text !=""){
					Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
					txt1.text ="序列号不存在";
				}
				}

			//如果该序列号已经过期
			if (jsonData ["code"].ToString () == "0301") {
				//				Debug.Log ("XXX");
				InputField input1=GameObject.Find("InputField1").GetComponent<InputField>();
				if(input1.textComponent.text !=""){
					Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
					txt1.text ="序列号已过期";
				}
			}

			//如果该序列号激活次数已满
			if (jsonData ["code"].ToString () == "0302") {
				//				Debug.Log ("XXX");
				InputField input1=GameObject.Find("InputField1").GetComponent<InputField>();
				if(input1.textComponent.text !=""){
					Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
					txt1.text ="序列号激活次数已满";
				}
			}


			Debug.Log (resultData);//获取返回的原始JSON
		}else{
			Debug.Log (www.error);
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

	//将字符串倒序的算法
	public string ReverseA(string text)
	{
		char[] cArray = text.ToCharArray();
		string reverse = String.Empty;
		for (int i = cArray.Length - 1; i > -1; i--)
		{
			reverse += cArray[i];
		}
		return reverse;
	}

	//Base64解密算法
	public static string Base64Decode(string message) {
		byte[] bytes = Convert.FromBase64String(message);
		return Encoding.GetEncoding("utf-8").GetString(bytes);
//		return System.Text.Encoding.UTF8.GetString(bytes,3,bytes.Length-3);

	}
	//将普通时间转化为时间戳的方法
	public static long ConvertDateTimeInt(System.DateTime time)
	{
		//double intResult = 0;
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1,0,0,0,0));
		//intResult = (time- startTime).TotalMilliseconds;
		long t = (time.Ticks - startTime.Ticks)/10000000; //除10000调整为13位
		return t;
	}

	/// 图片转为byte[]
	private byte[] ImageToBytes(string imageFileName) {
		return File.ReadAllBytes(imageFileName);
	}
	/// 图片转为Base64
	private string ImageToBase64(string imageFileName) {
		byte[] buffers = ImageToBytes (imageFileName);
		return Convert.ToBase64String (buffers);
	}
	/// Base64转为图片
	private void Base64ToSaveImage(string base64) {
		byte[] buffers = Convert.FromBase64String(base64);
		File.WriteAllBytes(Application.dataPath+"/Base64ToSaveImage.png", buffers);
	}

}
