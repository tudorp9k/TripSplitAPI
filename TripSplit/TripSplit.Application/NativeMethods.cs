using System.Runtime.InteropServices;

namespace TripSplit.Application
{
    public static class NativeMethods
    {
        public const string NativeLib = "pdf_gen.dll";

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "create_args")]
        public static extern unsafe void* CreateArgs([MarshalAs(UnmanagedType.LPStr)] string dest,
            [MarshalAs(UnmanagedType.LPStr)] string total);

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "create_user_expense")]
        public static extern unsafe void* CreateUserExpense(void* args, [MarshalAs(UnmanagedType.LPStr)] string user,
            [MarshalAs(UnmanagedType.LPStr)] string total);

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "add_expense")]
        public static extern unsafe void AddExpense(void* userExpense, [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string amount);

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "run")]
        public static extern unsafe void Run(void* args, [MarshalAs(UnmanagedType.LPStr)] string path);

        public class Args
        {
            private readonly unsafe void* ptr;

            public Args(string dest, string total)
            {
                unsafe
                {
                    ptr = CreateArgs(dest, total);
                }
            }

            public UserExpense CreateUserExpense(string user, string total)
            {
                unsafe
                {
                    var expensePtr = NativeMethods.CreateUserExpense(ptr, user, total);
                    return new UserExpense { ptr = expensePtr };
                }
            }

            public void Run(string path)
            {
                unsafe
                {
                    NativeMethods.Run(ptr, path);
                }
            }

            public class UserExpense
            {
                internal unsafe void* ptr;

                public void AddExpense(string name, string amount)
                {
                    unsafe
                    {
                        NativeMethods.AddExpense(ptr, name, amount);
                    }
                }
            }
        }
    }
}
