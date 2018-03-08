using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DyTestor.DataObject
{
    public class DataObjectBase
    {
        public Guid ID { get; set; }

        public virtual string ToURLParameter()
        {
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();

            List<string> param = new List<string>();
            foreach(PropertyInfo property in properties)
            {
                object value = property.GetValue(this).ToString();
                param.Add($"{property.Name}={value}");
            }
            return string.Join("&", param);
            
        }
    }
}
