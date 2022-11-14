using LogisticApi.Models.Data.DbModels;
using LogisticApi.Models.Data.Models;
using LogisticApi.Models.LogisticModels;
using LogisticApi.Models.Requests;
using LogisticApi.Models.Responses;
using LogisticApi.Services;
using LogisticApi.Services.LogisticService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;
using System.Formats.Asn1;

namespace LogisticApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticController : ControllerBase
    {
        private readonly ILogisticService service;

        public LogisticController(ILogisticService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("AddStation")]
        public async Task<IActionResult> AddStation(StationDTO station)
        {
            var res = await service.AddStation(station);

            if (res.IsSuccess)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }


        [HttpPost]
        [Route("AddTransport")]
        public async Task<IActionResult> AddTransport(TransportDTO transportDTO)
        {
            var res = await service.AddTransport(transportDTO);

            if (res.IsSuccess)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpPost]
        [Route("AddPassenger")]
        public async Task<IActionResult> AddPassenger(PassengerDTO passenger)
        {
            var res = await service.AddPassenger(passenger);

            if (res.IsSuccess)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpDelete]
        [Route("RemovePassengerFromTransport")]
        public async Task<IActionResult> RemovePassengerFromTransport(string passengerId, string transportId)
        {
            var res = await service.RemovePassengerFromTransport(passengerId, transportId);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }

            return BadRequest(res);
        }

        [HttpPut]
        [Route("AddPassengerToTransport")]
        public async Task<IActionResult> AddPassengerToTransport(string passengerId, string transportId)
        {
            var res = await service.AddPassengerToTransport(passengerId, transportId);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }

            return BadRequest(res);
        }

        [HttpPut]
        [Route("AddStationToTransport")]
        public async Task<IActionResult> AddStationToTransport(string stationId, string transportId)
        {
            var res = await service.AddStationToTransport(stationId, transportId);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }

            return BadRequest(res);
        }

        [HttpDelete]
        [Route("RemoveStationFromTransport")]
        public async Task<IActionResult> RemoveStationFromTransport(string stationId, string transportId)
        {
            var res = await service.RemoveStationFromTransport(stationId, transportId);
            if (res.IsSuccess)
            {
                return Ok(res);
            }
            else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }

            return BadRequest(res);
        }

        [HttpGet]
        [Route("GetStation/{stationId}")]
        public async Task<IActionResult> GetStation(string stationId)
        {
            var station = await service.GetStation(stationId);
            if (station == null)
            {
                return NotFound(new Response(false, "Station not found", 404));
            }
            return Ok(station);
        }

        [HttpPut]
        [Route("BuyTicketForPassanger")]
        public async Task<IActionResult> BuyTicketForPassanger(TicketRequestModel ticket)
        {
            var res = await service.
                BuyTicketForPassenger(ticket.PassengerId, ticket.StationStartId, ticket.EndStationId);

            if (res.IsSuccess)
            {
                return Ok(res);
            }
            else if (res.StatusCode == 404)
            {
                return NotFound(res);
            }

            return BadRequest(res);
        }

        [HttpGet]
        [Route("AllStations")]
        public async Task<IActionResult> GetAllStations()
        {
            var res = await service.GetAllStations();
            if (res == null)
            {
                return NotFound(new Response(false, "Stations not found", 404));
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("StartTrip")]
        public async Task<IActionResult> StartTrip(string transportId)
        {
            var raceRes = await service.StartRace(transportId);

            if (raceRes.IsSuccess)
            {
                return Ok(raceRes);
            }
            else
            {
                if (raceRes.StatusCode == 400)
                {
                    return NotFound(raceRes);
                }
                else
                {
                    return BadRequest(raceRes);
                }
            }
        }
    }
}

