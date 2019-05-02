using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoAPI.Models;

namespace ToDoAPI
{
    //File containing all startup information pertaining to the Todo API
    public class Startup
    {
        //Will hold configuration stored in a config file (appsettings.json)
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Initializing DB context for Todo items as well as User list using Sqlite as the persistent backend store
            //and connection string from appsettings.json file
            services.AddDbContext<ItemsContext>(opt => opt.UseSqlite(Configuration["ConnectionString"]));            
            services.AddDbContext<AuthContext>(opt => opt.UseSqlite(Configuration["ConnectionString"]));
            //Adding authentication information for our API using token based JwtBearer authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   //Same as our authenticate controller
                   //Get key, issuer and audience details from appsettings.json file
                   //Validate the issuer field in the token
                   ValidateIssuer = true,
                   //Validate the audience field in the token
                   ValidateAudience = true,
                   //Validate the issuer signing key in the token
                   ValidateIssuerSigningKey = true,
                   //Get the issuer from config file
                   ValidIssuer = Configuration["AuthenticationSettings:Issuer"],
                   //Get the audience from config file
                   ValidAudience = Configuration["AuthenticationSettings:Audience"],
                   //Get the issuer signing key from config file
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationSettings:EncryptionKey"]))                   
               };
           }
            );
            //Registering MVC services
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //Enabling authentication
            app.UseAuthentication();
            //Enabling MVC services (i.e. automatic routing management)
            app.UseMvc();
        }
    }
}