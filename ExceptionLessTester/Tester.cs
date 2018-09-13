using Exceptionless;
using Exceptionless.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ExceptionLessTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("OK");   
             
            ExceptionlessClient.Default.Configuration.ServerUrl = "http://exceptionless.manjinba.cn";
            ExceptionlessClient.Default.Configuration.ApiKey = "gtoRHHuReUWfJDgfALpI4R6Zdy7pjtooIYv9IJfe";

            ExceptionlessClient.Default.
                CreateLog(
                    typeof(UnitTest1).FullName, // source: �¼�Դ�������ڷ�Χ����
                        "�ύ����", // message: �¼���Ϣ
                        Exceptionless.Logging.LogLevel.Info) // level: �¼����𣬿����ڷ�Χ����
                    .AddObject(new { orderNo = "xxx001" }, "order") // ���Ӷ���
                    .AddTags("order", "xxx001") // tags: ���ӱ�ǩ���������������ز��ң��򵥸�ƥ��                                        
                    .SetUserIdentity(new Exceptionless.Models.Data.UserInfo() // ���õ�ǰ�û�
                    {
                        Name = "cnf", // �û���ʾ����
                        Identity = "10101" // �û���ʶ������������
                    })                 
                    .Submit(); // �ύ

            ExceptionlessClient.Default
               .CreateException(new ArgumentException("��������", "name")) // �쳣����
               .AddTags("order", "xxx001") // tags: ���ӱ�ǩ���������������ز��ң��򵥸�ƥ��                    
               .SetUserIdentity(new Exceptionless.Models.Data.UserInfo()
               {
                   Name = "cnf", // �û���ʾ����
                   Identity = "10101" // �û���ʶ������������
               })
                .AddObject(new { orderNo = "xxx001" }, "order") // ���Ӷ���    
                .SetSource(typeof(UnitTest1).FullName)// �¼�Դ�������ڷ�Χ����                 
                .Submit(); // �ύ
        
            System.Threading.Thread.Sleep(5000);
            return;

            //ExceptionlessClient.Default.Configuration.ApiKey = "iU5R6DIiJN86JdHciVEIuvcE0wA2OFg0EMFQmVpI";
            //ExceptionlessClient.Default.Configuration.ServerUrl = "https://api.exceptionless.io";

            // ExceptionlessClient.Default.SubmitLog("hehe ");

            // �ύһ��Log������:source,message,level
            // ExceptionlessClient.Default.SubmitLog(typeof(UnitTest1).FullName, "This is so easy", "Info");

            // Submit a 404
            //ExceptionlessClient.Default.SubmitNotFound("/somepage");
            //ExceptionlessClient.Default.CreateNotFound("/somepage").AddTags("Exceptionless").Submit();



            ExceptionlessClient.Default.SubmitEvent(new Event
            {
                Message = "Low Fuel",
                Type = "racecar",
                Source = "Fuel System",
                Tags = new TagSet(new string[] { "x", "y" })
            });

            System.Threading.Thread.Sleep(5000);

            // ExceptionlessClient.Default.CreateLog("Hehe").Submit();
        }
    }
}
