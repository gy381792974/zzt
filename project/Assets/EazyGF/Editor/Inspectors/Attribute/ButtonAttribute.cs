using System;
using UnityEngine;

namespace EazyGF
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ButtonAttribute : PropertyAttribute
    {
        public float height = 16;
        public readonly string[] funcNames = null;
        public ShowIfRunTime showIfRunTime = ShowIfRunTime.All;

        public ButtonAttribute(params string[] funcNames)
        {
            this.funcNames = funcNames;
        }
    }
}