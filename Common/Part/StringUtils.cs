using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Part
{
    /// <summary>
    /// 字符串辅助类
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 取哈希值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetHashCode(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return str.GetHashCode().ToString("x", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 是否忽略大小写相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 是否忽略大小写相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="index1"></param>
        /// <param name="s2"></param>
        /// <param name="index2"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(string s1, int index1, string s2, int index2, int length)
        {
            return (string.Compare(s1, index1, s2, index2, length, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// 是否忽略大小从子串开始
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(string s1, string s2)
        {
            //if (s2 == null)
            //{
            //    return false;
            //}
            //return (0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase));
            return s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase);
        }

        //public static bool EndsWith(string s, char c)
        //{
        //    int length = s.Length;
        //    if (length != 0)
        //    {
        //        return (s[length - 1] == c);
        //    }
        //    return false;
        //}

        /// <summary>
        /// 忽略大小写结束于子串
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(string s1, string s2)
        {
            //int indexA = s1.Length - s2.Length;
            //if (indexA < 0)
            //{
            //    return false;
            //}
            //return (0 == string.Compare(s1, indexA, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase));
            return s1.EndsWith(s2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string Repeat(string str, int times)
        {
            StringBuilder builder = new StringBuilder(str.Length * times);
            for (int i = 0; i < times; i++)
            {
                builder.Append(str);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 快速文本替换，替换第一个
        /// </summary>
        /// <param name="template">待替换文本</param>
        /// <param name="placeholder">替换内容</param>
        /// <param name="replacement">替换为</param>
        /// <param name="comparisonType">比较方式</param>
        /// <returns></returns>
        public static string ReplaceFirst(string template, string placeholder, string replacement,
            StringComparison comparisonType)
        {
            // 代码源自 NHibernate 的 StringHelper.cs
            // sometimes a null value will get passed in here -> SqlWhereStrings are a good example
            if (template == null)
            {
                return null;
            }

            int pos = template.IndexOf(placeholder, comparisonType);
            if (pos < 0)
            {
                return template;
            }
            else
            {
                return new StringBuilder(template.Substring(0, pos))
                    .Append(replacement)
                    .Append(template.Substring(pos + placeholder.Length)).ToString();
            }
        }

        /// <summary>
        /// 快速文本替换，全部替换
        /// </summary>
        /// <param name="template">待替换文本</param>
        /// <param name="placeholder">替换内容</param>
        /// <param name="replacement">替换为</param>
        /// <param name="comparisonType">比较方式</param>
        /// <returns></returns>
        public static string Replace(string template, string placeholder, string replacement,
            StringComparison comparisonType)
        {
            // 代码源自 NHibernate 的 StringHelper.cs
            // sometimes a null value will get passed in here -> SqlWhereStrings are a good example
            if (template == null)
            {
                return null;
            }

            int pos = template.IndexOf(placeholder, comparisonType);
            if (pos < 0)
            {
                return template;
            }
            else
            {
                return new StringBuilder(template.Substring(0, pos))
                    .Append(replacement)
                    .Append(Replace(template.Substring(pos + placeholder.Length),
                        placeholder, replacement, comparisonType)).ToString();
            }
        }

        //public static string[] ObjectArrayToStringArray(object[] objectArray)
        //{
        //    string[] array = new string[objectArray.Length];
        //    objectArray.CopyTo(array, 0);
        //    return array;
        //}

        /// <summary>
        /// 字节数组转为十六进制字符串
        /// </summary>
        public static string ByteArrayToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字节数组转为十六进制字符串
        /// </summary>
        public static byte[] HexToByteArray(string hex)
        {
            int count = hex.Length / 2;
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = Byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return result;
        }

        /// <summary>
        /// 取字符串的字节长度，1个汉字算两个字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetByteLength(string str)
        {
            if (str == null) return 0;
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                count += (int)ch < 128 ? 1 : 2;
            }
            return count;
        }

        /// <summary>
        /// 按字节长度取字串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string SubstringByByte(string str, int len)
        {
            int newLen;
            return SubstringByByte(str, len, out newLen);
        }

        /// <summary>
        /// 按字节长度取字串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="newLen"></param>
        /// <returns></returns>
        public static string SubstringByByte(string str, int len, out int newLen)
        {
            string answer = string.Empty;
            newLen = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                int chLen = (int)ch < 128 ? 1 : 2;
                if (newLen + chLen > len) break;
                answer += ch;
                newLen += chLen;
            }
            return answer;
        }

        /// <summary>
        /// 按字节取字符串中部分内容
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="index">起始索引，从0开始</param>
        /// <param name="length">子串字节长度</param>
        /// <returns>子串</returns>
        public static string SubstringByByte(string str, int index, int length)
        {
            string sb = string.Empty;
            int byteIndex = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                int chLen = (int)ch < 128 ? 1 : 2;

                if (byteIndex >= index && byteIndex < index + length)
                {
                    sb += ch;
                }
                byteIndex += chLen;
            }
            return sb;
        }

        /// <summary>
        /// 返回str的MD5码，长度为32字符；str为空时返回空字符串
        /// </summary>
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return ByteArrayToHex(bytes);
        }

        /// <summary>
        /// 加密数据并返回加密后的字节数组
        /// </summary>
        public static byte[] EncryptToByteArray(string str)
        {
            byte[] s = new UTF8Encoding().GetBytes(str);
            MemoryStream ms = new MemoryStream();
            try
            {
                CryptoStream cs = new CryptoStream(ms,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key(), Iv()),
                    CryptoStreamMode.Write);
                try
                {
                    cs.Write(s, 0, s.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
                finally
                {
                    cs.Dispose();
                }
            }
            finally
            {
                ms.Dispose();
            }
        }

        /// <summary>
        /// 加密数据并返回加密后的十六进制编码字符串；str为则空则返回string.Empty
        ///   加密结果长度计算公式：(str.Length / 8 + 1) * 16 （1=16、8=32、16=48、24=64、32=80、40=96、48=112、56=128、64=144）
        /// </summary>
        public static string Encrypt(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty :
                ByteArrayToHex(EncryptToByteArray(str));
        }

        /// <summary>
        /// 从字节数组解密数据并返回字符串
        /// </summary>
        public static string DecryptFromByteArray(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            try
            {
                CryptoStream cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(Key(), Iv()), CryptoStreamMode.Read);
                try
                {
                    byte[] s = new byte[data.Length];
                    int len = cs.Read(s, 0, s.Length);
                    return new UTF8Encoding().GetString(s, 0, len);
                }
                finally
                {
                    cs.Dispose();
                }
            }
            finally
            {
                ms.Dispose();
            }
        }

        /// <summary>
        /// 从十六进制字符串解密数据并返回字符串；hex为则空则返回string.Empty
        /// </summary>
        public static string Decrypt(string hex)
        {
            return string.IsNullOrEmpty(hex) ? string.Empty :
                DecryptFromByteArray(HexToByteArray(hex));
        }

        private static byte[] Key()
        {
            return new byte[24]
            {
                123, 97,  177, 109,  240, 131, 164, 237,
                87,  108, 118, 214,  230, 181, 23,  246,
                80,  103, 217, 52,   37,  149, 47,  208
            };
        }

        private static byte[] Iv()
        {
            return new byte[8]
            {
                96, 55, 83, 220, 97, 81, 172, 170
            };
        }

        /// <summary>
        /// Trim数组中的所有字符串并返回
        /// </summary>
        public static string[] TrimAll(string[] array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i].Trim();
            }
            return result;
        }

        private static readonly char[] cLowChineseChars =
        {
             '啊','芭','擦','搭','蛾','发','噶','哈','击','喀',
             '垃','妈','拿','欧','啪','期','然','撒','塌','挖',
             '昔','压','匝','雲'
        };
        private static readonly char[] cLowPinyins =
        {
             'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k',
             'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'w',
             'x', 'y', 'z', 'y'
        };
        private static readonly string[] cHighPinyins =
        {
            /*216*/"cjwgnspgcgnesypb" + "tyyzdxykygtdjnmj" + "qmbsgzscyjsyyzpg" +
                   "kbzgycywykgkljsw" + "kpjqhyzwddzlsgmr" + "ypywwcckznkydg",
            /*217*/"ttnjjeykkzytcjnm" + "cylqlypyqfqrpzsl" + "wbtgkjfyxjwzltbn" +
                   "cxjjjjzxdttsqzyc" + "dxxhgckbphffssyy" + "bgmxlpbylllhlx",
            /*218*/"spzmyjhsojnghdzq" + "yklgjhxgqzhxqgke" + "zzwyscscjxyeyxad" +
                   "zpmdssmzjzqjyzcd" + "jewqjbdzbxgznzcp" + "whkxhqkmwfbpby",
            /*219*/"dtjzzkqhylygxfpt" + "yjyyzpszlfchmqsh" + "gmxxsxjjsdcsbbqb" +
                   "efsjyhxwgzkpylqb" + "gldlcctnmayddkss" + "ngycsgxlyzaybn",
            /*220*/"ptsdkdylhgymylcx" + "pycjndqjwxqxfyyf" + "jlejbzrxccqwqqsb" +
                   "zkymgplbmjrqcfln" + "ymyqmsqyrbcjthzt" + "qfrxqhxmjjcjlx",
            /*221*/"qgjmshzkbswyemyl" + "txfsydsglycjqxsj" + "nqbsctyhbftdcyzd" +
                   "jwyghqfrxwckqkxe" + "bptlpxjzsrmebwhj" + "lbjslyysmdxlcl",
            /*222*/"qkxlhxjrzjmfqhxh" + "wywsbhtrxxglhqhf" + "nmcykldyxzpwlggs" +
                   "mtcfpajjzyljtyan" + "jgbjplqgdzyqyaxb" + "kysecjsznslyzh",
            /*223*/"zxlzcghpxzhznytd" + "sbcjkdlzayfmydle" + "bbgqyzkxgldndnys" +
                   "kjshdlyxbcghxypk" + "dqmmzngmmclgwzsz" + "xzjfznmlzzthcs",
            /*224*/"ydbdllscddnlkjyk" + "jsycjlkohqasdknh" + "csganhdaashtcplc" +
                   "pqybsdmpjlpcjoql" + "cdhjjysprchnknnl" + "hlyyqyhwzptczg",
            /*225*/"wwmzffjqqqqyxacl" + "bhkdjxdgmmydjxzl" + "lsygxgkjrywzwycl" +
                   "zmssjzldbydcpcxy" + "hlxchyzjqsqqagmn" + "yxpfrkssbjlyxy",
            /*226*/"syglnscmhcwwmnzj" + "jlxxhchsyd ctxry" + "cyxbyhcsmxjsznpw" +
                   "gpxxtaybgajcxlys" + "dccwzocwkccsbnhc" + "pdyznfcyytyckx",
            /*227*/"kybsqkkytqqxfcwc" + "hcykelzqbsqyjqcc" + "lmthsywhmktlkjly" +
                   "cxwheqqhtqhzpqsq" + "scfymmdmgbwhwlgs" + "llysdlmlxpthmj",
            /*228*/"hwljzyhzjxhtxjlh" + "xrswlwzjcbxmhzqx" + "sdzpmgfcsglsxymj" +
                   "shxpjxwmyqksmypl" + "rthbxftpmhyxlchl" + "hlzylxgsssstcl",
            /*229*/"sldclrpbhzhxyyfh" + "bbgdmycnqqwlqhjj" + "zywjzyejjdhpblqx" +
                   "tqkwhlchqxagtlxl" + "jxmslxhtzkzjecxj" + "cjnmfbycsfywyb",
            /*230*/"jzgnysdzsqyrsljp" + "clpwxsdwejbjcbcn" + "aytwgmpabclyqpcl" +
                   "zxsbnmsggfnzjjbz" + "sfzyndxhplqkzczw" + "alsbccjxjyzhwk",
            /*231*/"ypsgxfzfcdkhjgxd" + "lqfsgdslqwzkxtmh" + "sbgzmjzrglyjbpml" +
                   "msxlzjqqhzsjczyd" + "jwbmjklddpmjegxy" + "hylxhlqyqhkycw",
            /*232*/"cjmyyxnatjhyccxz" + "pcqlbzwwytwbqcml" + "pmyrjcccxfpznzzl" +
                   "jplxxyztzlgdldck" + "lyrlzgqtgjhhgjlj" + "axfgfjzslcfdqz",
            /*233*/"lclgjdjcsnclljpj" + "qdcclcjxmyzftsxg" + "cgsbrzxjqqctzhgy" +
                   "qtjqqlzxjylylbcy" + "amcstylpdjbyregk" + "jzyzhlyszqlznw",
            /*234*/"czcllwjqjjjkdgjz" + "olbbzppglghtgzxy" + "ghzmycnqsycyhbhg" +
                   "xkamtxyxnbskyzzg" + "jzlqjdfcjxdygjqj" + "jpmgwgjjjpkqsb",
            /*235*/"gbmmcjssclpqpdxc" + "dyykywcjddyygywr" + "hjrtgznyqldkljsz" +
                   "zgzqzjgdykshpzmt" + "lcpwnjafyzdjcnmw" + "escyglbtzcgmss",
            /*236*/"llyxqsxsbsjsbbgg" + "ghfjlypmzjnlyywd" + "qshzxtyywhmcyhyw" +
                   "dbxbtlmsyyyfsxjc" + "sdxxlhjhf sxzqhf" + "zmzcztqcxzxrtt",
            /*237*/"djhnnyzqqmnqdmmg" + "lydxmjgdhcdyzbff" + "allztdltfxmxqzdn" +
                   "gwqdbdczjdxbzgsq" + "qddjcmbkzffxmkdm" + "dsyyszcmljdsyn",
            /*238*/"sprskmkmpcklgdbq" + "tfzswtfgglyplljz" + "hgjjgypzltcsmcnb" +
                   "tjbqfkthbyzgkpbb" + "ymtdssxtbnpdkley" + "cjnycdykzddhqh",
            /*239*/"sdzsctarlltkzlge" + "cllkjlqjaqnbdkkg" + "hpjtzqksecshalqf" +
                   "mmgjnlyjbbtmlyzx" + "dcjpldlpcqdhzycb" + "zsczbzmsljflkr",
            /*240*/"zjsnfrgjhxpdhyjy" + "bzgdljcsezgxlblh" + "yxtwmabchecmwyjy" +
                   "zlljjyhlgbdjlsly" + "gkdzpzxjyyzlwcxs" + "zfgwyydlyhcljs",
            /*241*/"cmbjhblyzlycblyd" + "pdqysxqzbytdkyyj" + "yycnrjmpdjgklclj" +
                   "bctbjddbblblczqr" + "ppxjcglzcshltolj" + "nmdddlngkaqhqh",
            /*242*/"jhykheznmshrp qq" + "jchgmfprxhjgdych" + "ghlyrzqlcyqjnzsq" +
                   "tkqjymszswlcfqqq" + "xyfggyptqwlmcrnf" + "kkfsyylqbmqamm",
            /*243*/"myxctpshcptxxzzs" + "mphpshmclmldqfyq" + "xszyjdjjzzhqpdsz" +
                   "glstjbckbxyqzjsg" + "psxqzqzrqtbdkyxz" + "khhgflbcsmdldg",
            /*244*/"dzdblzyycxnncsyb" + "zbfglzzxswmsccmq" + "njqsbdqsjtxxmblt" +
                   "xzclzshzcxrqjgjy" + "lxzfjphyxzqqydfq" + "jjlzznzjcdgzyg",
            /*245*/"ctxmzysctlkphtxh" + "tlbjxjlxscdqxcbb" + "tjfqzfsltjbtkqbx" +
                   "xjjljchczdbzjdcz" + "jdcprnpqcjpfczlc" + "lzxbdmxmphjsgz",
            /*246*/"gszzqlylwtjpfsya" + "smcjbtzyycwmytcs" + "jjlqcqlwzmalbxyf" +
                   "bpnlsfhtgjwejjxx" + "glljstgshjqlzfkc" + "gnndszfdeqfhbs",
            /*247*/"aqtgylbxmmygszld" + "ydqmjjrgbjtkgdhg" + "kblqkbdmbylxwcxy" +
                   "ttybkmrtjzxqjbhl" + "mhmjjzmqasldcyxy" + "qdlqcafywyxqhz"
        };

        private static readonly int[] pyValue = new int[]
                {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
                };

        private static readonly string[] pyName = new string[]
                {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
                };

        private static Encoding gb2312 = Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 取简拼编码
        /// </summary>
        /// <param name="str">带中文的字符串</param>
        /// <returns>拼音简码</returns>
        public static string GetPinyinCode(string str)
        {
            StringBuilder result = new StringBuilder();
            foreach (char ch in str)
            {
                char newCh = ch;
                char? pyCode = null;

                if (ch >= '！' && ch <= '～') // 65281 到 65374
                {
                    const int offset = (int)'！' - (int)'!';
                    newCh = (char)((int)ch - offset);
                }
                else if (ch > 127)  // 表示是中文
                {
                    pyCode = GetCharPinyinCode(ch);
                }

                if (pyCode != null)
                {
                    result.Append(pyCode);
                }
                else if ((newCh >= '0' && newCh <= '9') ||
                    (newCh >= 'A' && newCh <= 'Z') ||
                    (newCh >= 'a' && newCh <= 'z')) // 只要汉字、字母、数字)
                {
                    result.Append(char.ToLower(newCh));
                }
            }
            return result.ToString();
        }

        private static int CompareChineseChar(byte[] bytes1, char char2)
        {
            byte[] bytes2 = gb2312.GetBytes(new char[] { char2 });

            if (bytes1[0] > bytes2[0])    // 先比第一个字节
            {
                return 1;
            }
            else if (bytes1[0] == bytes2[0])   // 如果第一个字节相等，则比第二个
            {
                return bytes1[1] - bytes2[1];
            }
            else
            {
                return -1;
            }
        }

        private static char GetHighPinyins(byte[] bytes)
        {
            return cHighPinyins[bytes[0] - 216][bytes[1] - 160 - 1];
        }

        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string GetFullPinyinCode(string str)
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]$"); // 匹配中文字符
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = str.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                if (regex.IsMatch(noWChar[j].ToString())) // 中文字符
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {

                        if (chrAsc == -9254)  // 修正“圳”字  // 修正部分文字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                else  // 非中文字符
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString.ToLower();
        }

        private static readonly object[] cPinyinMap = new object[]{
            19969,"dz",19975,"wm",19988,"qj",20048,"ly",20056,"cs",20060,"mn",20094,"qg",20127,"qj",20167,"cq",20193,"yg",
            20250,"hk",20256,"cz",20282,"cs",20285,"jgq",20291,"dt",20314,"yd",20340,"ne",20375,"dt",20389,"jy",20391,"cz",
            20415,"bp",20446,"ys",20447,"sq",20504,"tc",20608,"kg",20854,"qj",20857,"zc",20911,"fp",20504,"tc",20608,"kg",
            20854,"qj",20857,"zc",20985,"aw",21032,"pb",21048,"qx",21049,"cs",21089,"ys",21119,"jc",21242,"sb",
            21273,"sc",21305,"py",21306,"qo",21330,"zc",21333,"dcs",21345,"kq",21378,"ca",21397,"cs",21414,"sx",21442,"cs",
            21477,"jg",21480,"dt",21484,"zs",21494,"yx",21505,"yx",21512,"hg",21523,"xh",21537,"pb",21542,"fp",21549,"kh",
            21574,"da",21588,"td",21618,"zc",21621,"kha",21632,"jz",21654,"gk",21679,"lkg",21683,"kh",
            21719,"hy",21734,"woe",21780,"wn",21804,"hx",21899,"dz",21903,"nr",21908,"wo",
            21939,"zc",21956,"sa",21964,"ya",21970,"td",22031,"jg",22040,"xs",22060,"zc",22066,"cz",22079,"hm",
            22129,"xj",22179,"xa",22237,"nj",22244,"td",22280,"qj",22300,"yh",22313,"xw",22331,"yq",22343,"jy",22351,"hp",
            22395,"dc",22412,"td",22484,"pb",22500,"pb",22534,"dz",22549,"dh",22561,"bp",22612,"td",22771,"kq",22831,"hb",
            22841,"jg",22855,"qj",22865,"qx",23013,"lm",23081,"mw",23487,"sx",23558,"jq",23561,"wy",23586,"yw",23614,"wy",
            23615,"ns",23631,"pb",23646,"sz",23663,"tz",23673,"yg",23762,"dt",23769,"zs",23780,"qj",23884,"qk",24055,"xh",
            24113,"dc",24162,"cz",24191,"ga",24273,"qj",24324,"nl",24377,"td",24378,"qj",24439,"pf",24554,"zs",24683,"td",
            24694,"ew",24733,"lk",24925,"tn",25094,"zg",25100,"xq",25103,"xh",25153,"bp",25170,"bp",25179,"kg",25203,"bp",
            25240,"zs",25282,"fb",25303,"na",25324,"kg",25341,"zy",25373,"wz",25375,"xj",25528,"sd",
            25530,"cs",25552,"td",25774,"cz",25874,"zc",26044,"yw",26080,"wm",26292,"bp",26333,"pb",26355,"zy",26366,"cz",
            26397,"cz",26399,"qj",26415,"sz",26451,"sb",26526,"cz",26552,"jg",26561,"td",26588,"gj",26597,"cz",26629,"zs",
            26638,"ly",26646,"qx",26653,"kg",26657,"xj",26727,"gh",26894,"zc",26937,"zs",26946,"zc",26999,"kj",27099,"kj",
            27449,"yq",27481,"xs",27542,"zs",27663,"zs",27748,"ts",27784,"sc",27788,"zd",27795,"td",27850,"bp",
            27852,"mb",27895,"ls",27898,"lp",27973,"qj",27981,"kh",27986,"hx",27994,"xj",28044,"yc",28065,"wg",28177,"sm",
            28267,"qj",28291,"kh",28337,"zq",28463,"tl",28548,"dc",28601,"td",28689,"pb",28805,"jg",28820,"qg",28846,"pb",
            28952,"td",28975,"zc",29325,"qj",29575,"sl",29602,"fb",30010,"td",30044,"cx",30058,"pf",30091,"ysp",
            30111,"yn",30229,"xj",30427,"sc",30465,"sx",30631,"qy",30655,"qj",30684,"qjg",30707,"sd",30729,"xh",30796,"lg",
            30917,"bp",31074,"mn",31085,"jz",31109,"cs",31181,"zc",31192,"mlb",31293,"jq",31400,"yx",31584,"jy",31896,"zn",
            31909,"zy",31995,"xj",32321,"fp",32327,"yz",32418,"hg",32420,"xq",32421,"hg",32438,"lg",32473,"gj",32488,"dt",
            32521,"jq",32527,"pb",32562,"zsq",32564,"jz",32735,"zd",32793,"pb",33071,"pf",33098,"lx",33100,"ya",33152,"bp",
            33261,"cx",33324,"bp",33333,"dt",33406,"ay",33426,"mw",33432,"pb",33445,"jg",33486,"zn",33493,"st",33507,"jq",
            33540,"qj",33544,"zc",33564,"qx",33617,"yt",33632,"jq",33636,"hx",33637,"yx",33694,"gw",33705,"fp",33728,"wy",
            33882,"sr",34067,"mw",34074,"wy",34121,"jq",34255,"cz",34259,"xl",34425,"hj",34430,"xh",34485,"kh",34503,"sy",
            34532,"gh",34552,"sx",34558,"ey",34593,"lz",34660,"qy",34892,"hx",34928,"sc",34999,"jq",35048,"bp",35059,"sc",
            35098,"cz",35203,"tq",35265,"jx",35299,"jx",35782,"sz",35828,"sy",35843,"td",35895,"gy",35977,"hm",
            36158,"jg",36228,"qj",36426,"qx",36466,"dc",36710,"cj",36711,"zyg",36767,"bp",36866,"sk",36951,"yw",37034,"xy",
            37063,"hx",37218,"cz",37325,"zc",38063,"bp",38079,"dt",38085,"qy",38107,"dc",38116,"td",38123,"yd",38224,"hg",
            38241,"xtc",38271,"cz",38415,"ye",38426,"kh",38461,"yd",38463,"ae",38466,"pb",38477,"jx",38518,"ty",38551,"wk",
            38585,"zc",38704,"xs",38739,"lj",38761,"gj",38808,"qs",39048,"jg",39049,"jx",39052,"hg",39076,"cz",39271,"xt",
            39534,"td",39552,"td",39584,"bp",39647,"sb",39730,"lg",39748,"pbt",40109,"zq",40479,"nd",40516,"hg",40536,"hg",
            40583,"qj",40765,"yq",40784,"qj",40840,"yk",40863,"gjq"};
        private static Hashtable pinyinMap;

        /// <summary>
        /// 临时使用
        /// </summary>
        private static void WriteToFile()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb2.Append('{');
            int count = 0;
            for (int i = 0; i < cPinyinMap.Length / 2; i++)
            {
                int code = (int)cPinyinMap[i * 2];
                char ch = (char)code;
                string pinyin = (string)cPinyinMap[i * 2 + 1];

                if (i > 0)
                {
                    sb2.Append(',');
                }
                sb2.Append('"').Append(code).Append("\":\"").Append(pinyin).Append('"');

                sb.Append(ch).Append(' ').Append(code).Append(' ').Append(pinyin);

                char? pinyin0 = GetSingleCharPinyinCode(ch);
                if (pinyin0.HasValue && pinyin[0] != pinyin0.Value)
                {
                    sb.Append(' ').Append(pinyin0);
                    count++;
                }
                if (pinyin.Length == 1 && pinyin[0] == pinyin0.Value)
                {
                    sb.Append(" <-");
                }
                sb.AppendLine();
            }
            sb.AppendLine(count + " 个多音字第一音与字典中的不同");

            sb2.AppendLine("};");
            File.WriteAllText(@"E:\trd\Carpa.NET\docs\ref\多音字表.txt", sb.ToString() + sb2.ToString(),
                Encoding.Default);
        }

        private static char? GetCharPinyinCode(char chineseChar)
        {
            if (pinyinMap == null)
            {
                pinyinMap = new Hashtable();
                for (int i = 0; i < cPinyinMap.Length / 2; i++)
                {
                    pinyinMap[cPinyinMap[i * 2]] = cPinyinMap[i * 2 + 1];
                }
            }
            string pyCodes = (string)pinyinMap[(int)chineseChar];
            if (pyCodes != null)
            {
                return pyCodes[0];
            }

            return GetSingleCharPinyinCode(chineseChar);
        }

        private static char? GetSingleCharPinyinCode(char chineseChar)
        {
            byte[] bytes = gb2312.GetBytes(new char[] { chineseChar });

            if (bytes[0] < 216 || bytes[1] < 161)
            {
                for (int i = cLowChineseChars.Length - 1; i >= 0; i--)
                {
                    char char2 = cLowChineseChars[i];
                    if (CompareChineseChar(bytes, char2) >= 0)
                    {
                        return cLowPinyins[i];
                    }
                }
                return null; // 全角符号，如 （）
            }
            else
            {
                return GetHighPinyins(bytes);
            }
        }

        /// <summary>
        /// 按字节拆分
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string[] SplitByByte(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new string[1] { string.Empty };
            }

            IList<StringBuilder> list = new List<StringBuilder>();
            StringBuilder buffer = null;
            int newLen = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                int chLen = (int)ch < 128 ? 1 : 2;
                if (newLen + chLen > len)
                {
                    buffer = null; // 加到新新行中
                    newLen = 0;
                }

                if (buffer == null)
                {
                    buffer = new StringBuilder();
                    list.Add(buffer);
                }
                buffer.Append(ch);
                newLen += chLen;
            }
            string[] answer = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                answer[i] = list[i].ToString();
            }
            return answer;
        }

        /// <summary>
        /// 简单加密，Delphi中有对应的解密函数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SimpleEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            int startKey, multKey, addKey;
            GetSimpleEncryptKey(key, out startKey, out multKey, out addKey);
            int randomKey = new Random().Next(100);
            startKey = startKey + randomKey;
            multKey = multKey + randomKey;
            addKey = addKey + randomKey;
            string answer = randomKey.ToString("x2");
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            foreach (byte b in bytes)
            {
                byte xorByte = (byte)(b ^ (startKey >> 8));
                answer = answer + xorByte.ToString("x2");
                startKey = (xorByte + startKey) * multKey + addKey;
            }
            return answer;
        }

        /// <summary>
        /// 简单解密，Delphi中有对应的加密函数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SimpleDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            int startKey, multKey, addKey;
            GetSimpleEncryptKey(key, out startKey, out multKey, out addKey);
            int randomKey;
            if (!int.TryParse(str.Substring(0, 2), NumberStyles.HexNumber, null, out randomKey))
                throw new InvalidOperationException("密文错误");
            startKey = startKey + randomKey;
            multKey = multKey + randomKey;
            addKey = addKey + randomKey;
            int len = str.Length / 2 - 1;
            byte[] bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                byte b = byte.Parse(str.Substring(i * 2 + 2, 2), NumberStyles.HexNumber);
                byte xorByte = (byte)(b ^ (startKey >> 8));
                bytes[i] = xorByte;
                startKey = (b + startKey) * multKey + addKey;
            }
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        private static void GetSimpleEncryptKey(string key, out int startKey, out int multKey, out int addKey)
        {
            if (string.IsNullOrEmpty(key) || key.Length != 6)
                throw new InvalidOperationException("key 必须是6位数字");

            startKey = int.Parse(key.Substring(0, 2));
            multKey = int.Parse(key.Substring(2, 2));
            addKey = int.Parse(key.Substring(4, 2));
        }

        /// <summary>
        /// 字符串是否为空
        /// </summary>
        public static bool IsEmpty(this System.String target)
        {
            return string.IsNullOrEmpty(target);
        }
    }
}
