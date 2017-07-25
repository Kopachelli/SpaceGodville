using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace zpgServer
{
    public static class CmdParser
    {
        static List<Type> staticTypes = new List<Type>();
        public static void Initialize()
        {
            staticTypes.Add(typeof(Authorization));
            staticTypes.Add(typeof(Destiny));
            staticTypes.Add(typeof(Localization));
            staticTypes.Add(typeof(Looter));
            staticTypes.Add(typeof(Settings));
            staticTypes.Add(typeof(Terraformer));
            staticTypes.Add(typeof(Universe));
            staticTypes.Add(typeof(CmdParser));
        }
        public static void Handle(string cmd)
        {
            bool handled = false;
            StringBuilder output = new StringBuilder();
            // Formatting
            cmd = cmd.ToLower();
            // Basic commands
            switch (cmd)
            {
                case "help":
                    output.AppendLine("Available types:");
                    foreach (Type t in staticTypes)
                    {
                        output.AppendLine("- " + t.Name);
                    }
                    handled = true;
                    break;
                case "event.reload":
                    Destiny.FullReload();
                    break;
                case "exit":
                    ConsoleWindow.CloseInstance();
                    handled = true;
                    break;
            }
            // Still not handled
            if (!handled && cmd.Substring(0, 1) == "#")
            {
                string reflectionLine = ParseReflectionCommand(cmd.Substring(1));
                if (reflectionLine != null && reflectionLine.Length > 0)
                {
                    output.AppendLine(reflectionLine);
                    handled = true;
                }
                foreach (Type t in staticTypes)
                {
                    // Show all fields and properties
                    /*if (t.Name.ToLower() == cmd.ToLower())
                    {
                        output.AppendLine("Please note that the console functionality is still Work-In-Progress.");
                        var fields = t.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        foreach (FieldInfo field in fields)
                        {
                            output.AppendLine(field.Name + " = " + Parse(field.GetValue(null), 0));
                        }
                        var properties = t.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        foreach (PropertyInfo property in properties)
                        {
                            output.AppendLine(property.Name + " = " + Parse(property.GetValue(null, null), 0));
                        }
                        handled = true;
                        break;
                    }*/
                }
            }

            // Unknown
            if (!handled)
            {
                output.AppendLine("Unknown command");
            }
            // Remove last line break
            if (output.Length > 0)
                output.Remove(output.Length - 2, 2);
            // Flush to console
            ConsoleEx.Log(output.ToString(), false);
        }

        static string ParseReflectionCommand(string cmd)
        {
            if (!cmd.Contains("="))
            {
                string output = null;

                // Get the leftmost name and remaining cmd
                string leftmostName = cmd, remainingCmd = null;
                if (cmd != null && cmd.Contains("."))
                {
                    leftmostName = cmd.Split('.')[0];
                    remainingCmd = cmd.Substring(cmd.IndexOf('.') + 1);
                }

                foreach (Type t in staticTypes)
                {
                    //ConsoleEx.Log( + " - " + leftmostName.ToLower());
                    if (leftmostName.ToLower() == t.ToString().ToLower().Split('.')[1])
                        output = ParseReflectionGet(remainingCmd, t, null, 0);
                }
                return output;
            }
            else
                return "Variable setting not supported yet.";
        }

        static string ParseReflectionGet(string cmd, Type type, object target, int depth)
        {
            if (depth > 10)
                return null;

            // Parse the cmd
            string leftmostName = null, remainingCmd = null;
            if (cmd != null && cmd.Contains("."))
            {
                leftmostName = cmd.Split('.')[0];
                remainingCmd = cmd.Substring(cmd.IndexOf('.') + 1);
            }
            else if (cmd != null)
            {
                leftmostName = cmd;
            }
            int index = -1;
            if (cmd != null && leftmostName.Contains("[") && leftmostName.Contains("]"))
            {
                int indexOfIndexStart = leftmostName.IndexOf("[") + 1;
                string indexString = leftmostName.Substring(indexOfIndexStart, leftmostName.IndexOf("]") - indexOfIndexStart);
                leftmostName = leftmostName.Substring(0, leftmostName.IndexOf("["));
                Int32.TryParse(indexString, out index);
            }

            // No command left - just output stuff
            if (cmd == null || cmd.Length == 0)
            {
                StringBuilder output = new StringBuilder();
                // Basic types
                if (target != null
                    && (target.GetType() == typeof(string) || target.GetType() == typeof(int) || target.GetType() == typeof(decimal)
                    || target.GetType() == typeof(float) || target.GetType() == typeof(double)))
                {
                    output.AppendLine(target.ToString());
                }
                // Array types
                else if (target != null && typeof(IEnumerable).IsAssignableFrom(target.GetType()))
                {
                    int objectCount = 0;
                    IEnumerable objAsList = target as IEnumerable;
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (object listObj in objAsList)
                    {
                        //stringBuilder.AppendLine(ParseReflectionGet(null, listObj.GetType(), listObj, depth + 1));
                        objectCount += 1;
                    }
                    //return stringBuilder.ToString();
                    return "Array type. Object count: " + objectCount;
                }
                else
                {
                    var fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (FieldInfo field in fields)
                    {
                        //string fieldType = field.FieldType.ToString().Substring(field.FieldType.ToString().LastIndexOf('.') + 1);
                        string fieldType = field.FieldType.ToString();
                        output.AppendLine(fieldType + " " + field.Name + " = " + field.GetValue(target));
                    }
                }
                return output.ToString();
            }
            // Some command left
            else
            {
                var fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                {
                    object obj = field.GetValue(target);

                    // Array with index specified
                    if (field.Name.ToLower() == leftmostName && index != -1 && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                    {
                        object localObject = null;
                        IEnumerable objAsList = obj as IEnumerable;
                        int localIndex = 0;
                        foreach (object listObj in objAsList)
                        {
                            if (localIndex == index)
                            {
                                localObject = listObj;
                            }
                            localIndex += 1;
                        }
                        try
                        {
                            return ParseReflectionGet(remainingCmd, localObject.GetType(), localObject, depth + 1);
                        }
                        catch (Exception) { return "Invalid index"; }
                    }
                    // Array with no index specified
                    if (field.Name.ToLower() == leftmostName && index == -1 && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                    {
                        IEnumerable objAsList = obj as IEnumerable;
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (object listObj in objAsList)
                        {
                            stringBuilder.AppendLine(ParseReflectionGet(remainingCmd, listObj.GetType(), listObj, depth + 1));
                        }
                        return stringBuilder.ToString();
                    }
                    // Other
                    else if (field.Name.ToLower() == leftmostName)
                    {
                        return ParseReflectionGet(remainingCmd, field.FieldType, obj, depth + 1);
                    }
                    //else if (field.FieldType is IList && )
                }
            }
            return null;
        }

        /*static string Parse(object target, int depth)
        {
            if (target == null) { return ""; }
            Type t = target.GetType();
            /*if (depth > 0)
            {
                if (t == typeof(Player)) { return ((Player)target).username; }
                if (t == typeof(Reward)) { return ((Reward)target).ToString(); }
                if (t == typeof(Pilot)) { return ((Pilot)target).name; }
                if (t == typeof(Ship)) { return ((Ship)target).name; }
                if (t == typeof(Planet)) { return ((Planet)target).name; }
                else
                    return target.ToString();
            }

            // Parsing arrays
            var typeProperties = target.GetType().GetProperties();
            for (int i = 0; i < typeProperties.Length; i++)
            {
                if (typeProperties[i].PropertyType.IsArray)
                {
                    StringBuilder output = new StringBuilder("{");
                    foreach (object obj in (object[])target)
                    {
                        output.Append(Parse(obj, depth + 1));
                    }
                    return output.ToString();
                }
            }
            // Parsing lists
            if (target is IList && target.GetType().IsGenericType)
            {
                StringBuilder output = new StringBuilder("{");
                foreach (object obj in (IList)target)
                {
                    output.Append(Parse(obj, depth + 1));
                }
                return output.ToString();
            }

            if (1 == 2) { }
            if (target.GetType() == typeof(List<string>))
            {
                string output = "{";
                foreach (string s in (List<string>)target)
                {
                    output += s + ", ";
                }
                return output.Substring(0, output.Length - 2) + "}";
            }
            // Parsing generic arrays
            else if (target.GetType() == typeof(String[]))
            {
                string output = "{";
                foreach (string s in (String[])target)
                {
                    output += s + ", ";
                }
                return output.Substring(0, output.Length - 2) + "}";
            }
            //else if (!target.GetType().IsPrimitive && target.GetType() != typeof(string) && (target.GetType() != typeof(decimal)))
            else if (t == typeof(Planet) || t == typeof(Player) || t == typeof(Reward) || t == typeof(Pilot) || t == typeof(Ship)
                || t == typeof(LootItem) || t == typeof(Interval<int>) || t == typeof(Vector2<int>) || t == typeof(Event))
            {
                StringBuilder output = new StringBuilder("{");
                output.AppendLine();
                var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public); 
                foreach (FieldInfo field in fields)
                {
                    output.Append('\t', depth);
                    output.AppendLine(field.Name + " = " + Parse(field.GetValue(target), depth + 1) + "");
                }
                var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (PropertyInfo property in properties)
                {
                    output.Append('\t', depth);
                    output.AppendLine(property.Name + " = " + Parse(property.GetValue(target, null), depth + 1) + "");
                }

                if (output.Length > 1) { output = output.Remove(output.Length - 2, 2); }
                output.AppendLine();
                output.Append('\t', depth);
                output.AppendLine("}");
                return output.ToString();
            }
            // Hit the bottom
            else
            {
                return target.ToString();
            }
        }*/
    }
}
