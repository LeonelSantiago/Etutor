using System;
using System.Linq;
using System.Linq.Expressions;

namespace Etutor.Core.Utilities
{
    public static class ExpressionParser
    {
        public static Expression ToExpression2<T>(ParameterExpression paramExpr, string propName, string opr, string value, string andOrOperator = "Or", Expression<Func<T, bool>> expr = null)
        {
            Expression func = null;
            try
            {
                var arrProp = propName.Split('.').ToList();
                Expression binExpr = null;
                string partName = string.Empty;
                arrProp.ForEach(x =>
                {
                    Expression tempExpr = null;
                    partName = String.IsNullOrEmpty(partName) ? x : partName + "." + x;
                    if (partName == propName)
                    {
                        var member = NestedExprProp(paramExpr, partName);
                        var type = member.Type.Name == "Nullable`1"
                            ? Nullable.GetUnderlyingType(member.Type)
                            : member.Type;
                        if (opr == "contains" && (type != typeof(string)))
                        {
                            opr = "=";
                        }
                        tempExpr = ApplyFilter(opr, member, Expression.Convert(ToExprConstant(type, value), member.Type));
                    }
                    else
                        tempExpr = ApplyFilter("!=", NestedExprProp(paramExpr, partName), Expression.Constant(null));
                    binExpr = binExpr != null ? Expression.AndAlso(binExpr, tempExpr) : tempExpr;
                });
                return binExpr;
            }
            catch { }
            return func;
        }

        private static Expression ApplyFilter(string opr, Expression left, Expression right)
        {
            Expression InnerLambda = null;
            switch (opr)
            {
                case "==":
                case "=":
                case "equals":
                    InnerLambda = Expression.Equal(left, right);
                    break;
                case "<":
                    InnerLambda = Expression.LessThan(left, right);
                    break;
                case ">":
                    InnerLambda = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    InnerLambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    InnerLambda = Expression.LessThanOrEqual(left, right);
                    break;
                case "!=":
                    InnerLambda = Expression.NotEqual(left, right);
                    break;
                case "&&":
                    InnerLambda = Expression.And(left, right);
                    break;
                case "||":
                    InnerLambda = Expression.Or(left, right);
                    break;
                case "LIKE":
                case "Contains":
                case "contains":
                    InnerLambda = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right);
                    break;
                case "NOTLIKE":
                    InnerLambda = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right));
                    break;
            }
            return InnerLambda;
        }

        private static MemberExpression NestedExprProp(Expression expr, string propName)
        {
            string[] arrProp = propName.Split('.');
            int arrPropCount = arrProp.Length;
            return (arrPropCount > 1) ? Expression.Property(NestedExprProp(expr, arrProp.Take(arrPropCount - 1).Aggregate((a, i) => a + "." + i)), arrProp[arrPropCount - 1]) : Expression.Property(expr, propName);
        }

        public static Expression ToExprConstant(Type prop, string value)
        {
            if (String.IsNullOrEmpty(value))
                return Expression.Constant(value);
            object val = null;
            if (prop.BaseType == typeof(Guid))
                val = Guid.Parse(value);
            else
                val = Converter.To(value, prop);
            return Expression.Constant(val);
        }
    }
}
