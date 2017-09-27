using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chaffinch.Api.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Core.DI;
using Chaffinch.Core.WriteModel.Handlers;
using Chaffinch.CQRS.Events;
using Microsoft.AspNetCore.Http;

namespace Chaffinch.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();            
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateDocumentTypeValidator>());

            services.AddCQRSLite()
                .AddEventStore<InMemoryEventStore>()
                .RegisterCommandHandler(typeof(DocumentTypeCommandHandlers));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Chaffinch API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chaffinch API V1");
            });

            app.UseMvc();

            app.UseCQRSLiteBus(typeof(DocumentTypeCommandHandlers));
        }
    }
}
