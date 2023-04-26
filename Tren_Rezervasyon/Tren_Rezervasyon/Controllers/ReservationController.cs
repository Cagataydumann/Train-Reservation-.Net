using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tren_Rezervasyon.Controllers;
using Tren_Rezervasyon.Controllers.Tren_Rezervasyon;
using Tren_Rezervasyon.Models;

namespace Tren_Rezervasyon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ReservationResponse> Post(ReservationRequest request)
        {
            var response = new ReservationResponse { Reservations = new List<TrainCarReservation>() };

            // Calculate total occupancy of the train
            int totalOccupancy = request.Train.Vagons.Sum(c => c.Capacity);

            // Calculate remaining capacity for each vagon based on reservation size
            var remainingCapacities = request.Train.Vagons.Select(c => new { Car = c, RemainingCapacity = c.Capacity - (totalOccupancy - c.Capacity + request.ReservationSize) }).ToList();

            // Check if reservation can be made in each vagon
            foreach (var rc in remainingCapacities)
            {
                if (rc.RemainingCapacity >= request.ReservationSize || (request.AllowDifferentVagon && rc.RemainingCapacity > 0))
                {
                    // Make reservation
                    response.Reservations.Add(new TrainCarReservation { TrainCarName = rc.Car.Name, ReservationSize = request.ReservationSize });
                    totalOccupancy += request.ReservationSize;
                }
            }

            // Check if reservation was successful
            response.IsSuccessful = response.Reservations.Count > 0;

            // Return response
            return response;
        }

        [HttpPost("docs")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Docs(ReservationRequest request)
        {
            return Ok();
        }

        [HttpPost("reservationresult")]
        public ReservationResult PostReservationResult(ReservationRequest reservationRequest)
        {
            var train = reservationRequest.Train;
            var reservationSize = reservationRequest.ReservationSize;
            var allowDifferentVagon = reservationRequest.AllowDifferentVagon;

            var totalCapacity = train.TotalCapacity;
            var totalFilledSeats = train.TotalFilledSeats;

            if (reservationSize > totalCapacity - totalFilledSeats)
            {
                // Rezervasyon yapılamaz
                return new ReservationResult { IsSuccessful = false };
            }

            // Rezervasyon yapılabilir
            var seatAssignments = new List<SeatAssignment>();

            foreach (var vagon in train.Vagons)
            {
                var availableSeats = vagon.Capacity - vagon.FilledSeats;
                var seatsToAssign = Math.Min(reservationSize, availableSeats);

                if (seatsToAssign > 0)
                {
                    seatAssignments.Add(new SeatAssignment { VagonName = vagon.Name, NumberOfSeats = seatsToAssign });

                    if (!allowDifferentVagon)
                    {
                        break;
                    }

                    reservationSize -= seatsToAssign;
                }
            }

            return new ReservationResult { IsSuccessful = true, SeatAssignments = seatAssignments };
        }
    }



    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Vagon> Vagons { get; set; }
        public int TotalCapacity => Vagons.Sum(v => v.Capacity);
        public int TotalFilledSeats => Vagons.Sum(v => v.FilledSeats);
    }

    public class ReservationRequest
    {
        public Train Train { get; set; }
        public int ReservationSize { get; set; }
        public bool AllowDifferentVagon { get; set; }
    }


    public class ReservationResponse
    {
        public bool IsSuccessful { get; set; }
        public List<TrainCarReservation> Reservations { get; set; }
    }

    public class TrainCarReservation
    {
        public string TrainCarName { get; set; }
        public int ReservationSize { get; set; }
    }

    namespace Tren_Rezervasyon
    {
        public class ReservationRequest
        {
            public Train Train { get; set; }
            public int ReservationSize { get; set; }
            public bool AllowDifferentVagon { get; set; }
        }

        public class Vagon
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Capacity { get; set; }
            public int FilledSeats { get; set; }
        }

        public class ReservationResult
        {
            public bool IsSuccessful { get; set; }
            public List<SeatAssignment> SeatAssignments { get; set; }
        }

        public class SeatAssignment
        {
            public string VagonName { get; set; }
            public int NumberOfSeats { get; set; }
        }
    }

}

