using System;
using System.ComponentModel.DataAnnotations;

namespace Hatt.Dtos;

public record UserCreateDto
(
    [Required]
    string Firstname,
    [Required]
    string Lastname,
    [Required]
    string Email,
    [Required]
    string UserName,
    [Required]
    string Password
);

public record UserDisplayDto(
    [Required]
    string Firstname,
    [Required]
    string Lastname,
    [Required]
    string Email,
    [Required]
    string UserName
);