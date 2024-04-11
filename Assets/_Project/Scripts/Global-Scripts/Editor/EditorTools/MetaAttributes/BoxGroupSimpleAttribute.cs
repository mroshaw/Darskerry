using System;

namespace DaftAppleGames.Editor.EditorTools.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BoxGroupSimpleAttribute : MetaAttribute, IGroupAttribute
    {
        public string Name { get; private set; }

        public BoxGroupSimpleAttribute(string name = "")
        {
            Name = name;
        }
    }
}