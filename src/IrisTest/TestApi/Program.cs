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
                          Console.WriteLine("ConfigureWebHostDefaults ���ñ�Ҫ���������������־��");

                          webBuilder.ConfigureServices(services =>
                          {
                              Console.WriteLine("webBuilder ConfigureServices ��������ע��Ӧ�õ����");

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
                              Console.WriteLine("webBuilder Configure ע���м��������HttpContext�����������");
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
                          Console.WriteLine("ConfigureHostConfiguration ����Ӧ�ó���������ʱ�Ĳ���");
                      })
                      .ConfigureAppConfiguration(configure =>
                      {
                          Console.WriteLine("ConfigureAppConfiguration ����Ӧ�ó���ʹ�õ��Զ������");
                      })
                      .ConfigureServices(configure =>
                      {
                          Console.WriteLine("ConfigureService ��������ע��Ӧ�õ����");
                      })
                      .ConfigureLogging(configure =>
                      {
                          Console.WriteLine("ConfigureLogging ��������ע��Ӧ�õ����");
                      })
                  ;
    }
}
