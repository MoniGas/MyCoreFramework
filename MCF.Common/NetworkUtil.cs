using Microsoft.Win32;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MCF.Common
{
    /// <summary>
    /// 网络相关操作辅助类
    /// </summary>
    public class NetworkUtil
    {

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取以太网卡的物理地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress2()
        {
            NetworkInterface[] aclLocalNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string result = "";

            // 判断所有网卡的mac地址
            foreach (NetworkInterface adapter in aclLocalNetworkInterfaces)
            {
                // 网卡是以太网网卡
                if (!IsEthernet(adapter))
                {
                    continue;
                }

                // 将mac地址组织为00:11:22:33:44:55的格式
                result = GetMacAddr(adapter);
            }
            return result;
        }

        // 获取mac地址
        private static string GetMacAddr(NetworkInterface adapter)
        {
            string strMacAddr = "";
            PhysicalAddress clMacAddr = adapter.GetPhysicalAddress();
            byte[] abMacAddr = clMacAddr.GetAddressBytes();

            for (int i = 0; i < abMacAddr.Length; i++)
            {
                strMacAddr = strMacAddr + abMacAddr[i].ToString("X2");

                // 在每个字节间插入冒号
                if (abMacAddr.Length - 1 != i)
                {
                    strMacAddr = strMacAddr + ":";
                }
            }

            return strMacAddr;
        }

        // 网卡是以太网网卡
        private static bool IsEthernet(NetworkInterface adapter)
        {
            if (NetworkInterfaceType.Ethernet == adapter.NetworkInterfaceType)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取网卡列表
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetNetCardList()
        {
            List<KeyValuePair<string, string>> cardList = new List<KeyValuePair<string, string>>();
            try
            {
                RegistryKey regNetCards = Registry.LocalMachine.OpenSubKey(Win32Utils.REG_NET_CARDS_KEY);
                if (regNetCards != null)
                {
                    string[] names = regNetCards.GetSubKeyNames();
                    RegistryKey subKey = null;
                    foreach (string name in names)
                    {
                        subKey = regNetCards.OpenSubKey(name);
                        if (subKey != null)
                        {
                            object o = subKey.GetValue("ServiceName");
                            object Description = subKey.GetValue("Description");
                            if (o != null)
                            {
                                KeyValuePair<string, string> p = new KeyValuePair<string, string>(o.ToString(), Description.ToString());
                                cardList.Add(p);
                            }
                        }
                    }
                }
            }
            catch { }

            return cardList;
        }

        /// <summary>
        /// 获取未经修改过的MAC地址（真实的特理地址）
        /// </summary>
        /// <param name="cardId">网卡ID</param>
        /// <returns></returns>
        public static string GetPhysicalAddr(string cardId)
        {
            string macAddress = string.Empty;
            uint device = 0;
            try
            {
                string driveName = "\\\\.\\" + cardId;
                device = Win32Utils.CreateFile(driveName,
                                         Win32Utils.GENERIC_READ | Win32Utils.GENERIC_WRITE,
                                         Win32Utils.FILE_SHARE_READ | Win32Utils.FILE_SHARE_WRITE,
                                         0, Win32Utils.OPEN_EXISTING, 0, 0);
                if (device != Win32Utils.INVALID_HANDLE_VALUE)
                {
                    byte[] outBuff = new byte[6];
                    uint bytRv = 0;
                    int intBuff = Win32Utils.PERMANENT_ADDRESS;

                    if (0 != Win32Utils.DeviceIoControl(device, Win32Utils.IOCTL_NDIS_QUERY_GLOBAL_STATS,
                                        ref intBuff, 4, outBuff, 6, ref bytRv, 0))
                    {
                        string temp = string.Empty;
                        foreach (byte b in outBuff)
                        {
                            temp = Convert.ToString(b, 16).PadLeft(2, '0');
                            macAddress += temp;
                            temp = string.Empty;
                        }
                    }
                }
            }
            finally
            {
                if (device != 0)
                {
                    Win32Utils.CloseHandle(device);
                }
            }

            return macAddress;
        }


        /// <summary>
        /// 获取本机的计算机名
        /// </summary>
        public static string LocalHostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        /// 获取本机的局域网IP
        /// </summary>        
        public static string LANIP
        {
            get
            {
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //如果本机IP列表为空，则返回空字符串
                if (addressList.Length < 1)
                {
                    return "";
                }

                //返回本机的局域网IP
                return addressList[0].ToString();
            }
        }

        /// <summary>
        /// 获取本机在Internet网络的广域网IP
        /// </summary>        
        public static string WANIP
        {
            get
            {
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //如果本机IP列表小于2，则返回空字符串
                if (addressList.Length < 2)
                {
                    return "";
                }

                //返回本机的广域网IP
                return addressList[1].ToString();
            }
        }

        /// <summary>
        /// 获取远程客户机的IP地址
        /// </summary>
        /// <param name="clientSocket">客户端的socket对象</param>        
        public static string GetClientIP(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }

        #region 获取TcpListener对象的本地终结点
        /// <summary>
        /// 获取TcpListener对象的本地终结点
        /// </summary>
        /// <param name="tcpListener">TcpListener对象</param>        
        public static IPEndPoint GetLocalPoint(TcpListener tcpListener)
        {
            return (IPEndPoint)tcpListener.LocalEndpoint;
        }

        /// <summary>
        /// 获取TcpListener对象的本地终结点的IP地址
        /// </summary>
        /// <param name="tcpListener">TcpListener对象</param>        
        public static string GetLocalPoint_IP(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Address.ToString();
        }

        /// <summary>
        /// 获取TcpListener对象的本地终结点的端口号
        /// </summary>
        /// <param name="tcpListener">TcpListener对象</param>        
        public static int GetLocalPoint_Port(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Port;
        }

        /// <summary>  
        /// 获取本机已被使用的网络端点  
        /// </summary>  
        public IList<IPEndPoint> GetUsedIPEndPoint()
        {
            //获取一个对象，该对象提供有关本地计算机的网络连接和通信统计数据的信息。  
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPV4) 传输控制协议 (TCP) 侦听器的终结点信息。  
            IPEndPoint[] ipEndPointTCP = ipGlobalProperties.GetActiveTcpListeners();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPv4) 用户数据报协议 (UDP) 侦听器的信息。  
            IPEndPoint[] ipEndPointUDP = ipGlobalProperties.GetActiveUdpListeners();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPV4) 传输控制协议 (TCP) 连接的信息。  
            TcpConnectionInformation[] tcpConnectionInformation = ipGlobalProperties.GetActiveTcpConnections();

            IList<IPEndPoint> allIPEndPoint = new List<IPEndPoint>();
            foreach (IPEndPoint iep in ipEndPointTCP) allIPEndPoint.Add(iep);
            foreach (IPEndPoint iep in ipEndPointUDP) allIPEndPoint.Add(iep);
            foreach (TcpConnectionInformation tci in tcpConnectionInformation) allIPEndPoint.Add(tci.LocalEndPoint);

            return allIPEndPoint;
        }

        /// <summary>  
        /// 判断指定的网络端点（只判断端口）是否被使用  
        /// </summary>  
        public bool IsUsedIPEndPoint(int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  
        /// 判断指定的网络端点（判断IP和端口）是否被使用  
        /// </summary>  
        public bool IsUsedIPEndPoint(string ip, int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Address.ToString() == ip && iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// 检测本机是否联网（互联网）
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 检测本机是否联网
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectedInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                //已联网
                return true;
            }
            else
            {
                //未联网
                return false;
            }

        }

        [DllImport("WININET", CharSet = CharSet.Auto)]
        private static extern bool InternetGetConnectedState(ref InternetConnectionStatesType lpdwFlags, int dwReserved);

        /// <summary>
        /// 检测本机是否联网的连接属性
        /// </summary>
        public static InternetConnectionStatesType CurrentState
        {
            get
            {
                InternetConnectionStatesType state = 0;

                InternetGetConnectedState(ref state, 0);

                return state;
            }
        }



    }


    /// <summary>
    /// Internet连接状态枚举
    /// </summary>
    [Flags]
    public enum InternetConnectionStatesType : int
    {
        ModemConnection = 0x1,
        LANConnection = 0x2,
        ProxyConnection = 0x4,
        RASInstalled = 0x10,
        Offline = 0x20,
        ConnectionConfigured = 0x40
    }

    #region Win32Utils
    internal class Win32Utils
    {
        public const string REG_NET_CARDS_KEY = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkCards";
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint OPEN_EXISTING = 3;
        public const uint INVALID_HANDLE_VALUE = 0xffffffff;
        public const uint IOCTL_NDIS_QUERY_GLOBAL_STATS = 0x00170002;
        public const int PERMANENT_ADDRESS = 0x01010101;

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(uint hObject);

        [DllImport("kernel32.dll")]
        public static extern int DeviceIoControl(uint hDevice,
                                                  uint dwIoControlCode,
                                                  ref int lpInBuffer,
                                                  int nInBufferSize,
                                                  byte[] lpOutBuffer,
                                                  int nOutBufferSize,
                                                  ref uint lpbytesReturned,
                                                  int lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern uint CreateFile(string lpFileName,
                                              uint dwDesiredAccess,
                                              uint dwShareMode,
                                              int lpSecurityAttributes,
                                              uint dwCreationDisposition,
                                              uint dwFlagsAndAttributes,
                                              int hTemplateFile);

    }
    #endregion
}
