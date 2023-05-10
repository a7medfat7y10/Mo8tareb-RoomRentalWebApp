using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Constants
{
    public enum Roles
    {
        Admin,
        User,
        Owner
    }
    public static class Authorization
    {
    
        public static readonly string AdminUserName = "mo8tarebwebapp@mo8tareb.com";
        public static readonly string AdminEmail = "mo8tarebwebapp@mo8tareb.com";
        public static readonly string AdminPassword = "Admin1234.";
        public static readonly string Admin = "Admin";
                      
        public static readonly string OwnerUserName = "Ahmedfathy.1074";
        public static readonly string OwnerEmail = "Ahmedfathy.1074@gmail.com";
        public static readonly string OwnerPassword = "Owner1234.";
        public static readonly string Owner = "Owner";
                      
        public static readonly string UserUserName = "user@mo8tareb.com";
        public static readonly string UserEmail = "user@mo8tareb.com";
        public static readonly string UserPassword = "User1234.";
        public static readonly string User = "User";

    }
}
