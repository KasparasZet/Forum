namespace Forum.Auth.Model
{
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string SimpleUser = nameof(SimpleUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, SimpleUser };
    }
}
