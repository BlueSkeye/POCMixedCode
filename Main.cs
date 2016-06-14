using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

using Allocation;
using RefAllocation;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: CompilationRelaxations(8)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyDelaySign(true)]
[assembly: AssemblyKeyFile("keypair.snk")]

// ILOnly, Required32Bit, StrongNameSigned

namespace Global
{
    public static class Main
    {
        private static void AttemptFillPage(UIntPtr at)
        {
            unsafe {
                byte* target = (byte*)(at.ToPointer());
                for(int index = 0; index < 4096; index++) {
                    target[index] = (byte)(index % 256);
                }
                Console.WriteLine("Page successfully initialized.");
            }
        }

        private static void DynRefCall()
        {
            Console.WriteLine("Dynamic ref call");
            try {
                Assembly dynamicallyReferencedAssembly = Assembly.LoadFrom(@".\DynRefLib.dll");
                Type targetType = dynamicallyReferencedAssembly.GetType("DynRefAllocation.DynRefAllocManager");
                MethodInfo targetMethod = targetType.GetMethod("Alloc", BindingFlags.Public | BindingFlags.Static,
                    null, Type.EmptyTypes, null);
                if (null == targetMethod) { throw new ApplicationException("Method not found."); }
                unsafe {
                    UIntPtr allocation = (UIntPtr)(targetMethod.Invoke(null, null));

                    if (null == allocation) { Console.WriteLine("Allcation failed."); }
                    else {
                        Console.WriteLine("Allocated at 0x{0:X8}", allocation.ToUInt64());
                        AttemptFillPage(allocation);
                    }
                }
            }
            catch (SecurityException e) {
                for(Exception scannedException =e; null != scannedException; scannedException = scannedException.InnerException) {
                    Console.WriteLine(scannedException.Message);
                    Console.WriteLine(scannedException.StackTrace);
                    Console.WriteLine("========================================");
                }
            }
            return;
        }

        private static void DirectInternalCall()
        {
            Console.WriteLine("Direct internal call");
            try {
                unsafe {
                    UIntPtr allocation = (UIntPtr)AllocManager.Alloc();

                    if (null == allocation) { Console.WriteLine("Allcation failed."); }
                    else {
                        Console.WriteLine("Allocated at 0x{0:X8}", allocation.ToUInt64());
                        AttemptFillPage(allocation);
                    }
                }
            }
            catch (SecurityException e) {
                for(Exception scannedException =e; null != scannedException; scannedException = scannedException.InnerException) {
                    Console.WriteLine(scannedException.Message);
                    Console.WriteLine(scannedException.StackTrace);
                    Console.WriteLine("========================================");
                }
            }
            return;
        }

        public static int Entry(string[] args)
        {
            DirectInternalCall();
            RefCall();
            DynRefCall();
            return 0;
        }

        private static void RefCall()
        {
            Console.WriteLine("Ref call");
            try {
                unsafe {
                    UIntPtr allocation = (UIntPtr)RefAllocManager.Alloc();

                    if (null == allocation) { Console.WriteLine("Allcation failed."); }
                    else {
                        Console.WriteLine("Allocated at 0x{0:X8}", allocation.ToUInt64());
                        AttemptFillPage(allocation);
                    }
                }
            }
            catch (SecurityException e) {
                for(Exception scannedException =e; null != scannedException; scannedException = scannedException.InnerException) {
                    Console.WriteLine(scannedException.Message);
                    Console.WriteLine(scannedException.StackTrace);
                    Console.WriteLine("========================================");
                }
            }
            return;
        }
    }
}