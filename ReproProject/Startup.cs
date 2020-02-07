using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ReproProject.Models;
using ReproProject.Profiles;
using System.Linq;

namespace ReproProject
{
    public class Startup
    {
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(c => c.EnableEndpointRouting = false);
            services.AddOData();
            
            services.AddMvc(op =>
            {
                foreach (var formatter in op.OutputFormatters.OfType<ODataOutputFormatter>().Where(it => !it.SupportedMediaTypes.Any()))
                    formatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.mock-odata"));

                foreach (var formatter in op.InputFormatters.OfType<ODataInputFormatter>().Where(it => !it.SupportedMediaTypes.Any()))
                    formatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.mock-odata"));
            })
           .AddJsonOptions(op => { op.JsonSerializerOptions.IgnoreNullValues = true; });

            services.AddSingleton<IMapper>((sp) =>
            {
                var mce = new MapperConfigurationExpression();
                mce.ConstructServicesUsing(sp.GetService);

                mce.AddProfile(new EventProfile());
                mce.AddExpressionMapping();

                var mc = new MapperConfiguration(mce);
                mc.AssertConfigurationIsValid();

                return new Mapper(mc, t => sp.GetService(t));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Some App", Version = "v1" });
            });

            services.AddDbContext<ReproContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var ctx = scope.ServiceProvider.GetService<ReproContext>();
            ctx.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Some App");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller}/{action=Index}/{id?}");

                routes.EnableDependencyInjection();
                routes.Select().MaxTop(30).OrderBy().Filter().Count(QueryOptionSetting.Allowed);
            });
        }
    }
}
