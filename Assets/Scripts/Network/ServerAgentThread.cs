using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;

public class ServerAgentThread{

    private Socket ClientSocket;
    private Thread thread;
    private bool flag = false;              //线程方法标志位
    public static System.Object datalock = new System.Object();//锁

    public ServerAgentThread(Socket ClientSocket)
    {
        this.ClientSocket = ClientSocket;
        thread = new Thread(ReceiveMSG);
        flag = true;
        //启动线程，接收数据
        thread.Start();
    }

    //接受数据线程方法
    void ReceiveMSG()
    {
        while (flag)
        {
            try
            {
                //读取数据包长度
                int sign= readInt();
                Debug.Log("信号数据" + sign);
                if (sign == NetworkData.RECEIVEDROTATION)        //接收旋转数据 --0
                {

                    lock (datalock)//加锁
                    {
                        NetworkData.RotX = readFloat();
                        NetworkData.RotY = readFloat();
                        NetworkData.RotZ = readFloat();
                    }
                }
                else if (sign == NetworkData.RECEIVEDATA)  //接收链接数据 ---1
                {
                    lock (datalock)//加锁
                    {
                        NetworkData.TypeTraffic = (TrafficType)readInt();
                        NetworkData.TypeSign = (SignType)readInt();
                        NetworkData.TypeColor = (ColorType)readInt();
                        Debug.Log(NetworkData.TypeTraffic + "::::" + NetworkData.TypeSign + ":::" + NetworkData.TypeColor);
                        NetworkData.IsInit = true;
                    }
                }
                else if (sign == NetworkData.RECEIVEDCONNECT)  //请求连接 -- 2
                {
                    //发送成功连接信号
                    SendInt(NetworkData.SENDSUCCEED);
                }
                else if (sign == NetworkData.RECEICEDISCONNECT)//请求断开连接 -- 5
                {
                    //当前信息断开连接
                    Debug.Log("当前客户端断开连接");
                    NetworkData.IsUnload = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                Debug.Log("出现异常关闭连接");
                close();
            }
        }
    }
    #region 发送数据方法


    public void SendInt(int msg)
    {
        SendMSG(ByteUtil.int2ByteArray(msg));
    }
    public void SendFloat(float msg)
    {
        SendMSG(ByteUtil.float2ByteArray(msg));
    }

    //发送数据方法
    private void SendMSG(byte[] bytes)
    {
        if (!ClientSocket.Connected)//断开连接
        {
            Debug.Log("bread connect (sendMSG)");
        }
        try
        {
            //发送数组
            ClientSocket.Send(bytes, SocketFlags.None);

        }
        catch (Exception e)
        {
            Debug.Log("bread connect (sendMSG_catch)");
            Debug.Log(e.ToString());
        }
    }


    #endregion


    #region 读取数据方法
    ////读取数据包方法
    byte[] readPackage(int len)
    {
        byte[] bPackage = new byte[len];                    //收到的数据包
        int status = ClientSocket.Receive(bPackage);        //第一次接受的长度
        while (status != len)                               //循环接收 直到收够指定长度
        {
            int index = status;                             //记录已经收到的长度
            byte[] tempData = new byte[len - status];
            int count = ClientSocket.Receive(tempData);     //接受剩下的
            status += count;                                //更新已接受到的长度
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    bPackage[index + i] = tempData[i]; //合并数组
                }
            }
        }
        return bPackage;
    }
    //读取整形数据
    int readInt()
    {
        byte[] bint = readPackage(4);
        return ByteUtil.byteArray2Int(bint, 0);
    }
    float readFloat()
    {
        byte[] bfloat = readPackage(4);
        return ByteUtil.byteArray2Float(bfloat,0);
    }
    #endregion 
    public void close()
    {
        flag = false;
        ClientSocket.Close(); 
    }

}
