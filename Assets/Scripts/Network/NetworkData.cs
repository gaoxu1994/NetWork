using UnityEngine;
using System.Collections;

//网络传输数据
public enum TrafficType
{
    No = -1,
    Car = 0,        //汽车类型
    AirPlane = 1    //飞机类型
}

//组装的类型
public enum SignType
{
    NO = -1,
    SedanOrFighter = 0,    //轿车或者战斗机
    SUVOrPropeller = 1,     //SUV或者螺旋桨飞机
    TruckOrAirliner = 2     //卡车或者客机
}

public enum ColorType
{
    Black = 1,
    Red = 2,
    Blue = 3,
    Yellow = 4
}
//数据
public static class NetworkData {

    //接收旋转数据
    public static int RECEIVEDROTATION = 0;
    //接收链接数据
    public static int RECEIVEDATA = 1;

    //请求链接
    public static int RECEIVEDCONNECT = 2;
    //成功连接信号
    public static int SENDSUCCEED = 3;
    //连接失败信号
    public static int SENDFAIL = 4;
    //收到断开连接信号
    public static int RECEICEDISCONNECT = 5;



    //数据信号
    public static float RotX;
    public static float RotY;
    public static float RotZ;


    #region Data
    //是否进行初始化
    public static bool IsInit = false;
    //是否清楚数据

    public static bool IsUnload = false;


    //交通工具的类型///
    /// <summary>
    /// 汽车或者飞机
    /// </summary>
    public static TrafficType TypeTraffic = TrafficType.No;
    //选择组装类型的信号
    /// <summary>
    /// 0------轿车/战斗机
    /// 1------SUV/螺旋桨飞机
    /// 2------卡车/客机
    /// </summary>
    public static SignType TypeSign = SignType.NO;
    /// <summary>
    /// 组装的模型颜色以及材质
    /// </summary>
    public static ColorType TypeColor = ColorType.Black;
    
    #endregion
}
