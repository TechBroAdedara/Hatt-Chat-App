using System;
using System.ComponentModel.DataAnnotations;

namespace Hatt.Dtos;

public record AddUserDto
(
    [Required]
    string Firstname,
    [Required]
    string Lastname,
    [Required]
    string Email,
    [Required]
    string Username,
    [Required]
    string Password
);

public record DisplayUserDto(
    [Required]
    string Firstname,
    [Required]
    string Lastname,
    [Required]
    string Email,
    [Required]
    string Username
);