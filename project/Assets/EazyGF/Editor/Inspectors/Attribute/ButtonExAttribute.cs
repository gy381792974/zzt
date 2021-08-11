using System;

namespace EazyGF
{
    public enum ShowIfRunTime
    {
        All,
        Playing,
        Editing,
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonExAttribute : Attribute
    {
        public readonly string txtButtonName;
        public ShowIfRunTime showIfRunTime;

        public ButtonExAttribute(string txtButtonName = null)
        {
            this.txtButtonName = txtButtonName;
        }
    }
}