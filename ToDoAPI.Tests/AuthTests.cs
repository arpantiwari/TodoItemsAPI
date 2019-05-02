using System;
using Xunit;
using ToDoAPI;
using System.Linq;
using TodoApi.Controllers;
using ToDoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ToDoAPI.Tests
{
    //Unit Test class for AuthController
    public class AuthTests
    {
        [Fact]
        public void ShouldGenerateTokenForValidCredentials()
        {
            //Arrange
            //Defining in memory db context for mocking
            //Note: This won't ensure db referential integrity so we can actually use different techniques for mocking if database referential integrity is important
            var options = new DbContextOptionsBuilder<AuthContext>().UseInMemoryDatabase(databaseName: "ShouldGenerateTokenForValidCredentials").Options;            
            var context = new AuthContext(options);
            //Adding dummy username password (note: in production the password will be in encrypted form instead of plaintext)
            var user = new UserItem { UserID = 1, UserName = "admin", Password = "pass" };
            context.Users.Add(user);
            context.SaveChanges();
            //Passing appsettings configuration to controller
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            //Initialize controller with mock header
            var controller = new AuthController(context, configuration);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Passing valid username and password
            var passedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:pass"));
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + passedCredentials;
            //Act
            var result = controller.GenerateToken();
            var status = result as OkObjectResult;
            var token = status.Value as string;
            //Assert
            //Token string should be returned
            Assert.NotNull(token);
            //Status code should be 200
            Assert.Equal(200, status.StatusCode);
        }

        [Fact]
        public void ShouldNotGenerateTokenForInvalidCredentials()
        {
            //Arrange
            //Defining in memory db context for mocking            
            var options = new DbContextOptionsBuilder<AuthContext>().UseInMemoryDatabase(databaseName: "ShouldNotGenerateTokenForInvalidCredentials").Options;
            var context = new AuthContext(options);
            //Adding dummy username password (note: in production the password will be in encrypted form instead of plaintext)
            var user = new UserItem { UserID = 1, UserName = "admin", Password = "pass" };
            context.Users.Add(user);
            context.SaveChanges();
            //Passing appsettings configuration to controller
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            //Initialize controller with mock header
            var controller = new AuthController(context,configuration);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Passing invalid username and password
            var passedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("dev:dev"));
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + passedCredentials;
            //Act
            var result = controller.GenerateToken();
            var status = result as UnauthorizedResult;            
            //Assert
            //Status code should be 401
            Assert.Equal(401, status.StatusCode);
        }

        [Fact]
        public void ShouldNotGenerateTokenForInvalidOperation()
        {
            //Arrange
            //Defining in memory db context for mocking
            //Note: This won't ensure db referential integrity so we can actually use different techniques for mocking if database referential integrity is important
            var options = new DbContextOptionsBuilder<AuthContext>().UseInMemoryDatabase(databaseName: "ShouldNotGenerateTokenForInvalidOperation").Options;
            var context = new AuthContext(options);
            //Adding dummy username password (note: in production the password will be in encrypted form instead of plaintext)
            var user = new UserItem { UserID = 1, UserName = "admin", Password = "pass" };
            context.Users.Add(user);
            context.SaveChanges();
            //Passing appsettings configuration to controller
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            //Initialize controller with mock header
            var controller = new AuthController(context,configuration);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Passing valid username and password but invalid operation in header
            var passedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:pass"));
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bas " + passedCredentials;
            //Act
            var result = controller.GenerateToken();
            var status = result as BadRequestObjectResult;
            var message = status.Value as string;
            //Assert
            //Message returned should be Invalid Request Type
            Assert.Equal("Invalid request type", message);
            //Status code should be 400
            Assert.Equal(400, status.StatusCode);
        }
    }
}
