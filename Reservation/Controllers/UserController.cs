using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation.Core.Contract.Repository;
using Reservation.Core.Entities;
using Reservation.DTOs;

namespace Reservation.Controllers
{
  
    public class UserController : BaseApiController
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Seat> _seatRepo;

        public UserController(IGenericRepository<User> UserRepo , IMapper mapper , IGenericRepository<Seat> SeatRepo)
        {
            _userRepo = UserRepo;
            _mapper = mapper;
            _seatRepo = SeatRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userRepo.GetAsync(id);
            var mappedUser = _mapper.Map<User , UserDto>(user);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(mappedUser);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> AddUser(UserDto userDto)
        {
            var mappedUser = _mapper.Map<UserDto, User>(userDto);
            await _userRepo.AddedAsync(mappedUser); // Ensure that the method is awaited
            return Ok(mappedUser);
        }

        [HttpDelete("RemoveUser/{id}")]
        public async Task<ActionResult<String>> DeleteUser(int id)
        {
            var seats = await _seatRepo.GetSeatsByUserId(id);
            foreach(var seat in seats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }
            _seatRepo.UpdateAll(seats);

            var user = await _userRepo.GetAsync(id);
            if(user is null)
            {
                return NotFound();
            }
             _userRepo.DeleteAsync(user);
            return Ok("User Deleted Successfully");
        }

        [HttpPut("UpdatUser/{id}")]
        public async Task<ActionResult<UserDto>> UpdatUserData(int id , UserDto userDto)
        {
            var user = await _userRepo.GetAsync(id);

            if(user is null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.Phone = userDto.Phone;
            user.Email = userDto.Email;
            user.NumberOfReservation = userDto.NumberOfReservation;


           
            _userRepo.UpdateAsync(user);
            return Ok(user);
        }

        [HttpPost("ReserveSeat/{UserId}")]
        public async Task<ActionResult<string>> ReserveSeat(ReserveSeatDto reserveSeatDto , int UserId)
        {
            var user = await _userRepo.GetAsync(UserId);
            if(user is null)
            {
                return NotFound(new {message = "User Not Found"});
            }

            var seat = await _seatRepo.GetAsync(reserveSeatDto.SeatId);
            if (seat is null)
            {
                return NotFound(new { message = "Seat Not Found" });
            }
            if(seat.Status == SeatStatus.Reserved)
            {
                return BadRequest(new { message = "Seat Is Already Reserved" });
            }

            seat.UserId = UserId;
            seat.Status = SeatStatus.Reserved;

            _seatRepo.UpdateAsync(seat);
            return Ok("Seat Is Reserved Successfully");

        }

        [HttpGet("GetAllSeatsByUserId/{userId}")]
        public async Task<ActionResult<Seat>> GetAllSeatsByUserId(int userId)
        {
            var seats = await _seatRepo.GetSeatsByUserId(userId);
            return Ok(seats);
        }

        [HttpPut("RemoveAllSeatByUserId/{userId}")]
        public async Task<IActionResult> RemoveAllSeatByUserId(int userId)
        {
            // Retrieve all seats with the specified UserId
            var seats = await _seatRepo.GetSeatsByUserId(userId);

            // Check if any seats were found
            if (seats == null || !seats.Any())
            {
                return NotFound($"No seats found for UserId {userId}");
            }

            // Update each seat's UserId and Status
            foreach (var seat in seats)
            {
                seat.UserId = null;
                seat.Status = SeatStatus.free;
            }

            // Save the changes
             _seatRepo.UpdateAll(seats);

            // Return an Ok result
            return Ok();
        }


        [HttpGet("GetAllSeat")]
        public async Task<ActionResult<Seat>> GetAllSeats()
        {
            var seats = await _seatRepo.GetAllAsync();
            return Ok(seats);
        }
    }
}
