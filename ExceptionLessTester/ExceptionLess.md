# Exception Less #
> 目录<br>
> [一、安装](#1)<br>
> [二、配置](#2)<br>
> &nbsp;&nbsp;[2.1、创建项目](#2.1)<br>
> &nbsp;&nbsp;[2.2、项目配置](#2.2)<br>
> &nbsp;&nbsp;[2.3、客户端配置](#2.3)<br>
> [三、.Net集成](#3)<br>
> &nbsp;&nbsp;[3.1、准备](#3.1)<br>
> &nbsp;&nbsp;[3.2、写入事件](#3.2)<br>
> &nbsp;&nbsp;[3.3、查询事件](#3.3)<br>

<h2 id="1">一、安装</h2>
后续补充,目前直接在官网创建账户，并用做测试。<br>
地址: [https://exceptionless.com/](https://exceptionless.com/)

<h2 id="2">二、配置</h2>
<h3 id="2.1">1、创建项目</h3>

  "ALl Projects" -> "Add New Project" ，按提示输入项目信息，并创建项目。成功创建项目后，拿到项目的唯一ApiKey。（注：选择不同的项目类型后，后提示安装相应的NuGet包）。测试项目为NetCore控制台项目。如下图:
  ![](https://i.imgur.com/GTtT7Oz.png)
<br>

<h3 id="2.2">2、项目配置</h3>

"Admin" -> "Projects" -> 右侧列表选择项目 -> "Edit" -> "Settings"
![](https://i.imgur.com/KGoj3Wv.png)
<br>
**Data Exclusions**: <br>
排除指定的字段，以保护敏感数据不被泄漏。用','号隔开，也可以使用通配符。Demo:
<pre><code>Password : 字段Password
Password* : 以Password开头的字段
*Password : 以Password结尾的字段
*Password* : 包含Password的字段</code></pre>

**Error Stacking**<br>
设置异常只跟踪已配置的命名空间。<br>

**Common Methods**<br>
排除一些公共方法<br>

<h3 id="2.3">3、客户端配置</h3>
"Admin" -> "Projects" -> 右侧列表选择项目 -> "Edit" -> "Client Configuration"<br>
它维护一个Dictionary字典，每个配置项包含一个"Key"和"Value"，可以用于配置哪些事件应该被过滤，并实时通知客户端。[更多...](https://github.com/exceptionless/Exceptionless/wiki/Project-Settings)
![](https://i.imgur.com/xTWw4HT.png)
<br>


<h2 id="3">三、.Net集成</h2>
<h3 id="3.1">1、准备</h3>
注：ExceptionLess提供一个[RestApi](https://api.exceptionless.io/docs/)列表。需要通过用户名和密码调用登录接口，并拿到Token，加入到后续接口的请求头或queryString中"access_token"，下面的事例并不采用直接调用ExceptionLess的Rest接口，通过一个封装好的组件来与ExceptionLess交互。<br><br>

**安装客户端组件**
<pre><code>Install-Package Exceptionless</code></pre>

**ApiKey**<br>
创建项目后，拿到项目的ApiKey，参考[2.1、创建项目](#2.1)。

  **配置ApiKey<br>**
下面演示在代码中配置，更多配置方式，查看[这里](https://github.com/exceptionless/Exceptionless.Net/wiki/Configuration#exceptionlessclient-configuration)。<br>
<pre><code>using Exceptionless;

ExceptionlessClient.Default.Configuration.ApiKey = "YOUR_API_KEY"
ExceptionlessClient.Default.Configuration.ServerUrl = "https://api.exceptionless.io";</code></pre>

<h3 id="3.2">2、写入事件</h3>
更多演示，查看[这里](https://github.com/exceptionless/Exceptionless.Net/wiki/Sending-Events)。
<br>

ExceptionLess定义了几种常见的事件类型， **Exceptionless.Models.KnownTypes**<br>
**error**：<br>
**log**：<br>
**usage**：<br>
**404**：<br>
**session**：<br>

下列代码演示写入一个类型为"log"类型的事件，提交后，可在管理界面的"Log Message" -> "Most Recent"中查看，也可以在"All Events" -> "Most Recent" 中查看。<br>
<pre><code>ExceptionlessClient.Default.
                CreateLog(
                    typeof(UnitTest1).FullName, // source: 事件源，可用于范围查找
                    "提交订单", // message: 事件消息
                    Exceptionless.Logging.LogLevel.Info): level: 日志级别，可用于范围查找
                    .AddObject(new { orderNo = "xxx001" }, "order") // 附加对象
                    .AddTags("order", "xxx001") // tags: 附加标签，可用于搜索返回查找，或单个匹配                    
		    // 设置当前用户
                    .SetUserIdentity(new Exceptionless.Models.Data.UserInfo() { 
                         Name = "cnf", // 用户显示名称
                         Identity = "10101" // 用户标识，可用于搜索
                    })
                    .Submit(); // 提交
</code></pre>

下列代码演示写入一个类型为"error"类型的事件，提交后，可在管理界面的"Exceptions" -> "Most Recent"中查看，也可以在"All Events" -> "Most Recent" 中查看。<br>
<pre><code>ExceptionlessClient.Default
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
</code></pre>

<h3 id="3.3">3、查询事件</h3>
在管理界面，ExceptionLess提供比较丰富的查询语法，在搜索框输入要查询的内容即可。下面演示几个比较常用的查询，详细参考[这里](https://github.com/exceptionless/Exceptionless/wiki/Filtering-Searching)。<br>

**按事件类型搜索**：
<pre><code>type:error</code></pre>
**按日志级别搜索，只适用于类型为"log"的事件**：
<pre><code>level:info</code></pre>
**按自定义"source"搜索：**
<pre><code>source:my Source</code></pre>
**按自定义"tags"搜索：**
<pre><code>tag:myTag1 or tag:myTag2</code></pre>
**按用户Identity搜索：**
<pre><code>user:10101</code></pre>


