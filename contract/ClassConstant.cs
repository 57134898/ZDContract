using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace contract
{
    public enum UserPosition
    {
        None,
        /// <summary>
        /// 采购员
        /// </summary>
        CG,
        /// <summary>
        /// 销售员
        /// </summary>
        XS,
        /// <summary>
        /// 财务
        /// </summary>
        CW,
        /// <summary>
        /// 分公司查询
        /// </summary>
        CX_F,
        /// <summary>
        /// 集团查询
        /// </summary>
        CX_J,
        /// <summary>
        /// 管理员
        /// </summary>
        ADMIN
    }

    class ClassConstant
    {
        /// <summary>
        /// 账套
        /// </summary>
        public static string AccountingBook = string.Empty;
        public static string NB = "01";
        public static string WB = "02";
        public static string NHI = "05";
        public static string GF = "06";
        public static string ZJ = "03";
        public static MForm1 MF1;
        public static string CW_DATABASE = "n7_铸锻公司";
        //public static string CW_IP = ".";
        public static string CW_IP = "192.168.7.70";
        //public static string CW_IP = ".";
        //public static string CONNECT_STRING = "user id=sa;password=;initial catalog=" + CW_DATABASE + ";datasource=" + CW_IP + ";Provider=SQLOLEDB;connect Timeout=20";
        public static string CONNECT_STRING = "Data Source=" + CW_IP + ";Initial Catalog=" + CW_DATABASE + ";Provider=SQLOLEDB;User ID=sa";
        public static string DATABASE = "contract1";
        public static string USER_ID = "0110019999";
        public static string USER_NAME = "孙丽梅";
        public static string DW_ID = "0110";
        public static string DW_NAME = "沈阳铸锻工业有限公司销售公司";
        public static string BCODE = "011001";
        public static string BNAME = "财务部";
        public static string IP = ".";
        public static string BB = "1.001";//版本
        public static UserPosition QX = UserPosition.ADMIN;//采购 销售 财务 查询 管理员

        public static SortedList<string, UserPosition> QXCOLLECTIONS = new SortedList<string, UserPosition>() {
            { "00", UserPosition.None },   
            { "01", UserPosition.CG },
            { "02", UserPosition.XS },
            { "03", UserPosition.CW },
            { "04", UserPosition.CX_F },
            { "05", UserPosition.CX_J },
            { "06", UserPosition.ADMIN }
        };


        public static string[] QXC = new string[] { "采购", "销售", "财务", "查询", "集团查询", "管理员" };
        public static string[] fklx = new string[] { "", "预付款", "货款", "质保金", "工程款", "运费", "加工费", "进度款", "其它" };
        public static string[] fkfs = new string[] { "", "网银", "支票", "电汇", "现金", "承兑", "抹账" };

        //编码级长
        public static int[] LEVEL_AREA = { 1, 2, 3 };
        public static int[] LEVEL_DW = { 4, 2, 4 };
        public static int[] LEVEL_KH = { 2, 2, 4 };
        public static int[] LEVEL_HTLX = { 2, 2 };
        public static int[] LEVEL_YWY = { 4, 2, 4 };
        public static string LEVEL_CHAR = "__________________________________________________________";
        public static string GetLeveLChar(string name, int level)
        {
            switch (name)
            {
                case "LEVEL_AREA":
                    return LEVEL_CHAR.Substring(0, LEVEL_AREA[level]);
                case "LEVEL_DW":
                    return LEVEL_CHAR.Substring(0, LEVEL_DW[level]);
                case "LEVEL_KH":
                    return LEVEL_CHAR.Substring(0, LEVEL_KH[level]);
                case "LEVEL_HTLX":
                    return LEVEL_CHAR.Substring(0, LEVEL_HTLX[level]);
                case "LEVEL_YWY":
                    return LEVEL_CHAR.Substring(0, LEVEL_YWY[level]);
                default:
                    return "";
            }
        }
    }
}
