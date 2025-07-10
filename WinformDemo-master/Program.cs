//namespace myproject
//{
//    internal static class Program
//    {
//        /// <summary>  
//        ///  The main entry point for the application.  
//        /// </summary>  
//        [STAThread]
//        static void Main()
//        {
//            // To customize application configuration such as set high DPI settings or default font,  
//            // see https://aka.ms/applicationconfiguration.  
//            ApplicationConfiguration.Initialize();
//            Application.Run(new frmMain());

//        }
//    }
//}

// Program.cs
namespace myproject
{
    internal static class Program
    {
        // 全局静态变量，用于在窗体间传递登录用户的信息
        public static string? CurrentUserStudentNo { get; private set; }

        [STAThread]
        static void Main()
        {
            // 如果您使用 .NET 6 或更高版本，保留这行
            ApplicationConfiguration.Initialize();

            // 如果您使用 .NET Framework 或 .NET 5, 注释掉上面一行, 并取消下面两行的注释
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);

            frmLogin loginForm = new frmLogin();

            // 使用 ShowDialog() 模式显示登录窗口。
            // 程序会在此处暂停，直到 loginForm 关闭。
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 如果登录窗体的 DialogResult 是 OK，说明登录成功
                // 保存登录的学号
                CurrentUserStudentNo = loginForm.LoggedInStudentNo;
                // 运行主业务窗体
                //Application.Run(new frmSQL());
                Application.Run(new frmMain());
            }
            // 如果 DialogResult 不是 OK (例如用户关闭了窗口)，Main方法结束，程序退出。
        }
    }
}