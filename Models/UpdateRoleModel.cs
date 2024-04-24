﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


// Declare a public class named UpdateRoleModel.
public class UpdateRoleModel
{
    // Define a property representing the RoleId. It allows null values.
    public string? RoleId { get; set; }

    // Define a property representing the NewRoleName. It allows null values.
    public string? NewRoleName { get; set; }
}