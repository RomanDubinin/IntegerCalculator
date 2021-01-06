using System;
using System.Collections.Generic;

namespace Calculation
{
    public static class OperatorsProvider
    {
        public static readonly List<string> DefaultBinaryOperators = new List<string> {"+", "-", "*", "/"};
        public static readonly List<string> DefaultUnaryOperators = new List<string> {"+", "-"};

        public static readonly List<string> ExtendedOperators = new List<string> {"+", "-", "*", "/", "~"};

        public static Dictionary<string, string> UnaryOperatorMapping = new Dictionary<string, string>
        {
            {"-", "~"},
            {"+", ""}
        };

        public static readonly Dictionary<string, int> Priority = new Dictionary<string, int>
        {
            {"+", 0},
            {"-", 0},
            {"*", 1},
            {"/", 1},
            {"~", 2}
        };

        public static readonly Dictionary<string, Action<Stack<long>>> Actions = new Dictionary<string, Action<Stack<long>>>
        {
            {"+", operands => operands.Push(operands.Pop() + operands.Pop())},
            {"-", operands => operands.Push(-operands.Pop() + operands.Pop())},
            {"*", operands => operands.Push(operands.Pop() * operands.Pop())},
            {"/", operands =>
                {
                    var right = operands.Pop();
                    var left = operands.Pop();
                    operands.Push(left / right);
                }
            },
            {"~", operands => operands.Push(-operands.Pop())}
        };
    }
}