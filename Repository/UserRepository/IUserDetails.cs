using Demo.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Repository;

public interface IUserDetails
{

    Task<string> AddUser(UserDetailsDto userDto);
    Task<string> LoginUserAsync(UserLoginDto userLoginDto);



}

