// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: AddDebtStatusDto.cs
//  */



public enum DebtStatus
{
		Success,
		IdNotFound,
		ValidationError
}

public class AddDebtStatusDto
{
		public DebtStatus Status {get; set;}
		public string Message {get; set;}
}
