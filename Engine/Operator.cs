using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using iBet.Engine;
using iBet.DTO;

namespace iBet.Engine
{
    public class Person
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    class Operator
    {
        private static Dictionary<string, Func<object, object, bool>> s_operators;
        private static Dictionary<string, PropertyInfo> m_properties;
        private static Dictionary<string, PropertyInfo> o_properties;
        static Operator()
        {
            s_operators = new Dictionary<string, Func<object, object, bool>>();
            s_operators["greater_than"] = new Func<object, object, bool>(s_opGreaterThan);
            s_operators["equal"] = new Func<object, object, bool>(s_opEqual);

            m_properties = typeof(MatchDTO).GetProperties().ToDictionary(propInfo => propInfo.Name);
            o_properties = typeof(OddDTO).GetProperties().ToDictionary(propInfo => propInfo.Name);
        }

        public static bool Apply(MatchDTO match, string op, string prop, object target)
        {
            return s_operators[op](GetPropValue(match, prop), target);
        }
        public static bool Apply(OddDTO odd, string op, string prop, object target)
        {
            return s_operators[op](GetPropValue(odd, prop), target);
        }

        private static object GetPropValue(MatchDTO match, string prop)
        {
            PropertyInfo propInfo = m_properties[prop];
            return propInfo.GetGetMethod(false).Invoke(match, null);
        }
        private static object GetPropValue(OddDTO odd, string prop)
        {
            PropertyInfo propInfo = o_properties[prop];
            return propInfo.GetGetMethod(false).Invoke(odd, null);
        }

        #region Operators

        static bool s_opGreaterThan(object o1, object o2)
        {
            decimal d1, d2;
            if (decimal.TryParse(o1.ToString(), out d1) && decimal.TryParse(o2.ToString(), out d2))
            {
                if (d1 > d2)
                    return true;
            }
            if (o1 == null || o2 == null || o1.GetType() != o2.GetType() || !(o1 is IComparable))
                return false;
            return (o1 as IComparable).CompareTo(o2) > 0;
        }

        static bool s_opEqual(object o1, object o2)
        {
            decimal d1, d2;
            if (decimal.TryParse(o1.ToString(), out d1) && decimal.TryParse(o2.ToString(),out d2))
            {
                if (d1 == d2)
                    return true;
            }
            return o1 == o2;
        }

        #endregion        
    }
}
