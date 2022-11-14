using FluentAssertions;
using LogisticApi.Controllers;
using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Responses;
using LogisticApi.Services;
using LogisticApi.Services.LogisticService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LogisticApi.Tests
{
    public class LogistciApiTests
    {

        private readonly LogisticService _sut;
        private readonly Mock<ILogisticRepository> _logisticRepo = new Mock<ILogisticRepository>();
        public LogistciApiTests()
        {
            _sut = new LogisticService(_logisticRepo.Object);
        }

        [Fact]
        public async Task GetStation_ShouldReturnOk_WhenStationExists()
        {
            var stationId = "636d5fd0c00c86647a99f6c1";
            var station = new StationDTO()
            {
                Id = stationId,
                Latitude = 23.3234,
                Longitude = 23.2325,
                Name = "Station`s name"
            };

            //Arrange
            _logisticRepo.Setup(x => x.GetStation(stationId)).
                ReturnsAsync(station);

            //Act
            var stationRes = await _sut.GetStation(stationId);

            //Assert
            stationRes.Should().BeOfType<StationDTO>();
        }

        [Fact]
        public async Task GetStation_ShouldReturnNotFound_WhenStationDoesNotExists()
        {
            var stationId = "636d5fd0c00c86647a99f6c1";

            StationDTO stationDTO = null;
            
            //Arrange
            _logisticRepo.Setup(x => x.GetStation(stationId)).
                Returns(Task.FromResult(stationDTO));

            //Act
            var stationRes = await _sut.GetStation(stationId);

            //Assert
            stationRes.Should().Be(null);
        }

        [Fact]
        public async Task AddStation_ShouldReturnOk_WhenAdded()
        {
            var station = new StationDTO()
            {
                Latitude = 23.3234,
                Longitude = 23.2325,
                Name = "Station`s name"
            };

            var toReturn = new Response(true, "Any", 200);

            //Arrange
            _logisticRepo.Setup(x => x.AddStation(station)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddStation(station);

            // Assert
            stationRes.IsSuccess.Should().Be(true);
            stationRes.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddStation_ShouldReturnNull_WhenStationIsNullOrSomethingWentWrong()
        {
            StationDTO station = null;

            var toReturn = new Response(false, "Any", 400);

            //Arrange
            _logisticRepo.Setup(x => x.AddStation(station)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddStation(station);

            // Assert
            stationRes.IsSuccess.Should().Be(false);
            stationRes.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AddPassenger_ShouldReturnSuccess_WhenPassengerAdded()
        {
            PassengerDTO passenger = new PassengerDTO()
            {
                Ticket = new TicketDTO() { EndStationId = "dsdsdsds", StartStationId = "dsdsdsd" },  
            };

            var toReturn = new Response(true, "Any", 200);

            //Arrange
            _logisticRepo.Setup(x => x.AddPassenger(passenger)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddPassenger(passenger);

            // Assert
            stationRes.IsSuccess.Should().Be(true);
            stationRes.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddPassenger_ShouldReturnBadRequest400_WhenPassengerValueIsNullOrSmthWentWrong()
        {
            PassengerDTO passenger = null;

            var toReturn = new Response(false, "Any", 400);

            //Arrange
            _logisticRepo.Setup(x => x.AddPassenger(passenger)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddPassenger(passenger);

            // Assert
            stationRes.IsSuccess.Should().Be(false);
            stationRes.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AddTransport_ShouldReturnOk200()
        {
            TransportDTO transport = new TransportDTO();

            var toReturn = new Response(true, "Any", 200);

            //Arrange
            _logisticRepo.Setup(x => x.AddTransport(transport)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddTransport(transport);

            // Assert
            stationRes.IsSuccess.Should().Be(true);
            stationRes.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddTransport_ShouldReturnBadRequesWhenAddingNull()
        {
            TransportDTO transport = null;
            var toReturn = new Response(false, "Any", 400);

            //Arrange
            _logisticRepo.Setup(x => x.AddTransport(transport)).
                Returns(Task.FromResult(toReturn));

            //Act
            var stationRes = await _sut.AddTransport(transport);

            // Assert
            stationRes.IsSuccess.Should().Be(false);
            stationRes.StatusCode.Should().Be(400);
        }
    }
}
