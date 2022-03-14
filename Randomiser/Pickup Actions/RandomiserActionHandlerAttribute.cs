using System;

namespace Randomiser
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    sealed class RandomiserActionHandlerAttribute : Attribute
    {
        public string ActionID { get; }

        public RandomiserActionHandlerAttribute(string actionID)
        {
            ActionID = actionID;
        }
    }
}
