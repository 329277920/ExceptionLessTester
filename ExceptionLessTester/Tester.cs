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
                    typeof(UnitTest1).FullName, // source: 事件源，可用于范围查找
                        "提交订单", // message: 事件消息
                        Exceptionless.Logging.LogLevel.Info) // level: 事件级别，可用于范围查找
                    .AddObject(new { orderNo = "xxx001" }, "order") // 附加对象
                    .AddTags("order", "xxx001") // tags: 附加标签，可用于搜索返回查找，或单个匹配                                        
                    .SetUserIdentity(new Exceptionless.Models.Data.UserInfo() // 设置当前用户
                    {
                        Name = "cnf", // 用户显示名称
                        Identity = "10101" // 用户标识，可用于搜索
                    })                 
                    .Submit(); // 提交

            ExceptionlessClient.Default
               .CreateException(new ArgumentException("参数错误", "name")) // 异常对象
               .AddTags("order", "xxx001") // tags: 附加标签，可用于搜索返回查找，或单个匹配                    
               .SetUserIdentity(new Exceptionless.Models.Data.UserInfo()
               {
                   Name = "cnf", // 用户显示名称
                   Identity = "10101" // 用户标识，可用于搜索
               })
                .AddObject(new { orderNo = "xxx001" }, "order") // 附加对象    
                .SetSource(typeof(UnitTest1).FullName)// 事件源，可用于范围查找                 
                .Submit(); // 提交
        
            System.Threading.Thread.Sleep(5000);
            return;

            //ExceptionlessClient.Default.Configuration.ApiKey = "iU5R6DIiJN86JdHciVEIuvcE0wA2OFg0EMFQmVpI";
            //ExceptionlessClient.Default.Configuration.ServerUrl = "https://api.exceptionless.io";

            // ExceptionlessClient.Default.SubmitLog("hehe ");

            // 提交一个Log，参数:source,message,level
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
