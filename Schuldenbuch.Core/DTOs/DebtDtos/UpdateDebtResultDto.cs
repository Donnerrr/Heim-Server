// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: UpdateDebtResultDto.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 13.06.2026 01:57:55
 * Projekt: Schuldenbuch.Core.DTOs.DebtDtos
 * Datei: UpdateDebtResultDto
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using System;
using System.Security.Cryptography.X509Certificates;

namespace Schuldenbuch.Core.DTOs.DebtDtos
{
    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    /// 
    /// 
    public enum UpdateStatus
    {
        Success,
        ValidationError,

        Failed
    }


    public class UpdateDebtResultDto
    {
        public int Id { get; set; }
        public string Amount { get; set; }

        public string Message { get; set; }

        public UpdateStatus Status { get; set; }
    }



}