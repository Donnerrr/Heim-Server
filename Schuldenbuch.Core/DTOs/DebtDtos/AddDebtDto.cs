using System;
using System.Collections.Generic;
using System.Text;

namespace Schuldenbuch.Core.DTOs.DebtDtos
{

	public class AddDebtDto
	{
		public int PersonId { get; set; }
		public string Amount { get; set; }
		public string Description { get; set; }
	}
}