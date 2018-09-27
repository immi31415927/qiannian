using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EC.Libraries.Util
{
    /// <summary>
    /// 枚举工具
    /// </summary>
    public static class EnumUtil
    {
        private static Dictionary<string, IDictionary<int, string>> _enumList = new Dictionary<string, IDictionary<int, string>>(); //枚举缓存池
        private static readonly object sync = new object();

        /// <summary>
        /// 枚举转换成字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>字典</returns>
        public static IDictionary<int, string> ToDictionary(Type enumType)
        {
            IDictionary<int, string> list = new Dictionary<int, string>();

            foreach (int i in Enum.GetValues(enumType))
            {
                string name = Enum.GetName(enumType, i);

                //取描述名称
                string showName = string.Empty;
                FieldInfo enumInfo = enumType.GetField(name);
                var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                showName = enumAttributes.Length > 0 ? enumAttributes[0].Description : name;

                list.Add(i, showName);
            }

            //string keyname = enumType.FullName;
            //if (!_enumList.ContainsKey(keyname))
            //{
            //    IDictionary<int, string> list = new Dictionary<int, string>();

            //    foreach (int i in Enum.GetValues(enumType))
            //    {
            //        string name = Enum.GetName(enumType, i);

            //        //取描述名称
            //        string showName = string.Empty;
            //        FieldInfo enumInfo = enumType.GetField(name);
            //        var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //        showName = enumAttributes.Length > 0 ? enumAttributes[0].Description : name;

            //        list.Add(i, showName);
            //    }

            //    if (!_enumList.ContainsKey(keyname))
            //    {
            //        lock (sync)
            //        {
            //            _enumList.Add(keyname, list);
            //        }
            //    }
            //}
            //return _enumList[keyname];
            return list;
        }

        /// <summary>
        /// 获取枚举类型Description(没有缓存)
        /// </summary>
        /// <param name="e">枚举类型</param>
        /// <returns>Description</returns>
        public static string GetDescription(this Enum e)
        {
            FieldInfo enumInfo = e.GetType().GetField(e.ToString());
            var enumAttributes =
                (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.ToString();
        }

        /// <summary>
        /// 获取枚举值对应的显示名称(取缓存)
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="intValue">枚举项对应的int值</param>
        /// <returns>枚举名称</returns>
        public static string GetDescription(Type enumType, int intValue)
        {
            var list = ToDictionary(enumType);
            if (list.ContainsKey(intValue))
                return list[intValue];
            return string.Empty;
        }

        /// <summary>
        /// 返回指定枚举类型的指定名称或值的描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举名称或值</param>
        /// <returns>枚举的描述</returns>
        public static string GetDescription(Type type, object value)
        {
            try
            {
                string name = value is int ? Enum.GetName(type, value) : value.ToString();
                FieldInfo fi = type.GetField(name);
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : name;
            }
            catch
            {
                return "未知";
            }
        }

        /// <summary>
        /// 转换Enum到SelectListItem
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>Text:Description Value:Name</returns>
        public static List<SelectListItem> ToListItem<T>() where T : struct, IConvertible
        {
            var list = new List<SelectListItem>();
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            Array arr = Enum.GetValues(typeof(T));
            list.AddRange(from T value in arr
                          select new SelectListItem
                          {
                              Value = value.ToString(),
                              Text = string.Format("{0}{1}", value, GetDescription(typeof(T), value))
                          });
            return list;
        }

        /// <summary>
        /// 访问列表，并把一个空值函数作用于列表中每个元素
        /// </summary>
        /// <typeparam name="T">泛型元素</typeparam>
        /// <param name="sequence">列表</param>
        /// <param name="action">空值函数</param>
        /// <returns></returns>
        /// 调用示例：
        /// var values = new List<int> {10,20,35};
        /// values.Apply(v=>Console.Write(x));
        public static void Apply<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence != null && sequence.Count() > 0)
            {
                foreach (T item in sequence)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// 访问列表，并把一个空值函数作用于列表中每个元素(并行)
        /// </summary>
        /// <typeparam name="T">泛型元素</typeparam>
        /// <param name="sequence">列表</param>
        /// <param name="action">空值函数</param>
        /// <returns></returns>
        /// 调用示例：
        /// var values = new List<int> {10,20,35};
        /// values.ApplyParallel(v=>Console.Write(x));
        public static void ApplyParallel<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence != null && sequence.Count() > 0)
            {
                Parallel.ForEach(sequence, item => action(item));
            }
        }

        /// <summary>
        /// 转换Enum到SelectListItem
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>返回List SelectListItem </returns>
        public static void ToListItem<T>(ref List<SelectListItem> selectList) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            selectList.AddRange(from int s in Enum.GetValues(typeof(T))
                                select new SelectListItem
                                {
                                    Value = s.ToString(CultureInfo.InvariantCulture),
                                    Text = Enum.GetName(typeof(T), s)
                                });
        }

        /// <summary>
        /// Enum To SelectList
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="defaultText">默认显示标头</param>
        /// <param name="selectedValue">要选中选项值</param>
        /// <returns>SelectList</returns>
        public static SelectList EnumToSelectList<T>() where T : struct, IConvertible
        {
            var itemList = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                if (item.GetHashCode() > 0)
                {
                    itemList.Add(new SelectListItem() { Value = item.GetHashCode().ToString(), Text = Enum.GetName(typeof(T), item.GetHashCode()) });
                }
            }

            return new SelectList(itemList, "Value", "Text");
        }

        /// <summary>
        /// Enum To SelectList
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="defaultText">默认显示标头</param>
        /// <param name="selectedValue">要选中选项值</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectListExclude<T>() where T : struct, IConvertible
        {
            var itemList = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                if (item.GetHashCode() > 0 && item.GetHashCode() != 40)
                {
                    itemList.Add(new SelectListItem() { Value = item.GetHashCode().ToString(), Text = Enum.GetName(typeof(T), item.GetHashCode()) });
                }
            }

            return new SelectList(itemList, "Value", "Text");
        }

        /// <summary>
        /// Enum To SelectList
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="defaultText">默认显示标头</param>
        /// <param name="selectedValue">要选中选项值</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(string defaultText = null, object selectedValue = null) where T : struct, IConvertible
        {
            var itemList = new List<SelectListItem>();

            if (defaultText != null)
            {
                itemList.Add(new SelectListItem() { Text = defaultText, Value = "" });
            }

            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型!");
            itemList.AddRange(from int x in Enum.GetValues(typeof(T))
                              select new SelectListItem
                              {
                                  Value = x.ToString(CultureInfo.InvariantCulture),
                                  Text = Enum.GetName(typeof(T), x)
                              });
            return new SelectList(itemList, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// Enum Set SelectList
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValueList">枚举参数列表</param>
        /// <param name="defaultText">默认显示标头</param>
        /// <param name="selectedValue">要选中选项值</param>
        /// <returns>SelectList</returns>
        public static SelectList SetSelectList<T>(List<object> enumValueList, string defaultText = null, object selectedValue = null) where T : struct, IConvertible
        {
            var itemList = new List<SelectListItem>();

            if (defaultText != null)
            {
                itemList.Add(new SelectListItem() { Text = defaultText, Value = "" });
            }

            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型!");
            foreach (var item in from object item in Enum.GetValues(typeof(T)) from value in enumValueList.Where(value => (int)item == (int)value) select item)
            {
                itemList.AddRange(new[] {new SelectListItem()
                {
                    Value = ((int)item).ToString(CultureInfo.InvariantCulture),
                    Text = Enum.GetName(typeof(T), item)
                }});
            }

            return new SelectList(itemList, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// 枚举是否存在该参数
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="value">参数</param>
        /// <returns>是否成功</returns>
        public static bool IsEnumExist<T>(object value)
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型!");
            var isExist = false;
            foreach (object key in Enum.GetValues(typeof(T)))
            {
                if (((int)value)==((int)key))
                {
                    isExist = true;
                }
            }

            return isExist;
        }

        /// <summary>
        /// 获取枚举项列表
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="selectedKeyList">选择中Key列表</param>
        /// <returns>枚举项列表</returns>
        public static List<EnumItem> GetEnumItemList<T>(List<object> selectedKeyList)
        {
            var enumItemList = new List<EnumItem>();

            if (selectedKeyList == null) selectedKeyList = new List<object>();

            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型!");

            foreach (object key in Enum.GetValues(typeof(T)))
            {
                enumItemList.Add(new EnumItem
                {
                    ItemKey = ((int)key).ToString(CultureInfo.InvariantCulture),
                    ItemValue = Enum.GetName(typeof(T), key),
                    IsChecked = selectedKeyList.Any(p => ((int)p) == ((int)key))
                });
            }

            return enumItemList;
        }

        /// <summary>
        /// 转换Enum到SelectListItem
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>Text:Description Value:Name</returns>
        public static SelectList ToSelectListDescription<T>(string defaultText = null, object selectedValue = null) where T : struct, IConvertible
        {
            var itemList = new List<SelectListItem>();

            if (defaultText != null)
            {
                itemList.Add(new SelectListItem() { Text = defaultText, Value = "" });
            }

            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型!");
            itemList.AddRange(from int x in Enum.GetValues(typeof(T))
                              select new SelectListItem
                              {
                                  Value = x.ToString(CultureInfo.InvariantCulture),
                                  Text = GetDescription(typeof(T), x)
                              });
            return new SelectList(itemList, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// Enum To Json
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns>获取json数据</returns>
        /// <remarks>2016-8-1 蒋浪石 创建</remarks>
        public static string GetJsonEnum(Type enumType)
        {
            var values = (int[])Enum.GetValues(enumType);
            var names = Enum.GetNames(enumType);
            var pairs = new string[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                pairs[i] = names[i] + ":" + values[i];
            }

            return "{" + string.Join(",", pairs) + "}";
        }
    }

    /// <summary>
    /// 枚举项
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// Key值
        /// </summary>
        public object ItemKey { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object ItemValue { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
