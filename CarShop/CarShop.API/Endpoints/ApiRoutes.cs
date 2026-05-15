namespace CarShop.API.Endpoints;

public static class ApiRoutes
{
    public const string ApiBase = "/api";

    public static class Cars
    {
        public const string Base = ApiBase + "/cars";
        public const string ById = ApiBase + "/cars/{id:int}";
        public const string Photos = ApiBase + "/cars/{id:int}/photos";
    }

    public static class Favorites
    {
        public const string Base = ApiBase + "/favorites";
        public const string ByCarId = ApiBase + "/favorites/{carId:int}";
    }

    public static class Users
    {
        public const string Base = ApiBase + "/users";
        public const string Register = ApiBase + "/users/register";
        public const string Login = ApiBase + "/users/login";
    }

    public static class Brands
    {
        public const string Base = ApiBase + "/brands";
    }
}