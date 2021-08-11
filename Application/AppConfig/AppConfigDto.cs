using System;
using System.Collections.Generic;

namespace Application._AppConfig
{
    public class AppConfigDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public int Order { get; set; }	
		public int Type { get; set; }
		public int ConfigTypeId { get; set; }
		public string ConfigType { get; set; }
		public string Det1 { get; set; }
		public string Det2 { get; set; }
		public string Det3 { get; set; }
		public string Det4 { get; set; }
		public string Det5 { get; set; }						
    }
}
