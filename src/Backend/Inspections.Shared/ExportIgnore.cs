﻿using System;
using JetBrains.Annotations;

namespace Inspections.Shared;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public sealed class ExportIgnoreAttribute : Attribute
{
    public ExportIgnoreAttribute(string[] exceptForRoles = null)
    {
        ExceptForRoles = exceptForRoles;
    }

    public string[] ExceptForRoles { [UsedImplicitly] get; }

}
