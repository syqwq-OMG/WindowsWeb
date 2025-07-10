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
        // ȫ�־�̬�����������ڴ���䴫�ݵ�¼�û�����Ϣ
        public static string? CurrentUserStudentNo { get; private set; }

        [STAThread]
        static void Main()
        {
            // �����ʹ�� .NET 6 ����߰汾����������
            ApplicationConfiguration.Initialize();

            // �����ʹ�� .NET Framework �� .NET 5, ע�͵�����һ��, ��ȡ���������е�ע��
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);

            frmLogin loginForm = new frmLogin();

            // ʹ�� ShowDialog() ģʽ��ʾ��¼���ڡ�
            // ������ڴ˴���ͣ��ֱ�� loginForm �رա�
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // �����¼����� DialogResult �� OK��˵����¼�ɹ�
                // �����¼��ѧ��
                CurrentUserStudentNo = loginForm.LoggedInStudentNo;
                // ������ҵ����
                //Application.Run(new frmSQL());
                Application.Run(new frmMain());
            }
            // ��� DialogResult ���� OK (�����û��ر��˴���)��Main���������������˳���
        }
    }
}