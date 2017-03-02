using UnityEngine;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ServerThread
{
    private Socket serverSocket;

    private ServerAgentThread sat;

    private bool flag = false;
    private static ServerThread Instance;
    private int port = 8711;

    private Thread myThread;



    public static ServerThread getInstance()
    {
        if(Instance == null)
        {
            Instance = new ServerThread();
        }
        return Instance;
    }
    ServerThread()//构造器
    {
        try
        {
            //服务器Ip地址
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, port);
            serverSocket.Bind(ipe);     //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
            Debug.Log("Listening On " + port + "..........");
            flag = true;
            myThread = new Thread(ListenClientConnect);
            flag = true;
            //启动线程
            myThread.Start();
        }catch(Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    /// <summary>  
    /// 监听客户端连接  
    /// </summary>  
    private void ListenClientConnect()
    {
        while (flag)
        {
            Debug.Log("ListenClientConnect");
            try
            {
                Socket clientSocket = serverSocket.Accept();
                Debug.Log("Clint side +1");
                //创建客户端代理线程
                sat = new ServerAgentThread(clientSocket);
            }
            catch (Exception e)
            {
                //打印报错信息
                Debug.LogError(e.ToString());
            }
        }
    }
    //关闭Socket连接
    public void close()
    {
        if (sat != null)
            sat.close();

        flag = false;
        serverSocket.Close();
        
    }


}
