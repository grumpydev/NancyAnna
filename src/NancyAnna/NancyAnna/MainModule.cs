namespace NancyAnna
{
    using Nancy;

    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => View["/Views/Index.cshtml", new { FirstName = "Anna" }];
        }
    }
}