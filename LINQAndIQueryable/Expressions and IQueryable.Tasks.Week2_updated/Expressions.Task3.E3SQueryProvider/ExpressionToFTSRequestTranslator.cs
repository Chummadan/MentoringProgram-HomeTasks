using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case "Equals":
                        Visit(node.Object);
                        _resultStringBuilder.Append("(");
                        Visit(node.Arguments[0]);
                        _resultStringBuilder.Append(")");
                        return node;
                    case "StartsWith":
                        Visit(node.Object);
                        _resultStringBuilder.Append("(");
                        Visit(node.Arguments[0]);
                        _resultStringBuilder.Append("*)");
                        return node;
                    case "EndsWith":
                        Visit(node.Object);
                        _resultStringBuilder.Append("(*");
                        Visit(node.Arguments[0]);
                        _resultStringBuilder.Append(")");
                        return node;
                    case "Contains":
                        Visit(node.Object);
                        _resultStringBuilder.Append("(*");
                        Visit(node.Arguments[0]);
                        _resultStringBuilder.Append("*)");
                        return node;
                }
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    var leftNodeType = node.Left.NodeType;
                    var rightNodeType = node.Right.NodeType;

                    var memberAccessOperand = leftNodeType == ExpressionType.MemberAccess ? node.Left :
                        rightNodeType == ExpressionType.MemberAccess ? node.Right :
                        throw new NotSupportedException($"Operation '{node.NodeType}' should contain property/field operand");

                    var constantOperand = leftNodeType == ExpressionType.Constant ? node.Left :
                        rightNodeType == ExpressionType.Constant ? node.Right :
                        throw new NotSupportedException($"Operation '{node.NodeType}' should contain constant operand");

                    Visit(memberAccessOperand);
                    _resultStringBuilder.Append("(");
                    Visit(constantOperand);
                    _resultStringBuilder.Append(")");
                    break;
                case ExpressionType.AndAlso:
                    //_resultStringBuilder.Append("\"statements\": [{\"query\":\"");
                    Visit(node.Left);
                    _resultStringBuilder.Append(",");
                    //_resultStringBuilder.Append("\"}, {\"query\":\"");
                    Visit(node.Right);
                    //_resultStringBuilder.Append("\"}]");
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
