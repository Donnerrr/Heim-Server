// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: Program.cs
//  */

using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.Services;
using System.Runtime.CompilerServices;


public class Program
{

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var context = new SchuldenbuchContext();
        var database = new SqliteDatabase(context);
        var service = new PersonService(database);
        var debtservice = new DebtService(database);


        // personDto wird erstellt, um die Funktionalität des Hinzufügens einer Person zu testen
        /* var personDto = new AddPersonDto
         {
             Name = "Max Mustermann",
             Street = "Musterstraße 1",
             ZipCode = "12345",
             City = "Musterstadt"
         };

         var result = service.AddPerson(personDto);

         var personDto2 = new AddPersonDto
         {
             Name = "Maximus Mustermann",
             Street = "Musterstraße 1",
             ZipCode = "12345",
             City = "Musterstadt"
         };

         var result2 = service.AddPerson(personDto2);

         var personDto3 = new AddPersonDto
         {
             Name = "Maxime Mustermann",
             Street = "Musterstraße 1",
             ZipCode = "12345",
             City = "Musterstadt"
         };

         var result3 = service.AddPerson(personDto3);


         service.DeletePerson(6);
         service.DeletePerson(3); */

       

    }
}
