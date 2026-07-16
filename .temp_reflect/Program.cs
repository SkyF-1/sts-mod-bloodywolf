using System;
using System.Linq;
using System.Reflection;

class Program {
    static void Main() {
        var path = @"D:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64\sts2.dll";
        var asm = Assembly.LoadFrom(path);
        var types = asm.GetTypes().Where(t => t.Name == "CustomPowerModel" || t.Name == "Hook" || t.Name.EndsWith("PowerModel") || t.Name.EndsWith("Power") || (t.Namespace?.Contains("Hooks") ?? false)).ToArray();
        foreach (var type in types.OrderBy(t=>t.FullName)) {
            Console.WriteLine("TYPE:" + type.FullName);
            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).OrderBy(m=>m.Name)) {
                if (m.Name.Contains("Turn") || m.Name.Contains("Side") || m.Name.Contains("Before") || m.Name.Contains("After")) {
                    Console.WriteLine("  " + m);
                }
            }
        }
    }
}
