using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using Demo.Repository;
using Demo.Dtos;
using System;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserDetails userDetails;

    public UserController(IUserDetails _userDetails)
    {
        userDetails = _userDetails;
    }

   
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDetailsDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await userDetails.AddUserAsync(userDto);
            return Ok(new
            {
                message = "User registered successfully"
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        if (userLoginDto == null)
        {
            return BadRequest(new { message = "User login data is required." });
        }

        if (string.IsNullOrEmpty(userLoginDto.UserName) || string.IsNullOrEmpty(userLoginDto.Password))
        {
            return BadRequest(new { message = "Username or password cannot be null or empty." });
        }

        try
        {
            
            var token = await userDetails.LoginUserAsync(userLoginDto);
            
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException )
        {
            return Unauthorized();
        }
        
    }
}
