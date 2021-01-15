using System.IO;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.EFCore.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Mytime.Distribution.Filters;
using AutoMapper;
using Mytime.Distribution.Configs;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using FluentValidation.AspNetCore;
using Mytime.Distribution.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.Entities;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.WxOpen;
using Essensoft.AspNetCore.Payment.WeChatPay;
using MediatR;
using Mytime.Distribution.HostedServices;
using Mytime.Distribution.Middlewares;

namespace Mytime.Distribution
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private const string POLICY_NAME = "AllowCors";

        private IWebHostEnvironment _environment;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }
        /// <summary>
        /// 配置
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            CustomizeMVC(services);

            CustomizeServices(services);

            ConfigureAuth(services);

            ConfigureRepository(services);

            ConfigureOtherService(services);

#if DEBUG

            ConfigureSwagger(services);

#endif

            CustomHostedServices(services);
        }

        private void CustomHostedServices(IServiceCollection services)
        {
            services.AddHostedService<PaymentReceivedListener>();
        }

        private void CustomizeMVC(IServiceCollection services)
        {
            //添加跨域
            services.AddCors(option => option.AddPolicy(POLICY_NAME, builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            services.AddMediatR(typeof(Startup));

            services.Configure<IISServerOptions>(o =>
            {
                o.AllowSynchronousIO = true;
            }).Configure<KestrelServerOptions>(o =>
            {
                o.AllowSynchronousIO = true;
            });

            services.AddMemoryCache();
            // services.AddSession();

            services.AddControllers(o =>
            {
                o.Filters.Add<ModelStateValidatorFilter>();
                o.Filters.Add<GlobalExceptionFilter>();
            })
            .ConfigureApiBehaviorOptions(o =>
            {
                o.SuppressModelStateInvalidFilter = true;
            }).AddFluentValidation();

            services.AddSenparcGlobalServices(Configuration).AddSenparcWeixinServices(Configuration);

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));
        }

        private void CustomizeServices(IServiceCollection services)
        {
            services.AddScoped<AdminUserRegisterFilterAttribute>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IWechatService, WechatService>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IAdminUserManager, AdminUserManager>();

            services.Configure<Kuaidi100Config>(Configuration.GetSection(nameof(Kuaidi100Config)));
            services.AddScoped<IPackageService, PackageService>();

            services.Configure<SmsConfig>(Configuration.GetSection("Cloud253SMSConfig"));
            services.AddScoped<ISmsService, SmsService>();

            services.Configure<PartnerConfig>(Configuration.GetSection("Partner"));
            services.Configure<RabbitMQConnectionConfig>(Configuration.GetSection(nameof(RabbitMQConnectionConfig)));
            services.AddScoped<IRabbitMQClient, RabbitMQClient>();
        }

        private void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IRepositoryByInt<>), typeof(RepositoryByInt<>));

#if DEBUG
            services.AddEFCoreUseSqlServer(Configuration.GetConnectionString("Default"));
#else
            services.AddEFCoreUseMySql(Configuration.GetConnectionString("MySql"));
#endif

        }

        private void ConfigureAuth(IServiceCollection services)
        {
            var authConfig = new AuthenticationConfig();
            Configuration.GetSection(nameof(AuthenticationConfig)).Bind(authConfig);
            services.AddSingleton(authConfig);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.Key)),
                ValidIssuer = authConfig.Issuer,
                ValidateIssuer = true,
                ValidAudience = authConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = false, //验证令牌过期
                ClockSkew = TimeSpan.FromMinutes(5),
                RequireExpirationTime = true
            };
            services.AddSingleton(tokenValidationParameters);

            //添加Jwt授权
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = tokenValidationParameters;
                });
        }

        private void ConfigureOtherService(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'V";
            });

            services.AddWeChatPay();
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var providers = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in providers.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "脉享分销系统",
                        Version = description.ApiVersion.ToString()
                    });
                }

                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = ""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });

                var xmlPath = Path.Combine(_environment.ContentRootPath, $"{this.GetType().Namespace}.xml");
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="senparcSetting"></param>
        /// <param name="senparcWeixinSetting"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.Use(next => context =>
            // {
            //     context.Request.EnableBuffering();

            //     return next(context);
            // });

#if DEBUG
            UseSwagger(app);
#endif

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            // app.UseSession();

            app.UseMiddleware<RequestResponseLogMiddleware>();

            UseSenparc(app, env, senparcSetting, senparcWeixinSetting);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(POLICY_NAME);
        }

        private void UseSenparc(IApplicationBuilder app,
                                IWebHostEnvironment env,
                                IOptions<SenparcSetting> senparcSetting,
                                IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            // 启动 CO2NET 全局注册，必须！
            var registerServices = app.UseSenparcGlobal(env, senparcSetting.Value, register =>
              {
                  #region APM 系统运行状态统计记录配置

                  //测试APM缓存过期时间（默认情况下可以不用设置）
                  Senparc.CO2NET.APM.Config.EnableAPM = false;//默认已经为开启，如果需要关闭，则设置为 false
                  Senparc.CO2NET.APM.Config.DataExpire = TimeSpan.FromMinutes(60);

                  #endregion
              }, true).UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
               {

                   //#region 注册公众号或小程序（按需）

                   //注册公众号（可注册多个）                                                    -- DPBMARK MP
                   weixinRegister.RegisterMpAccount(senparcWeixinSetting.Value, "【蒂脉】公众号")// DPBMARK_END

                   //注册多个公众号或小程序（可注册多个）                                        -- DPBMARK MiniProgram
                   .RegisterWxOpenAccount(senparcWeixinSetting.Value, "【蒂脉】小程序");// DPBMARK_END

               });
        }

        private void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                var providers = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                foreach (var description in providers.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"脉享分销 {description.ApiVersion}");
                }
            });
        }
    }
}
