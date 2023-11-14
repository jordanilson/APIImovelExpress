using API.Controllers;
using APIImovelExpress.Controllers;
using Application.DTOs;
using Application.Interfaaces;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Castle.Core.Configuration;
using Domain.Interfaces;
using Domain.Models;
using Infra.data.Context;
using Infra.data.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ApiImovelExprexXUnit
{
    public class xUnitTestControllerProperties
    {
        private readonly IPropertiesRepository _propertiesRepository;
        private readonly IPropertiesServices _propertiesServices;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly TestController _testController;


        public static DbContextOptions<AppDbContext> _context { get; set; }
        public static string strBD = "Server=DESKTOP-22EISHQ\\SQLEXPRESS;Database=ProducaoImovelExpressOito;User=sa;Password=265794;trustServerCertificate=true";

        static xUnitTestControllerProperties()
        {
            _context = new DbContextOptionsBuilder<AppDbContext>()
                          .UseSqlServer(strBD)
                          .Options;
        }

        public xUnitTestControllerProperties()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingsProfile());
            });
            _mapper = config.CreateMapper();
            var context = new AppDbContext(_context);
            _propertiesRepository = new PropertiesRepository(context);
            _propertiesServices = new PropertiesServices(_propertiesRepository, _mapper);
            _configuration = new ConfigurationBuilder().Build();
            _testController = new TestController();
        }

        [Fact]
        public async void GetProperties_ReturnList()
        {
            var userId = "f0cc287c-ba1c-4699-a081-3be8c6793785";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            var data = await controller.GetProperties();
            Assert.IsType<List<PropertiesDTO>>(data.Value);
        }

        [Fact]
        public async void GetPropertiesById_ReturnsNotFound()
        {
            var userId = "f0cc287c-ba1c-4699-a081-3be8c6793785";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            string reservaId = null;
            var data = await controller.GetPropertyById(reservaId);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        [Fact]
        public async void GetPropertiesById_ReturnsOkResult()
        {
            var userId = "f0cc287c-ba1c-4699-a081-3be8c6793785";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            string ownersId = "63136d69-e73e-4c45-8276-f5ba90e8bbc9";
            var data = await controller.GetPropertyById(ownersId);

            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public async void PostProperties_ReturnOkResult()
        {
            var userId = "63136d69-e73e-4c45-8276-f5ba90e8bbc9";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            var propertiesPost = new PropertiesDTO()
            {

                City = "TestexUnit",
                Price = 55,
                Availability = true,
                Amenities = "TestexUnit",
                Name = "TestexUnit",
                Photo = null,
                Description = "TestexUnit",
                ImgUrl = "TestexUnit",
                OwnersId = userId
            };
            var data = await controller.PostProperties(propertiesPost);

            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public async void UpdateProperties_ReturnOkResult()
        {

            var userId = "63136d69-e73e-4c45-8276-f5ba90e8bbc9";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            var propertiesUpdate = new PropertiesDTO()
            {
                Id = "d9c0d06d-a1cf-41db-87e0-ec1da6bd3281",
                City = "atualizando teste",
                Price = 89899,
                Availability = true,
                Amenities = "atualizando teste",
                Name = "atualizando teste",
                Photo = null,
                Description = "atualizando teste",
                OwnersId = userId
            };

            var result = await controller.UpdateProperties(propertiesUpdate);
            Assert.IsType<PropertiesDTO>(result.Value);
        }

        [Fact]
        public async void DeleteProperties_ReturnOkResult()
        {
            var userId = "63136d69-e73e-4c45-8276-f5ba90e8bbc9";
            _testController.MockCurrentUser(userId);

            var controller = new PropertiesController(_propertiesServices, _configuration, _mapper);
            controller.ControllerContext = _testController.ControllerContext;

            var deleteReservation = "c88ceeb0-ba2a-46c9-9dd7-f2fecf8682de";
            var data = await controller.DeleteProperties(deleteReservation);

            Assert.IsType<OkObjectResult>(data.Result);
        }
    }
}
