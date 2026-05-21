namespace AuthAPI.Data.Entitys
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }


        //Foreign Key
        public int RoleId { get; set; }
        //public string EmployeeId { get; set; }

        public RoleEntiry Role { get; set; }

    }
}
