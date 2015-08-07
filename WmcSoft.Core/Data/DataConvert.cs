﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Data
{
    public static class DataConvert
    {
        public static T ChangeType<T>(object value) {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (value == DBNull.Value) {
                if (underlyingType != null || !typeof(T).IsValueType)
                    return default(T);
                throw new InvalidCastException();
            }
            return (T)Convert.ChangeType(value, underlyingType ?? typeof(T));
        }
    }
}
