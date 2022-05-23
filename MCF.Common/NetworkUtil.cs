using Microsoft.Win32;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MCF.Common
{
    /// <summary>
    /// ������ز���������
    /// </summary>
    public class NetworkUtil
    {

        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //�õ�������
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //��IP��ַ�б���ɸѡ��IPv4���͵�IP��ַ
                    //AddressFamily.InterNetwork��ʾ��IPΪIPv4,
                    //AddressFamily.InterNetworkV6��ʾ�˵�ַΪIPv6����
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
        /// ��ȡ��̫�����������ַ
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress2()
        {
            NetworkInterface[] aclLocalNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string result = "";

            // �ж�����������mac��ַ
            foreach (NetworkInterface adapter in aclLocalNetworkInterfaces)
            {
                // ��������̫������
                if (!IsEthernet(adapter))
                {
                    continue;
                }

                // ��mac��ַ��֯Ϊ00:11:22:33:44:55�ĸ�ʽ
                result = GetMacAddr(adapter);
            }
            return result;
        }

        // ��ȡmac��ַ
        private static string GetMacAddr(NetworkInterface adapter)
        {
            string strMacAddr = "";
            PhysicalAddress clMacAddr = adapter.GetPhysicalAddress();
            byte[] abMacAddr = clMacAddr.GetAddressBytes();

            for (int i = 0; i < abMacAddr.Length; i++)
            {
                strMacAddr = strMacAddr + abMacAddr[i].ToString("X2");

                // ��ÿ���ֽڼ����ð��
                if (abMacAddr.Length - 1 != i)
                {
                    strMacAddr = strMacAddr + ":";
                }
            }

            return strMacAddr;
        }

        // ��������̫������
        private static bool IsEthernet(NetworkInterface adapter)
        {
            if (NetworkInterfaceType.Ethernet == adapter.NetworkInterfaceType)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ��ȡ�����б�
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
        /// ��ȡδ���޸Ĺ���MAC��ַ����ʵ�������ַ��
        /// </summary>
        /// <param name="cardId">����ID</param>
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
        /// ��ȡ�����ļ������
        /// </summary>
        public static string LocalHostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        /// ��ȡ�����ľ�����IP
        /// </summary>        
        public static string LANIP
        {
            get
            {
                //��ȡ������IP�б�,IP�б��еĵ�һ���Ǿ�����IP���ڶ����ǹ�����IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //�������IP�б�Ϊ�գ��򷵻ؿ��ַ���
                if (addressList.Length < 1)
                {
                    return "";
                }

                //���ر����ľ�����IP
                return addressList[0].ToString();
            }
        }

        /// <summary>
        /// ��ȡ������Internet����Ĺ�����IP
        /// </summary>        
        public static string WANIP
        {
            get
            {
                //��ȡ������IP�б�,IP�б��еĵ�һ���Ǿ�����IP���ڶ����ǹ�����IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //�������IP�б�С��2���򷵻ؿ��ַ���
                if (addressList.Length < 2)
                {
                    return "";
                }

                //���ر����Ĺ�����IP
                return addressList[1].ToString();
            }
        }

        /// <summary>
        /// ��ȡԶ�̿ͻ�����IP��ַ
        /// </summary>
        /// <param name="clientSocket">�ͻ��˵�socket����</param>        
        public static string GetClientIP(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }

        #region ��ȡTcpListener����ı����ս��
        /// <summary>
        /// ��ȡTcpListener����ı����ս��
        /// </summary>
        /// <param name="tcpListener">TcpListener����</param>        
        public static IPEndPoint GetLocalPoint(TcpListener tcpListener)
        {
            return (IPEndPoint)tcpListener.LocalEndpoint;
        }

        /// <summary>
        /// ��ȡTcpListener����ı����ս���IP��ַ
        /// </summary>
        /// <param name="tcpListener">TcpListener����</param>        
        public static string GetLocalPoint_IP(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Address.ToString();
        }

        /// <summary>
        /// ��ȡTcpListener����ı����ս��Ķ˿ں�
        /// </summary>
        /// <param name="tcpListener">TcpListener����</param>        
        public static int GetLocalPoint_Port(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Port;
        }

        /// <summary>  
        /// ��ȡ�����ѱ�ʹ�õ�����˵�  
        /// </summary>  
        public IList<IPEndPoint> GetUsedIPEndPoint()
        {
            //��ȡһ�����󣬸ö����ṩ�йر��ؼ�������������Ӻ�ͨ��ͳ�����ݵ���Ϣ��  
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //��ȡ�йر��ؼ�����ϵ� Internet Э��汾 4 (IPV4) �������Э�� (TCP) ���������ս����Ϣ��  
            IPEndPoint[] ipEndPointTCP = ipGlobalProperties.GetActiveTcpListeners();

            //��ȡ�йر��ؼ�����ϵ� Internet Э��汾 4 (IPv4) �û����ݱ�Э�� (UDP) ����������Ϣ��  
            IPEndPoint[] ipEndPointUDP = ipGlobalProperties.GetActiveUdpListeners();

            //��ȡ�йر��ؼ�����ϵ� Internet Э��汾 4 (IPV4) �������Э�� (TCP) ���ӵ���Ϣ��  
            TcpConnectionInformation[] tcpConnectionInformation = ipGlobalProperties.GetActiveTcpConnections();

            IList<IPEndPoint> allIPEndPoint = new List<IPEndPoint>();
            foreach (IPEndPoint iep in ipEndPointTCP) allIPEndPoint.Add(iep);
            foreach (IPEndPoint iep in ipEndPointUDP) allIPEndPoint.Add(iep);
            foreach (TcpConnectionInformation tci in tcpConnectionInformation) allIPEndPoint.Add(tci.LocalEndPoint);

            return allIPEndPoint;
        }

        /// <summary>  
        /// �ж�ָ��������˵㣨ֻ�ж϶˿ڣ��Ƿ�ʹ��  
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
        /// �ж�ָ��������˵㣨�ж�IP�Ͷ˿ڣ��Ƿ�ʹ��  
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
        /// ��Ȿ���Ƿ���������������
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// ��Ȿ���Ƿ�����
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectedInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                //������
                return true;
            }
            else
            {
                //δ����
                return false;
            }

        }

        [DllImport("WININET", CharSet = CharSet.Auto)]
        private static extern bool InternetGetConnectedState(ref InternetConnectionStatesType lpdwFlags, int dwReserved);

        /// <summary>
        /// ��Ȿ���Ƿ���������������
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
    /// Internet����״̬ö��
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
