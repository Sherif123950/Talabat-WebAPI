using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService token, IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._token = token;
            this._mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto model)
        {
            if (CheckIfEmailExists(model.Email).Result.Value) return BadRequest(new ApiResponse(400,"This Email Is In Use"));
            var User = new ApplicationUser()
            {
                UserName = model.Email.Split('@')[0],
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(User, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto() { DisplayName = User.DisplayName, Email = User.Email, Token = await _token.CreateTokenAsync(User, _userManager) });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LoginUser(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));
            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto() { DisplayName = User.DisplayName, Email = User.Email, Token = await _token.CreateTokenAsync(User, _userManager) });
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }


        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.GetUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateAddressAsync(AddressDto UpdatedAddress)
        {
            var user = await _userManager.GetUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var Address = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            Address.Id = user.Address.Id;
            user.Address = Address;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckIfEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
