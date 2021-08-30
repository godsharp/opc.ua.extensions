using System;
using System.Linq.Expressions;

namespace GodSharp.Extensions.Opc.Ua.Utilities
{
    public class InstanceHelper
    {
        public static T Instance<T>()
        {
            return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile()();
        }
        
        public static dynamic Instance(Type type)
        {
            return Expression.Lambda<Func<dynamic>>(Expression.New(type)).Compile()();
        }

        public static T Generic<T>(Type genericType)
        {
            var constructedType = genericType.MakeGenericType(typeof(T));
            return (T) Activator.CreateInstance(constructedType);
        }

        public static dynamic Generic(Type genericType, Type type)
        {
            var constructedType = genericType.MakeGenericType(type);
            return Activator.CreateInstance(constructedType);
        }
    }

    public class ExpressionHelper
    {
        public static string GetMemberName(Expression expression)
        {
            var name = expression switch
            {
                UnaryExpression ue => ((MemberExpression)ue.Operand).Member.Name,
                MemberExpression me => me.Member.Name,
                ParameterExpression pe => pe.Type.Name,
                _ => throw new InvalidCastException("Only field and property can set.")
            };

            return name;
        }
    }
}