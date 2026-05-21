namespace AuthAPI.Data.Entitys
{
    public class RoleEntiry
    {
        public int RoleId { get; set; }
        public required string RoleName { get; set; }

        public List<UserEntity> Users { get; set; }
    }
}
