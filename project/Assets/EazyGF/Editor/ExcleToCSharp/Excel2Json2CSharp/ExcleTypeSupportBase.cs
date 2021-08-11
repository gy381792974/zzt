using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace EazyGF
{
    public abstract class ExcleTypeSupportBase
    {
        public string defineName;
        public string singleShortName;
        public string arrayShortName;
        public string arrayMultipShort;

        public virtual bool GetValue(string value, out object result)
        {
            result = null;
            return false;
        }
    }

    //下面是支持的类型
    public class ExcleIntType : ExcleTypeSupportBase
    {
        public ExcleIntType(string _defineName, string _singleShortName, string _arrayShortName,string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }
        public override bool GetValue(string value, out object result)
        {
            if (int.TryParse(value, out var tempValue))
            {
                result = tempValue;
                return true;
            }
            result = 0;
            return false;
        }
        
    }
    public class ExcleLongType : ExcleTypeSupportBase
    {
        public ExcleLongType(string _defineName, string _singleShortName, string _arrayShortName, string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }

        public override bool GetValue(string value, out object result)
        {
            if (long.TryParse(value, out var tempValue))
            {
                result = tempValue;
                return true;
            }
            result = 0;
            return false;
        }
    }

    public class ExcleFloatType : ExcleTypeSupportBase
    {
        public ExcleFloatType(string _defineName, string _singleShortName, string _arrayShortName, string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }

        public override bool GetValue(string value, out object result)
        {
            if (float.TryParse(value, out var tempValue))
            {
                string UpValye = value.ToUpper();
                if (UpValye.Contains("E"))
                {
                    result = tempValue.ToString("F6", CultureInfo.InvariantCulture);
                }
                else
                {
                    result = tempValue;
                }

                return true;
            }
            result = 0;
            return false;
        }
    }

    public class ExcleDoubleType : ExcleTypeSupportBase
    {
        public ExcleDoubleType(string _defineName, string _singleShortName, string _arrayShortName, string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }

        public override bool GetValue(string value, out object result)
        {
            if (double.TryParse(value, out var tempValue))
            {
                string UpValye = value.ToUpper();
                //if (UpValye.Contains("E"))
                //{
                //    result = tempValue.ToString("F10", CultureInfo.InvariantCulture);
                //}
                //else
                //{
                //    result = tempValue;
                //}
                result = tempValue.ToString("F10", CultureInfo.InvariantCulture);

                return true;
            }
            result = 0;
            return false;
        }
    }

    public class ExcleBigIntegerType : ExcleTypeSupportBase
    {
        public ExcleBigIntegerType(string _defineName, string _singleShortName, string _arrayShortName, string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }
        public override bool GetValue(string value, out object result)
        {
            if (BigInteger.TryParse(value, NumberStyles.Any, null, out var tempValue))
            {
                result = tempValue;
                return true;
            }
            result = 0;
            return false;
        }
    }

    public class ExcleStringType : ExcleTypeSupportBase
    {
        public ExcleStringType(string _defineName, string _singleShortName, string _arrayShortName, string _arrayMultipShort)
        {
            defineName = _defineName;
            singleShortName = _singleShortName;
            arrayShortName = _arrayShortName;
            arrayMultipShort = _arrayMultipShort;
        }

        public override bool GetValue(string value, out object result)
        {
            string s = value.Replace("\n", "");//避免手动回车，这里去除掉这个格式
            s = s.Replace('”', '"');
            s = s.Replace('“', '"');
            s = s.Replace('‘', '\'');
            s = s.Replace('’', '\'');
            List<int> indexList = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals('"'))
                {
                    indexList.Add(i);
                }
            }

            int count = 0;
            for (int i = 0; i < indexList.Count; i++)
            {
                s = s.Insert(indexList[i] + count, @"\");
                count++;
            }
            result = $"\"{s}\"";
            return true;
        }
    }

}
