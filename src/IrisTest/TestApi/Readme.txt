一、 ASP.NET Core应用程序的启动过程
1. ConfigureWebHostDefault
配置必要的组件，容器、日志等

2. ConfigureHostConfiguration
配置应用程序启动的时的参数

3. ConfigureAppConfiguration
配置应用程序使用的自定义参数

4. 
• ConfigureService
• ConfigureLogging
• Startup
• Startup.ConfigureServices  (等同于 webBuilder.ConfigureServices())
往容器里注入应用的组件

5. Startup.Configure (等同于 webBuilder.Configure())
注入中间件，处理HttpContext整个请求过程


二、 依赖注入
1. 普通注册类型、接口
2. 直接注入实例
3. 注入工厂，由工厂创建实例
4. 尝试注册、移除和替换注册
5. 注册泛型


1. 从容器中获取实例两种方式
1.1 单个接口使用
1.2 整个类使用