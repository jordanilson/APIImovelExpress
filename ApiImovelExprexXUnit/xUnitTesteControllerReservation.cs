using APIImovelExpress.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Infra.data.Context;
using Infra.data.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiImovelExprexXUnit
{
    public class xUnitTesteControllerReservation
    {
        private readonly IReservatioService _reservatioService; 
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly TestController _testController;
   

        public static DbContextOptions<AppDbContext> _context {  get; set; }
        public static string strBD = "Server=DESKTOP-22EISHQ\\SQLEXPRESS;Database=ProducaoImovelExpressOito;User=sa;Password=265794;trustServerCertificate=true";

        static xUnitTesteControllerReservation()
        {
            _context = new DbContextOptionsBuilder<AppDbContext>()
                          .UseSqlServer(strBD)
                          .Options;
        }

        public xUnitTesteControllerReservation()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingsProfile());
            });
            _mapper = config.CreateMapper();
            var context = new AppDbContext(_context);
            _reservationRepository = new ReservationRepository(context);
            _reservatioService = new ReservationServices(_reservationRepository, _mapper);
            _testController = new TestController();
        }

        [Fact]
        public async void GetReservation_ReturnList()
        {
            var userId = "e7ff895d-6b8d-41ec-b6d6-b637c407d";
            _testController.MockCurrentUser(userId);

            var controller = new ReservationController(_reservatioService);
            controller.ControllerContext = _testController.ControllerContext;

            var data = await controller.GetReservation();
            Assert.IsType<List<ReservationDTO>>(data.Value);
        }

        [Fact]
        public  async void GetReservationById_ReturnsNotFound()
        {
            var userId = "e7ff895d-6b8d-41ec-b6d6-b637c407d";
            _testController.MockCurrentUser(userId);

            var controller = new ReservationController(_reservatioService);
            controller.ControllerContext = _testController.ControllerContext;

            string reservaId = null;
            var data = await controller.GetReservationById(reservaId);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }


        [Fact]
        public async void GetReservationById_ReturnsOkResult()
        {
            var userId = "e7ff895d-6b8d-41ec-b6d6-b637c407d38f";
            _testController.MockCurrentUser(userId);

            var controller = new ReservationController(_reservatioService);
            controller.ControllerContext = _testController.ControllerContext;

            string reservaId = "7b6597c4-3487-4569-8ede-a9f949510fca";
            var data = await controller.GetReservationById(reservaId);

            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public async void PostReservation_ReturnOkResult()
        {
            var userId = "e7ff895d-6b8d-41ec-b6d6-b637c407d38f";
            _testController.MockCurrentUser(userId);

            var controller = new ReservationController(_reservatioService);
            controller.ControllerContext = _testController.ControllerContext;

            var reservaPost = new ReservationDTO()
            {
     
                ReservationProperties = "aletorio",
                ClientsId = "e7ff895d-6b8d-41ec-b6d6-b637c407d38f"
            };
            var data = await controller.PostReservation(reservaPost);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void DeleteReservation_ReturnOkResult()
        {
            var userId = "e7ff895d-6b8d-41ec-b6d6-b637c407d38f";
            _testController.MockCurrentUser(userId);

            var controller = new ReservationController(_reservatioService);
            controller.ControllerContext = _testController.ControllerContext;

            var deleteReservation = "85f93d06-2036-4d06-ac19-3cd3a8875db7";
            var data = await controller.DeleteReservation(deleteReservation);

            Assert.IsType<OkObjectResult>(data.Result);
        }



    }
}
