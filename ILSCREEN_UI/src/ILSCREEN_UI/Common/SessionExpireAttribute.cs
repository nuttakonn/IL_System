using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;

namespace ILSCREEN_UI.Common
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        protected readonly IConnectionMultiplexer _redis;
        public SessionExpireAttribute(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var ignoredList = AppSetting.Configuration?["RedisSSO:IgnoredAction"]?.Split(",");
                if (ignoredList.Any(action => context.HttpContext.Request?.Path.Value?.ToLower().Trim() == action.ToLower().Trim())) return;

                var sessionkey = context.HttpContext.Request.Cookies["k"];
                if (string.IsNullOrWhiteSpace(sessionkey))
                {
                    context.Result = new RedirectResult("~/Home/BlankPage");
                }
                else
                {
                    var db = _redis.GetDatabase();
                    var isExits = db.KeyExists(sessionkey);
                    if (!isExits)
                    {
                        context.Result = new RedirectResult("~/Home/BlankPage");
                    }
                }
                return;
            }
            catch
            {
                //
            }

            base.OnActionExecuting(context);
        }
    }
}
