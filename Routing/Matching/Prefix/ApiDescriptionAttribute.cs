using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace WebApplicationDemo.Routing.Matching.Prefix
{
    public class ApiDescriptionAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        public ApiDescriptionAttribute(string title, string? version = null, string? desc = null, int position = int.MaxValue)
        {
            GroupName = version != null ? $"{title}-{version}" : title;
            Title = title;
            Version = version;
            Description = desc;
            Position = position;
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string? GroupName { get; set; }
        /// <summary>
        /// Swagger 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 分组顺序
        /// </summary>
        public int Position { get; set; }
    }
}
