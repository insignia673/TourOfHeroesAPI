using ORMF23.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static ORMF23.Statics.SQLExpression;

namespace ORMF23.Statics
{
    public class QueryTools
    {
        public static string[,] DisectEntity<T>(T entity, bool ignoreId = true)
        {
            var props = typeof(T).GetProperties();
            if (props.Any(p => p.Name == "Id") && ignoreId)
            {
                var temp = props.ToList();
                temp.RemoveAll(x => x.Name == "Id");
                props = temp.ToArray();
            }
            var value = props.Select(p => p.GetValue(entity)?.ToString()).ToArray();

            var output = new string[props.Length, 2];

            for (int i = 0; i < props.Length; i++)
            {
                output[i, 0] = props[i].Name;
                output[i, 1] = value[i];

                var type = Type.GetTypeCode(props[i].PropertyType);
                if (type == TypeCode.String || type == TypeCode.Char)
                {
                    output[i, 1] = "\'" + output[i, 1] + "\'";
                }
            }

            return output;
        }
        // This Method Assumes varchars to be max, this could also be fixed with attributes
        // also assumes Property named Id is primary key and autoincrements, also fix with attributes
        public static string[,] EntityToSql<T>()
        {
            var props = typeof(T).GetProperties();
            var output = new string[props.Length, 2];

            for (int i = 0; i < props.Length; i++)
            {
                output[i, 0] = props[i].Name;
                output[i, 1] = SqlType(props[i].PropertyType);

                if (props[i].Name == "Id")
                {
                    output[i, 1] += " IDENTITY(1, 1) PRIMARY KEY";
                }
            }

            return output;
        }
        public static string GetTableName<T>() =>
            ((AliasAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(AliasAttribute)))?.Name ?? typeof(T).Name;
        public static string CreateFromPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null || predicate == default)
                return string.Empty;

            List<string> conditions = new List<string>();

            BreakTree(predicate.Body, conditions);

            return string.Join(" ", conditions);
        }
        private static void BreakTree(Expression predicate, List<string> conditions)
        {
            if (predicate.NodeType == ExpressionType.AndAlso ||
                predicate.NodeType == ExpressionType.OrElse)
            {
                conditions.Add("(");
                BreakTree(((BinaryExpression)predicate).Left, conditions);

                switch (predicate.NodeType)
                {
                    case ExpressionType.AndAlso:
                        conditions.Add(") " + AND() + '(');
                        break;
                    case ExpressionType.OrElse:
                        conditions.Add(") " + OR() + '(');
                        break;
                }

                BreakTree(((BinaryExpression)predicate).Right, conditions);

                conditions.Add(")");
            }

            if (predicate.NodeType == ExpressionType.Equal ||
                predicate.NodeType == ExpressionType.NotEqual ||
                predicate.NodeType == ExpressionType.GreaterThanOrEqual ||
                predicate.NodeType == ExpressionType.LessThanOrEqual ||
                predicate.NodeType == ExpressionType.GreaterThan ||
                predicate.NodeType == ExpressionType.LessThan)
            {
                CreateSqlComparason((BinaryExpression)predicate, conditions);
            }

            //this assumes the method is string.Contains(), more methods could be implemented
            if (predicate.NodeType == ExpressionType.Call)
            {
                CreateSqlContains((MethodCallExpression)predicate, conditions);
            }
        }
        private static void CreateSqlContains(MethodCallExpression predicate, List<string> conditions)
        {
            string value = "";
            var arg = predicate.Arguments[0];
            //string value = GetMemeber(arg);
            if (arg is ConstantExpression constExpr)
                value = (string)constExpr.Value;
            else if (arg is MemberExpression expr)
                value = Expression.Lambda(expr).Compile().DynamicInvoke().ToString();

            var comparason = "\'%" + value + "%\'";
            var member = ((MemberExpression)predicate.Object).Member.Name;

            conditions.Add(LIKE(member, comparason));
        }
        // this method could be a generic in case the member in expression has different
        // name in database, this could be fixed with alias attribute
        private static void CreateSqlComparason(BinaryExpression predicate, List<string> conditions)
        {
            string toAdd;
            switch (predicate.NodeType)
            {
                case ExpressionType.Equal:
                    toAdd = "{0} = {1}";
                    break;
                case ExpressionType.NotEqual:
                    toAdd = NOT() + "{0} = {1}";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    toAdd = "{0} >= {1}";
                    break;
                case ExpressionType.LessThanOrEqual:
                    toAdd = "{0} <= {1}";
                    break;
                case ExpressionType.GreaterThan:
                    toAdd = "{0} > {1}";
                    break;
                case ExpressionType.LessThan:
                    toAdd = "{0} < {1}";
                    break;
                default:
                    toAdd = "{0} = {1}";
                    break;
            }

            string left = NodeTypeConverter(predicate.Left);
            string right = NodeTypeConverter(predicate.Right);

            conditions.Add(string.Format(toAdd, left, right));
        }
        private static string NodeTypeConverter(Expression value)
        {
            object newVal;
            if (value.NodeType == ExpressionType.MemberAccess)
            {
                //nodeType parameter vs memeberaccess
                if (((MemberExpression)value).Expression.NodeType == ExpressionType.Parameter)
                {
                    return ((MemberExpression)value).Member.Name;
                }
                else
                {
                    newVal = Expression.Lambda((MemberExpression)value).Compile().DynamicInvoke();
                }
                //return Expression.Lambda((MemberExpression)value).Compile().DynamicInvoke().ToString();
            }
            else
            {
                newVal = ((ConstantExpression)value).Value;
                //var newVal = ((ConstantExpression)value);
                //if (newVal.Type == typeof(string))
                //{
                //    return ("\'" + newVal.Value + "\'").ToString();
                //}
                //return newVal.Value.ToString();
            }

            if (newVal is string output)
            {
                return ("\'" + output + "\'").ToString();
            }
            return newVal.ToString();
        }
        //private static string GetMemeber(Expression arg)
        //{
        //    string value = string.Empty;
        //    if (arg is ConstantExpression constExpr)
        //        value = (string)constExpr.Value;
        //    else if (arg is MemberExpression memberExpr)
        //    {
        //        static object? getValue(object target, MemberInfo member)
        //        {
        //            if (member is FieldInfo field)
        //                return field.GetValue(target);
        //            else if (member is PropertyInfo property)
        //                return property.GetValue(target);

        //            return null;
        //        }

        //        var argMember = arg as MemberExpression;
        //        constExpr = argMember.Expression as ConstantExpression;
        //        value = getValue(constExpr.Value, argMember.Member) as string;
        //    }

        //    return value;
        //}
    }
}