using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Shared
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class ExportIgnoreAttribute : Attribute
    {
        public ExportIgnoreAttribute(string[] exceptForRoles = null)
        {
            ExceptForRoles = exceptForRoles;
        }

        public string[] ExceptForRoles { get; }

    }
}
