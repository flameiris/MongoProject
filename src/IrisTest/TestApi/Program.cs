using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TestApi.IService;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Confluent.Kafka;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                  Host.CreateDefaultBuilder(args)
                      .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                      {
                          builder.RegisterType<OrderV2Service>().As<IOrderV2Service>();
                          builder.RegisterType<OrderV3Service>().Named<IOrderV2Service>("OrderV3Service");
                      }))
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          Console.WriteLine("ConfigureWebHostDefaults 配置必要的组件，容器、日志等");

                          webBuilder.ConfigureServices(services =>
                          {
                              Console.WriteLine("webBuilder ConfigureServices 往容器里注入应用的组件");

                              services.AddControllers();


                              services.AddTransient<IOrderService, OrderService>();
                              services.AddSingleton(service =>
                              {
                                  var confProducer = new ProducerConfig { BootstrapServers = "192.168.5.25:19092,192.168.5.25:19093,192.168.5.25:19094" };
                                  var confConsumer = new ConsumerConfig
                                  {
                                      GroupId = "test-consumer-group",
                                      BootstrapServers = "192.168.5.25:19092,192.168.5.25:19093,192.168.5.25:19094",
                                      AutoOffsetReset = AutoOffsetReset.Earliest
                                  };
                                  return new KafkaManager(confProducer, confConsumer);
                              });


                          });




                          webBuilder.Configure((webHostBuilderContext, app) =>
                          {
                              var env = webHostBuilderContext.HostingEnvironment;
                              Console.WriteLine("webBuilder Configure 注入中间件，处理HttpContext整个请求过程");
                              if (env.IsDevelopment())
                              {
                                  app.UseDeveloperExceptionPage();
                              }

                              app.UseRouting();

                              app.UseEndpoints(endpoints =>
                              {
                                  endpoints.MapControllers();
                              });


                              //var _o = app.ApplicationServices.GetService<IOrderService>();

                          });
                          //webBuilder.UseStartup<Startup>();
                      })
                      .ConfigureHostConfiguration(congifure =>
                      {
                          Console.WriteLine("ConfigureHostConfiguration 配置应用程序启动的时的参数");
                      })
                      .ConfigureAppConfiguration(configure =>
                      {
                          Console.WriteLine("ConfigureAppConfiguration 配置应用程序使用的自定义参数");
                      })
                      .ConfigureServices(configure =>
                      {
                          Console.WriteLine("ConfigureService 往容器里注入应用的组件");
                      })
                      .ConfigureLogging(configure =>
                      {
                          Console.WriteLine("ConfigureLogging 往容器里注入应用的组件");
                      })
                  ;
    }
}
