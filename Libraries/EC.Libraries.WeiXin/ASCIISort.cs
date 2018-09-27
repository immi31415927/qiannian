﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC.Libraries.WeiXin
{
    /// <summary>
    /// ASCII字典排序
    /// </summary>
    public class ASCIISort : IComparer
    {
        /// <summary>
        /// 创建新的ASCIISort实例
        /// </summary>
        /// <returns></returns>
        public static ASCIISort Create()
        {
            return new ASCIISort();
        }

        public int Compare(object x, object y)
        {

#if NET35 || NET40 || NET45
            byte[] xBytes = System.Text.Encoding.Default.GetBytes(x.ToString());
            byte[] yBytes = System.Text.Encoding.Default.GetBytes(y.ToString());
#else
            byte[] xBytes = System.Text.Encoding.ASCII.GetBytes(x.ToString());
            byte[] yBytes = System.Text.Encoding.ASCII.GetBytes(y.ToString());
#endif
            int xLength = xBytes.Length;
            int yLength = yBytes.Length;
            int minLength = Math.Min(xLength, yLength);

            for (int i = 0; i < minLength; i++)
            {
                var xByte = xBytes[i];
                var yByte = yBytes[i];
                if (xByte > yByte)
                {
                    return 1;
                }
                else if (xByte < yByte)
                {
                    return -1;
                }
            }

            if (xLength == yLength)
            {
                return 0;
            }
            else
            {
                if (xLength > yLength)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
