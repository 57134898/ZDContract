using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using EvalGuy;
using System.Data;
using System.Net;

namespace contract
{
    class ClassCustom
    {
        #region 截取“：”前的编号
        /// <summary>
        /// 截取“：”前的编号
        /// 例：1001:现金 
        /// </summary>
        /// <param name="code">1001:现金 </param>
        /// <returns>编号不为空返回编号 否则返回 CODE本身</returns>
        public static string codeSub(String code)
        {
            try
            {
                if (code == "")
                    return "";
                if (code.IndexOf(":") < 0)
                {
                    return code;
                }
                else
                {
                    return code.Substring(0, code.IndexOf(":"));
                }
            }
            catch { throw; }
        }
        #endregion

        #region 截取“：”后字符
        public static string codeSub1(string code)
        {
            try
            {
                if (code == "")
                    return "";
                if (code.IndexOf(":") < 0)
                {
                    return code;
                }
                else
                {
                    return code.Substring(1 + code.IndexOf(":"));
                }
            }
            catch { throw; }
        }
        #endregion

        #region 获取图片
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>相对路径的图片</returns>
        public static Image getImage(string path)
        {
            try
            {
                return Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\" + path);
            }
            catch (Exception ex)
            {
                MessageView.MessageErrorShow(ex);

                return null;
            }

        }
        #endregion

        #region DATAGIRDVIEW数据导出EXCEL
        /// <summary>
        /// DATAGIRDVIEW数据导出EXCEL
        /// 例：this.ExportDataGridview1(dataGridView1, true);
        /// </summary>
        /// <param name="dataGridView1">要导出的DATAGIRDVIEW</param>
        /// <param name="isShowExcle">是否显示导出的EXCEL</param>
        /// <returns>导出成功返回TRUE失败返回FALSE</returns>
        public static Excel.Application ExportDataGridview1(DataGridView dataGridView1, string name)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                    return null;
                //建立Excel对象
                Excel.Application excel = new Excel.Application();
                excel.Application.Workbooks.Add(true);
                excel.Visible = true;
                excel.get_Range("A1", excel.Cells[2, dataGridView1.Columns.Count]).Merge(false);
                excel.Cells[1, 1] = name;
                (excel.Cells[1, 1] as Excel.Range).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //生成字段名称
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    excel.Cells[3, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                //填充数据
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1[j, i].ValueType == typeof(string))
                        {
                            excel.Cells[i + 4, j + 1] = "'" + (dataGridView1[j, i].Value == null ? "" : dataGridView1[j, i].Value.ToString());
                        }
                        else
                        {
                            excel.Cells[i + 4, j + 1] = (dataGridView1[j, i].Value == null ? "" : dataGridView1[j, i].Value.ToString());
                        }
                    }
                }
                excel.get_Range("A1", excel.Cells[dataGridView1.Rows.Count + 3, dataGridView1.Columns.Count]).EntireColumn.AutoFit();
                DrawExcelBorders(excel, "A3", excel.Cells[dataGridView1.Rows.Count + 3, dataGridView1.Columns.Count]);
                return excel;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                throw ex;
                //return false;
            }
        }
        #endregion

        #region 汉语拼音 汉语拼音首字母
        /// <summary>
        /// 汉语拼音 汉语拼音首字母
        /// </summary>
        public class ChinesePY
        {
            #region private
            static Hashtable ht = null;
            static Hashtable Ht
            {
                get
                {
                    if (ht == null)
                    {
                        ht = new Hashtable();
                        ht.Add(-20319, "a");
                        ht.Add(-20317, "ai"); ht.Add(-20304, "an"); ht.Add(-20295, "ang");
                        ht.Add(-20292, "ao"); ht.Add(-20283, "ba"); ht.Add(-20265, "bai");
                        ht.Add(-20257, "ban"); ht.Add(-20242, "bang"); ht.Add(-20230, "bao");
                        ht.Add(-20051, "bei"); ht.Add(-20036, "ben"); ht.Add(-20032, "beng");
                        ht.Add(-20026, "bi"); ht.Add(-20002, "bian"); ht.Add(-19990, "biao");
                        ht.Add(-19986, "bie"); ht.Add(-19982, "bin"); ht.Add(-19976, "bing");
                        ht.Add(-19805, "bo"); ht.Add(-19784, "bu"); ht.Add(-19775, "ca");
                        ht.Add(-19774, "cai"); ht.Add(-19763, "can"); ht.Add(-19756, "cang");
                        ht.Add(-19751, "cao"); ht.Add(-19746, "ce"); ht.Add(-19741, "ceng");
                        ht.Add(-19739, "cha"); ht.Add(-19728, "chai"); ht.Add(-19725, "chan");
                        ht.Add(-19715, "chang"); ht.Add(-19540, "chao"); ht.Add(-19531, "che");
                        ht.Add(-19525, "chen"); ht.Add(-19515, "cheng"); ht.Add(-19500, "chi");
                        ht.Add(-19484, "chong"); ht.Add(-19479, "chou"); ht.Add(-19467, "chu");
                        ht.Add(-19289, "chuai"); ht.Add(-19288, "chuan"); ht.Add(-19281, "chuang");
                        ht.Add(-19275, "chui"); ht.Add(-19270, "chun"); ht.Add(-19263, "chuo");
                        ht.Add(-19261, "ci"); ht.Add(-19249, "cong"); ht.Add(-19243, "cou");
                        ht.Add(-19242, "cu"); ht.Add(-19238, "cuan"); ht.Add(-19235, "cui");
                        ht.Add(-19227, "cun"); ht.Add(-19224, "cuo"); ht.Add(-19218, "da");
                        ht.Add(-19212, "dai"); ht.Add(-19038, "dan"); ht.Add(-19023, "dang");
                        ht.Add(-19018, "dao"); ht.Add(-19006, "de"); ht.Add(-19003, "deng");
                        ht.Add(-18996, "di"); ht.Add(-18977, "dian"); ht.Add(-18961, "diao");
                        ht.Add(-18952, "die"); ht.Add(-18783, "ding"); ht.Add(-18774, "diu");
                        ht.Add(-18773, "dong"); ht.Add(-18763, "dou"); ht.Add(-18756, "du");
                        ht.Add(-18741, "duan"); ht.Add(-18735, "dui"); ht.Add(-18731, "dun");
                        ht.Add(-18722, "duo"); ht.Add(-18710, "e"); ht.Add(-18697, "en");
                        ht.Add(-18696, "er"); ht.Add(-18526, "fa"); ht.Add(-18518, "fan");
                        ht.Add(-18501, "fang"); ht.Add(-18490, "fei"); ht.Add(-18478, "fen");
                        ht.Add(-18463, "feng"); ht.Add(-18448, "fo"); ht.Add(-18447, "fou");
                        ht.Add(-18446, "fu"); ht.Add(-18239, "ga"); ht.Add(-18237, "gai");
                        ht.Add(-18231, "gan"); ht.Add(-18220, "gang"); ht.Add(-18211, "gao");
                        ht.Add(-18201, "ge"); ht.Add(-18184, "gei"); ht.Add(-18183, "gen");
                        ht.Add(-18181, "geng"); ht.Add(-18012, "gong"); ht.Add(-17997, "gou");
                        ht.Add(-17988, "gu"); ht.Add(-17970, "gua"); ht.Add(-17964, "guai");
                        ht.Add(-17961, "guan"); ht.Add(-17950, "guang"); ht.Add(-17947, "gui");
                        ht.Add(-17931, "gun"); ht.Add(-17928, "guo"); ht.Add(-17922, "ha");
                        ht.Add(-17759, "hai"); ht.Add(-17752, "han"); ht.Add(-17733, "hang");
                        ht.Add(-17730, "hao"); ht.Add(-17721, "he"); ht.Add(-17703, "hei");
                        ht.Add(-17701, "hen"); ht.Add(-17697, "heng"); ht.Add(-17692, "hong");
                        ht.Add(-17683, "hou"); ht.Add(-17676, "hu"); ht.Add(-17496, "hua");
                        ht.Add(-17487, "huai"); ht.Add(-17482, "huan"); ht.Add(-17468, "huang");
                        ht.Add(-17454, "hui"); ht.Add(-17433, "hun"); ht.Add(-17427, "huo");
                        ht.Add(-17417, "ji"); ht.Add(-17202, "jia"); ht.Add(-17185, "jian");
                        ht.Add(-16983, "jiang"); ht.Add(-16970, "jiao"); ht.Add(-16942, "jie");
                        ht.Add(-16915, "jin"); ht.Add(-16733, "jing"); ht.Add(-16708, "jiong");
                        ht.Add(-16706, "jiu"); ht.Add(-16689, "ju"); ht.Add(-16664, "juan");
                        ht.Add(-16657, "jue"); ht.Add(-16647, "jun"); ht.Add(-16474, "ka");
                        ht.Add(-16470, "kai"); ht.Add(-16465, "kan"); ht.Add(-16459, "kang");
                        ht.Add(-16452, "kao"); ht.Add(-16448, "ke"); ht.Add(-16433, "ken");
                        ht.Add(-16429, "keng"); ht.Add(-16427, "kong"); ht.Add(-16423, "kou");
                        ht.Add(-16419, "ku"); ht.Add(-16412, "kua"); ht.Add(-16407, "kuai");
                        ht.Add(-16403, "kuan"); ht.Add(-16401, "kuang"); ht.Add(-16393, "kui");
                        ht.Add(-16220, "kun"); ht.Add(-16216, "kuo"); ht.Add(-16212, "la");
                        ht.Add(-16205, "lai"); ht.Add(-16202, "lan"); ht.Add(-16187, "lang");
                        ht.Add(-16180, "lao"); ht.Add(-16171, "le"); ht.Add(-16169, "lei");
                        ht.Add(-16158, "leng"); ht.Add(-16155, "li"); ht.Add(-15959, "lia");
                        ht.Add(-15958, "lian"); ht.Add(-15944, "liang"); ht.Add(-15933, "liao");
                        ht.Add(-15920, "lie"); ht.Add(-15915, "lin"); ht.Add(-15903, "ling");
                        ht.Add(-15889, "liu"); ht.Add(-15878, "long"); ht.Add(-15707, "lou");
                        ht.Add(-15701, "lu"); ht.Add(-15681, "lv"); ht.Add(-15667, "luan");
                        ht.Add(-15661, "lue"); ht.Add(-15659, "lun"); ht.Add(-15652, "luo");
                        ht.Add(-15640, "ma"); ht.Add(-15631, "mai"); ht.Add(-15625, "man");
                        ht.Add(-15454, "mang"); ht.Add(-15448, "mao"); ht.Add(-15436, "me");
                        ht.Add(-15435, "mei"); ht.Add(-15419, "men"); ht.Add(-15416, "meng");
                        ht.Add(-15408, "mi"); ht.Add(-15394, "mian"); ht.Add(-15385, "miao");
                        ht.Add(-15377, "mie"); ht.Add(-15375, "min"); ht.Add(-15369, "ming");
                        ht.Add(-15363, "miu"); ht.Add(-15362, "mo"); ht.Add(-15183, "mou");
                        ht.Add(-15180, "mu"); ht.Add(-15165, "na"); ht.Add(-15158, "nai");
                        ht.Add(-15153, "nan"); ht.Add(-15150, "nang"); ht.Add(-15149, "nao");
                        ht.Add(-15144, "ne"); ht.Add(-15143, "nei"); ht.Add(-15141, "nen");
                        ht.Add(-15140, "neng"); ht.Add(-15139, "ni"); ht.Add(-15128, "nian");
                        ht.Add(-15121, "niang"); ht.Add(-15119, "niao"); ht.Add(-15117, "nie");
                        ht.Add(-15110, "nin"); ht.Add(-15109, "ning"); ht.Add(-14941, "niu");
                        ht.Add(-14937, "nong"); ht.Add(-14933, "nu"); ht.Add(-14930, "nv");
                        ht.Add(-14929, "nuan"); ht.Add(-14928, "nue"); ht.Add(-14926, "nuo");
                        ht.Add(-14922, "o"); ht.Add(-14921, "ou"); ht.Add(-14914, "pa");
                        ht.Add(-14908, "pai"); ht.Add(-14902, "pan"); ht.Add(-14894, "pang");
                        ht.Add(-14889, "pao"); ht.Add(-14882, "pei"); ht.Add(-14873, "pen");
                        ht.Add(-14871, "peng"); ht.Add(-14857, "pi"); ht.Add(-14678, "pian");
                        ht.Add(-14674, "piao"); ht.Add(-14670, "pie"); ht.Add(-14668, "pin");
                        ht.Add(-14663, "ping"); ht.Add(-14654, "po"); ht.Add(-14645, "pu");
                        ht.Add(-14630, "qi"); ht.Add(-14594, "qia"); ht.Add(-14429, "qian");
                        ht.Add(-14407, "qiang"); ht.Add(-14399, "qiao"); ht.Add(-14384, "qie");
                        ht.Add(-14379, "qin"); ht.Add(-14368, "qing"); ht.Add(-14355, "qiong");
                        ht.Add(-14353, "qiu"); ht.Add(-14345, "qu"); ht.Add(-14170, "quan");
                        ht.Add(-14159, "que"); ht.Add(-14151, "qun"); ht.Add(-14149, "ran");
                        ht.Add(-14145, "rang"); ht.Add(-14140, "rao"); ht.Add(-14137, "re");
                        ht.Add(-14135, "ren"); ht.Add(-14125, "reng"); ht.Add(-14123, "ri");
                        ht.Add(-14122, "rong"); ht.Add(-14112, "rou"); ht.Add(-14109, "ru");
                        ht.Add(-14099, "ruan"); ht.Add(-14097, "rui"); ht.Add(-14094, "run");
                        ht.Add(-14092, "ruo"); ht.Add(-14090, "sa"); ht.Add(-14087, "sai");
                        ht.Add(-14083, "san"); ht.Add(-13917, "sang"); ht.Add(-13914, "sao");
                        ht.Add(-13910, "se"); ht.Add(-13907, "sen"); ht.Add(-13906, "seng");
                        ht.Add(-13905, "sha"); ht.Add(-13896, "shai"); ht.Add(-13894, "shan");
                        ht.Add(-13878, "shang"); ht.Add(-13870, "shao"); ht.Add(-13859, "she");
                        ht.Add(-13847, "shen"); ht.Add(-13831, "sheng"); ht.Add(-13658, "shi");
                        ht.Add(-13611, "shou"); ht.Add(-13601, "shu"); ht.Add(-13406, "shua");
                        ht.Add(-13404, "shuai"); ht.Add(-13400, "shuan"); ht.Add(-13398, "shuang");
                        ht.Add(-13395, "shui"); ht.Add(-13391, "shun"); ht.Add(-13387, "shuo");
                        ht.Add(-13383, "si"); ht.Add(-13367, "song"); ht.Add(-13359, "sou");
                        ht.Add(-13356, "su"); ht.Add(-13343, "suan"); ht.Add(-13340, "sui");
                        ht.Add(-13329, "sun"); ht.Add(-13326, "suo"); ht.Add(-13318, "ta");
                        ht.Add(-13147, "tai"); ht.Add(-13138, "tan"); ht.Add(-13120, "tang");
                        ht.Add(-13107, "tao"); ht.Add(-13096, "te"); ht.Add(-13095, "teng");
                        ht.Add(-13091, "ti"); ht.Add(-13076, "tian"); ht.Add(-13068, "tiao");
                        ht.Add(-13063, "tie"); ht.Add(-13060, "ting"); ht.Add(-12888, "tong");
                        ht.Add(-12875, "tou"); ht.Add(-12871, "tu"); ht.Add(-12860, "tuan");
                        ht.Add(-12858, "tui"); ht.Add(-12852, "tun"); ht.Add(-12849, "tuo");
                        ht.Add(-12838, "wa"); ht.Add(-12831, "wai"); ht.Add(-12829, "wan");
                        ht.Add(-12812, "wang"); ht.Add(-12802, "wei"); ht.Add(-12607, "wen");
                        ht.Add(-12597, "weng"); ht.Add(-12594, "wo"); ht.Add(-12585, "wu");
                        ht.Add(-12556, "xi"); ht.Add(-12359, "xia"); ht.Add(-12346, "xian");
                        ht.Add(-12320, "xiang"); ht.Add(-12300, "xiao"); ht.Add(-12120, "xie");
                        ht.Add(-12099, "xin"); ht.Add(-12089, "xing"); ht.Add(-12074, "xiong");
                        ht.Add(-12067, "xiu"); ht.Add(-12058, "xu"); ht.Add(-12039, "xuan");
                        ht.Add(-11867, "xue"); ht.Add(-11861, "xun"); ht.Add(-11847, "ya");
                        ht.Add(-11831, "yan"); ht.Add(-11798, "yang"); ht.Add(-11781, "yao");
                        ht.Add(-11604, "ye"); ht.Add(-11589, "yi"); ht.Add(-11536, "yin");
                        ht.Add(-11358, "ying"); ht.Add(-11340, "yo"); ht.Add(-11339, "yong");
                        ht.Add(-11324, "you"); ht.Add(-11303, "yu"); ht.Add(-11097, "yuan");
                        ht.Add(-11077, "yue"); ht.Add(-11067, "yun"); ht.Add(-11055, "za");
                        ht.Add(-11052, "zai"); ht.Add(-11045, "zan"); ht.Add(-11041, "zang");
                        ht.Add(-11038, "zao"); ht.Add(-11024, "ze"); ht.Add(-11020, "zei");
                        ht.Add(-11019, "zen"); ht.Add(-11018, "zeng"); ht.Add(-11014, "zha");
                        ht.Add(-10838, "zhai"); ht.Add(-10832, "zhan"); ht.Add(-10815, "zhang");
                        ht.Add(-10800, "zhao"); ht.Add(-10790, "zhe"); ht.Add(-10780, "zhen");
                        ht.Add(-10764, "zheng"); ht.Add(-10587, "zhi"); ht.Add(-10544, "zhong");
                        ht.Add(-10533, "zhou"); ht.Add(-10519, "zhu"); ht.Add(-10331, "zhua");
                        ht.Add(-10329, "zhuai"); ht.Add(-10328, "zhuan"); ht.Add(-10322, "zhuang");
                        ht.Add(-10315, "zhui"); ht.Add(-10309, "zhun"); ht.Add(-10307, "zhuo");
                        ht.Add(-10296, "zi"); ht.Add(-10281, "zong"); ht.Add(-10274, "zou");
                        ht.Add(-10270, "zu"); ht.Add(-10262, "zuan"); ht.Add(-10260, "zui");
                        ht.Add(-10256, "zun"); ht.Add(-10254, "zuo"); ht.Add(-10247, "zz");
                    }
                    return ht;
                }
            }
            static string g(int num)
            {
                if (num < -20319 || num > -10247)
                    return "";
                while (!Ht.ContainsKey(num))
                    num--;
                return Ht[num].ToString();
            }
            static bool In(int Lp, int Hp, int Value)
            {
                return ((Value <= Hp) && (Value >= Lp));
            }
            #endregion
            /// <summary> 
            /// 获取汉字拼音，特殊字符去掉，英文不做处理 
            /// </summary> 
            /// <param name="hz"></param> 
            /// <returns></returns> 
            public static string GetPinYin(string hz)
            {
                byte[] b = System.Text.Encoding.Default.GetBytes(hz);
                int p;
                StringBuilder ret = new StringBuilder();
                for (int i = 0; i < b.Length; i++)
                {
                    p = (int)b[i];
                    if (p > 160)
                    {
                        p = p * 256 + b[++i] - 65536;
                        ret.Append(g(p));
                    }
                    else
                    {
                        ret.Append((char)p);
                    }
                }
                return ret.ToString();
            }

            /// <summary> 
            /// 获取汉字拼音的首字母
            /// </summary> 
            /// <param name="str"></param> 
            /// <returns></returns> 
            public static string GetPinYinIndex(string str)
            {
                StringBuilder ret = new StringBuilder();
                for (int i = 0; i < str.Length; i++)
                {
                    double tmp = (double)str[i];
                    if (tmp >= 0x4e00 && tmp < 0x9fa5)
                    {
                        ret.Append(Convert(str[i]));
                    }

                }
                return ret.ToString();
            }
            /// <summary> 
            /// 获取一个汉字的拼音声母 
            /// </summary> 
            /// <param name="chinese">Unicode格式的一个汉字</param> 
            /// <returns>汉字的声母</returns> 
            public static char Convert(Char chinese)
            {
                Encoding gb2312 = Encoding.GetEncoding("GB2312");
                Encoding unicode = Encoding.Unicode;

                // Convert the string into a byte[]. 
                byte[] unicodeBytes = unicode.GetBytes(new Char[] { chinese });
                // Perform the conversion from one encoding to the other. 
                byte[] asciiBytes = Encoding.Convert(unicode, gb2312, unicodeBytes);

                // 计算该汉字的GB-2312编码 
                int n = (int)asciiBytes[0] << 8;
                n += (int)asciiBytes[1];

                // 根据汉字区域码获取拼音声母 
                if (In(0xB0A1, 0xB0C4, n)) return 'A';
                if (In(0xB0C5, 0xB2C0, n)) return 'B';
                if (In(0xB2C1, 0xB4ED, n)) return 'C';
                if (In(0xB4EE, 0xB6E9, n)) return 'D';
                if (In(0xB6EA, 0xB7A1, n)) return 'E';
                if (In(0xB7A2, 0xB8c0, n)) return 'F';
                if (In(0xB8C1, 0xB9FD, n)) return 'G';
                if (In(0xB9FE, 0xBBF6, n)) return 'H';
                if (In(0xBBF7, 0xBFA5, n)) return 'J';
                if (In(0xBFA6, 0xC0AB, n)) return 'K';
                if (In(0xC0AC, 0xC2E7, n)) return 'L';
                if (In(0xC2E8, 0xC4C2, n)) return 'M';
                if (In(0xC4C3, 0xC5B5, n)) return 'N';
                if (In(0xC5B6, 0xC5BD, n)) return 'O';
                if (In(0xC5BE, 0xC6D9, n)) return 'P';
                if (In(0xC6DA, 0xC8BA, n)) return 'Q';
                if (In(0xC8BB, 0xC8F5, n)) return 'R';
                if (In(0xC8F6, 0xCBF0, n)) return 'S';
                if (In(0xCBFA, 0xCDD9, n)) return 'T';
                if (In(0xCDDA, 0xCEF3, n)) return 'W';
                if (In(0xCEF4, 0xD188, n)) return 'X';
                if (In(0xD1B9, 0xD4D0, n)) return 'Y';
                if (In(0xD4D1, 0xD7F9, n)) return 'Z';
                return '\0';
            }
        }
        #endregion

        #region 操作XML
        private static XmlDocument xmlDoc = new XmlDocument();

        /// <summary>
        /// 查询XML
        /// </summary>
        /// <param name="key">查找标记</param>
        /// <returns>XML值</returns>
        //public static String xml_select(String key)
        //{
        //    try
        //    {
        //        xmldoc.Load("config.xml");
        //        XmlNode root = xmldoc.SelectSingleNode("info");
        //        XmlNodeList Li = root.ChildNodes;
        //        String value = "";
        //        foreach (XmlNode item in Li)
        //        {
        //            XmlElement eL = (XmlElement)item;
        //            if (key == eL.Name)
        //            {
        //                value = eL.InnerText;
        //                break;
        //            }
        //        }
        //        return cfs.AESDecrypt(value);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        return "";
        //        throw new Exception("XML操作出错");
        //    }
        //}
        ///// <summary>
        ///// 修改XML
        ///// </summary>
        ///// <param name="key">键</param>
        ///// <param name="value">值</param>
        //public static void xml_update(String key, String value)
        //{
        //    try
        //    {
        //        xmldoc.Load("config.xml");
        //        XmlNode root = xmldoc.SelectSingleNode("info");
        //        XmlNodeList Li = root.ChildNodes;
        //        foreach (XmlNode item in Li)
        //        {
        //            XmlElement eL = (XmlElement)item;
        //            if (key == eL.Name)
        //            {
        //                //Console.WriteLine("111111");
        //                eL.InnerText = cfs.AESEncrypt(value);
        //                xmldoc.Save("config.xml");
        //                break;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw new Exception("XML操作出错");
        //    }
        //}


        /// <summary>
        /// Config.xml插入结点
        /// </summary>
        /// <param name="ID">software的ID属性</param>
        /// <param name="name">software的name属性</param>
        /// <param name="server">software的server元素值</param>
        /// <param name="databaseName">software的databaseName元素值</param>
        /// <param name="userId">software的userId元素值</param>
        /// <param name="password">software的password元素值</param>
        public static void XmlNodeInsert_Config(string ID, string name, string server, string databaseName, string userId, string password, string flag)
        {
            string _ID = ClassForSercet.AESEncrypt(ID);
            string _name = ClassForSercet.AESEncrypt(name);
            string _server = ClassForSercet.AESEncrypt(server);
            string _databaseName = ClassForSercet.AESEncrypt(databaseName);
            string _userId = ClassForSercet.AESEncrypt(userId);
            string _password = ClassForSercet.AESEncrypt(password);
            string _flag = ClassForSercet.AESEncrypt(flag);

            xmlDoc.Load("config.xml");
            XmlNode root = xmlDoc.SelectSingleNode("system");//查找<software>
            XmlElement xe1 = xmlDoc.CreateElement("software");//创建一个<software>节点
            xe1.SetAttribute("ID", _ID);//设置该节点ID属性
            xe1.SetAttribute("name", _name);//设置该节点name属性
            xe1.SetAttribute("flag", _flag);//设置该节点flag属性

            XmlElement xesub1 = xmlDoc.CreateElement("server");
            xesub1.InnerText = _server;//设置文本节点
            xe1.AppendChild(xesub1);//添加到<software>节点中

            XmlElement xesub2 = xmlDoc.CreateElement("databaseName");
            xesub2.InnerText = _databaseName;
            xe1.AppendChild(xesub2);

            XmlElement xesub3 = xmlDoc.CreateElement("userId");
            xesub3.InnerText = _userId;
            xe1.AppendChild(xesub3);

            XmlElement xesub4 = xmlDoc.CreateElement("password");
            xesub4.InnerText = _password;
            xe1.AppendChild(xesub4);

            root.AppendChild(xe1);//添加到<system>节点中
            xmlDoc.Save("config.xml");
        }

        /// <summary>
        /// Config.xml修改结点
        /// </summary>
        /// <param name="ID">software的ID属性</param>
        /// <param name="name">software的name属性</param>
        /// <param name="server">software的server元素值</param>
        /// <param name="databaseName">software的databaseName元素值</param>
        /// <param name="userId">software的userId元素值</param>
        /// <param name="password">software的password元素值</param>
        /// <param name="password">software的flag属性</param>
        public static void XmlNodeUpdate(string ID, string name, string server, string databaseName, string userId, string password, string flag)
        {
            string _ID = ClassForSercet.AESEncrypt(ID);
            string _name = ClassForSercet.AESEncrypt(name);
            string _server = ClassForSercet.AESEncrypt(server);
            string _databaseName = ClassForSercet.AESEncrypt(databaseName);
            string _userId = ClassForSercet.AESEncrypt(userId);
            string _password = ClassForSercet.AESEncrypt(password);
            string _flag = ClassForSercet.AESEncrypt(flag);
            xmlDoc.Load("config.xml");
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("system").ChildNodes;//获取software节点的所有子节点
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                if (xe.GetAttribute("ID") == _ID)//如果ID属性值为“_ID”
                {
                    xe.SetAttribute("name", _name);//则修改该属性为“update李赞红”
                    xe.SetAttribute("flag", _flag);//则修改该属性为“update李赞红”
                    XmlNodeList nls = xe.ChildNodes;//继续获取xe子节点的所有子节点
                    nls[0].InnerText = _server;
                    nls[1].InnerText = _databaseName;
                    nls[2].InnerText = _userId;
                    nls[3].InnerText = _password;
                }
            }
            xmlDoc.Save("config.xml");
        }

        /// <summary>
        /// Config.xml删除结点
        /// </summary>
        /// <param name="ID">software的ID属性</param>

        public static void XmlNodeDelete(string ID)
        {
            xmlDoc.Load("config.xml");
            XmlNodeList xnl = xmlDoc.SelectSingleNode("system").ChildNodes;
            string _ID = ClassForSercet.AESEncrypt(ID);
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;

                if (xe.GetAttribute("ID") == _ID)
                {
                    xe.RemoveAll();//删除该节点的全部内容
                }
            }
            xmlDoc.Save("config.xml");

        }

        /// <summary>
        /// Config.xml查询结点
        /// 
        /// </summary>
        /// <param name="ID">software的ID属性</param>
        public static List<string> XmlNodeView(string ID)
        {
            try
            {
                xmlDoc.Load("config.xml");
                XmlNode xn = xmlDoc.SelectSingleNode("system");
                string _ID = ClassForSercet.AESEncrypt(ID);
                XmlNodeList xnl = xn.ChildNodes;
                List<string> List = new List<string>();
                foreach (XmlNode xnf in xnl)
                {

                    XmlElement xe = (XmlElement)xnf;
                    if (xe.GetAttribute("ID") == _ID)
                    {
                        List.Add(ClassForSercet.AESDecrypt(xe.GetAttribute("ID")));//显示属性值
                        List.Add(ClassForSercet.AESDecrypt(xe.GetAttribute("name")));
                        List.Add(ClassForSercet.AESDecrypt(xe.GetAttribute("flag")));
                        XmlNodeList xnf1 = xe.ChildNodes;
                        foreach (XmlNode xn2 in xnf1)
                        {
                            List.Add(ClassForSercet.AESDecrypt(xn2.InnerText));//显示子节点点文本
                        }
                    }
                }
                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 查询条件生成
        /// <summary>
        /// 查询条件生成
        /// </summary>
        /// <param name="cbbcolumn">列名</param>
        /// <param name="cbbtj">符号</param>
        /// <param name="tb">值</param>
        /// <returns>查询条件</returns>
        public static string Tj(ComboBox column, ComboBox cbbtj, TextBox tb)
        {
            if (tb.Text == "")
            {
                return "";
            }
            if (cbbtj.Text == "等于")
            {
                return column.SelectedValue + " = '" + tb.Text + "'";
            }
            if (cbbtj.Text == "相似于")
            {
                return column.SelectedValue + " LIKE '" + tb.Text + "%'";
            }
            if (cbbtj.Text == "大于")
            {
                return column.SelectedValue + " > '" + tb.Text + "'";
            }
            if (cbbtj.Text == "小于")
            {
                return column.SelectedValue + " < '" + tb.Text + "'";
            }
            if (cbbtj.Text == "不等于")
            {
                return column.SelectedValue + " != '" + tb.Text + "'";
            }
            if (cbbtj.Text == "不相似于")
            {
                return column.SelectedValue + " NOT LIKE '" + tb.Text + "%'";
            }
            return "";
        }
        #endregion

        #region 调用EXCEL打印
        /// <summary>
        /// 调用EXCEL打印
        /// 例：ClassCustom.PrintF(ClassCustom.ExportDataGridview1(this.dataGridView1), Excel.XlPaperSize.xlPaperA4, Excel.XlPageOrientation.xlLandscape);
        /// </summary>
        /// <param name="excel">EXCEL对象</param>
        /// <param name="papersize">纸张 A4 A3....</param>
        /// <param name="orientation">纸张方向</param>
        public static void PrintE(Excel.Application excel, DataGridView dgv, Excel.XlPaperSize papersize, Excel.XlPageOrientation orientation)
        {
            Excel.Worksheet sheet1 = excel.Worksheets[1] as Excel.Worksheet;
            sheet1.PageSetup.PaperSize = papersize;
            sheet1.PageSetup.Orientation = orientation;
            sheet1.Name = "打印预览";
            excel.get_Range("A1", excel.Cells[dgv.Rows.Count + 1, dgv.Columns.Count + 1]).EntireColumn.AutoFit();
            DrawExcelBorders(excel, "A1", excel.Cells[dgv.Rows.Count + 3, dgv.Columns.Count - 1]);
            sheet1.PageSetup.PrintTitleRows = "$1:$1";
            (excel.Workbooks[1].Worksheets[1] as Excel.Worksheet).PrintPreview(true);
            excel.Workbooks[1].Close(false, null, null);
            excel.Quit();
            excel = null;
        }
        /// <summary>
        /// EXCEL画线
        /// </summary>
        /// <param name="excel"></param>
        public static void DrawExcelBorders(Excel.Application excel, object cell1, object cell2)
        {
            Excel.Range range = excel.get_Range(cell1, cell2);
            //range.Select();
            Excel.Borders borders = range.Borders;
            borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

            borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlInsideHorizontal].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlEdgeTop].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

            borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
        }
        #endregion

        #region 金额大写转换
        public static string UpMoney(decimal D_Mstr_theMoney)      //支票金额转换
        {
            string G_str_Money = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字     
            string G_str_MoneyString = "万仟佰拾亿仟佰拾万仟佰拾元角分";   //数字位所对应的汉字     
            string G_str_Timoney = "";   //从原D_Mstr_theMoney值中取出的值     
            string G_str_NumberString = "";  //数字的字符串形式     
            string G_str_UpMoney = ""; //人民币大写金额形式     
            int i;         //循环变量     
            int j;         //D_Mstr_theMoney的值乘以100的字符串长度     
            string G_ch_Chine = "";  //数字的汉语读法     
            string G_ch_Chineses = ""; //数字位的汉字读法     
            int G_int_ZeroCount = 0;//用来计算连续的零值是几个     
            int G_int_G_int_temp; //从原D_Mstr_theMoney值中取出的值     
            D_Mstr_theMoney = Math.Round(Math.Abs(D_Mstr_theMoney), 2);//将D_Mstr_theMoney取绝对值并四舍五入取2位小数     
            G_str_NumberString = ((long)(D_Mstr_theMoney * 100)).ToString(); //将D_Mstr_theMoney乘100并转换成字符串形式     
            j = G_str_NumberString.Length;//找出最高位     
            if (j > 15) { return "溢出"; }
            G_str_MoneyString = G_str_MoneyString.Substring(15 - j); //取出对应位数的G_str_MoneyString的值。如：200.55,j为5所以G_str_MoneyString=佰拾元角分     

            for (i = 0; i < j; i++)
            {//循环取出每一位需要转换的值    
                G_str_Timoney = G_str_NumberString.Substring(i, 1); //取出需转换的某一位的值     
                G_int_G_int_temp = Convert.ToInt32(G_str_Timoney); //转换为数字     
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时     
                    if (G_str_Timoney == "0")
                    {
                        G_ch_Chine = "";
                        G_ch_Chineses = "";
                        G_int_ZeroCount = G_int_ZeroCount + 1;
                    }
                    else
                    {
                        if (G_str_Timoney != "0" && G_int_ZeroCount != 0)
                        {
                            G_ch_Chine = "零" + G_str_Money.Substring(G_int_G_int_temp * 1, 1);
                            G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                            G_int_ZeroCount = 0;
                        }
                        else
                        {
                            G_ch_Chine = G_str_Money.Substring(G_int_G_int_temp * 1, 1);
                            G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                            G_int_ZeroCount = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位     
                    if (G_str_Timoney != "0" && G_int_ZeroCount != 0)
                    {
                        G_ch_Chine = "零" + G_str_Money.Substring(G_int_G_int_temp * 1, 1);
                        G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                        G_int_ZeroCount = 0;
                    }
                    else
                    {
                        if (G_str_Timoney != "0" && G_int_ZeroCount == 0)
                        {
                            G_ch_Chine = G_str_Money.Substring(G_int_G_int_temp * 1, 1);
                            G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                            G_int_ZeroCount = 0;
                        }
                        else
                        {
                            if (G_str_Timoney == "0" && G_int_ZeroCount >= 3)
                            {
                                G_ch_Chine = "";
                                G_ch_Chineses = "";
                                G_int_ZeroCount = G_int_ZeroCount + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    G_ch_Chine = "";
                                    G_int_ZeroCount = G_int_ZeroCount + 1;
                                }
                                else
                                {
                                    G_ch_Chine = "";
                                    G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                                    G_int_ZeroCount = G_int_ZeroCount + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上     
                    G_ch_Chineses = G_str_MoneyString.Substring(i, 1);
                }
                G_str_UpMoney = G_str_UpMoney + G_ch_Chine + G_ch_Chineses;

                if (i == j - 1 && G_str_Timoney == "0")
                {
                    //最后一位（分）为0时，加上“整”     
                    G_str_UpMoney = G_str_UpMoney + '整';
                }
            }
            if (D_Mstr_theMoney == 0)
            {
                G_str_UpMoney = "零元整";
            }
            return G_str_UpMoney;
        }
        #endregion

        #region EVAL
        /// <summary>
        /// EVAL函数
        /// 例：string code = "OleDbConnection conn;\n" +
        ///                 "string constrA = \"Data Source=.;Initial Catalog=ContractM_C;Provider=SQLOLEDB;Persist Security Info=True;User ID=sa\";\n" +
        ///                 "conn = new OleDbConnection(constrA);\n" +
        ///                 "conn.Open();\n" +
        ///                 "OleDbDataAdapter oledb = new OleDbDataAdapter(\"SELECT * FROM 合同基本信息\", conn);\n" +
        ///                 "DataSet ds1 = new DataSet();\n" +
        ///                 "oledb.Fill(ds1, \"VIEW\");\n" +
        ///                 "conn.Close();\n" +
        ///                 "return ds1;\n";
        ///     this.dataGridView1.DataSource = (ClassCustom.EVAL(code, "VIEW", typeof(DataSet)) as DataSet).Tables[0];
        /// </summary>
        /// <param name="code">要执行的代码</param>
        /// <param name="paras">方法参数 形参 c_object</param>
        /// <param name="type">方法返回值类型</param>
        /// <returns>返回值</returns>
        public static object EVAL(string code, object paras, Type type)
        {
            try
            {
                EvaluatorItem[] items = { 
                                new EvaluatorItem(type/*方法返回值类型*/,code/*要执行的字符串代码*/, "EVAL"/*方法名称*/)
                        };
                Evaluator eval = new Evaluator(items);
                //object obj = (object)s;/*方法需要的参数,在方法里面的 形参为(c_object) */
                return eval.Evaluate("EVAL"/*要调用的方法名*/, paras/*实参*/);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取本机IP
        public static string GetIP()   //获取本地IP 
        {
            string hostName = Dns.GetHostName();
            IPHostEntry iphe = Dns.GetHostEntry(hostName);
            IPAddress[] addressList = iphe.AddressList;
            string value = "";
            foreach (IPAddress ip in addressList)
            {
                if (ip.ToString().StartsWith("192.168.1"))
                {
                    value = ip.ToString();
                    break;
                }
            }
            return value;

        }
        #endregion
    }
}
