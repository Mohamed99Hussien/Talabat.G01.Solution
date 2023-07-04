using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Excensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IService;

namespace Talabat.APIs.Controllers
{
   
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController
            (
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )

            {
                _userManager = userManager;
                _signInManager = signInManager;
                _tokenService = tokenService;
                _mapper = mapper;
        }

        [HttpPost("login")] // POST: /api/Accounts/login

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false); //false تهل لما يداخل الباس ورد هقفله الاكونت اكيد لا علشان كده عملت 
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            
            return Ok(new UserDto 
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user,_userManager)
            });
        }

        [HttpPost("register")] // POST: /api/Accounts/Register

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) 
        {
            if (CheekEmailExists(registerDto.Email).Result.Value) // .Result.Value because donot use await 
                return BadRequest(new ApiValidationErrorResponse() { Errors = new[] {"this Email is already in Use"} });
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email.Split("@")[0]

            };
            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)

            });
        }

        [Authorize]
        [HttpGet("getcurrentuser")] // GET: /api/Accounts/getcurrentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser() // get user who is login now
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByIdAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            });
        }

        [Authorize]
        [HttpPut("address")] //PUT: /api/Accounts/address

        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            var appUser = await _userManager.FindUserWithAddressByEmailAsync(User);// User is build in

            appUser.Address= address;
            var result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded) return BadRequest(new ApiResponse(400, "An Error Occured during Updating the User Adrress"));
            return Ok(_mapper.Map<Address,AddressDto>(appUser.Address));
        }

        [Authorize]
        [HttpGet("address")] //PUT: /api/Accounts/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var appUser = await _userManager.FindUserWithAddressByEmailAsync(User);

            return Ok(_mapper.Map<Address, AddressDto>(appUser.Address));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheekEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
