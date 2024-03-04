using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class UpdateRoleModel
    {
        public string? RoleId { get; set; }
        public string? NewRoleName { get; set; }
    }