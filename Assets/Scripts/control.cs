using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

    private ServerThread serverThread;
    GameObject MainCom;
    Object preb;
    MeshRenderer mRender;


	// Use this for initialization
	void Start () {
        if (serverThread == null)
        {
            //启动服务器线程
            serverThread =  ServerThread.getInstance();
        }
        StartCoroutine("InitData");
        StartCoroutine(UnLoad());
        
	}
	
    IEnumerator InitData()
    {
        while (true)
        {
            if (NetworkData.IsInit)
            {
                switch (NetworkData.TypeTraffic)
                {
                    case TrafficType.Car:
                        preb = Resources.Load("Cube", typeof(GameObject));
                        break;
                    case TrafficType.AirPlane:
                        preb = Resources.Load("Capsule", typeof(GameObject));
                        break;

                }
                MainCom = Instantiate(preb) as GameObject;
                MainCom.transform.parent = transform;
                MainCom.name = "MainCom";
                MainCom.transform.localPosition = Vector3.zero;
                mRender = MainCom.GetComponent<MeshRenderer>();
                switch (NetworkData.TypeColor)
                {
                    case ColorType.Black:
                        mRender.material.color = new Color(0, 0, 0);
                        break;
                    case ColorType.Blue:
                        mRender.material.color = new Color(0, 0, 1);
                        break;
                    case ColorType.Red:
                        mRender.material.color = new Color(1, 0, 0);
                        break;
                    case ColorType.Yellow:
                        mRender.material.color = new Color(1, 1, 0);
                        break;
                }
                NetworkData.IsInit = false;

            }
            yield return new WaitForSeconds(0.2f);
        }
        
    }



    IEnumerator UnLoad()
    {
        while (true)
        {
            Debug.Log("等待卸载数据");
            if (NetworkData.IsUnload)
            {
                foreach(Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                NetworkData.IsUnload = false;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(NetworkData.RotX, NetworkData.RotY, NetworkData.RotZ);
	}

    //程序退出关闭相关线程
    private void OnApplicationQuit()
    {
        serverThread.close();
    }
}
