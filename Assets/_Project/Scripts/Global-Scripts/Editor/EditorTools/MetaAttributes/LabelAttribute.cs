﻿using System;

namespace DaftAppleGames.Editor.EditorTools.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : MetaAttribute
    {
        public string Label { get; private set; }

        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}