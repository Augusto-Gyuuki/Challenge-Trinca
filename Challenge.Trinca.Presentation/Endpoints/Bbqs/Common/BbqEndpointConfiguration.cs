namespace Challenge.Trinca.Presentation.Endpoints.Bbqs.Common;

public static class BbqEndpointConfiguration
{
    public const string RoutePrefix = "churras";
    public const string ModeratePrefix = "moderar";
    public const string ShoppingListPrefix = "lista_compras";
    public const string BbqIdParam = "churrasId";

    public const string CreateRoute = $"{RoutePrefix}";
    public const string ListRoute = $"{RoutePrefix}";
    public const string ModerateRoute = $"{RoutePrefix}/{{{BbqIdParam}}}/{ModeratePrefix}";
    public const string ShoppingListRoute = $"{RoutePrefix}/{{{BbqIdParam}}}/{ShoppingListPrefix}";
}
